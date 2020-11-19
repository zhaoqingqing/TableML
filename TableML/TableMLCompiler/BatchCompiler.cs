#region Copyright (c) 2015 KEngine / Kelly <http: //github.com/mr-kelly>, All rights reserved.

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
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DotLiquid;
using System.Linq;

namespace TableML.Compiler
{
    /// <summary>
    /// 配置生成CSharp的参数
    /// </summary>
    public class GenParam
    {
        public bool forceAll;
        public bool genCSharpClass;

        public string genCodeTemplateString;

        /// <summary>
        /// 生成的代码保存文件路径
        /// </summary>
        public string genCodeFilePath;

        public string ExportLuaPath;
        public string nameSpace = "AppSettings";
        public string changeExtension = ".tsv";
        public string settingCodeIgnorePattern;

        public TableCompileResult compileResult;

        /// <summary>
        /// 如果是生成Manager Class 一定要在外部初始化此字段
        /// </summary>
        public Dictionary<string, TableTemplateVars> templateVars;
    }

    /// <summary>
    /// 批量编译某个目录下的excel
    /// </summary>
    public partial class BatchCompiler
    {
        /// <summary>
        /// 当生成的类名，包含数组中字符时，不生成代码
        /// </summary>
        /// <example>
        /// GenerateCodeFilesFilter = new []
        /// {
        ///     "SubdirSubSubDirExample3",
        /// };
        /// </example>
        public string[] GenerateCodeFilesFilter = null;

        /// <summary>
        /// 条件编译变量
        /// </summary>
        public string[] CompileSettingConditionVars;

        /// <summary>
        /// 缺省时，默认生成代码存放的路径
        /// </summary>
        public const string DefaultGenCodeDir = "GenCode\\";

        /// <summary>
        /// 管理所有tab表的单例类Manger Class名字
        /// </summary>
        public const string ManagerClassName = "SettingsManager.cs";

        /// <summary>
        /// 生成代码的文件名=表名+后缀+.cs，建议和模版中的一致
        /// </summary>
        public const string FileNameSuffix = "Setting";

        /// <summary>
        /// 可以为模板提供额外生成代码块！返回string即可！
        /// 自定义[InitializeOnLoad]的类并设置这个委托
        /// </summary>
        public CustomExtraStringDelegate CustomExtraString;

        public delegate string CustomExtraStringDelegate(TableCompileResult tableCompileResult);

        /// <summary>
        /// Generate static code from settings
        /// </summary>
        /// <param name="templateString"></param>
        /// <param name="genCodeFilePath"></param>
        /// <param name="nameSpace"></param>
        /// <param name="files"></param>
        void GenerateCode(string templateString, string genCodeFilePath, string nameSpace, List<Hash> files)
        {
            var codeTemplates = new Dictionary<string, string>()
            {
                {templateString, genCodeFilePath},
            };

            foreach (var kv in codeTemplates)
            {
                var templateStr = kv.Key;
                var exportPath = kv.Value;

                // 生成代码
                var template = Template.Parse(templateStr);
                var topHash = new Hash();
                topHash["NameSpace"] = nameSpace;
                topHash["Files"] = files;

                if (!string.IsNullOrEmpty(exportPath))
                {
                    var genCode = template.Render(topHash);
                    if (File.Exists(exportPath)) // 存在，比较是否相同
                    {
                        if (File.ReadAllText(exportPath) != genCode)
                        {
                            //EditorUtility.ClearProgressBar();
                            // 不同，会触发编译，强制停止Unity后再继续写入
                            //if (EditorApplication.isPlaying)
                            {
                                //Console.WriteLine("[CAUTION]AppSettings code modified! Force stop Unity playing");
                                //EditorApplication.isPlaying = false;
                            }
                            File.WriteAllText(exportPath, genCode, Encoding.UTF8);
                            Console.WriteLine("{0} update code file complete", exportPath);
                        }
                    }
                    else
                    {
                        //判断目录是否存在
                        var exportDir = Path.GetDirectoryName(exportPath);
                        if (!string.IsNullOrEmpty(exportDir) && Directory.Exists(exportDir) == false)
                        {
                            Directory.CreateDirectory(exportDir);
                        }

                        File.WriteAllText(exportPath, genCode, Encoding.UTF8);
                        Console.WriteLine("{0} code file gen complete", exportPath);
                    }
                }
            }

            // make unity compile
            //AssetDatabase.Refresh();
        }

        #region 编译指定目录下的excel

        /// <summary>
        /// 处理文件名，符合微软的C#命名风格
        /// copy from TableTemplateVars.DefaultClassNameParse
        /// </summary>
        /// <param name="tabFilePath"></param>
        /// <returns></returns>
        public string ParseClassName(string tabFilePath)
        {
            // 未处理路径的类名, 去掉后缀扩展名
            var classNameOrigin = Path.ChangeExtension(tabFilePath, null);

            // 子目录合并，首字母大写, 组成class name
            var className = classNameOrigin.Replace("/", "_").Replace("\\", "_");
            className = className.Replace(" ", "");
            className = string.Join("", (from name
                        in className.Split(new[] {'_'}, StringSplitOptions.RemoveEmptyEntries)
                    select (name[0].ToString().ToUpper() + name.Substring(1, name.Length - 1)))
                .ToArray());

            // 去掉+或#号后面的字符
            var plusSignIndex = className.IndexOf("+");
            className = className.Substring(0, plusSignIndex == -1 ? className.Length : plusSignIndex);
            plusSignIndex = className.IndexOf("#");
            className = className.Substring(0, plusSignIndex == -1 ? className.Length : plusSignIndex);

            return className;
        }

        /// <summary>
        /// 生成代码文件
        /// </summary>
        public void BeforeGenCodeFile(GenParam param)
        {
            if (param.compileResult == null)
            {
                ConsoleHelper.Error("GenCodeFile faild . compileResult is null.");
                return;
            }

            // 根据编译结果，构建vars，同class名字的，进行合并
            if (param.templateVars == null)
            {
                param.templateVars = new Dictionary<string, TableTemplateVars>();
            }

            if (!string.IsNullOrEmpty(param.settingCodeIgnorePattern))
            {
                var ignoreRegex = new Regex(param.settingCodeIgnorePattern);
                if (ignoreRegex.IsMatch(param.compileResult.TabFileRelativePath))
                    return; // ignore this 
            }

            var customExtraStr = CustomExtraString != null ? CustomExtraString(param.compileResult) : null;
            var templateVar = new TableTemplateVars(param.compileResult, customExtraStr);

            // 尝试类过滤
            var ignoreThisClassName = false;
            if (GenerateCodeFilesFilter != null)
            {
                for (var i = 0; i < GenerateCodeFilesFilter.Length; i++)
                {
                    var filterClass = GenerateCodeFilesFilter[i];
                    if (templateVar.ClassName.Contains(filterClass))
                    {
                        ignoreThisClassName = true;
                        break;
                    }
                }
            }

            if (!ignoreThisClassName)
            {
                if (!param.templateVars.ContainsKey(templateVar.ClassName))
                {
                    param.templateVars.Add(templateVar.ClassName, templateVar);
                }
                else
                {
                    param.templateVars[templateVar.ClassName].RelativePaths.Add(param.compileResult.TabFileRelativePath);
                }
            }

            //首字母大写，符合微软命名规范
            var newFileName = string.Concat(ParseClassName(param.compileResult.TabFileRelativePath), FileNameSuffix, ".cs");
            if (string.IsNullOrEmpty(param.genCodeFilePath))
            {
                param.genCodeFilePath += string.Concat(DefaultGenCodeDir, newFileName);
            }
            else
            {
                param.genCodeFilePath += newFileName;
            }


            // 整合成字符串模版使用的List
            var templateHashes = new List<Hash>();
            foreach (var kv in param.templateVars)
            {
                //NOTE render 加多一项TabFilName
                var templateVar2 = kv.Value;
                var renderTemplateHash = Hash.FromAnonymousObject(templateVar2);
                templateHashes.Add(renderTemplateHash);
            }

            if (param.forceAll)
            {
                // force 才进行代码编译
                GenerateCode(param.genCodeTemplateString, param.genCodeFilePath, param.nameSpace, templateHashes);
            }
        }

        void GenManagerClass(List<TableCompileResult> results, GenParam param)
        {
            var nameSpace = !string.IsNullOrEmpty(param.nameSpace) ? param.nameSpace : "AppSettings";
            var exportPath = string.Concat(param.genCodeFilePath, ManagerClassName);
            if (string.IsNullOrEmpty(exportPath))
            {
                ConsoleHelper.Error("ManagerClass ExportPath is null!");
                return;
            }

            // 生成代码
            var template = Template.Parse(DefaultTemplate.GenManagerCodeTemplate);
            var topHash = new Hash();
            topHash["NameSpace"] = nameSpace;
            var allFiles = Directory.GetFiles(param.genCodeFilePath, "*.cs", SearchOption.AllDirectories);
            var builder = new StringBuilder();
            var lastIdx = allFiles.Length - 1;
            for (int i = 0; i < allFiles.Length; i++)
            {
                var t = Path.GetFileNameWithoutExtension(allFiles[i]);
                if (ManagerClassName.Contains(t))
                    continue;
                var className = string.Concat(i != 0 ? "\t\t\t\t\t\t" : "\t", t, "._instance", i != lastIdx ? " ," : "");
                builder.AppendLine(className);
            }

            topHash["ClassNames"] = builder.ToString();
            
            var genCode = template.Render(topHash);
            if (File.Exists(exportPath)) // 存在，比较是否相同
            {
                if (File.ReadAllText(exportPath) != genCode)
                {
                    File.WriteAllText(exportPath, genCode, Encoding.UTF8);
                    Console.WriteLine("{0} update code file complete", exportPath);
                }
            }
            else
            {
                //判断目录是否存在
                var exportDir = Path.GetDirectoryName(exportPath);
                if (!string.IsNullOrEmpty(exportDir) && Directory.Exists(exportDir) == false)
                {
                    Directory.CreateDirectory(exportDir);
                }

                File.WriteAllText(exportPath, genCode, Encoding.UTF8);
                Console.WriteLine("{0} code file gen complete", exportPath);
            }
        }


        /// <summary>
        /// 编译所有的文件，并且每个文件生成一个代码文件
        /// Compile one directory 's all settings, and return behaivour results
        /// </summary>
        /// <param name="sourcePath">需要的编译的Excel路径</param>
        /// <param name="baseDir">编译后的tml存放路径</param>
        /// <param name="genParam">生成代码的参数</param>
        /// <param name="compilerParam">编译单个文件的参数</param>
        /// <returns></returns>
        public List<TableCompileResult> CompileAll(string sourcePath, string baseDir, GenParam genParam, CompilerParam compilerParam)
        {
            var results = new List<TableCompileResult>();
            var compileBaseDir = baseDir;
            var exportToPath = genParam.genCodeFilePath;
            // excel compiler
            var compiler = new Compiler(new CompilerConfig() {ConditionVars = CompileSettingConditionVars});

            var excelExt = new HashSet<string>() {".xls", ".xlsx", ".tsv", "*.csv"};
            var copyExt = new HashSet<string>() {".txt"};
            if (Directory.Exists(sourcePath) == false)
            {
                Console.WriteLine("Error! {0} 路径不存在！", sourcePath);
                return results;
            }

            var findDir = sourcePath;
            try
            {
                Dictionary<string, string> dst2src = new Dictionary<string, string>();
                var allFiles = Directory.GetFiles(findDir, "*.*", SearchOption.AllDirectories);
                var nowFileIndex = -1; // 开头+1， 起始为0
                foreach (var excelPath in allFiles)
                {
                    //清空上一次的值 
                    genParam.genCodeFilePath = exportToPath;
                    nowFileIndex++;
                    var ext = Path.GetExtension(excelPath);
                    var fileName = Path.GetFileNameWithoutExtension(excelPath);

                    var relativePath = excelPath.Replace(findDir, "").Replace("\\", "/");
                    if (relativePath.StartsWith("/"))
                        relativePath = relativePath.Substring(1);
                    if (excelExt.Contains(ext) && !fileName.StartsWith("~")) // ~开头为excel临时文件，不要读
                    {
                        //NOTE 开始编译Excel 成 tsv文件， 每编译一个Excel就生成一个代码文件
                        //NOTE 设置编译后文件的文件名(tsv文件名)
                        if (ext == ".tsv")
                        {
                            relativePath = Path.GetFileName(excelPath);
                        }
                        else if (ext == "*.csv")
                        {
                            relativePath = SimpleCSVFile.GetOutFileName(excelPath);
                        }
                        else
                        {
                            relativePath = SimpleExcelFile.GetOutFileName(excelPath);
                        }

                        if (string.IsNullOrEmpty(relativePath))
                        {
                            ConsoleHelper.Error("{0} 输出文件名为空，跳过", fileName);
                            continue;
                        }

                        var compileToPath = string.Format("{0}/{1}", compileBaseDir,
                            Path.ChangeExtension(relativePath, genParam.changeExtension));
                        var srcFileInfo = new FileInfo(excelPath);
                        var dstFileName = Path.GetFileNameWithoutExtension(compileToPath);
                        dst2src[dstFileName] = Path.GetFileName(excelPath);
                        Console.WriteLine("Compiling Excel to Tab..." + string.Format("{0} -> {1}", excelPath, compileToPath));

                        // 如果已经存在，判断修改时间是否一致，用此来判断是否无需compile，节省时间
                        bool doCompile = true;
                        if (File.Exists(compileToPath))
                        {
                            var toFileInfo = new FileInfo(compileToPath);
                            if (!genParam.forceAll && srcFileInfo.LastWriteTime == toFileInfo.LastWriteTime)
                            {
                                //Log.DoLog("Pass!SameTime! From {0} to {1}", excelPath, compileToPath);
                                doCompile = false;
                            }
                        }

                        if (doCompile)
                        {
                            Console.WriteLine("[SettingModule]Compile from {0} to {1}", excelPath, compileToPath);
                            Console.WriteLine(); //打印空白行，美观一下 
                            //lua文件保存路径
                            var exportLuaPath = genParam.ExportLuaPath + Path.GetFileNameWithoutExtension(excelPath) + ".lua";
                            //填充部分值
                            if (compilerParam == null) compilerParam = new CompilerParam();
                            compilerParam.path = excelPath;
                            if (!string.IsNullOrEmpty(compilerParam.ExportLuaPath)) compilerParam.ExportLuaPath = exportLuaPath;
                            compilerParam.ExportTsvPath = compileToPath;
                            compilerParam.compileBaseDir = compileBaseDir;
                            compilerParam.doRealCompile = doCompile;

                            var compileResult = compiler.Compile(compilerParam);
                            if (genParam.genCSharpClass)
                            {
                                // 添加模板值
                                results.Add(compileResult);

                                var compiledFileInfo = new FileInfo(compileToPath);
                                compiledFileInfo.LastWriteTime = srcFileInfo.LastWriteTime;
                                //仅仅是生成单个Class，只需要当前的CompileResult
                                genParam.compileResult = compileResult;
                                BeforeGenCodeFile(genParam);
                            }
                        }
                    }
                    else if (copyExt.Contains(ext)) // .txt file, just copy
                    {
                        // just copy the files with these ext
                        var compileToPath = string.Format("{0}/{1}", compileBaseDir,
                            relativePath);
                        var compileToDir = Path.GetDirectoryName(compileToPath);
                        if (!Directory.Exists(compileToDir))
                            Directory.CreateDirectory(compileToDir);
                        File.Copy(excelPath, compileToPath, true);

                        Console.WriteLine("Copy File ..." + string.Format("{0} -> {1}", excelPath, compileToPath));
                    }
                }

                if (genParam.genCSharpClass)
                {
                    //把其它settings填充到Manager class
                    GenManagerClass(results, genParam);
                }

                SaveCompileResult(dst2src);
            }
            finally
            {
                //EditorUtility.ClearProgressBar();
            }

            return results;
        }

        /// <summary>
        /// NOTE 目前我们的源始excel文件后和编译后的不一样，把结果输出到文件作个记录
        /// </summary>
        /// <param name="dst2Src"></param>
        public static void SaveCompileResult(Dictionary<string, string> dst2Src)
        {
            if (dst2Src == null)
            {
                return;
            }

            //获取exe所在的路径
            var startPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            var savePath = startPath + "/" + "compile_result.csv";
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }

            using (var sw = File.CreateText(savePath))
            {
                sw.WriteLine("[dst]目标表名,[src]源始Excel文件名");
                foreach (KeyValuePair<string, string> kv in dst2Src)
                {
                    sw.WriteLine("{0},{1}", kv.Key, kv.Value);
                }
            }

            ConsoleHelper.InfoWithNewLine("共编译{0}表，编译结果保存在：{1}", dst2Src.Count, savePath);
        }

        #endregion

        #region 编译指定目录下的excel且全部代码编译到一个cs文件中

        /// <summary>
        /// 编译全部文件，并把代码放到一个cs中，后续不维护此特性
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="compilePath"></param>
        /// <returns></returns>
        public List<TableCompileResult> CompileAllToOneFile(string sourcePath, string compilePath, GenParam genParam)
        {
            var results = new List<TableCompileResult>();
            var compileBaseDir = compilePath;
            // excel compiler
            var compiler = new Compiler(new CompilerConfig() {ConditionVars = CompileSettingConditionVars});

            var excelExt = new HashSet<string>() {".xls", ".xlsx", ".tsv"};
            var copyExt = new HashSet<string>() {".txt"};
            if (Directory.Exists(sourcePath) == false)
            {
                Console.WriteLine("Error! {0} 路径不存在！{0}", sourcePath);
                return results;
            }

            var findDir = sourcePath;
            try
            {
                var allFiles = Directory.GetFiles(findDir, "*.*", SearchOption.AllDirectories);

                var nowFileIndex = -1; // 开头+1， 起始为0
                foreach (var excelPath in allFiles)
                {
                    nowFileIndex++;
                    var ext = Path.GetExtension(excelPath);
                    var fileName = Path.GetFileNameWithoutExtension(excelPath);

                    var relativePath = excelPath.Replace(findDir, "").Replace("\\", "/");
                    if (relativePath.StartsWith("/"))
                        relativePath = relativePath.Substring(1);
                    if (excelExt.Contains(ext) && !fileName.StartsWith("~")) // ~开头为excel临时文件，不要读
                    {
                        // it's an excel file

                        var compileToPath = string.Format("{0}/{1}", compileBaseDir,
                            Path.ChangeExtension(relativePath, genParam.changeExtension));
                        var srcFileInfo = new FileInfo(excelPath);

                        Console.WriteLine("Compiling Excel to Tab..." + string.Format("{0} -> {1}", excelPath, compileToPath));

                        // 如果已经存在，判断修改时间是否一致，用此来判断是否无需compile，节省时间
                        bool doCompile = true;
                        if (File.Exists(compileToPath))
                        {
                            var toFileInfo = new FileInfo(compileToPath);

                            if (!genParam.forceAll && srcFileInfo.LastWriteTime == toFileInfo.LastWriteTime)
                            {
                                //Log.DoLog("Pass!SameTime! From {0} to {1}", excelPath, compileToPath);
                                doCompile = false;
                            }
                        }

                        if (doCompile)
                        {
                            Console.WriteLine("[SettingModule]Compile from {0} to {1}", excelPath, compileToPath);
                            Console.WriteLine(); //美观一下 打印空白行 //TODO lua导出路径
                            var param = new CompilerParam()
                                {path = excelPath, ExportTsvPath = compileToPath, index = 0, compileBaseDir = compileBaseDir, doRealCompile = doCompile};
                            var compileResult = compiler.Compile(param);

                            // 添加模板值
                            results.Add(compileResult);

                            var compiledFileInfo = new FileInfo(compileToPath);
                            compiledFileInfo.LastWriteTime = srcFileInfo.LastWriteTime;
                        }
                    }
                    else if (copyExt.Contains(ext)) // .txt file, just copy
                    {
                        // just copy the files with these ext
                        var compileToPath = string.Format("{0}/{1}", compileBaseDir,
                            relativePath);
                        var compileToDir = Path.GetDirectoryName(compileToPath);
                        if (!Directory.Exists(compileToDir))
                            Directory.CreateDirectory(compileToDir);
                        File.Copy(excelPath, compileToPath, true);

                        Console.WriteLine("Copy File ..." + string.Format("{0} -> {1}", excelPath, compileToPath));
                    }
                }

                // 根据编译结果，构建vars，同class名字的，进行合并
                var templateVars = new Dictionary<string, TableTemplateVars>();
                foreach (var compileResult in results)
                {
                    if (!string.IsNullOrEmpty(genParam.settingCodeIgnorePattern))
                    {
                        var ignoreRegex = new Regex(genParam.settingCodeIgnorePattern);
                        if (ignoreRegex.IsMatch(compileResult.TabFileRelativePath))
                            continue; // ignore this 
                    }

                    var customExtraStr = CustomExtraString != null ? CustomExtraString(compileResult) : null;

                    var templateVar = new TableTemplateVars(compileResult, customExtraStr);

                    // 尝试类过滤
                    var ignoreThisClassName = false;
                    if (GenerateCodeFilesFilter != null)
                    {
                        for (var i = 0; i < GenerateCodeFilesFilter.Length; i++)
                        {
                            var filterClass = GenerateCodeFilesFilter[i];
                            if (templateVar.ClassName.Contains(filterClass))
                            {
                                ignoreThisClassName = true;
                                break;
                            }
                        }
                    }

                    if (!ignoreThisClassName)
                    {
                        if (!templateVars.ContainsKey(templateVar.ClassName))
                            templateVars.Add(templateVar.ClassName, templateVar);
                        else
                        {
                            templateVars[templateVar.ClassName].RelativePaths.Add(compileResult.TabFileRelativePath);
                        }
                    }
                }

                // 整合成字符串模版使用的List
                var templateHashes = new List<Hash>();
                foreach (var kv in templateVars)
                {
                    var templateVar = kv.Value;
                    var renderTemplateHash = Hash.FromAnonymousObject(templateVar);
                    templateHashes.Add(renderTemplateHash);
                }


                if (genParam.forceAll)
                {
                    // force 才进行代码编译
                    GenerateCode(genParam.genCodeTemplateString, genParam.genCodeFilePath, genParam.nameSpace, templateHashes);
                }
            }
            finally
            {
                //EditorUtility.ClearProgressBar();
            }

            return results;
        }

        #endregion
    }
}