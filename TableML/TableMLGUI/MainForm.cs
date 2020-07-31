using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TableML.Compiler;

namespace TableMLGUI
{
    public partial class MainForm : Form
    {

        /// <summary>
        /// 输出tml文件路径
        /// </summary>
        public string GenTmlPath = "..\\client_setting";
        public string srcFullPath;

        /// <summary>
        /// 生成的代码路径
        /// </summary>
        public string GenCodePath = "..\\client_code\\";
        /// <summary>
        /// tml文件后缀
        /// </summary>
        public string TmlExtensions = ".tsv";

        public string NameSpace = "AppSettings";
        private string sqlDataPath;

        /// <summary>
        /// 简单三行格式文件
        /// </summary>
        public bool IsSimpleRule
        {
            get { return cbSimpleRule.Checked; }
        }
        
        /// <summary>
        /// ksframework的表格默认格式
        /// </summary>
        public bool IsKSFrameworkRule
        {
            get { return cbKSFormat.Checked; }
        }
        
        public bool GenCSCode
        {
            get { return cbGenCS.Checked; }
        }

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
            //tbFileList.Text = @"e:\3dsn\plan\005ConfigTable\Src\NPC配置表.xlsx";
        }


        public void Init()
        {
            string useAbsolutePathStr = ConfigurationManager.AppSettings.Get("UseAbsolutePath").Trim().ToLower();
            bool useAbsolutePath = useAbsolutePathStr == "true" || useAbsolutePathStr == "1";

            //源始excel路径
            var srcExcelPath = ConfigurationManager.AppSettings.Get("srcExcelPath");
            var excelSrc = useAbsolutePath ? srcExcelPath : Path.GetFullPath(Application.StartupPath + srcExcelPath);
            this.tbSrcPath.Text = excelSrc;
            srcFullPath = Path.GetFullPath(Application.StartupPath + srcExcelPath);
            //sql的database文件存放路径
            var sqlDBPath = ConfigurationManager.AppSettings.Get("DBPath");
            var sqlScriptsPath = ConfigurationManager.AppSettings.Get("sqlScriptsPath");
            if (!string.IsNullOrEmpty(sqlDBPath))
            {
                sqlDataPath = useAbsolutePath ? sqlDBPath : Path.GetFullPath(Application.StartupPath + sqlDBPath);
                var sqlStPath = useAbsolutePath ? sqlScriptsPath : Path.GetFullPath(Application.StartupPath + sqlScriptsPath);
                SQLiteHelper.Init(sqlDataPath, sqlStPath);
            }

            //tml文件格式
            var tmlFileEx = ConfigurationManager.AppSettings.Get("TmlExtensions");
            if (!string.IsNullOrEmpty(tmlFileEx))
            {
                TmlExtensions = tmlFileEx;
            }

            //tml路径
            var genTmlPath = ConfigurationManager.AppSettings.Get("GenTmlPath");
            GenTmlPath = useAbsolutePath ? genTmlPath : Path.GetFullPath(Application.StartupPath + genTmlPath);

            //代码路径
            var genCodePath = ConfigurationManager.AppSettings.Get("GenCodePath");
            GenCodePath = useAbsolutePath ? genCodePath : Path.GetFullPath(Application.StartupPath + genCodePath);

            //客户端代码路径
            var dstClientCodePath = ConfigurationManager.AppSettings.Get("dstClientCodePath");
            var dstClientCode = useAbsolutePath ? dstClientCodePath : Path.GetFullPath(Application.StartupPath + dstClientCodePath);
            this.txtCodePath.Text = dstClientCode;

            //客户端tml路径
            var dstClientTmlPath = ConfigurationManager.AppSettings.Get("dstClientTmlPath");
            var dstClientTml = useAbsolutePath ? dstClientTmlPath : Path.GetFullPath(Application.StartupPath + dstClientTmlPath);
            this.txtTmlPath.Text = dstClientTml;

            openFileDialog1.InitialDirectory = srcFullPath;
            openFileDialog1.DefaultExt = "*.xls,*.tsv,*.csv";
            openFileDialog1.Multiselect = true;
            openFileDialog1.SupportMultiDottedExtensions = true;

            groupBoxCS.Visible = cbGenCS.Checked;
            InitExcelFormat();
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
                Console.WriteLine("Error !{0} 目录不存在或不是目录", dragDir);
                return;
            }
            this.tbSrcPath.Text = dragDir;
        }
        #endregion

        private void btnCompileSelect_Click(object sender, EventArgs e)
        {
            CompileSelect();
        }

        public void CompileSelect(bool msgResult = false)
        {
            CompileSelect(fileList, msgResult);
        }

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
                    //                    throw new Exception(string.Format("无法打开Excel: {0}, 可能原因：正在打开？或是Office2007格式（尝试另存为）？ {1}", filePath, e.Message));
                    ConsoleHelper.Error(string.Format("无法打开Excel: {0}, 可能原因：正在打开？或是Office2007格式（尝试另存为）？ {1}", filePath, e.Message));
                    return null;
                }
            }

            if (Workbook == null)
            {
                //                    throw new Exception(filePath + " Null Workbook");
                ConsoleHelper.Error(filePath + " Null Workbook");
                return null;
            }
            return Workbook;
        }
        /// <summary>
        /// 编译选中的excel
        /// </summary>
        /// <param name="msgResult"></param>
        public void CompileSelect(string[] fullPaths, bool msgResult = false)
        {
            //编译选定的表
            List<string> tmlList = new List<string>();
            var startPath = Environment.CurrentDirectory;
            Console.WriteLine("当前目录：{0}", startPath);
            var compiler = new Compiler();
            Dictionary<string, string> dst2src = new Dictionary<string, string>();
            int comileCount = 0;

            Action<TableCompileResult> genCSharp = (TableCompileResult compileResult) =>
            {
//                if (NeedGenCSharp)
                {
                    //生成代码
                    BatchCompiler batchCompiler = new BatchCompiler();
                    batchCompiler.GenCodeFile(compileResult, DefaultTemplate.GenSingleClassCodeTemplate, GenCodePath, NameSpace, TmlExtensions, null, true);
                }
            };
            foreach (var filePath in fullPaths)
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    continue;
                }
                Console.WriteLine(filePath);
                var ext = Path.GetExtension(filePath).Trim().ToLower();
                if (ext.Contains(".xls"))
                {
                    var workbook = PreParseExcel(filePath);
                    for (int index = 0; index < workbook.NumberOfSheets; index++)
                    {
                        string savePath = null;
                        string outputName = SimpleExcelFile.GetOutFileName(filePath, index);
                        if (string.IsNullOrEmpty(outputName))
                        {
                            continue;
                        }
                        savePath = GenTmlPath + "\\" + outputName + TmlExtensions;
                        //编译成tml
                        TableCompileResult compileResult = compiler.Compile(filePath, savePath, index);
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
                        genCSharp(compileResult);

                        if (compileResult != null)
                        {
                            comileCount += 1;
                        }
                    }
                }
                else if (ext == ".csv" || ext == ".tsv")
                {
                    string outputName = (ext == ".csv") ? SimpleCSVFile.GetOutFileName(filePath) : Path.GetFileNameWithoutExtension(filePath);
                    if (string.IsNullOrEmpty(outputName))
                    {
                        continue;
                    }
                    var savePath = GenTmlPath + "\\" + outputName + TmlExtensions;
                    //编译成tml
                    var compileResult = compiler.Compile(filePath, savePath, 0);
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
                    genCSharp(compileResult);

                    if (compileResult != null)
                    {
                        comileCount += 1;
                    }
                }
                else
                {
                    Console.WriteLine("跳过" + filePath);
                }

            }
            BatchCompiler.SaveCompileResult(dst2src);

            //将结果插入到sqlite中
            SQLiteHelper.UpdateDB(tmlList.ToArray());

            if (msgResult) { ShowCompileResult(comileCount); }
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
            //            var batchCompiler = new BatchCompiler();
            //
            //            string templateString = DefaultTemplate.GenSingleClassCodeTemplate;
            //
            //            var results = batchCompiler.CompileAll(srcDirectory, GenTmlPath, GenCodePath,
            //               templateString, NameSpace, TmlExtensions, null, !string.IsNullOrEmpty(GenCodePath), GenCSCode);
//                        if (msgResult) { ShowCompileResult(results.Count); }
        }


        private void btnCompileAll_Click(object sender, EventArgs e)
        {
            CompileAllExcel();
            SQLiteHelper.UpdateDB(GenTmlPath);
        }

        public void ShowCompileResult(int count)
        {
            MessageBox.Show(string.Format("共编译{0}张表", count), "编译完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUpdateCSSyntax_Click(object sender, EventArgs e)
        {
            ExcelHelper.UpdateAllTableSyntax();
        }

        private void btnSyncCode_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(txtCodePath.Text) == false)
            {
                Directory.CreateDirectory(txtCodePath.Text);
            }
            FileHelper.CopyFolder(GenCodePath, txtCodePath.Text);
            Console.WriteLine("copy {0} to \r\n {1}", GenCodePath, txtCodePath.Text);
            MessageBox.Show(string.Format("{0}\r\n同步完成", txtCodePath.Text), "同步完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSyncTml_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(txtTmlPath.Text) == false)
            {
                Directory.CreateDirectory(txtTmlPath.Text);
            }
            FileHelper.CopyFolder(GenTmlPath, txtTmlPath.Text);
            Console.WriteLine("copy {0} to \r\n {1}", GenTmlPath, txtTmlPath.Text);
            MessageBox.Show(string.Format("{0}\r\n同步完成", txtTmlPath.Text), "同步完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            FileHelper.OpenFolder(GenCodePath);
        }

        private void btnOpenTmlDir_Click(object sender, EventArgs e)
        {
            FileHelper.OpenFolder(GenTmlPath);
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
                Console.WriteLine("数据类型更新完成");
            }
        }

        private void btnSqlite_Click(object sender, EventArgs e)
        {
            SQLiteHelper.TestInsert();
        }

        private void btnUpdateDB_Click(object sender, EventArgs e)
        {
            SQLiteHelper.UpdateDB(GenTmlPath);
        }

        private void btnCompileExcel_Click(object sender, EventArgs e)
        {
            CompileAllExcel();
        }

        /// <summary>
        /// 命令行模式编译并插入到sqlite中
        /// </summary>
        public void CMDCompile()
        {
            CompileAllExcel();
            SQLiteHelper.UpdateDB(GenTmlPath);
        }

        private void btnOpenDB_Click(object sender, EventArgs e)
        {
            var dirPath = Path.GetDirectoryName(sqlDataPath);
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

        private void cbGenCS_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxCS.Visible = cbGenCS.Checked;
        }

        private void btnExecuteSql_Click(object sender, EventArgs e)
        {
            SQLiteHelper.ExecuteSql();
        }

        private void cbKSFormat_Click(object sender, EventArgs e)
        {
            InitExcelFormat(true);
        }

        void InitExcelFormat(bool show_msg = false)
        {
            if (show_msg)
            {
                MessageBox.Show("是否选中"+cbKSFormat.Checked);
            }
            ExcelConfig.IsKSFrameworkFormat = IsKSFrameworkRule;
        }
    }
}
