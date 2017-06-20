using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TableMLGUI
{
    /// <summary>
    /// 用来存放配置数据
    /// </summary>
    public static class ConfigData
    {
        /// <summary>
        /// 简单三行格式文件的格式
        /// </summary>
        public const string SimpleFileEx = ".tsv";
        /// <summary>
        ///可以插入到sqlite中的格式
        /// </summary>
        public static string[] CanToSqlEx = new string[] { ".tsv","*.tml" };

        public static string StartPath
        {
            get { return Application.StartupPath; }
        }
    }
}
