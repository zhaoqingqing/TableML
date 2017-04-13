using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using CommandLine.Text;
using NPOI.SS.UserModel;
using TableML.Compiler;

namespace TableCompilerConsole
{
    /// <summary>
    /// 调试代码
    /// </summary>
    class LocalDebug
    {
        public static void Main(string[] args)
        {
            //CompileOne();
            
            //UpdateTableAll();

            CompileAll();
        }

        /// <summary>
        /// 编译单个excel
        /// </summary>
        public static void CompileOne()
        {
            var startPath = Environment.CurrentDirectory;
            //源excel文件路径
            var srcFile = Path.Combine(startPath, "settingsrc", "Test.xlsx");
            //输出tml文件路径
            var OutputDirectory = Path.Combine(startPath, "setting", "Test.tml");
            //生成的代码路径
            var CodeFilePath = "Code.cs";
            if (File.Exists(srcFile) == false)
            {
                Console.WriteLine("{0} 源文件不存在！", srcFile);
                return;
            }
            Console.WriteLine("当前编译的Excel：{0}", srcFile);
            //TODO 代码的重新生成
            Compiler compiler = new Compiler();
            compiler.Compile(srcFile, OutputDirectory);


            Console.WriteLine("Done!");
        }

        /// <summary>
        /// 编译整个目录的excel，每个表生成一个cs文件
        /// </summary>
        public static void CompileAll()
        {
            var startPath = Environment.CurrentDirectory;
            Console.WriteLine("当前目录：{0}", startPath);
            //源excel文件路径
            //var srcDirectory = "settingsrc";
            var srcDirectory = "ConfigTable";
            //输出tml文件路径
            var OutputDirectory = "setting";
            //生成的代码路径
            var CodeFilePath = "GenCode\\";
            string settingCodeIgnorePattern = "(I18N/.*)|(StringsTable.*)|(tool/*)|(log/*)|(server/*)|(client/*)";
            var batchCompiler = new BatchCompiler();

            string templateString = DefaultTemplate.GenSingleClassCodeTemplate;

            var results = batchCompiler.CompileTableMLAllInSingleFile(srcDirectory, OutputDirectory, CodeFilePath,
               templateString, "AppSettings", ".k",settingCodeIgnorePattern, !string.IsNullOrEmpty(CodeFilePath));

            Console.WriteLine("Done!");
        }

        public static void UpdateTableAll()
        {
            var findPath = System.Environment.CurrentDirectory + "\\ConfigTable";
            var files = Directory.GetFiles(findPath);
            Console.WriteLine("开始更新{0}张Excel表",files.Length);
            foreach (string file in files)
            {
                UpdateCSharpSinpt(file);
            }
            Console.WriteLine("更新Excel表完成。");
        }
		
		//替换成C#语法
        public static void UpdateCSharpSinpt(string filePath)
        {
            IWorkbook Workbook;
            ISheet Worksheet;
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) // no isolation
            {
                try
                {
                    Workbook = WorkbookFactory.Create(file);
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("无法打开Excel: {0}, 可能原因：正在打开？或是Office2007格式（尝试另存为）？ {1}", filePath,
                        e.Message));
                    //IsLoadSuccess = false;
                }
            }
            Worksheet = Workbook.GetSheetAt(0);
            List<ICell> cells = Worksheet.GetRow(4).Cells;
            foreach (ICell cell in cells)
            {
                if (cell.StringCellValue == "num")
                {
                    cell.SetCellValue("int");
                }

                if (cell.StringCellValue == "str")
                {
                    cell.SetCellValue("string");
                }
                if (cell.StringCellValue.StartsWith("arr"))
                {
                    cell.SetCellValue("string");
                }
                if (cell.StringCellValue.StartsWith("ssg"))
                {
                    cell.SetCellValue("string");
                }
            }

            using (var memStream = new MemoryStream())
            {
                Workbook.Write(memStream);
                memStream.Flush();
                memStream.Position = 0;

                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    var data = memStream.ToArray();
                    fileStream.Write(data, 0, data.Length);
                    fileStream.Flush();
                }
            }
        }
    }
}
