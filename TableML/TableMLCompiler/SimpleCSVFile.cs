using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;

namespace TableML.Compiler
{
    /// <summary>
    /// 解析csv,内容格式和excel一样
    /// csv行数从1开始，列数从0开始
    /// 带有头部、声明、注释
    /// </summary>
    public class SimpleCSVFile : ITableSourceFile
    {
        public Dictionary<string, int> ColName2Index { get; set; }
        public Dictionary<int, string> Index2ColName { get; set; }
        public Dictionary<string, string> ColName2Statement { get; set; } //  string,or something

        public Dictionary<string, string> ColName2Constraints { get; set; } //  string,or something
        public Dictionary<string, string> ColName2Comment { get; set; } // string comment
        public Dictionary<int, string[]> Row2Content { get; set; } // 表的内容，{行，列[]}
        /// <summary>
        /// 原始文件名
        /// </summary>
        public string ExcelFileName { get; set; }

        //NOTE npoi的列数是从0开始,而excel的列数是从1开始
        //by zhaoqingqing 根据特殊的Excel格式定制
        /// <summary>
        /// Header, Statement, Comment, at lease 3 rows
        /// 预留行数
        /// </summary>
        public int RowTypeIndex = 3;
        /// <summary>
        /// 前端字段名
        /// </summary>
        public int RowNameIndex = 4;
        /// <summary>
        /// 前端数据约束
        /// </summary>
        public int RowConstraints = 5;
        /// <summary>
        /// 详细注释
        /// </summary>
        public int RowDetailCommentIndex = 10;
        /// <summary>
        /// 注释
        /// </summary>
        public int RowCommentIndex = 11;
        /// <summary>
        /// 预留行数
        /// </summary>
        public int PreserverRowCount = 12;
        /// <summary>
        /// 输出文件名，定义在第几行
        /// </summary>
        public int outFileNameLine = 1;
        /// <summary>
        /// 从指定列开始读,默认是0
        /// </summary>
        public const int StartColumnIdx = 1;

        public string Path;
        /// <summary>
        /// 输出文件名
        /// </summary>
        public string outFileName = "";
        private CsvReader csvReader;
        public bool IsLoadSuccess = true;
        //总列数
        private int _columnCount;
        //总行数
        public int CurrentLineIndex = 0;

        public SimpleCSVFile(string excelPath)
        {
            Path = excelPath;
            ColName2Index = new Dictionary<string, int>();
            Index2ColName = new Dictionary<int, string>();
            ColName2Statement = new Dictionary<string, string>();
            ColName2Comment = new Dictionary<string, string>();
            ColName2Constraints = new Dictionary<string, string>();
            Row2Content = new Dictionary<int, string[]>();
            ExcelFileName = System.IO.Path.GetFileName(excelPath);
            if (ParseColName(excelPath))
            {
                ParseCSV(excelPath);
            }
        }

        /// <summary>
        /// 先解析字段名，并检查表是否符合规范
        /// </summary>
        /// <param name="filePath"></param>
        private bool ParseColName(string filePath)
        {
            var shouldOutput = 0;
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(fileStream, Encoding.GetEncoding("GB2312")))
                {
                    csvReader = new CsvReader(reader);
                    csvReader = new CsvReader(reader);
                    /**表头结构如下所示：
                    *   Id  Name    CDTime
                    *   int string int
                    *   编号 名称 CD时间
                    */
                    var tmpLine = 0;
                    while (csvReader.Read())
                    {
                        if (tmpLine == outFileNameLine)
                        {
                            //列数
                            _columnCount = csvReader.Context.Record.Length;
                            csvReader.TryGetField<int>(1, out shouldOutput);
                            if (shouldOutput <= 0)
                            {
                                break;
                            }
                            csvReader.TryGetField<string>(2, out outFileName);
                        }

                        if (tmpLine == RowNameIndex)
                        {
                            // 列头名(字段名) 
                            //NOTE by qingqing-zhao 列名：从指定的列开始读取
                            int emptyColumn = 0;
                            var headerRow = csvReader.Context.Record;
                            for (int columnIndex = StartColumnIdx; columnIndex < _columnCount; columnIndex++)
                            {
                                var realIdx = columnIndex - StartColumnIdx;
                                var cell = headerRow[columnIndex];
                                var headerName = cell != null ? cell.ToString().Trim() : ""; // trim!

                                if (string.IsNullOrEmpty(headerName))
                                {
                                    //NOTE 如果列名是空，当作注释处理，因为老表可能已经写了#1，#2
                                    emptyColumn += 1;
                                    headerName = string.Concat("#Comment#", emptyColumn);
                                }
                                ColName2Index[headerName] = realIdx;
                                Index2ColName[realIdx] = headerName;
                            }
                            break;
                        }
                        tmpLine += 1;
                    }
                }
            }
            return shouldOutput > 0 && !string.IsNullOrEmpty(outFileName);
        }

        /// <summary>
        /// Parse csv file to data grid
        /// </summary>
        /// <param name="filePath"></param>
        private void ParseCSV(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(fileStream, Encoding.GetEncoding("GB2312")))
                {
                    csvReader = new CsvReader(reader);
                    string[] commentRow = new string[] { }, commentRowDetail = new string[] { };
                    var hasConstraints = false;
                    while (csvReader.Read())
                    {
                        if (CurrentLineIndex == RowTypeIndex)
                        {
                            // 表头声明(数据类型)
                            var statementRow = csvReader.Context.Record;
                            for (int columnIndex = StartColumnIdx; columnIndex < _columnCount; columnIndex++)
                            {
                                var realIdx = columnIndex - StartColumnIdx;
                                if (Index2ColName.ContainsKey(realIdx) == false)
                                {
                                    continue;
                                }
                                var colName = Index2ColName[realIdx];
                                var statementCell = statementRow[columnIndex];
                                var statementString = statementCell != null ? statementCell.ToString() : "";
                                ColName2Statement[colName] = statementString;
                            }
                        }
                        if (CurrentLineIndex == RowConstraints)
                        {
                            // 数据约束
                            var constraintsRow = csvReader.Context.Record;
                            if (csvReader.GetField<string>(0) == "前端·数据约束")
                            {
                                for (int columnIndex = StartColumnIdx; columnIndex < _columnCount; columnIndex++)
                                {
                                    var realIdx = columnIndex - StartColumnIdx;
                                    if (Index2ColName.ContainsKey(realIdx) == false)
                                    {
                                        continue;
                                    }
                                    var colName = Index2ColName[realIdx];
                                    var statementCell = constraintsRow[columnIndex];
                                    var statementString = statementCell != null ? statementCell.ToString() : "";
                                    ColName2Constraints[colName] = statementString;
                                }
                                hasConstraints = true;
                            }
                            else
                            {
                                RowDetailCommentIndex--;
                                RowCommentIndex--;
                                PreserverRowCount--;
                            }
                        }

                        if (CurrentLineIndex >= PreserverRowCount)
                        {
                            //保存所有的内容
                            Row2Content.Add(CurrentLineIndex, csvReader.Context.Record);
                        }

                        if (CurrentLineIndex >= RowDetailCommentIndex && CurrentLineIndex <= RowCommentIndex)
                        {
                            // 表头注释(字段注释) 我们有两行注释
                            if (CurrentLineIndex == RowDetailCommentIndex)
                            {
                                commentRowDetail = csvReader.Context.Record;
                            }
                            if (CurrentLineIndex == RowCommentIndex)
                            {
                                commentRow = csvReader.Context.Record;
                                ParseComment(commentRow, commentRowDetail);
                            }
                        }
                        CurrentLineIndex = CurrentLineIndex + 1;
                    }
                }
            }
        }

        /// <summary>
        /// 表头注释(字段注释) 我们有两行注释
        /// </summary>
        /// <param name="commentRow"></param>
        /// <param name="commentRowDetail"></param>
        public void ParseComment(string[] commentRow, string[] commentRowDetail)
        {
            if (commentRow == null)
            {
                return;
            }
            if (CurrentLineIndex == RowDetailCommentIndex || CurrentLineIndex == RowCommentIndex)
            {
                for (int columnIndex = StartColumnIdx; columnIndex < _columnCount; columnIndex++)
                {
                    var realIdx = columnIndex - StartColumnIdx;
                    if (Index2ColName.ContainsKey(realIdx) == false)
                    {
                        continue;
                    }
                    var colName = Index2ColName[realIdx];
                    var commentCell = columnIndex < commentRow.Length ? commentRow[columnIndex] : String.Empty;

                    string commentString = string.Empty;
                    if (commentCell != null)
                    {
                        commentString += string.Concat(commentCell, "\n");
                    }
                    if (commentRowDetail != null)
                    {
                        var commentCellDetail = columnIndex < commentRowDetail.Length ? commentRowDetail[columnIndex] : string.Empty;
                        if (!string.IsNullOrEmpty(commentCellDetail))
                        {
                            commentString += commentCellDetail;
                        }
                    }

                    //fix 注释包含\r\n
                    if (commentString.Contains("\n"))
                    {
                        commentString = CombieLine(commentString, "\n");
                    }
                    else if (commentString.Contains("\r\n"))
                    {
                        commentString = CombieLine(commentString, "\r\n");
                    }
                    ColName2Comment[colName] = commentString;
                }
            }
        }

        public string CombieLine(string commentString, string lineStr)
        {
            if (commentString.Contains(lineStr))
            {
                var comments = commentString.Split(new string[] { lineStr }, StringSplitOptions.None);
                StringBuilder sb = new StringBuilder();
                sb.Append(comments[0]);
                for (int idx = 1; idx < comments.Length; idx++)
                {
                    if (string.IsNullOrEmpty(comments[idx])) continue;
                    sb.Append(string.Concat("\r\n", "       ///  ", comments[idx]));
                }
                commentString = sb.ToString();
            }
            return commentString;
        }

        /// <summary>
        /// 是否存在列名
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public bool HasColumn(string columnName)
        {
            return ColName2Index.ContainsKey(columnName);
        }

        /// <summary>
        ///  获取某行的内容数据
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="dataRow">无计算表头的数据行数</param>
        /// <returns></returns>
        public string GetString(string columnName, int dataRow)
        {
            if (csvReader == null) return null;
            dataRow += PreserverRowCount;
            if (Row2Content.ContainsKey(dataRow))
            {
                var theRow = Row2Content[dataRow];
                var colIndex = ColName2Index[columnName] + SimpleCSVFile.StartColumnIdx;
                return theRow[colIndex];
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 不带预留头的数据总行数
        /// </summary>
        /// <returns></returns>
        public int GetRowsCount()
        {
            return CurrentLineIndex - PreserverRowCount;
        }

        /// <summary>
        /// 获取列总数
        /// </summary>
        /// <returns></returns>
        public int GetColumnCount()
        {
            return _columnCount - StartColumnIdx;
        }

        /// <summary>
        /// 读表中的字段获取输出文件名
        /// 做好约定输出tml文件名在指定的单元格，不用遍历整表让解析更快
        /// </summary>
        /// <returns></returns>
        public static string GetOutFileName(string filePath)
        {
            var outFileName = string.Empty;
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(fileStream, Encoding.GetEncoding("GB2312")))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    try
                    {
                        csvReader = new CsvReader(reader);
                        int lineNum = 0;
                        while (csvReader.Read())
                        {
                            lineNum = lineNum + 1;
                            if (lineNum == 2)
                            {
                                var shouldOutput = 0;
                                csvReader.TryGetField<int>(1, out shouldOutput);
                                if (shouldOutput > 0)
                                {
                                    csvReader.TryGetField<string>(2, out outFileName);
                                }
                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ConsoleHelper.Error(string.Format("无法打开csv: {0}, 可能原因：文件正在打开，或占用？{1}", filePath, e.Message));
                        return outFileName;
                    }
                }
            }

            return outFileName;
        }

    }
}