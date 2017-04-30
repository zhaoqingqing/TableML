using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NPOI.OpenXml4Net.OPC.Internal;
using TableML.Compiler;

namespace TableMLGUI
{
    public partial class MainForm : Form
    {

        /// <summary>
        /// 输出tml文件路径
        /// </summary>
        public string GenTmlPath = "..\\client_setting";

        /// <summary>
        /// 生成的代码路径
        /// </summary>
        public string GenCodePath = "..\\client_code\\";
        /// <summary>
        /// tml文件后缀
        /// </summary>
        public string TmlExtensions = ".k";

        public string NameSpace = "AppSettings";

        public MainForm()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            //源始excel路径
            var excelSrc = Path.GetFullPath(Application.StartupPath + ConfigurationManager.AppSettings.Get("srcExcelPath"));
            this.tbFileDir.Text = excelSrc;


            GenTmlPath = ConfigurationManager.AppSettings.Get("GenTmlPath");
            GenCodePath = ConfigurationManager.AppSettings.Get("GenCodePath");

            //客户端代码路径
            var dstClientCode = Path.GetFullPath(Application.StartupPath + ConfigurationManager.AppSettings.Get("dstClientCodePath"));
            this.txtCodePath.Text = dstClientCode;

            //客户端tml路径
            var dstClientTml = Path.GetFullPath(Application.StartupPath + ConfigurationManager.AppSettings.Get("dstClientTmlPath"));
            this.txtTmlPath.Text = dstClientTml;
        }

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
            this.tbFileDir.Text = dragDir;
        }

        public string[] fileList
        {
            get
            {
                return tbFileList.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        private void btnCompileSelect_Click(object sender, EventArgs e)
        {
            //编译选定的表
            Console.Clear();

            var startPath = Environment.CurrentDirectory;
            Console.WriteLine("当前目录：{0}", startPath);
            var compiler = new Compiler();
            
            int comileCount = 0;
            foreach (var filePath in fileList)
            {
                Console.WriteLine(filePath);
                var savePath = GenTmlPath + "\\" + SimpleExcelFile.GetOutFileName(filePath) + TmlExtensions;
                //TODO 编译表时，生成代码
                TableCompileResult compileResult = compiler.Compile(filePath, savePath);
                Console.WriteLine("编译结果:{0}---->{1}", filePath, savePath);
                Console.WriteLine();
                //生成代码
                BatchCompiler batchCompiler = new BatchCompiler();

                //NOTE 替换成相对路径
                string repStr = string.Empty;
                if (GenCodePath.Contains("..\\") || GenCodePath.Contains("../"))
                {
                    repStr = Directory.GetParent(compileResult.TabFileRelativePath).FullName;
                }
                else
                {
                    repStr = Path.GetFullPath(compileResult.TabFileRelativePath);
                }
                
                compileResult.TabFileRelativePath = compileResult.TabFileRelativePath.Replace(repStr, "");
                batchCompiler.GenCodeFile(compileResult, DefaultTemplate.GenSingleClassCodeTemplate, GenCodePath, NameSpace, TmlExtensions, null, true);
                
                if (compileResult != null)
                {
                    comileCount += 1;
                }
            }
            
            ShowCompileResult(comileCount);
        }

        private void btnCompileAll_Click(object sender, EventArgs e)
        {
            //编译整个目录
            var startPath = Environment.CurrentDirectory;
            Console.WriteLine("当前目录：{0}", startPath);

            var srcDirectory = tbFileDir.Text;


            var batchCompiler = new BatchCompiler();

            string templateString = DefaultTemplate.GenSingleClassCodeTemplate;

            var results = batchCompiler.CompileTableMLAllInSingleFile(srcDirectory, GenTmlPath, GenCodePath,
               templateString, "AppSettings", ".k", null, !string.IsNullOrEmpty(GenCodePath));
            ShowCompileResult(results.Count);
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
                MessageBox.Show("目录不存在！", string.Format("{0}\r\n不存在", txtCodePath.Text), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FileHelper.CopyFolder(GenCodePath, txtCodePath.Text);
            Console.WriteLine("copy {0} to \r\n {1}", GenCodePath, txtCodePath.Text);
        }

        private void btnSyncTml_Click(object sender, EventArgs e)
        {

            if (Directory.Exists(txtTmlPath.Text) == false)
            {
                MessageBox.Show("目录不存在！", string.Format("{0}\r\n不存在", txtTmlPath.Text), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FileHelper.CopyFolder(GenTmlPath, txtTmlPath.Text);
            Console.WriteLine("copy {0} to \r\n {1}", GenTmlPath, txtTmlPath.Text);


        }

        private void btnCheckNameRepet_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbFileList.Text))
            {
                ExcelHelper.CheckNameRepet(fileList);
            }
        }
    }
}
