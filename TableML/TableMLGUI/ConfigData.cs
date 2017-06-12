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
        /// excel编译后的文件格式
        /// </summary>
        public const string TmlExtension = ".tsv";

        public static string StartPath
        {
            get { return Application.StartupPath; }
        }
    }
}
