using System;
using CommandLine;
using CommandLine.Text;
using TableML.Compiler;
using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using CommandLine.Text;
using NPOI.SS.UserModel;
using TableML;
using TableML.Compiler;

namespace TableCompilerConsole
{
    class Option
    {
        [Option('s', "Src", Required = true,
            HelpText = "Input excel or tsv files directory to be processed.")]
        public string Directory { get; set; }

        [Option('t', "To", Required = true,
            HelpText = "Output compiled files")]
        public string OutputDirectory { get; set; }

        [Option('c', "CodeFile", Required = false,
            HelpText = "code file")]
        public string CodeFilePath { get; set; }

        [Option("TemplateFile", Required = false,
            HelpText = "code generate template file")]
        public string TemplateFilePath { get; set; }


        [Option('v', "verbose", DefaultValue = true,
            HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }

    /// <summary>
    /// 命令行模式编译Excel，参数通过命令行传递
    /// </summary>
    class TableCompilerConsole
    {
        public static void Main(string[] args)
        {
            ParseArgs(args);
            //CompileAllToCSharp();
            //CompileAllToLua();
            //CompileAllToOneCSharp();
        }

        static void ParseArgs(string[] args)
        {
            var options = new Option();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                // Values are available here
                if (options.Verbose) Console.WriteLine("Filename: {0}", options.Directory);

                var batchCompiler = new BatchCompiler();

                string templateString = DefaultTemplate.GenCodeTemplate;
                if (!string.IsNullOrEmpty(options.TemplateFilePath))
                {
                    Console.WriteLine(options.TemplateFilePath);
                    templateString = System.IO.File.ReadAllText(options.TemplateFilePath);
                }

                var genParam = new GenParam(){genCodeFilePath = options.CodeFilePath,genCodeTemplateString = templateString,genCSharpClass = true,forceAll = true};
                batchCompiler.CompileAllToOneFile(options.Directory, options.OutputDirectory,genParam);

                Console.WriteLine("Done!");
            }
        }

        #region 测试编译功能

        /// <summary>
        /// 编译单个excel
        /// </summary>
        public static void CompileOne()
        {
            var startPath = Environment.CurrentDirectory;
            //源excel文件路径
            var srcFile = Path.Combine(startPath, "settingsrc", "Y002 邮件配置表.xlsx");
            //输出tml文件路径
            var OutputDirectory = Path.Combine(startPath, "setting", "Test.tsv");
            //生成的代码路径
            var CodeFilePath = "Code.cs";
            if (File.Exists(srcFile) == false)
            {
                Console.WriteLine("{0} 源文件不存在！", srcFile);
                return;
            }

            Console.WriteLine("当前编译的Excel：{0}", srcFile);
            //代码的重新生成
            var param = new CompilerParam() {path = srcFile, ExportTsvPath = OutputDirectory};
            new Compiler().Compile(param);
            Console.WriteLine("Done!");
        }

        /// <summary>
        /// 编译整个目录的excel，每个表生成一个lua文件，不生成c#和tsv
        /// </summary>
        public static void CompileAllToLua()
        {
            var startPath = Environment.CurrentDirectory;
            Console.WriteLine("当前目录：{0}", startPath);
            //源excel文件路径
            var srcPath = "./SettingSource/";
            //输出tml文件路径
            var exportTsvPath = "./Setting/";
            var exportLuaPath = "./Lua/configs/";

            string settingCodeIgnorePattern = "(I18N/.*)|(StringsTable.*)|(tool/*)|(log/*)|(server/*)|(client/*)";
            var genParam = new GenParam()
            {
                settingCodeIgnorePattern = settingCodeIgnorePattern,
                genCSharpClass = false, genCodeFilePath = null, forceAll = true, ExportLuaPath = exportLuaPath
            };
            var compilerParam = new CompilerParam() {CanExportTsv = false, ExportTsvPath = exportTsvPath, ExportLuaPath = exportLuaPath};
            var results = new BatchCompiler().CompileAll(srcPath, exportTsvPath, genParam, compilerParam);

            Console.WriteLine("Done!");
        }

        /// <summary>
        /// 编译整个目录的excel为c#+tsv
        /// </summary>
        public static void CompileAllToCSharp()
        {
            var startPath = Environment.CurrentDirectory;
            Console.WriteLine("当前目录：{0}", startPath);
            //源excel文件路径
            var srcPath = "./SettingSource/";
            //输出tml文件路径
            var exportTsvPath = "./Setting/";
            var exportCSPath = "./CSharp/";

            string settingCodeIgnorePattern = "(I18N/.*)|(StringsTable.*)|(tool/*)|(log/*)|(server/*)|(client/*)";
            var genParam = new GenParam()
            {
                genCodeTemplateString = DefaultTemplate.GenSingleClassCodeTemplate, settingCodeIgnorePattern = settingCodeIgnorePattern,
                genCSharpClass = true,  genCodeFilePath = exportCSPath, forceAll = true
            };
            var compilerParam = new CompilerParam() {CanExportTsv = true, ExportTsvPath = exportTsvPath, ExportLuaPath = null};
            var results = new BatchCompiler().CompileAll(srcPath, exportTsvPath, genParam, compilerParam);

            Console.WriteLine("Done!");
        }
        
        public static void CompileAllToOneCSharp()
        {
            var startPath = Environment.CurrentDirectory;
            Console.WriteLine("当前目录：{0}", startPath);
            //源excel文件路径
            var srcPath = "./SettingSource/";
            //输出tml文件路径
            var exportTsvPath = "./Setting/";
            var exportCSPath = "./CSharp/";

            string settingCodeIgnorePattern = "(I18N/.*)|(StringsTable.*)|(tool/*)|(log/*)|(server/*)|(client/*)";
            var genParam = new GenParam()
            {
                genCodeTemplateString = DefaultTemplate.GenSingleClassCodeTemplate, settingCodeIgnorePattern = settingCodeIgnorePattern,
                genCSharpClass = true, genCodeFilePath = exportCSPath, forceAll = true
            };
            var compilerParam = new CompilerParam() {CanExportTsv = true, ExportTsvPath = exportTsvPath, ExportLuaPath = null};
            var results = new BatchCompiler().CompileAll(srcPath, exportTsvPath, genParam, compilerParam);

            Console.WriteLine("Done!");
        }
        
        public static void LoadTest()
        {
            var fileContent =
                "id\ttype\texp\thp\tattack\nint\tint\tint\tint\tint\t\n1\t1\t100\t1\t1001\n2\t2\t1000\t1\t1002\n3\t3\t2000\t1\t1003\n4\t4\t3000\t1\t1004\n5\t5\t5000\t1\t1005\n6\t6\t6000\t1\t1006\n7\t7\t7000\t1\t1007\n8\t8\t8000\t1\t1008\n9\t9\t9000\t1\t1009\n10\t10\t10000\t1\t1010\n11\t11\t20000\t1\t1010\n12\t12\t40000\t1\t1010\n13\t13\t50000\t1\t1010\n14\t14\t70000\t1\t1010\n15\t15\t80000\t1\t1010\n16\t16\t90000\t1\t1010\n17\t17\t100000\t1\t1010\n18\t18\t122000\t1\t1010\n19\t19\t136000\t1\t1010\n20\t20\t150000\t1\t1010\n21\t21\t235000\t1\t1010\n22\t22\t320000\t1\t1010\n23\t23\t405000\t1\t1010\n24\t24\t490000\t1\t1010\n25\t25\t575000\t1\t1010\n26\t26\t660000\t1\t1010\n27\t27\t745000\t1\t1010\n28\t28\t830000\t1\t1010\n29\t29\t915000\t1\t1010\n30\t30\t1000000\t1\t1010\n31\t31\t1200000\t1\t1010\n32\t32\t1400000\t1\t1010\n33\t33\t1600000\t1\t1010\n34\t34\t1800000\t1\t1010\n35\t35\t2000000\t1\t1010\n36\t36\t2200000\t1\t1010\n37\t37\t2400000\t1\t1010\n38\t38\t2600000\t1\t1010\n39\t39\t2800000\t1\t1010\n40\t40\t3000000\t1\t1010\n41\t41\t3200000\t1\t1010\n42\t42\t3400000\t1\t1010\n43\t43\t3600000\t1\t1010\n44\t44\t3800000\t1\t1010\n45\t45\t4000000\t1\t1010\n46\t46\t4200000\t1\t1010\n47\t47\t4400000\t1\t1010\n48\t48\t4800000\t1\t1010\n49\t49\t5500000\t1\t1010\n50\t50\t6500000\t1\t1010\n51\t51\t7500000\t1\t1010\n52\t52\t9000000\t1\t1010\n53\t53\t12000000\t1\t1010\n54\t54\t15000000\t1\t1010\n55\t55\t18000000\t1\t1010\n56\t56\t21000000\t1\t1010\n57\t57\t24000000\t1\t1010\n58\t58\t27000000\t1\t1010\n59\t59\t30000000\t1\t1010\n60\t60\t34000000\t1\t1010\n61\t61\t38000000\t1\t1010\n62\t62\t42000000\t1\t1010\n63\t63\t46000000\t1\t1010\n64\t64\t50000000\t1\t1010\n65\t65\t54000000\t1\t1010\n66\t66\t58000000\t1\t1010\n67\t67\t62000000\t1\t1010\n68\t68\t66000000\t1\t1010\n69\t69\t70000000\t1\t1010\n70\t70\t75000000\t1\t1010\n71\t71\t80000000\t1\t1010\n72\t72\t85000000\t1\t1010\n73\t73\t100000000\t1\t1010\n74\t74\t115000000\t1\t1010\n75\t75\t130000000\t1\t1010\n76\t76\t145000000\t1\t1010\n77\t77\t160000000\t1\t1010\n78\t78\t175000000\t1\t1010\n79\t79\t195000000\t1\t1010\n80\t80\t215000000\t1\t1010\n81\t81\t250000000\t1\t1010\n82\t82\t280000000\t1\t1010\n83\t83\t310000000\t1\t1010\n84\t84\t350000000\t1\t1010\n85\t85\t390000000\t1\t1010\n86\t86\t440000000\t1\t1010\n87\t87\t624000000\t1\t1010\n88\t88\t780000000\t1\t1010\n89\t89\t952000000\t1\t1010\n90\t90\t1064000000\t1\t1010\n91\t91\t1848000000\t1\t1010\n92\t92\t1900000000\t1\t1010\n93\t93\t2088000000\t1\t1010\n94\t94\t2208000000\t1\t1010\n95\t95\t2328000000\t1\t1010\n96\t96\t2448000000\t1\t1010\n97\t97\t2568000000\t1\t1010\n98\t98\t2750000000\t1\t1010\n99\t99\t2900000000\t1\t1010\n100\t100\t3300000000\t1\t1010\n101\t101\t3700000000\t1\t1010\n102\t102\t4100000000\t1\t1010\n103\t103\t4500000000\t1\t1010\n104\t104\t4900000000\t1\t1010\n105\t105\t5300000000\t1\t1010\n106\t1\t1\t2\t1002\n107\t2\t2\t2\t1003\n108\t3\t3\t2\t1004\n109\t4\t4\t2\t1005\n110\t5\t5\t2\t1006\n111\t6\t6\t2\t1007\n112\t7\t7\t2\t1008\n113\t8\t8\t2\t1009\n114\t9\t9\t2\t1010\n115\t10\t10\t2\t1010\n116\t11\t11\t2\t1010\n117\t12\t12\t2\t1010\n118\t13\t13\t2\t1010\n119\t14\t14\t2\t1010\n120\t15\t15\t2\t1010\n121\t16\t16\t2\t1010\n122\t17\t17\t2\t1010\n123\t18\t18\t2\t1010\n124\t19\t19\t2\t1010\n125\t20\t20\t2\t1010\n126\t21\t21\t2\t1010\n127\t22\t22\t2\t1010\n128\t23\t23\t2\t1010\n129\t24\t24\t2\t1010\n130\t25\t25\t2\t1010\n131\t26\t26\t2\t1010\n132\t27\t27\t2\t1010\n133\t28\t28\t2\t1010\n134\t29\t29\t2\t1010\n135\t30\t30\t2\t1010\n136\t31\t31\t2\t1010\n137\t32\t32\t2\t1010\n138\t33\t33\t2\t1010\n139\t34\t34\t2\t1010\n140\t35\t35\t2\t1010\n141\t36\t36\t2\t1010\n142\t37\t37\t2\t1010\n143\t38\t38\t2\t1010\n144\t39\t39\t2\t1010\n145\t40\t40\t2\t1010\n146\t41\t41\t2\t1010\n147\t42\t42\t2\t1010\n148\t43\t43\t2\t1010\n149\t44\t44\t2\t1010\n150\t45\t45\t2\t1010\n151\t46\t46\t2\t1010\n152\t47\t47\t2\t1010\n153\t48\t48\t2\t1010\n154\t49\t49\t2\t1010\n155\t50\t50\t2\t1010\n156\t51\t51\t2\t1010\n157\t52\t52\t2\t1010\n158\t53\t53\t2\t1010\n159\t54\t54\t2\t1010\n160\t55\t55\t2\t1010\n161\t56\t56\t2\t1010\n162\t57\t57\t2\t1010\n163\t58\t58\t2\t1010\n164\t59\t59\t2\t1010\n165\t60\t60\t2\t1010\n166\t61\t61\t2\t1010\n167\t62\t62\t2\t1010\n168\t63\t63\t2\t1010\n169\t64\t64\t2\t1010\n170\t65\t65\t2\t1010\n171\t66\t66\t2\t1010\n172\t67\t67\t2\t1010\n173\t68\t68\t2\t1010\n174\t69\t69\t2\t1010\n175\t70\t70\t2\t1010\n176\t71\t71\t2\t1010\n177\t72\t72\t2\t1010\n178\t73\t73\t2\t1010\n179\t74\t74\t2\t1010\n180\t75\t75\t2\t1010\n181\t76\t76\t2\t1010\n182\t77\t77\t2\t1010\n183\t78\t78\t2\t1010\n184\t79\t79\t2\t1010\n185\t80\t80\t2\t1010\n186\t81\t81\t2\t1010\n187\t82\t82\t2\t1010\n188\t83\t83\t2\t1010\n189\t84\t84\t2\t1010\n190\t85\t85\t2\t1010\n191\t86\t86\t2\t1010\n192\t87\t87\t2\t1010\n193\t88\t88\t2\t1010\n194\t89\t89\t2\t1010\n195\t90\t90\t2\t1010\n196\t91\t91\t2\t1010\n197\t92\t92\t2\t1010\n198\t93\t93\t2\t1010\n199\t94\t94\t2\t1010\n200\t95\t95\t2\t1010\n201\t96\t96\t2\t1010\n202\t97\t97\t2\t1010\n203\t98\t98\t2\t1010\n204\t99\t99\t2\t1010\n205\t100\t100\t2\t1010\n206\t101\t101\t2\t1010\n207\t102\t102\t2\t1010\n208\t103\t103\t2\t1010\n209\t104\t104\t2\t1010\n210\t105\t105\t2\t1010";
            var tab = TableFile.LoadFromString(fileContent);
            var aa = tab;
        }

        #endregion
    }
}