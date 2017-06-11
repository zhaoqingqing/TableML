using System;
using System.Windows.Forms;

namespace TableMLGUI
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool cmdModle = false;
            if (args != null && args.Length >= 1)
            {
                ConsoleHelper.Confirmation("TableMLGUI以命令行模式启动编译。");
                cmdModle = true;
            }
            if (cmdModle)
            {
                //NOTE 经测试Hide和Visible都无法实现完全不显示GUI
                MainForm mainForm = new MainForm();
                mainForm.Hide();
                mainForm.CMDCompile();
                Application.Run(mainForm);
                //mainForm.Visible = false;
            }
            else
            {
                Application.Run(new MainForm());
            }
        }
    }
}
