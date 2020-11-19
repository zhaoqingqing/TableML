
namespace TableML.Compiler
{

    /// <summary>
    /// Default template, for Unity + KEngine
    /// </summary>
    public class DefaultTemplate
    {
        #region LicenseStr
        
        public static string LicenseStr = @"#region Copyright (c) 2015 KEngine / Kelly <http://github.com/mr-kelly>, All rights reserved.

// KEngine - Asset Bundle framework for Unity3D
// ===================================
// 
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
";
        #endregion

        #region 全部表生成到一个cs中
        
        public static string GenCodeTemplate = string.Concat(LicenseStr, @"
// This file is auto generated by SettingModuleEditor.cs!
// Don't manipulate me!
// Default Template for KEngine!

using System.Collections;
using System.Collections.Generic;
using KEngine;
using KEngine.Modules;
using TableML;
namespace {{ NameSpace }}
{
	/// <summary>
    /// All settings list here, so you can reload all settings manully from the list.
	/// </summary>
    public partial class SettingsManager
    {
        private static IReloadableSettings[] _settingsList;
        public static IReloadableSettings[] SettingsList
        {
            get
            {
                if (_settingsList == null)
                {
                    _settingsList = new IReloadableSettings[]
                    { {% for file in Files %}
                        {{ file.ClassName }}Settings._instance,{% endfor %}
                    };
                }
                return _settingsList;
            }
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem(""KEngine/Settings/Try Reload All Settings Code"")]
#endif
	    public static void AllSettingsReload()
	    {
	        for (var i = 0; i < SettingsList.Length; i++)
	        {
	            var settings = SettingsList[i];
                if (settings.Count > 0 // if never reload, ignore
#if UNITY_EDITOR
                    || !UnityEditor.EditorApplication.isPlaying // in editor and not playing, force load!
#endif
                    )
                {
                    settings.ReloadAll();
                }

	        }
	    }

    }

{% for file in Files %}
	/// <summary>
	/// Auto Generate for Tab File: {{ file.TabFilePaths }}
    /// No use of generic and reflection, for better performance,  less IL code generating
	/// </summary>>
    public partial class {{file.ClassName}}Settings : IReloadableSettings
    {
        /// <summary>
        /// How many reload function load?
        /// </summary>>
        public static int ReloadCount { get; private set; }

		public static readonly string[] TabFilePaths = 
        {
            {{ file.TabFilePaths }}
        };
        internal static {{file.ClassName}}Settings _instance = new {{file.ClassName}}Settings();
        Dictionary<{{ file.PrimaryKeyField.FormatType }}, {{file.ClassName}}Setting> _dict = new Dictionary<{{ file.PrimaryKeyField.FormatType }}, {{file.ClassName}}Setting>();

        /// <summary>
        /// Trigger delegate when reload the Settings
        /// </summary>>
	    public static System.Action OnReload;

        /// <summary>
        /// Constructor, just reload(init)
        /// When Unity Editor mode, will watch the file modification and auto reload
        /// </summary>
	    private {{file.ClassName}}Settings()
	    {
        }

        /// <summary>
        /// Get the singleton
        /// </summary>
        /// <returns></returns>
	    public static {{file.ClassName}}Settings GetInstance()
	    {
            if (ReloadCount == 0)
            {
                _instance._ReloadAll(true);
    #if UNITY_EDITOR
                if (SettingModule.IsFileSystemMode)
                {
                    for (var j = 0; j < TabFilePaths.Length; j++)
                    {
                        var tabFilePath = TabFilePaths[j];
                        SettingModule.WatchSetting(tabFilePath, (path) =>
                        {
                            if (path.Replace(""\\"", ""/"").EndsWith(path))
                            {
                                _instance.ReloadAll();
                                Log.LogConsole_MultiThread(""File Watcher! Reload success! -> "" + path);
                            }
                        });
                    }

                }
    #endif
            }

	        return _instance;
	    }
        
        public int Count
        {
            get
            {
                return _dict.Count;
            }
        }

        /// <summary>
        /// Do reload the setting file: {{ file.ClassName }}, no exception when duplicate primary key
        /// </summary>
        public void ReloadAll()
        {
            _ReloadAll(false);
        }

        /// <summary>
        /// Do reload the setting class : {{ file.ClassName }}, no exception when duplicate primary key, use custom string content
        /// </summary>
        public void ReloadAllWithString(string context)
        {
            _ReloadAll(false, context);
        }

        /// <summary>
        /// Do reload the setting file: {{ file.ClassName }}
        /// </summary>
	    void _ReloadAll(bool throwWhenDuplicatePrimaryKey, string customContent = null)
        {
            for (var j = 0; j < TabFilePaths.Length; j++)
            {
                var tabFilePath = TabFilePaths[j];
                TableFile tableFile;
                if (customContent == null)
                    tableFile = SettingModule.Get(tabFilePath, false);
                else
                    tableFile = TableFile.LoadFromString(customContent);

                using (tableFile)
                {
                    foreach (var row in tableFile)
                    {
                        var pk = {{ file.ClassName }}Setting.ParsePrimaryKey(row);
                        {{file.ClassName}}Setting setting;
                        if (!_dict.TryGetValue(pk, out setting))
                        {
                            setting = new {{file.ClassName}}Setting(row);
                            _dict[setting.{{ file.PrimaryKeyField.Name }}] = setting;
                        }
                        else 
                        {
                            if (throwWhenDuplicatePrimaryKey) throw new System.Exception(string.Format(""DuplicateKey, Class: {0}, File: {1}, Key: {2}"", this.GetType().Name, tabFilePath, pk));
                            else setting.Reload(row);
                        }
                    }
                }
            }

	        if (OnReload != null)
	        {
	            OnReload();
	        }

            ReloadCount++;
            Log.Info(""Reload settings: {0}, Row Count: {1}, Reload Count: {2}"", GetType(), Count, ReloadCount);
        }

	    /// <summary>
        /// foreachable enumerable: {{ file.ClassName }}
        /// </summary>
        public static IEnumerable GetAll()
        {
            foreach (var row in GetInstance()._dict.Values)
            {
                yield return row;
            }
        }

        /// <summary>
        /// GetEnumerator for `MoveNext`: {{ file.ClassName }}
        /// </summary> 
	    public static IEnumerator GetEnumerator()
	    {
	        return GetInstance()._dict.Values.GetEnumerator();
	    }
         
	    /// <summary>
        /// Get class by primary key: {{ file.ClassName }}
        /// </summary>
        public static {{file.ClassName}}Setting Get({{ file.PrimaryKeyField.FormatType }} primaryKey)
        {
            {{file.ClassName}}Setting setting;
            if (GetInstance()._dict.TryGetValue(primaryKey, out setting)) return setting;
            return null;
        }

        // ========= CustomExtraString begin ===========
        {% if file.Extra %}{{ file.Extra }}{% endif %}
        // ========= CustomExtraString end ===========
    }

	/// <summary>
	/// Auto Generate for Tab File: {{ file.TabFilePaths }}
    /// Singleton class for less memory use
	/// </summary>
	public partial class {{file.ClassName}}Setting : TableRowFieldParser
	{
		{% for field in file.Fields %}
        /// <summary>
        /// {{ field.Comment }}
        /// </summary>
        public {{ field.FormatType }} {{ field.Name}} { get; private set;}
        {% endfor %}

        internal {{file.ClassName}}Setting(TableFileRow row)
        {
            Reload(row);
        }

        internal void Reload(TableFileRow row)
        { {% for field in file.Fields %}
            {{ field.Name}} = row.Get_{{ field.TypeMethod }}(row.Values[{{ field.Index }}], ""{{ field.DefaultValue }}""); {% endfor %}
        }

        /// <summary>
        /// Get PrimaryKey from a table row
        /// </summary>
        /// <param name=""row""></param>
        /// <returns></returns>
        public static {{ file.PrimaryKeyField.FormatType }} ParsePrimaryKey(TableFileRow row)
        {
            var primaryKey = row.Get_{{ file.PrimaryKeyField.TypeMethod }}(row.Values[{{ file.PrimaryKeyField.Index }}], ""{{ file.PrimaryKeyField.DefaultValue }}"");
            return primaryKey;
        }
	}
{% endfor %} 
}
");
        #endregion

        #region Manager类模版

        /// <summary>
        /// Manager类模版
        /// </summary>
        public static string GenManagerCodeTemplate =string.Concat(LicenseStr,@"
// 此代码由工具生成，请勿修改，如需扩展请编写同名Class并加上partial关键字

using System.Collections;
using System.Collections.Generic;
using KEngine;
using KEngine.Modules;
using TableML;
namespace {{ NameSpace }}
{
	/// <summary>
    /// All settings list here, so you can reload all settings manully from the list.
	/// </summary>
    public partial class SettingsManager
    {
        private static IReloadableSettings[] _settingsList;
        public static IReloadableSettings[] SettingsList
        {
            get
            {
                if (_settingsList == null)
                {
                    _settingsList = new IReloadableSettings[]
                    {
                     {{ ClassNames }}
                    };
                }
                return _settingsList;
            }
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem(""KEngine/Settings/Try Reload All Settings Code"")]
#endif
	    public static void AllSettingsReload()
	    {
	        for (var i = 0; i < SettingsList.Length; i++)
	        {
	            var settings = SettingsList[i];
                if (settings.Count > 0 // if never reload, ignore
#if UNITY_EDITOR
                    || !UnityEditor.EditorApplication.isPlaying // in editor and not playing, force load!
#endif
                    )
                {
                    settings.ReloadAll();
                }

	        }
	    }

    }
}
");
        #endregion
        
        #region 每张表(Class)对应的模版(Template)

        /// <summary>
        /// 单张表(Class)对应的模版(Template)
        /// </summary>
        public static string GenSingleClassCodeTemplate = string.Concat(LicenseStr, @"
// 此代码由工具生成，请勿修改，如需扩展请编写同名Class并加上partial关键字

using System.Collections;
using System.Collections.Generic;
using KEngine;
using KEngine.Modules;
using TableML;
namespace {{ NameSpace }}
{
{% for file in Files %}
	/// <summary>
	/// Auto Generate for Tab File: {{ file.TabFilePaths }}
    /// Excel File: {{ file.TabFileNames }}
    /// No use of generic and reflection, for better performance,  less IL code generating
	/// </summary>>
    public partial class {{file.ClassName}}Settings : IReloadableSettings
    {
        /// <summary>
        /// How many reload function load?
        /// </summary>>
        public static int ReloadCount { get; private set; }

		public static readonly string[] TabFilePaths = 
        {
            {{ file.TabFilePaths }}
        };
        internal static {{file.ClassName}}Settings _instance = new {{file.ClassName}}Settings();
        Dictionary<{{ file.PrimaryKeyField.FormatType }}, {{file.ClassName}}Setting> _dict = new Dictionary<{{ file.PrimaryKeyField.FormatType }}, {{file.ClassName}}Setting>();

        /// <summary>
        /// Trigger delegate when reload the Settings
        /// </summary>>
	    public static System.Action OnReload;

        /// <summary>
        /// Constructor, just reload(init)
        /// When Unity Editor mode, will watch the file modification and auto reload
        /// </summary>
	    private {{file.ClassName}}Settings()
	    {
        }

        /// <summary>
        /// Get the singleton
        /// </summary>
        /// <returns></returns>
	    public static {{file.ClassName}}Settings GetInstance()
	    {
            if (ReloadCount == 0)
            {
                _instance._ReloadAll(true);
    #if UNITY_EDITOR
                if (SettingModule.IsFileSystemMode)
                {
                    for (var j = 0; j < TabFilePaths.Length; j++)
                    {
                        var tabFilePath = TabFilePaths[j];
                        SettingModule.WatchSetting(tabFilePath, (path) =>
                        {
                            if (path.Replace(""\\"", ""/"").EndsWith(path))
                            {
                                _instance.ReloadAll();
                                Log.LogConsole_MultiThread(""File Watcher! Reload success! -> "" + path);
                            }
                        });
                    }

                }
    #endif
            }

	        return _instance;
	    }
        
        public int Count
        {
            get
            {
                return _dict.Count;
            }
        }

        /// <summary>
        /// Do reload the setting file: {{ file.ClassName }}, no exception when duplicate primary key
        /// </summary>
        public void ReloadAll()
        {
            _ReloadAll(false);
        }

        /// <summary>
        /// Do reload the setting class : {{ file.ClassName }}, no exception when duplicate primary key, use custom string content
        /// </summary>
        public void ReloadAllWithString(string context)
        {
            _ReloadAll(false, context);
        }

        /// <summary>
        /// Do reload the setting file: {{ file.ClassName }}
        /// </summary>
	    void _ReloadAll(bool throwWhenDuplicatePrimaryKey, string customContent = null)
        {
            for (var j = 0; j < TabFilePaths.Length; j++)
            {
                var tabFilePath = TabFilePaths[j];
                TableFile tableFile;
                if (customContent == null)
                    tableFile = SettingModule.Get(tabFilePath, false);
                else
                    tableFile = TableFile.LoadFromString(customContent);

                using (tableFile)
                {
                    foreach (var row in tableFile)
                    {
                        var pk = {{ file.ClassName }}Setting.ParsePrimaryKey(row);
                        {{file.ClassName}}Setting setting;
                        if (!_dict.TryGetValue(pk, out setting))
                        {
                            setting = new {{file.ClassName}}Setting(row);
                            _dict[setting.{{ file.PrimaryKeyField.Name }}] = setting;
                        }
                        else 
                        {
                            if (throwWhenDuplicatePrimaryKey) throw new System.Exception(string.Format(""DuplicateKey, Class: {0}, File: {1}, Key: {2}"", this.GetType().Name, tabFilePath, pk));
                            else setting.Reload(row);
                        }
                    }
                }
            }

	        if (OnReload != null)
	        {
	            OnReload();
	        }

            ReloadCount++;
            Log.Info(""Reload settings: {0}, Row Count: {1}, Reload Count: {2}"", GetType(), Count, ReloadCount);
        }

	    /// <summary>
        /// foreachable enumerable: {{ file.ClassName }}
        /// </summary>
        public static IEnumerable GetAll()
        {
            foreach (var row in GetInstance()._dict.Values)
            {
                yield return row;
            }
        }

        /// <summary>
        /// GetEnumerator for `MoveNext`: {{ file.ClassName }}
        /// </summary> 
	    public static IEnumerator GetEnumerator()
	    {
	        return GetInstance()._dict.Values.GetEnumerator();
	    }
         
	    /// <summary>
        /// Get class by primary key: {{ file.ClassName }}
        /// </summary>
        public static {{file.ClassName}}Setting Get({{ file.PrimaryKeyField.FormatType }} primaryKey)
        {
            {{file.ClassName}}Setting setting;
            if (GetInstance()._dict.TryGetValue(primaryKey, out setting)) return setting;
            return null;
        }

        // ========= CustomExtraString begin ===========
        {% if file.Extra %}{{ file.Extra }}{% endif %}
        // ========= CustomExtraString end ===========
    }

	/// <summary>
	/// Auto Generate for Tab File: {{ file.TabFilePaths }} 
    /// Excel File: {{ file.TabFileNames }}
    /// Singleton class for less memory use
	/// </summary>
	public partial class {{file.ClassName}}Setting : TableRowFieldParser
	{
		{% for field in file.Fields %}
        /// <summary>
        /// {{ field.Comment }}
        /// </summary>
        public {{ field.FormatType }} {{ field.Name}} { get; private set;}
        {% endfor %}

        internal {{file.ClassName}}Setting(TableFileRow row)
        {
            Reload(row);
        }

        internal void Reload(TableFileRow row)
        { {% for field in file.Fields %}
            {{ field.Name}} = row.Get_{{ field.TypeMethod }}(row.Values[{{ field.Index }}], ""{{ field.DefaultValue }}""); {% endfor %}
        }

        /// <summary>
        /// Get PrimaryKey from a table row
        /// </summary>
        /// <param name=""row""></param>
        /// <returns></returns>
        public static {{ file.PrimaryKeyField.FormatType }} ParsePrimaryKey(TableFileRow row)
        {
            var primaryKey = row.Get_{{ file.PrimaryKeyField.TypeMethod }}(row.Values[{{ file.PrimaryKeyField.Index }}], ""{{ file.PrimaryKeyField.DefaultValue }}"");
            return primaryKey;
        }
	}
{% endfor %} 
}
");
        #endregion
    }
}
