namespace TableMLGUI
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCompileSelect = new System.Windows.Forms.Button();
            this.tbFileList = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSrcPath = new System.Windows.Forms.TextBox();
            this.btnCompileAll = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnUpdateCSSyntax = new System.Windows.Forms.Button();
            this.btnCheckNameRepet = new System.Windows.Forms.Button();
            this.btnOpenCodeDir = new System.Windows.Forms.Button();
            this.btnOpenTmlDir = new System.Windows.Forms.Button();
            this.btnCheckNameEmpty = new System.Windows.Forms.Button();
            this.cbSimpleRule = new System.Windows.Forms.CheckBox();
            this.btnCheckCSKW = new System.Windows.Forms.Button();
            this.btnUpdateSelectCSSyntax = new System.Windows.Forms.Button();
            this.btnCompileExcel = new System.Windows.Forms.Button();
            this.btnSqlite = new System.Windows.Forms.Button();
            this.btnUpdateDB = new System.Windows.Forms.Button();
            this.btnOpenDB = new System.Windows.Forms.Button();
            this.groupBoxTools = new System.Windows.Forms.GroupBox();
            this.btnHelp = new System.Windows.Forms.Button();
            this.groupBoxOther = new System.Windows.Forms.GroupBox();
            this.btnExecuteSql = new System.Windows.Forms.Button();
            this.btnClearConsole = new System.Windows.Forms.Button();
            this.btnFileBrowser = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cbKSFormat = new System.Windows.Forms.CheckBox();
            this.groupBoxTools.SuspendLayout();
            this.groupBoxOther.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCompileSelect
            // 
            this.btnCompileSelect.Location = new System.Drawing.Point(266, 643);
            this.btnCompileSelect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCompileSelect.Name = "btnCompileSelect";
            this.btnCompileSelect.Size = new System.Drawing.Size(251, 57);
            this.btnCompileSelect.TabIndex = 0;
            this.btnCompileSelect.Text = "编译上面框中的Excel";
            this.btnCompileSelect.UseVisualStyleBackColor = true;
            this.btnCompileSelect.Click += new System.EventHandler(this.btnCompileSelect_Click);
            // 
            // tbFileList
            // 
            this.tbFileList.AllowDrop = true;
            this.tbFileList.Location = new System.Drawing.Point(24, 44);
            this.tbFileList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbFileList.Multiline = true;
            this.tbFileList.Name = "tbFileList";
            this.tbFileList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbFileList.Size = new System.Drawing.Size(980, 561);
            this.tbFileList.TabIndex = 1;
            this.tbFileList.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbFileList_DragDrop);
            this.tbFileList.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbFileList_DragEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(345, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "请拖入需要编译的单个或多个Excel文件(可手动增加或删除路径)";
            // 
            // tbSrcPath
            // 
            this.tbSrcPath.AllowDrop = true;
            this.tbSrcPath.Location = new System.Drawing.Point(6, 844);
            this.tbSrcPath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbSrcPath.Multiline = true;
            this.tbSrcPath.Name = "tbSrcPath";
            this.tbSrcPath.Size = new System.Drawing.Size(518, 41);
            this.tbSrcPath.TabIndex = 3;
            this.tbSrcPath.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbFileDir_DragDrop);
            this.tbSrcPath.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbFileDir_DragEnter);
            // 
            // btnCompileAll
            // 
            this.btnCompileAll.Location = new System.Drawing.Point(3, 905);
            this.btnCompileAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCompileAll.Name = "btnCompileAll";
            this.btnCompileAll.Size = new System.Drawing.Size(163, 57);
            this.btnCompileAll.TabIndex = 0;
            this.btnCompileAll.Text = "编译并插入到SQLite中";
            this.btnCompileAll.UseVisualStyleBackColor = true;
            this.btnCompileAll.Click += new System.EventHandler(this.btnCompileAll_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 798);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "编译这个目录下所有的Excel";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.Location = new System.Drawing.Point(3, 997);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(242, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "任务栏点击我选TableGUI，可查看输出日志";
            // 
            // btnUpdateCSSyntax
            // 
            this.btnUpdateCSSyntax.Location = new System.Drawing.Point(155, 102);
            this.btnUpdateCSSyntax.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUpdateCSSyntax.Name = "btnUpdateCSSyntax";
            this.btnUpdateCSSyntax.Size = new System.Drawing.Size(140, 40);
            this.btnUpdateCSSyntax.TabIndex = 0;
            this.btnUpdateCSSyntax.Text = "批量改表的字段类型";
            this.btnUpdateCSSyntax.UseVisualStyleBackColor = true;
            this.btnUpdateCSSyntax.Click += new System.EventHandler(this.btnUpdateCSSyntax_Click);
            // 
            // btnCheckNameRepet
            // 
            this.btnCheckNameRepet.Location = new System.Drawing.Point(8, 34);
            this.btnCheckNameRepet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCheckNameRepet.Name = "btnCheckNameRepet";
            this.btnCheckNameRepet.Size = new System.Drawing.Size(128, 40);
            this.btnCheckNameRepet.TabIndex = 0;
            this.btnCheckNameRepet.Text = "检查字段名重复";
            this.btnCheckNameRepet.UseVisualStyleBackColor = true;
            this.btnCheckNameRepet.Click += new System.EventHandler(this.btnCheckNameRepet_Click);
            // 
            // btnOpenCodeDir
            // 
            this.btnOpenCodeDir.Location = new System.Drawing.Point(8, 28);
            this.btnOpenCodeDir.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOpenCodeDir.Name = "btnOpenCodeDir";
            this.btnOpenCodeDir.Size = new System.Drawing.Size(128, 40);
            this.btnOpenCodeDir.TabIndex = 0;
            this.btnOpenCodeDir.Text = "打开生成的代码";
            this.btnOpenCodeDir.UseVisualStyleBackColor = true;
            this.btnOpenCodeDir.Click += new System.EventHandler(this.btnOpenCodeDir_Click);
            // 
            // btnOpenTmlDir
            // 
            this.btnOpenTmlDir.Location = new System.Drawing.Point(155, 28);
            this.btnOpenTmlDir.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOpenTmlDir.Name = "btnOpenTmlDir";
            this.btnOpenTmlDir.Size = new System.Drawing.Size(128, 40);
            this.btnOpenTmlDir.TabIndex = 0;
            this.btnOpenTmlDir.Text = "打开编译后的表";
            this.btnOpenTmlDir.UseVisualStyleBackColor = true;
            this.btnOpenTmlDir.Click += new System.EventHandler(this.btnOpenTmlDir_Click);
            // 
            // btnCheckNameEmpty
            // 
            this.btnCheckNameEmpty.Location = new System.Drawing.Point(155, 34);
            this.btnCheckNameEmpty.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCheckNameEmpty.Name = "btnCheckNameEmpty";
            this.btnCheckNameEmpty.Size = new System.Drawing.Size(128, 40);
            this.btnCheckNameEmpty.TabIndex = 0;
            this.btnCheckNameEmpty.Text = "检查字段名空白";
            this.btnCheckNameEmpty.UseVisualStyleBackColor = true;
            this.btnCheckNameEmpty.Click += new System.EventHandler(this.btnCheckNameEmpty_Click);
            // 
            // cbSimpleRule
            // 
            this.cbSimpleRule.AutoSize = true;
            this.cbSimpleRule.Location = new System.Drawing.Point(14, 742);
            this.cbSimpleRule.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbSimpleRule.Name = "cbSimpleRule";
            this.cbSimpleRule.Size = new System.Drawing.Size(121, 21);
            this.cbSimpleRule.TabIndex = 6;
            this.cbSimpleRule.Text = "我是三行简单TSV";
            this.cbSimpleRule.UseVisualStyleBackColor = true;
            // 
            // btnCheckCSKW
            // 
            this.btnCheckCSKW.Location = new System.Drawing.Point(292, 34);
            this.btnCheckCSKW.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCheckCSKW.Name = "btnCheckCSKW";
            this.btnCheckCSKW.Size = new System.Drawing.Size(170, 40);
            this.btnCheckCSKW.TabIndex = 0;
            this.btnCheckCSKW.Text = "检查字段名是否C#关键字";
            this.btnCheckCSKW.UseVisualStyleBackColor = true;
            this.btnCheckCSKW.Click += new System.EventHandler(this.btnCheckCSKW_Click);
            // 
            // btnUpdateSelectCSSyntax
            // 
            this.btnUpdateSelectCSSyntax.Location = new System.Drawing.Point(9, 102);
            this.btnUpdateSelectCSSyntax.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUpdateSelectCSSyntax.Name = "btnUpdateSelectCSSyntax";
            this.btnUpdateSelectCSSyntax.Size = new System.Drawing.Size(140, 40);
            this.btnUpdateSelectCSSyntax.TabIndex = 0;
            this.btnUpdateSelectCSSyntax.Text = "改框中表的字段类型";
            this.btnUpdateSelectCSSyntax.UseVisualStyleBackColor = true;
            this.btnUpdateSelectCSSyntax.Click += new System.EventHandler(this.btnUpdateSelectCSSyntax_Click);
            // 
            // btnCompileExcel
            // 
            this.btnCompileExcel.Location = new System.Drawing.Point(178, 905);
            this.btnCompileExcel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCompileExcel.Name = "btnCompileExcel";
            this.btnCompileExcel.Size = new System.Drawing.Size(163, 57);
            this.btnCompileExcel.TabIndex = 0;
            this.btnCompileExcel.Text = "编译Excel";
            this.btnCompileExcel.UseVisualStyleBackColor = true;
            this.btnCompileExcel.Click += new System.EventHandler(this.btnCompileExcel_Click);
            // 
            // btnSqlite
            // 
            this.btnSqlite.Location = new System.Drawing.Point(8, 163);
            this.btnSqlite.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSqlite.Name = "btnSqlite";
            this.btnSqlite.Size = new System.Drawing.Size(128, 40);
            this.btnSqlite.TabIndex = 0;
            this.btnSqlite.Text = "插入十万条测试数据";
            this.btnSqlite.UseVisualStyleBackColor = true;
            this.btnSqlite.Click += new System.EventHandler(this.btnSqlite_Click);
            // 
            // btnUpdateDB
            // 
            this.btnUpdateDB.Location = new System.Drawing.Point(362, 905);
            this.btnUpdateDB.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUpdateDB.Name = "btnUpdateDB";
            this.btnUpdateDB.Size = new System.Drawing.Size(163, 57);
            this.btnUpdateDB.TabIndex = 0;
            this.btnUpdateDB.Text = "仅插入到Sqlite";
            this.btnUpdateDB.UseVisualStyleBackColor = true;
            this.btnUpdateDB.Click += new System.EventHandler(this.btnUpdateDB_Click);
            // 
            // btnOpenDB
            // 
            this.btnOpenDB.Location = new System.Drawing.Point(155, 93);
            this.btnOpenDB.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOpenDB.Name = "btnOpenDB";
            this.btnOpenDB.Size = new System.Drawing.Size(140, 40);
            this.btnOpenDB.TabIndex = 0;
            this.btnOpenDB.Text = "打开生成的data.db";
            this.btnOpenDB.UseVisualStyleBackColor = true;
            this.btnOpenDB.Click += new System.EventHandler(this.btnOpenDB_Click);
            // 
            // groupBoxTools
            // 
            this.groupBoxTools.Controls.Add(this.btnCheckNameEmpty);
            this.groupBoxTools.Controls.Add(this.btnUpdateCSSyntax);
            this.groupBoxTools.Controls.Add(this.btnUpdateSelectCSSyntax);
            this.groupBoxTools.Controls.Add(this.btnCheckNameRepet);
            this.groupBoxTools.Controls.Add(this.btnCheckCSKW);
            this.groupBoxTools.Location = new System.Drawing.Point(553, 613);
            this.groupBoxTools.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxTools.Name = "groupBoxTools";
            this.groupBoxTools.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxTools.Size = new System.Drawing.Size(472, 150);
            this.groupBoxTools.TabIndex = 7;
            this.groupBoxTools.TabStop = false;
            this.groupBoxTools.Text = "检查Excel错误";
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHelp.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnHelp.Cursor = System.Windows.Forms.Cursors.Help;
            this.btnHelp.Location = new System.Drawing.Point(304, 93);
            this.btnHelp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(140, 40);
            this.btnHelp.TabIndex = 0;
            this.btnHelp.Text = "我要帮助";
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // groupBoxOther
            // 
            this.groupBoxOther.Controls.Add(this.btnOpenCodeDir);
            this.groupBoxOther.Controls.Add(this.btnSqlite);
            this.groupBoxOther.Controls.Add(this.btnOpenTmlDir);
            this.groupBoxOther.Controls.Add(this.btnExecuteSql);
            this.groupBoxOther.Controls.Add(this.btnClearConsole);
            this.groupBoxOther.Controls.Add(this.btnHelp);
            this.groupBoxOther.Controls.Add(this.btnOpenDB);
            this.groupBoxOther.Location = new System.Drawing.Point(553, 788);
            this.groupBoxOther.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxOther.Name = "groupBoxOther";
            this.groupBoxOther.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxOther.Size = new System.Drawing.Size(472, 231);
            this.groupBoxOther.TabIndex = 7;
            this.groupBoxOther.TabStop = false;
            this.groupBoxOther.Text = "其它辅助项";
            // 
            // btnExecuteSql
            // 
            this.btnExecuteSql.Location = new System.Drawing.Point(8, 93);
            this.btnExecuteSql.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExecuteSql.Name = "btnExecuteSql";
            this.btnExecuteSql.Size = new System.Drawing.Size(128, 40);
            this.btnExecuteSql.TabIndex = 0;
            this.btnExecuteSql.Text = "执行Sql脚本";
            this.btnExecuteSql.UseVisualStyleBackColor = true;
            this.btnExecuteSql.Click += new System.EventHandler(this.btnExecuteSql_Click);
            // 
            // btnClearConsole
            // 
            this.btnClearConsole.Location = new System.Drawing.Point(304, 28);
            this.btnClearConsole.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClearConsole.Name = "btnClearConsole";
            this.btnClearConsole.Size = new System.Drawing.Size(128, 40);
            this.btnClearConsole.TabIndex = 0;
            this.btnClearConsole.Text = "清空控制台输出";
            this.btnClearConsole.UseVisualStyleBackColor = true;
            this.btnClearConsole.Click += new System.EventHandler(this.btnClearConsole_Click);
            // 
            // btnFileBrowser
            // 
            this.btnFileBrowser.Location = new System.Drawing.Point(24, 643);
            this.btnFileBrowser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnFileBrowser.Name = "btnFileBrowser";
            this.btnFileBrowser.Size = new System.Drawing.Size(155, 57);
            this.btnFileBrowser.TabIndex = 0;
            this.btnFileBrowser.Text = "浏览......";
            this.btnFileBrowser.UseVisualStyleBackColor = true;
            this.btnFileBrowser.Click += new System.EventHandler(this.btnFileBrowser_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // cbKSFormat
            // 
            this.cbKSFormat.AutoSize = true;
            this.cbKSFormat.Checked = true;
            this.cbKSFormat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbKSFormat.Location = new System.Drawing.Point(266, 742);
            this.cbKSFormat.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbKSFormat.Name = "cbKSFormat";
            this.cbKSFormat.Size = new System.Drawing.Size(131, 21);
            this.cbKSFormat.TabIndex = 6;
            this.cbKSFormat.Text = "KSFramework格式";
            this.cbKSFormat.UseVisualStyleBackColor = true;
            this.cbKSFormat.Click += new System.EventHandler(this.cbKSFormat_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1031, 1031);
            this.Controls.Add(this.groupBoxTools);
            this.Controls.Add(this.groupBoxOther);
            this.Controls.Add(this.cbKSFormat);
            this.Controls.Add(this.cbSimpleRule);
            this.Controls.Add(this.btnUpdateDB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbSrcPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbFileList);
            this.Controls.Add(this.btnCompileExcel);
            this.Controls.Add(this.btnCompileAll);
            this.Controls.Add(this.btnFileBrowser);
            this.Controls.Add(this.btnCompileSelect);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.Text = "Excel配置表编译工具";
            this.groupBoxTools.ResumeLayout(false);
            this.groupBoxOther.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnCompileSelect;
        private System.Windows.Forms.TextBox tbFileList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSrcPath;
        private System.Windows.Forms.Button btnCompileAll;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnUpdateCSSyntax;
        private System.Windows.Forms.Button btnCheckNameRepet;
        private System.Windows.Forms.Button btnOpenCodeDir;
        private System.Windows.Forms.Button btnOpenTmlDir;
        private System.Windows.Forms.Button btnCheckNameEmpty;
        private System.Windows.Forms.CheckBox cbSimpleRule;
        private System.Windows.Forms.Button btnCheckCSKW;
        private System.Windows.Forms.Button btnUpdateSelectCSSyntax;
        private System.Windows.Forms.Button btnCompileExcel;
        private System.Windows.Forms.Button btnSqlite;
        private System.Windows.Forms.Button btnUpdateDB;
        private System.Windows.Forms.Button btnOpenDB;
        private System.Windows.Forms.GroupBox groupBoxTools;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.GroupBox groupBoxOther;
        private System.Windows.Forms.Button btnClearConsole;
        private System.Windows.Forms.Button btnFileBrowser;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnExecuteSql;
        private System.Windows.Forms.CheckBox cbKSFormat;
    }
}

