using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace TableMLGUI
{

    /// <summary>
    /// 程序的启动类型
    /// </summary>
    public enum StartType
    {
        GUI,
        CompileMulti,

        CompileAll
    }
    static class Program
    {
        public static StartType type = StartType.GUI;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args != null && args.Length >= 1)
            {
                //NOTE 经测试Hide和Visible都无法实现完全不显示GUI
                string templete = "TableMLGUI以 {0} 模式启动编译";
                if (args[0].Trim().ToLower().Contains("all"))
                {
                    type = StartType.CompileAll;
                    Console.WriteLine(templete, "编译全部");
                    MainForm mainForm = new MainForm();
                    mainForm.CMDCompile();
                    Application.Run(mainForm);
                }
                else
                {
                    //NOTE BAT传递过来的参数可能有换行！
                    StringBuilder tBuilder = new StringBuilder();
                    StringBuilder argBuilder = new StringBuilder();
                    foreach (string arg in args)
                    {
                        if (string.IsNullOrEmpty(arg)) continue;
                        tBuilder.AppendLine(arg);
                        var nArg = arg.Replace("\r\n", "").Replace("\n", "");
                        argBuilder.Append(nArg);
                    }
                    Console.WriteLine("命令行参数长度：{0}", args.Length);
                    Console.WriteLine("内容：{0}", tBuilder.ToString());
                    Console.WriteLine("处理后的要编译文件列表：");
                    var files = argBuilder.ToString().Split('\"');
                    foreach (string file in files)
                    {
                        Console.WriteLine(file);
                    }
                    type = StartType.CompileMulti;
                    ConsoleHelper.ConfirmationWithBlankLine(templete, "编译指定的");

                    MainForm mainForm = new MainForm();
                    mainForm.CompileSelect(files);
                    Application.Run(mainForm);
                }
            }
            else
            {
                Application.Run(new MainForm());
            }
        }
    }
}
