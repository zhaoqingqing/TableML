using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TableML.Compiler;

namespace TableMLGUI
{
    /// <summary>
    /// 作者：赵青青(569032731@qq.com)
    /// 日期：2017-4-23
    /// 功能：tableml的gui
    /// </summary>
    public partial class MainForm : Form
    {
        #region 配置字段

        /// <summary>
        /// 输出tml文件路径
        /// </summary>
        public string ExportTsvPath;
        public string srcFullPath;

        /// <summary>
        /// 生成的代码路径
        /// </summary>
        public string ExportCSharpPath;
        public string ExportLuaPath ;
        /// <summary>
        /// tml文件后缀
        /// </summary>
        public string TmlExtensions = ".tsv";

        public string NameSpace = "AppSettings";
        private string sqlDBPath;

        /// <summary>
        /// 是否简单三行格式文件
        /// </summary>
        public bool IsSimpleRule
        {
            get { return cbSimpleRule.Checked; }
        }
        
        /// <summary>
        /// 是否为ksframework的表格默认格式
        /// </summary>
        public bool IsKSFrameworkRule
        {
            get { return cbKSFormat.Checked; }
        }
        
        public bool EnableToSqlite;
        public bool EnableGenCSharp; 
        public bool EnableGenLua; 
        public bool EnableGenTsv; 
        
        #endregion
        
        /// <summary>
        /// 框中的文件列表
        /// </summary>
        public string[] fileList
        {
            get
            {
                return tbFileList.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
        
        public MainForm()
        {
            InitializeComponent();
            Init();
            //test
            //tbFileList.Text = srcFullPath + @"\Billboard.xlsx";
        }

        public void Init()
        {
            //禁止调整窗体大小：窗体FormBorderStyle属性设置为：FixedSingle，并把最大化禁用
            bool useAbsolutePath = ConfigurationManager.AppSettings.Get("UseAbsolutePath").Trim() == "1";
            //源始excel路径
            var srcExcelPath = ConfigurationManager.AppSettings.Get("srcExcelPath");
            srcFullPath = useAbsolutePath ? Path.GetFullPath(srcExcelPath) : Path.GetFullPath(Application.StartupPath + srcExcelPath);
            this.tbSrcPath.Text = srcFullPath;

            EnableToSqlite = ConfigurationManager.AppSettings.Get("EnableToSqlite").Trim() == "1";
            EnableGenCSharp = ConfigurationManager.AppSettings.Get("EnableGenCSharp").Trim() == "1";
            EnableGenLua = ConfigurationManager.AppSettings.Get("EnableGenLua").Trim() == "1";
            EnableGenTsv = ConfigurationManager.AppSettings.Get("EnableGenTsv").Trim() == "1";
            
            //sql的database文件存放路径
            var _sqlScriptsPath = ConfigurationManager.AppSettings.Get("ExportSqlScriptsPath");
            var dbPath = ConfigurationManager.AppSettings.Get("ExportDBPath");
            if (!string.IsNullOrEmpty(dbPath))
            {
                this.sqlDBPath = useAbsolutePath ? dbPath : Path.GetFullPath(Application.StartupPath + dbPath);
                var sqlScriptPath = useAbsolutePath ? _sqlScriptsPath : Path.GetFullPath(Application.StartupPath + _sqlScriptsPath);
                FileHelper.CheckFolder(sqlScriptPath);
                SQLiteHelper.Init(this.sqlDBPath, sqlScriptPath);
            }

            //tml文件格式
            var tmlFileEx = ConfigurationManager.AppSettings.Get("TmlExtensions");
            if (!string.IsNullOrEmpty(tmlFileEx))  TmlExtensions = tmlFileEx;

            //tml路径
            var genTmlPath = ConfigurationManager.AppSettings.Get("ExportTsvPath");
            ExportTsvPath = useAbsolutePath ? genTmlPath : Path.GetFullPath(Application.StartupPath + genTmlPath);

            //代码路径
            var genCodePath = ConfigurationManager.AppSettings.Get("ExportCSharpPath");
            ExportCSharpPath = useAbsolutePath ? genCodePath : Path.GetFullPath(Application.StartupPath + genCodePath);
            var luaPath = ConfigurationManager.AppSettings.Get("ExportLuaPath");
            ExportLuaPath = useAbsolutePath ? luaPath : Path.GetFullPath(Application.StartupPath + luaPath);
            #region 用于拷贝文件到指定路径下
            //客户端代码路径
            var dstClientCodePath = ConfigurationManager.AppSettings.Get("dstClientCodePath");
            var dstClientCode = useAbsolutePath ? dstClientCodePath : Path.GetFullPath(Application.StartupPath + dstClientCodePath);
//            this.txtCodePath.Text = dstClientCode;

            //客户端tml路径
            var dstClientTmlPath = ConfigurationManager.AppSettings.Get("dstClientTmlPath");
            var dstClientTml = useAbsolutePath ? dstClientTmlPath : Path.GetFullPath(Application.StartupPath + dstClientTmlPath);
//            this.txtTmlPath.Text = dstClientTml;
            #endregion
            openFileDialog1.InitialDirectory = srcFullPath;
            openFileDialog1.DefaultExt = "*.xls,*.tsv,*.csv";
            openFileDialog1.Multiselect = true;
            openFileDialog1.SupportMultiDottedExtensions = true;

            ExcelConfig.IsKSFrameworkFormat = IsKSFrameworkRule;
            SetCheckboxState();
        }

        #region  文件拖拽到列表
        private void FileField_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void tbFileList_DragEnter(object sender, DragEventArgs e)
        {
            FileField_DragEnter(sender, e);
        }

        private void tbFileList_DragDrop(object sender, DragEventArgs e)
        {
            var files = ((System.Array)e.Data.GetData(DataFormats.FileDrop));
            foreach (var file in files)
            {
                this.tbFileList.Text += "\r\n" + file;
            }
        }

        private void tbFileDir_DragEnter(object sender, DragEventArgs e)
        {
            FileField_DragEnter(sender, e);
        }

        private void tbFileDir_DragDrop(object sender, DragEventArgs e)
        {
            var files = ((System.Array)e.Data.GetData(DataFormats.FileDrop));
            var dragDir = files.GetValue(0).ToString();
            if (Directory.Exists(dragDir) == false)
            {
                ConsoleHelper.Error("{0} 目录不存在或不是目录", dragDir);
                return;
            }
            this.tbSrcPath.Text = dragDir;
        }
        #endregion
        
        #region 编译按钮点击事件
        
        private IWorkbook PreParseExcel(string filePath)
        {
            IWorkbook Workbook;
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) // no isolation
            {
                try
                {
                    Workbook = WorkbookFactory.Create(file);
                }
                catch (Exception e)
                {
                    ConsoleHelper.Error(string.Format("无法打开Excel: {0}, 可能原因：正在打开？或是Office2007格式（尝试另存为）？ {1}", filePath, e.Message));
                    return null;
                }
            }

            if (Workbook == null)
            {
                ConsoleHelper.Error(filePath + " Null Workbook");
                return null;
            }
            return Workbook;
        }
        
        void GenCodeFile (TableCompileResult compileResult,string fileName)
        {
            if (EnableGenCSharp)
            {
                //生成csharp代码
                BatchCompiler batchCompiler = new BatchCompiler();
                var param = new GenParam()
                {
                    compileResult = compileResult, genCodeTemplateString = DefaultTemplate.GenSingleClassCodeTemplate, genCodeFilePath = ExportCSharpPath,
                    nameSpace = NameSpace, changeExtension = TmlExtensions, forceAll = true
                };
                batchCompiler.GenCodeFile(param);
            }
            //NOTE 如果是使用sqlite则生成此文件
            //LuaHelper.GenLuaFile(compileResult, ExportLuaPath +  fileName + ".lua");
        }
        
        /// <summary>
        /// 编译选中的excel
        /// </summary>
        /// <param name="msgResult"></param>
        public void CompileSelect(string[] fullPaths, bool msgResult = false)
        {
            if (fullPaths == null || fullPaths.Length <= 0)
            {
                ConsoleHelper.Error("路径不能传入空，不进行编译");
                return;
            }
            //编译选定的表
            List<string> tmlList = new List<string>();
            var startPath = Environment.CurrentDirectory;
            ConsoleHelper.Info("当前目录：{0}", startPath);
            var compiler = new Compiler();
            Dictionary<string, string> dst2src = new Dictionary<string, string>();
            int comileCount = 0;
            Stopwatch watch  = new Stopwatch();
            watch.Start();
            foreach (var filePath in fullPaths)
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    continue;
                }
                ConsoleHelper.Info(filePath);
                var ext = Path.GetExtension(filePath).Trim().ToLower();
                if (ext.Contains(".xls") || ext.Contains(".xlsx"))
                {
                    var workbook = PreParseExcel(filePath);
                    //ksframework只编译第一个sheet页
                    var sheetCount = IsKSFrameworkRule ? 1 : workbook.NumberOfSheets;
                    for (int index = 0; index < sheetCount; index++)
                    {
                        string savePath = null;
                        string outputName = SimpleExcelFile.GetOutFileName(filePath, index);
                        if (string.IsNullOrEmpty(outputName))
                        {
                            continue;
                        }

                        savePath =  ExportTsvPath + "\\" + outputName + TmlExtensions ;
                        var saveLuaPath = EnableGenLua? ExportLuaPath + "\\" + outputName + ".lua":null;
                        //编译成tml
                        var param = new CompilerParam() {path = filePath, ExportTsvPath = savePath, CanExportTsv = EnableGenTsv,ExportLuaPath = saveLuaPath, index = index, doRealCompile = true};
                        TableCompileResult compileResult = compiler.Compile(param);
                        tmlList.Add(Path.GetFullPath(savePath));
                        var dstFileName = Path.GetFileNameWithoutExtension(savePath);
                        if (dst2src.ContainsKey(dstFileName) == false)
                        {
                            dst2src.Add(dstFileName, Path.GetFileName(filePath));
                        }
                        //Console.WriteLine("编译结果:{0}---->{1}", newFilePath, savePath);
                        //Console.WriteLine();
                        //NOTE 替换成相对路径(保证最后只有文件名)
                        string repStr = Directory.GetParent(compileResult.TabFileRelativePath).FullName + "\\";
                        compileResult.TabFileRelativePath = compileResult.TabFileRelativePath.Replace(repStr, "");
                        GenCodeFile(compileResult,outputName);
                        
                        comileCount += 1;
                    }
                }
                else if (ext == ".csv" || ext == ".tsv")
                {
                    string outputName = (ext == ".csv") ? SimpleCSVFile.GetOutFileName(filePath) : Path.GetFileNameWithoutExtension(filePath);
                    if (string.IsNullOrEmpty(outputName))
                    {
                        continue;
                    }
                    var savePath =  ExportTsvPath + "\\" + outputName + TmlExtensions;
                    var saveLuaPath = EnableGenLua ? ExportLuaPath + "\\" + outputName + ".lua" : null;
                    //编译成tml
                    var param = new CompilerParam(){path = filePath,ExportTsvPath = savePath,CanExportTsv = EnableGenTsv ,ExportLuaPath = saveLuaPath,index = 0,doRealCompile = false};
                    TableCompileResult compileResult = compiler.Compile(param);
                    tmlList.Add(Path.GetFullPath(savePath));
                    var dstFileName = Path.GetFileNameWithoutExtension(savePath);
                    if (dst2src.ContainsKey(dstFileName) == false)
                    {
                        dst2src.Add(dstFileName, Path.GetFileName(filePath));
                    }
                    //Console.WriteLine("编译结果:{0}---->{1}", filePath, savePath);
                    //Console.WriteLine();

                    //NOTE 替换成相对路径(保证最后只有文件名)
                    string repStr = Directory.GetParent(compileResult.TabFileRelativePath).FullName + "\\";
                    compileResult.TabFileRelativePath = compileResult.TabFileRelativePath.Replace(repStr, "");
                    GenCodeFile(compileResult,outputName);

                    if (compileResult != null)
                    {
                        comileCount += 1;
                    }
                }
                else
                {
                    ConsoleHelper.Info("跳过" + filePath);
                }

            }
            BatchCompiler.SaveCompileResult(dst2src);
            if (EnableToSqlite)
            {
                //将结果插入到sqlite中
                SQLiteHelper.UpdateDB(tmlList.ToArray());
            }
            watch.Stop();
            if (msgResult)
            {
                MessageBox.Show(string.Format("共编译{0}张表，耗时{1}s", comileCount,watch.ElapsedMilliseconds*0.001f), "编译完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 编译所有的excel
        /// </summary>
        /// <param name="msgResult">是否弹出编译结果</param>
        public void CompileAllExcel(bool msgResult = false)
        {
            //编译整个目录
             var files = Directory.GetFiles(tbSrcPath.Text, "*.*", SearchOption.AllDirectories)
                .Where(file=>file.ToLower().EndsWith("csv")||file.ToLower().EndsWith("xls")
                || file.ToLower().EndsWith("xlsx") || file.ToLower().EndsWith("tsv")).ToList();
            tbFileList.Text = string.Join("\r\n", files);
            CompileSelect(files.ToArray(), msgResult);
        }

        private void btnCompileExcel_Click(object sender, EventArgs e)
        {
            //编译某个目录下全部的excel
            CompileAllExcel(true);
            if (EnableToSqlite)
            {
                SQLiteHelper.UpdateDB(ExportTsvPath);
            }
        }
        
        private void btnCompileSelect_Click(object sender, EventArgs e)
        {
            //编译已选择的excel
            CompileSelect(fileList, true);
            if(EnableToSqlite) SQLiteHelper.UpdateDB(ExportTsvPath);
        }
        
        private void btnUpdateDB_Click(object sender, EventArgs e)
        {
            if (EnableToSqlite)
            {
                SQLiteHelper.UpdateDB(ExportTsvPath);
            }
            else
            {
                MessageBox.Show("未启用Sqlite功能，请在App.config中启用", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        #endregion
        
        #region 辅助功能的Click事件
        
        private void btnUpdateCSSyntax_Click(object sender, EventArgs e)
        {
            ExcelHelper.UpdateAllTableSyntax();
        }

        private void btnSyncCode_Click(object sender, EventArgs e)
        {
            /*if (Directory.Exists(txtCodePath.Text) == false)
            {
                Directory.CreateDirectory(txtCodePath.Text);
            }
            FileHelper.CopyFolder(GenCodePath, txtCodePath.Text);
            Console.WriteLine("copy {0} to \r\n {1}", GenCodePath, txtCodePath.Text);
            MessageBox.Show(string.Format("{0}\r\n同步完成", txtCodePath.Text), "同步完成", MessageBoxButtons.OK, MessageBoxIcon.Information);*/
        }

        private void btnSyncTml_Click(object sender, EventArgs e)
        {
            /*if (Directory.Exists(txtTmlPath.Text) == false)
            {
                Directory.CreateDirectory(txtTmlPath.Text);
            }
            FileHelper.CopyFolder(GenTmlPath, txtTmlPath.Text);
            Console.WriteLine("copy {0} to \r\n {1}", GenTmlPath, txtTmlPath.Text);
            MessageBox.Show(string.Format("{0}\r\n同步完成", txtTmlPath.Text), "同步完成", MessageBoxButtons.OK, MessageBoxIcon.Information);*/
        }

        private void btnCheckNameRepet_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbFileList.Text))
            {
                ExcelHelper.CheckNameRepet(fileList);
            }
        }

        private void btnOpenCodeDir_Click(object sender, EventArgs e)
        {
            FileHelper.OpenFolder(ExportCSharpPath);
        }

        private void btnOpenTmlDir_Click(object sender, EventArgs e)
        {
            FileHelper.OpenFolder(ExportTsvPath);
        }

        private void btnCheckNameEmpty_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbFileList.Text))
            {
                ExcelHelper.CheckNameEmpty(fileList);
            }
        }

        private void btnCheckCSKW_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(tbFileList.Text))
            {
                ExcelHelper.CheckHasKeyWords(fileList);
            }
        }

        private void btnUpdateSelectCSSyntax_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbFileList.Text))
            {
                foreach (string filePath in fileList)
                {
                    ExcelHelper.ToCSharpSyntax(filePath);
                }
                ConsoleHelper.Info("数据类型更新完成");
            }
        }

        private void btnSqlite_Click(object sender, EventArgs e)
        {
            SQLiteHelper.TestInsert();
        }
        
        /// <summary>
        /// 命令行模式编译并插入到sqlite中
        /// </summary>
        public void CMDCompile()
        {
            CompileAllExcel();
            if (EnableToSqlite)
            {
                SQLiteHelper.UpdateDB(ExportTsvPath);
            }
        }

        private void btnOpenDB_Click(object sender, EventArgs e)
        {
            var dirPath = Path.GetDirectoryName(sqlDBPath);
            FileHelper.OpenFolder(dirPath);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            HelpForm helpForm = new HelpForm();
            helpForm.Show();
        }

        private void btnClearConsole_Click(object sender, EventArgs e)
        {
            Console.Clear();
        }

        private void btnFileBrowser_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                var selectFiles = openFileDialog1.FileNames;
                foreach (string file in selectFiles)
                {
                    tbFileList.Text += file + "\r\n";
                }
            }
        }
        
        private void btnExecuteSql_Click(object sender, EventArgs e)
        {
            SQLiteHelper.ExecuteSql();
        }

        private void btnOpenLuaDir_Click(object sender, EventArgs e)
        {
            FileHelper.OpenFolder(ExportLuaPath);
        }
        
        #endregion
        
        #region 界面修改配置
        void SetCheckboxState()
        {
            //btnUpdateDB.Visible = EnableToSqlite;
            cb_sql.Checked = EnableToSqlite;
            cb_lua.Checked = EnableGenLua;
            cb_csharp.Checked = EnableGenCSharp;
            cb_tsv.Checked = EnableGenTsv;
        }
        
        private void cbKSFormat_Click(object sender, EventArgs e)
        {
            var box = sender as CheckBox;
            if (box != null)
            {
                if (!box.Checked)
                {
                    MessageBox.Show("对于非KSFramework格式的Excel需要修改SimpleExcel的代码");
                    return;
                }
                ExcelConfig.IsKSFrameworkFormat = IsKSFrameworkRule;
            }
        }
        
        private void Cb_sql_CheckedChanged(object sender, EventArgs e)
        {
            var box = sender as CheckBox;
            if (box!=null)
            {
                EnableToSqlite = box.Checked;
                AppConfigHelper.SetValue("EnableToSqlite", box.Checked ? "1" : "0");
                ConsoleHelper.Log($"是否启用导出到sqlite?{box.Checked} ,config:{AppConfigHelper.GetValue("EnableToSqlite")}");
            }
        }
        
        private void cb_lua_CheckedChanged(object sender, EventArgs e)
        {
            var box = sender as CheckBox;
            if (box!=null)
            {
                EnableGenLua = box.Checked;
                AppConfigHelper.SetValue("EnableGenLua", box.Checked ? "1" : "0");
                ConsoleHelper.Log($"是否启用生成Lua?{box.Checked} ,config:{AppConfigHelper.GetValue("EnableGenLua")}");
            }
        }

        private void cb_csharp_CheckedChanged(object sender, EventArgs e)
        {
            var box = sender as CheckBox;
            if (box!=null)
            {
                EnableGenCSharp = box.Checked;
                AppConfigHelper.SetValue("EnableGenCSharp", box.Checked ? "1" : "0");
                ConsoleHelper.Log($"是否启用生成CSharp?{box.Checked} ,config:{AppConfigHelper.GetValue("EnableGenCSharp")}");
            }
        }

        private void cb_tsv_CheckedChanged(object sender, EventArgs e)
        {
            var box = sender as CheckBox;
            if (box!=null)
            {
                EnableGenTsv = box.Checked;
                AppConfigHelper.SetValue("EnableGenTsv", box.Checked ? "1" : "0");
                ConsoleHelper.Log($"是否启用生成Tsv?{box.Checked} ,config:{AppConfigHelper.GetValue("EnableGenTsv")}");
            }
        }
        #endregion
    }
}
