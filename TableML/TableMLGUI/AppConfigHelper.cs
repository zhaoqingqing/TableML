using System;
using System.Windows.Forms;
using System.Xml;

namespace TableMLGUI
{
    /// <summary>
    /// 提供像xml一样方法修改App.config，因为通过ConfigurationManager修改config文件会丢失注释
    /// by qingqing.zhao (569032731@qq.com)
    /// </summary>
    public static class AppConfigHelper
    {
        //NOTE：使用系统提供的API会重新生成app.config,导致里面的注释丢失，所以使用AppConfigHelper
        /*var newKeyValue =  box.Checked ? "1" : "0";
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        config.AppSettings.Settings["UseSqlite"].Value = newKeyValue;
        config.Save(ConfigurationSaveMode.Modified);
        ConfigurationManager.RefreshSection("appSettings");*/
       public static void SetValue(string key,string value)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(GetAppConfigPath());
            XmlNode node = doc.SelectSingleNode(@"//appSettings");

            var select = node.SelectSingleNode($@"//add[@key='{key}']");
            if (select == null)
            {
                ConsoleHelper.Error($"not find key:{key}");
                return;
            }
            XmlElement ele = (XmlElement)select;
            ele.SetAttribute("value", value);
            doc.Save(GetAppConfigPath());
        }

        public static string GetAppConfigPath()
        {
            //TODO 区分是在vs中运行的，还是独立exe运行环境
            if (false)
            {
                int intPos = Application.StartupPath.Trim().IndexOf("bin") - 1;
                string strDirectoryPath = System.IO.Path.Combine(Application.StartupPath.Substring(0, intPos), "App.config");

                return strDirectoryPath;
            }
            else
            {
                return System.Windows.Forms.Application.ExecutablePath + ".config";
            }
        }

        public static string GetValue(string appKey)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.Load(GetAppConfigPath());
                XmlNode xNode;
                XmlElement xElem;
                xNode = xDoc.SelectSingleNode("//appSettings");　　　　//检查你的app.config 文件中是否包含xml根节点：<appSetting> </appSetting>
                xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='" + appKey + "']");
                if (xElem != null)
                {
                    return xElem.GetAttribute("value");
                }
                else
                {
                    ConsoleHelper.Error($"not find key:{appKey}");
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
