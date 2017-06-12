using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace TableMLGUI
{
    /// <summary>
    /// 如果加载慢，可以试试：https://stackoverflow.com/questions/3086063/winform-webbrowser-control-slow-to-load-best-method-for-pre-loading
    /// </summary>
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
            DoStart();
        }

        public void DoStart()
        {
            string helpUrl = ConfigurationManager.AppSettings.Get("HelpUrl").Trim();
            webBrowser.Navigated += WebBrowser_Navigated;
            webBrowser.ScriptErrorsSuppressed = true;
            webBrowser.Navigate(helpUrl);
            //            ConsoleHelper.WriteLine("nav start");
        }

        private void WebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            //            ConsoleHelper.WriteLine("nav end");
            //此方法不能滚动到指定位置
            //            webBrowser.AutoScrollOffset = new Point(0, 300);
        }
    }
}
