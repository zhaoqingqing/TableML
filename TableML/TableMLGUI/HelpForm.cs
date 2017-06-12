using System.Configuration;
using System.Windows.Forms;

namespace TableMLGUI
{
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
            webBrowser.ScriptErrorsSuppressed = true;
            webBrowser.Navigate(helpUrl);
        }
    }
}
