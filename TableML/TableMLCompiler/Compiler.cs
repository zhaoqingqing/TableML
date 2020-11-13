#region Copyright (c) 2015 KEngine / Kelly <http://github.com/mr-kelly>, All rights reserved.

// KEngine - Toolset and framework for Unity3D
// ===================================
// 
// Filename: SettingModuleEditor.cs
// Date:     2015/12/03
// Author:  Kelly
// Email: 23110388@qq.com
// Github: https://github.com/mr-kelly/KEngine
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3.0 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library.

#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TableML.Compiler
{

    /// <summary>
    /// Compile Excel to TSV
    /// </summary>
    public class Compiler
    {

        /// <summary>
        /// 编译时，判断格子的类型
        /// </summary>
        public enum CellType
        {
            Value,
            Comment,
            If,
            Endif
        }

        private readonly CompilerConfig _config;

        public Compiler()
            : this(new CompilerConfig()
            {
            })
        {
        }

        public Compiler(CompilerConfig cfg)
        {
            _config = cfg;
        }

        /// <summary>
        /// 生成tml文件内容
        /// </summary>
        /// <returns></returns>
        private TableCompileResult DoCompilerExcelReader(CompilerParam param, ITableSourceFile excelFile)
        {
            var renderVars = new TableCompileResult();
            renderVars.ExcelFile = excelFile;
            renderVars.FieldsInternal = new List<TableColumnVars>();

            var tableBuilder = new StringBuilder();
            var rowBuilder = new StringBuilder();
            //导出lua配置文件
            var luaComentBuilder = new StringBuilder();
            var luaBuilder = new StringBuilder();
            luaComentBuilder.AppendLine(string.Format("---auto generate by tools\r\n---@class {0}",Path.GetFileNameWithoutExtension(param.path)));
            luaBuilder.AppendLine("return {");
            var ignoreColumns = new HashSet<int>();

            #region 写入tml第一行
            // Header Column
            foreach (var colNameStr in excelFile.ColName2Index.Keys)
            {
                if (string.IsNullOrEmpty(colNameStr))
                {
                    continue;
                }
                var colIndex = excelFile.ColName2Index[colNameStr];
                var isCommentColumn = CheckCellType(colNameStr) == CellType.Comment;
                if (isCommentColumn)
                {
                    ignoreColumns.Add(colIndex);
                }
                else
                {
                    //NOTE by qingqing-zhao 分隔符为\t 。如果从指定的列开始读取，但是dict的索引是从0开始
                    if (colIndex > 0)
                    {
                        tableBuilder.Append("\t");
                    }
                    tableBuilder.Append(colNameStr);

                    string typeName = "string";
                    string defaultVal = "";

                    var attrs = excelFile.ColName2Statement[colNameStr]
                        .Split(new char[] {'|', '/'}, StringSplitOptions.RemoveEmptyEntries);
                    // Type
                    if (attrs.Length > 0)
                    {
                        typeName = attrs[0];
                    }
                    // Default Value
                    if (attrs.Length > 1)
                    {
                        defaultVal = attrs[1];
                    }
                    if (attrs.Length > 2)
                    {
                        if (attrs[2] == "pk")
                        {
                            renderVars.PrimaryKey = colNameStr;
                        }
                    }

                    renderVars.FieldsInternal.Add(new TableColumnVars
                    {
                        Index = colIndex - ignoreColumns.Count, // count the comment columns
                        Type = typeName,
                        Name = colNameStr,
                        DefaultValue = defaultVal,
                        Comment = excelFile.ColName2Comment[colNameStr],
                    });
                    luaComentBuilder.AppendLine(string.Format("---@field public {0} {1}",colNameStr,typeName));
                }
            }
            tableBuilder.Append("\n");
            //以上是tml写入的第一行
            #endregion

            #region 写入tml第二行
            // Statements rows, keeps
            foreach (var kv in excelFile.ColName2Statement)
            {
                var statementStr = kv.Value;
                if (string.IsNullOrEmpty(statementStr))
                {
                    continue;
                }
                var colName = kv.Key;
                var colIndex = excelFile.ColName2Index[colName];

                if (ignoreColumns.Contains(colIndex)) // comment column, ignore
                    continue;
                //NOTE by qingqing-zhao 加入\t，从指定的列开始读取，但是dict的索引是从0开始
                if (colIndex > 0)
                {
                    tableBuilder.Append("\t");
                }
                tableBuilder.Append(statementStr);
            }
            tableBuilder.Append("\n");
            //以上是tml写入的第二行
            #endregion
            
            #region 写入tml其它行
            // #if check, 是否正在if false模式, if false时，行被忽略
            var ifCondtioning = true;
            if (param.doRealCompile)
            {
                // 如果不需要真编译，获取头部信息就够了
                // Data Rows
                var rowsCount = excelFile.GetRowsCount();
                var lastRowIdx = rowsCount -1;
                for (var startRow = 0; startRow < rowsCount; startRow++)
                {
                    rowBuilder.Length = 0;
                    rowBuilder.Capacity = 0;
                    var columnCount = excelFile.GetColumnCount();
                    var lastColumnIdx = columnCount -1;
                    if (ignoreColumns.Count > 0)
                    {
                        var temp = new List<int>();
                        //取出最后一列的索引
                        for (var loopColumn = 0; loopColumn < columnCount; loopColumn++)
                        {
                            if (!ignoreColumns.Contains(loopColumn))
                            {
                                temp.Add(loopColumn);
                            }
                        }

                        if (temp.Count > 0) lastColumnIdx = temp[temp.Count-1];
                    }
                    for (var loopColumn = 0; loopColumn < columnCount; loopColumn++)
                    {
                        //读取每一列的内容
                        if (!ignoreColumns.Contains(loopColumn)) // comment column, ignore 注释列忽略
                        {
                            if (excelFile.Index2ColName.ContainsKey(loopColumn) == false)
                            {
                                continue;
                            }
                            var columnName = excelFile.Index2ColName[loopColumn];
                            var cellStr = excelFile.GetString(columnName, startRow);

                            if (loopColumn == 0)
                            {
                                var cellType = CheckCellType(cellStr);
                                if (cellType == CellType.Comment) // 如果行首为#注释字符，忽略这一行)
                                    break;

                                // 进入#if模式
                                if (cellType == CellType.If)
                                {
                                    var ifVars = GetIfVars(cellStr);
                                    var hasAllVars = true;
                                    foreach (var var in ifVars)
                                    {
                                        if (_config.ConditionVars == null ||
                                            !_config.ConditionVars.Contains(var)) // 定义的变量，需要全部配置妥当,否则if失败
                                        {
                                            hasAllVars = false;
                                            break;
                                        }
                                    }
                                    ifCondtioning = hasAllVars;
                                    break;
                                }
                                if (cellType == CellType.Endif)
                                {
                                    ifCondtioning = true;
                                    break;
                                }

                                if (!ifCondtioning) // 这一行被#if 忽略掉了
                                    break;


                                if (startRow != 0) // 不是第一行，往添加换行，首列
                                    rowBuilder.Append("\n");
                                luaBuilder.AppendLine(string.Concat("[",ParseLua(cellStr),"] = {"));
                            }
                            /*
                                NOTE by qingqing-zhao 因为是从指定的列开始读取，所以>有效列 才加入\t
                                如果这列是空白的也不需要加入
                            */
                            bool hasColumn = !string.IsNullOrEmpty(columnName)
                                                 && loopColumn > 0
                                                 && loopColumn < columnCount; //列是否有效
                            if(hasColumn) rowBuilder.Append("\t");
                            // 如果单元格是字符串，换行符改成\\n
                            cellStr = cellStr.Replace("\n", "\\n");
                            rowBuilder.Append(cellStr);
                            //NOTE lua语法如果是字符串则加上"" 
                            luaBuilder.AppendLine(string.Format("\t{0}={1}{2}", columnName, ParseLua(cellStr), loopColumn != lastColumnIdx ? "," : ""));
                        }
                    }
                  
                    // 如果这行，之后\t或换行符，无其它内容，认为是可以省略的
                    if (!string.IsNullOrEmpty(rowBuilder.ToString().Trim()))
                    {
                        tableBuilder.Append(rowBuilder);
                        luaBuilder.AppendLine(string.Concat("}", startRow == lastRowIdx ? "" : ","));
                    }
                }
                luaBuilder.AppendLine("}");
            }
            //以上是tml写入其它行
            #endregion

            var fileName = Path.GetFileNameWithoutExtension(param.path);
            string exportPath;
            if (!string.IsNullOrEmpty(param.compileToFilePath))
            {
                exportPath = param.compileToFilePath;
            }
            else
            {
                // use default
                exportPath = fileName + _config.ExportTabExt;
            }

            var exportDirPath = Path.GetDirectoryName(exportPath);
            if (!Directory.Exists(exportDirPath))
                Directory.CreateDirectory(exportDirPath);
            exportDirPath = Path.GetDirectoryName(param.compileToLuaFilePath);
            if (!Directory.Exists(exportDirPath))
                Directory.CreateDirectory(exportDirPath);
            // 是否写入文件
            if (param.doRealCompile)
            {
                File.WriteAllText(exportPath, tableBuilder.ToString());
                File.WriteAllText(param.compileToLuaFilePath, luaComentBuilder.ToString() + luaBuilder.ToString());
            }


            // 基于base dir路径
            var tabFilePath = exportPath; // without extension
            var fullTabFilePath = Path.GetFullPath(tabFilePath).Replace("\\", "/"); ;
            if (!string.IsNullOrEmpty(param.compileBaseDir))
            {
                var fullCompileBaseDir = Path.GetFullPath(param.compileBaseDir).Replace("\\", "/"); ;
                tabFilePath = fullTabFilePath.Replace(fullCompileBaseDir, ""); // 保留后戳
            }
            if (tabFilePath.StartsWith("/"))
                tabFilePath = tabFilePath.Substring(1);

            renderVars.TabFileFullPath = fullTabFilePath;
            renderVars.TabFileRelativePath = tabFilePath;

            return renderVars;
        }

        /// <summary>
        /// 获取#if A B语法的变量名，返回如A B数组
        /// </summary>
        /// <param name="cellStr"></param>
        /// <returns></returns>
        private string[] GetIfVars(string cellStr)
        {
            return cellStr.Replace("#if", "").Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 检查一个表头名，是否是可忽略的注释
        /// 或检查一个字符串
        /// </summary>
        /// <param name="colNameStr"></param>
        /// <returns></returns>
        private CellType CheckCellType(string colNameStr)
        {
            if (colNameStr.StartsWith("#if"))
                return CellType.If;
            if (colNameStr.StartsWith("#endif"))
                return CellType.Endif;
            foreach (var commentStartsWith in _config.CommentStartsWith)
            {
                if (colNameStr.ToLower().Trim().StartsWith(commentStartsWith.ToLower()))
                {
                    return CellType.Comment;
                }
            }

            return CellType.Value;
        }

        /// <summary>
        /// Compile the specified path, auto change extension to config `ExportTabExt`
        /// </summary>
        /// <param name="path">Path.</param>
        public TableCompileResult Compile(string path)
        {
            var outputPath = System.IO.Path.ChangeExtension(path, this._config.ExportTabExt);
            var param = new CompilerParam(){path = path,compileToFilePath = outputPath};
            return Compile(param);
        }

        /// <summary>
        /// Compile a setting file, return a hash for template
        /// </summary>
        /// <param name="path"></param>
        /// <param name="compileToFilePath"></param>
        /// <param name="compileBaseDir"></param>
        /// <param name="doRealCompile">Real do, or just get the template var?</param>
        /// <returns></returns>
        public TableCompileResult Compile(CompilerParam param)
        {
            // 确保目录存在
            param.compileToFilePath = Path.GetFullPath(param.compileToFilePath);
            var compileToFileDirPath = Path.GetDirectoryName(param.compileToFilePath);

            if (!Directory.Exists(compileToFileDirPath))
                Directory.CreateDirectory(compileToFileDirPath);

            var ext = Path.GetExtension(param.path);

            ITableSourceFile sourceFile = null;
            if (ext == ".tsv")
            {
                sourceFile = new SimpleTSVFile(param.path);
            }
            else if (ext.Contains(".xls"))
            {
                sourceFile = new SimpleExcelFile(param.path, param.index);
            }
            else if (ext == ".csv")
            {
                sourceFile = new SimpleCSVFile(param.path);
            }
            var hash = DoCompilerExcelReader(param, sourceFile);
            return hash;
        }

        #region 解析lua语法

        public string ParseLua(string src)
        {
            if (MyHelper.IsNumber(src))
            {
                return src;
            }
            //TODO 如果有特殊的需求，需要成自定义的语法
            //else if (src.StartsWith("{"))
            return string.Concat("\"", src, "\"");
        }
        #endregion
    }

    public class CompilerParam
    {
        public string path;
        public string compileToFilePath;
        /// <summary>
        /// TODO 未传入lua路径，则不生成lua file
        /// </summary>
        public string compileToLuaFilePath;
        public int index = 0;
        public string compileBaseDir = null;
        /// <summary>
        /// TODO 对于csv/tsv doRealCompile = false,但也要生成lua file
        /// </summary>
        public bool doRealCompile = true;
        
    }

}