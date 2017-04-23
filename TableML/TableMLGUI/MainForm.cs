using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TableML.Compiler;

namespace TableMLGUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            var defaultSrc = Path.GetFullPath(Application.StartupPath + "\\..\\Src\\");
            this.tbFileDir.Text = defaultSrc;
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

        private void btnCompileSelect_Click(object sender, EventArgs e)
        {
            //编译选定的表
            Console.Clear();
            var fileList = tbFileList.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            var startPath = Environment.CurrentDirectory;
            Console.WriteLine("当前目录：{0}", startPath);
            var compiler = new Compiler();
            var saveDir = startPath + ".\\..\\client_setting\\";
            int comileCount = 0;
            foreach (var filePath in fileList)
            {
                Console.WriteLine(filePath);
                var savePath = saveDir + "\\" + SimpleExcelFile.GetOutFileName(filePath) + ".k";
              TableCompileResult result =   compiler.Compile(filePath, savePath);
                Console.WriteLine("编译结果:{0}---->{1}", filePath, savePath);
                Console.WriteLine();
                if (result != null)
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
            //输出tml文件路径
            var OutputDirectory = "..\\client_setting";
            //生成的代码路径
            var CodeFilePath = "..\\client_code\\";

            var batchCompiler = new BatchCompiler();

            string templateString = DefaultTemplate.GenSingleClassCodeTemplate;

            var results = batchCompiler.CompileTableMLAllInSingleFile(srcDirectory, OutputDirectory, CodeFilePath,
               templateString, "AppSettings", ".k", null, !string.IsNullOrEmpty(CodeFilePath));
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
    }
}
