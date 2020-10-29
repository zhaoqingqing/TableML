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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnUpdateCSSyntax = new System.Windows.Forms.Button();
            this.btnCheckNameRepet = new System.Windows.Forms.Button();
            this.btnOpenCodeDir = new System.Windows.Forms.Button();
            this.btnOpenLuaDir = new System.Windows.Forms.Button();
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
            this.btnOpenTml = new System.Windows.Forms.Button();
            this.btnExecuteSql = new System.Windows.Forms.Button();
            this.btnClearConsole = new System.Windows.Forms.Button();
            this.btnFileBrowser = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cbKSFormat = new System.Windows.Forms.CheckBox();
            this.cb_sql = new System.Windows.Forms.CheckBox();
            this.groupBoxTools.SuspendLayout();
            this.groupBoxOther.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCompileSelect
            // 
            this.btnCompileSelect.Location = new System.Drawing.Point(228, 454);
            this.btnCompileSelect.Name = "btnCompileSelect";
            this.btnCompileSelect.Size = new System.Drawing.Size(215, 40);
            this.btnCompileSelect.TabIndex = 0;
            this.btnCompileSelect.Text = "编译上面框中的Excel";
            this.btnCompileSelect.UseVisualStyleBackColor = true;
            this.btnCompileSelect.Click += new System.EventHandler(this.btnCompileSelect_Click);
            // 
            // tbFileList
            // 
            this.tbFileList.AllowDrop = true;
            this.tbFileList.Location = new System.Drawing.Point(21, 31);
            this.tbFileList.Multiline = true;
            this.tbFileList.Name = "tbFileList";
            this.tbFileList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbFileList.Size = new System.Drawing.Size(841, 397);
            this.tbFileList.TabIndex = 1;
            this.tbFileList.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbFileList_DragDrop);
            this.tbFileList.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbFileList_DragEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(347, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "请拖入需要编译的单个或多个Excel文件(可手动增加或删除路径)";
            // 
            // tbSrcPath
            // 
            this.tbSrcPath.AllowDrop = true;
            this.tbSrcPath.Location = new System.Drawing.Point(5, 596);
            this.tbSrcPath.Multiline = true;
            this.tbSrcPath.Name = "tbSrcPath";
            this.tbSrcPath.Size = new System.Drawing.Size(445, 30);
            this.tbSrcPath.TabIndex = 3;
            this.tbSrcPath.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbFileDir_DragDrop);
            this.tbSrcPath.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbFileDir_DragEnter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 563);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "编译这个目录下所有的Excel";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.Location = new System.Drawing.Point(3, 704);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(233, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "任务栏点击我选TableGUI，可查看输出日志";
            // 
            // btnUpdateCSSyntax
            // 
            this.btnUpdateCSSyntax.Location = new System.Drawing.Point(133, 72);
            this.btnUpdateCSSyntax.Name = "btnUpdateCSSyntax";
            this.btnUpdateCSSyntax.Size = new System.Drawing.Size(110, 28);
            this.btnUpdateCSSyntax.TabIndex = 0;
            this.btnUpdateCSSyntax.Text = "批量修改字段类型";
            this.btnUpdateCSSyntax.UseVisualStyleBackColor = true;
            this.btnUpdateCSSyntax.Click += new System.EventHandler(this.btnUpdateCSSyntax_Click);
            // 
            // btnCheckNameRepet
            // 
            this.btnCheckNameRepet.Location = new System.Drawing.Point(7, 24);
            this.btnCheckNameRepet.Name = "btnCheckNameRepet";
            this.btnCheckNameRepet.Size = new System.Drawing.Size(110, 28);
            this.btnCheckNameRepet.TabIndex = 0;
            this.btnCheckNameRepet.Text = "检查字段名重复";
            this.btnCheckNameRepet.UseVisualStyleBackColor = true;
            this.btnCheckNameRepet.Click += new System.EventHandler(this.btnCheckNameRepet_Click);
            // 
            // btnOpenCodeDir
            // 
            this.btnOpenCodeDir.Location = new System.Drawing.Point(7, 20);
            this.btnOpenCodeDir.Name = "btnOpenCodeDir";
            this.btnOpenCodeDir.Size = new System.Drawing.Size(110, 28);
            this.btnOpenCodeDir.TabIndex = 0;
            this.btnOpenCodeDir.Text = "打开生成的代码";
            this.btnOpenCodeDir.UseVisualStyleBackColor = true;
            this.btnOpenCodeDir.Click += new System.EventHandler(this.btnOpenCodeDir_Click);
            // 
            // btnOpenLuaDir
            // 
            this.btnOpenLuaDir.Location = new System.Drawing.Point(268, 20);
            this.btnOpenLuaDir.Name = "btnOpenLuaDir";
            this.btnOpenLuaDir.Size = new System.Drawing.Size(110, 28);
            this.btnOpenLuaDir.TabIndex = 0;
            this.btnOpenLuaDir.Text = "打开生成的Lua";
            this.btnOpenLuaDir.UseVisualStyleBackColor = true;
            this.btnOpenLuaDir.Click += new System.EventHandler(this.btnOpenLuaDir_Click);
            // 
            // btnCheckNameEmpty
            // 
            this.btnCheckNameEmpty.Location = new System.Drawing.Point(133, 24);
            this.btnCheckNameEmpty.Name = "btnCheckNameEmpty";
            this.btnCheckNameEmpty.Size = new System.Drawing.Size(110, 28);
            this.btnCheckNameEmpty.TabIndex = 0;
            this.btnCheckNameEmpty.Text = "检查字段名空白";
            this.btnCheckNameEmpty.UseVisualStyleBackColor = true;
            this.btnCheckNameEmpty.Click += new System.EventHandler(this.btnCheckNameEmpty_Click);
            // 
            // cbSimpleRule
            // 
            this.cbSimpleRule.AutoSize = true;
            this.cbSimpleRule.Location = new System.Drawing.Point(12, 524);
            this.cbSimpleRule.Name = "cbSimpleRule";
            this.cbSimpleRule.Size = new System.Drawing.Size(114, 16);
            this.cbSimpleRule.TabIndex = 6;
            this.cbSimpleRule.Text = "我是三行简单TSV";
            this.cbSimpleRule.UseVisualStyleBackColor = true;
            // 
            // btnCheckCSKW
            // 
            this.btnCheckCSKW.Location = new System.Drawing.Point(254, 24);
            this.btnCheckCSKW.Name = "btnCheckCSKW";
            this.btnCheckCSKW.Size = new System.Drawing.Size(146, 28);
            this.btnCheckCSKW.TabIndex = 0;
            this.btnCheckCSKW.Text = "检查字段名是否C#关键字";
            this.btnCheckCSKW.UseVisualStyleBackColor = true;
            this.btnCheckCSKW.Click += new System.EventHandler(this.btnCheckCSKW_Click);
            // 
            // btnUpdateSelectCSSyntax
            // 
            this.btnUpdateSelectCSSyntax.Location = new System.Drawing.Point(7, 72);
            this.btnUpdateSelectCSSyntax.Name = "btnUpdateSelectCSSyntax";
            this.btnUpdateSelectCSSyntax.Size = new System.Drawing.Size(110, 28);
            this.btnUpdateSelectCSSyntax.TabIndex = 0;
            this.btnUpdateSelectCSSyntax.Text = "改上面表字段类型";
            this.btnUpdateSelectCSSyntax.UseVisualStyleBackColor = true;
            this.btnUpdateSelectCSSyntax.Click += new System.EventHandler(this.btnUpdateSelectCSSyntax_Click);
            // 
            // btnCompileExcel
            // 
            this.btnCompileExcel.Location = new System.Drawing.Point(153, 639);
            this.btnCompileExcel.Name = "btnCompileExcel";
            this.btnCompileExcel.Size = new System.Drawing.Size(140, 40);
            this.btnCompileExcel.TabIndex = 0;
            this.btnCompileExcel.Text = "编译Excel";
            this.btnCompileExcel.UseVisualStyleBackColor = true;
            this.btnCompileExcel.Click += new System.EventHandler(this.btnCompileExcel_Click);
            // 
            // btnSqlite
            // 
            this.btnSqlite.Location = new System.Drawing.Point(7, 120);
            this.btnSqlite.Name = "btnSqlite";
            this.btnSqlite.Size = new System.Drawing.Size(110, 28);
            this.btnSqlite.TabIndex = 0;
            this.btnSqlite.Text = "插入十万条测试数据";
            this.btnSqlite.UseVisualStyleBackColor = true;
            this.btnSqlite.Click += new System.EventHandler(this.btnSqlite_Click);
            // 
            // btnUpdateDB
            // 
            this.btnUpdateDB.Location = new System.Drawing.Point(310, 639);
            this.btnUpdateDB.Name = "btnUpdateDB";
            this.btnUpdateDB.Size = new System.Drawing.Size(140, 40);
            this.btnUpdateDB.TabIndex = 0;
            this.btnUpdateDB.Text = "仅插入到Sqlite";
            this.btnUpdateDB.UseVisualStyleBackColor = true;
            this.btnUpdateDB.Click += new System.EventHandler(this.btnUpdateDB_Click);
            // 
            // btnOpenDB
            // 
            this.btnOpenDB.Location = new System.Drawing.Point(133, 66);
            this.btnOpenDB.Name = "btnOpenDB";
            this.btnOpenDB.Size = new System.Drawing.Size(120, 28);
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
            this.groupBoxTools.Location = new System.Drawing.Point(474, 433);
            this.groupBoxTools.Name = "groupBoxTools";
            this.groupBoxTools.Size = new System.Drawing.Size(405, 106);
            this.groupBoxTools.TabIndex = 7;
            this.groupBoxTools.TabStop = false;
            this.groupBoxTools.Text = "检查Excel错误";
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHelp.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnHelp.Cursor = System.Windows.Forms.Cursors.Help;
            this.btnHelp.Location = new System.Drawing.Point(268, 120);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(120, 28);
            this.btnHelp.TabIndex = 0;
            this.btnHelp.Text = "我要帮助";
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // groupBoxOther
            // 
            this.groupBoxOther.Controls.Add(this.btnOpenCodeDir);
            this.groupBoxOther.Controls.Add(this.btnSqlite);
            this.groupBoxOther.Controls.Add(this.btnOpenTml);
            this.groupBoxOther.Controls.Add(this.btnOpenLuaDir);
            this.groupBoxOther.Controls.Add(this.btnExecuteSql);
            this.groupBoxOther.Controls.Add(this.btnClearConsole);
            this.groupBoxOther.Controls.Add(this.btnHelp);
            this.groupBoxOther.Controls.Add(this.btnOpenDB);
            this.groupBoxOther.Location = new System.Drawing.Point(474, 556);
            this.groupBoxOther.Name = "groupBoxOther";
            this.groupBoxOther.Size = new System.Drawing.Size(405, 163);
            this.groupBoxOther.TabIndex = 7;
            this.groupBoxOther.TabStop = false;
            this.groupBoxOther.Text = "其它辅助项";
            // 
            // btnOpenTml
            // 
            this.btnOpenTml.Location = new System.Drawing.Point(133, 21);
            this.btnOpenTml.Name = "btnOpenTml";
            this.btnOpenTml.Size = new System.Drawing.Size(110, 28);
            this.btnOpenTml.TabIndex = 0;
            this.btnOpenTml.Text = "打开生成的CSV";
            this.btnOpenTml.UseVisualStyleBackColor = true;
            this.btnOpenTml.Click += new System.EventHandler(this.btnOpenTmlDir_Click);
            // 
            // btnExecuteSql
            // 
            this.btnExecuteSql.Location = new System.Drawing.Point(268, 66);
            this.btnExecuteSql.Name = "btnExecuteSql";
            this.btnExecuteSql.Size = new System.Drawing.Size(110, 28);
            this.btnExecuteSql.TabIndex = 0;
            this.btnExecuteSql.Text = "执行Sql脚本";
            this.btnExecuteSql.UseVisualStyleBackColor = true;
            this.btnExecuteSql.Click += new System.EventHandler(this.btnExecuteSql_Click);
            // 
            // btnClearConsole
            // 
            this.btnClearConsole.Location = new System.Drawing.Point(6, 66);
            this.btnClearConsole.Name = "btnClearConsole";
            this.btnClearConsole.Size = new System.Drawing.Size(110, 28);
            this.btnClearConsole.TabIndex = 0;
            this.btnClearConsole.Text = "清空控制台输出";
            this.btnClearConsole.UseVisualStyleBackColor = true;
            this.btnClearConsole.Click += new System.EventHandler(this.btnClearConsole_Click);
            // 
            // btnFileBrowser
            // 
            this.btnFileBrowser.Location = new System.Drawing.Point(21, 454);
            this.btnFileBrowser.Name = "btnFileBrowser";
            this.btnFileBrowser.Size = new System.Drawing.Size(133, 40);
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
            this.cbKSFormat.Location = new System.Drawing.Point(179, 523);
            this.cbKSFormat.Name = "cbKSFormat";
            this.cbKSFormat.Size = new System.Drawing.Size(114, 16);
            this.cbKSFormat.TabIndex = 6;
            this.cbKSFormat.Text = "KSFramework格式";
            this.cbKSFormat.UseVisualStyleBackColor = true;
            this.cbKSFormat.Click += new System.EventHandler(this.cbKSFormat_Click);
            // 
            // cb_sql
            // 
            this.cb_sql.AutoSize = true;
            this.cb_sql.Checked = true;
            this.cb_sql.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_sql.Location = new System.Drawing.Point(329, 524);
            this.cb_sql.Name = "cb_sql";
            this.cb_sql.Size = new System.Drawing.Size(96, 16);
            this.cb_sql.TabIndex = 6;
            this.cb_sql.Text = "导出到SQLite";
            this.cb_sql.UseVisualStyleBackColor = true;
            this.cb_sql.CheckedChanged += new System.EventHandler(this.Cb_sql_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 728);
            this.Controls.Add(this.groupBoxTools);
            this.Controls.Add(this.groupBoxOther);
            this.Controls.Add(this.cb_sql);
            this.Controls.Add(this.cbKSFormat);
            this.Controls.Add(this.cbSimpleRule);
            this.Controls.Add(this.btnUpdateDB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbSrcPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbFileList);
            this.Controls.Add(this.btnCompileExcel);
            this.Controls.Add(this.btnFileBrowser);
            this.Controls.Add(this.btnCompileSelect);
            this.Name = "MainForm";
            this.Text = "Excel配置表编译工具";
            this.groupBoxTools.ResumeLayout(false);
            this.groupBoxOther.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnOpenLuaDir;
        private System.Windows.Forms.Button btnOpenTml;

        #endregion

        private System.Windows.Forms.Button btnCompileSelect;
        private System.Windows.Forms.TextBox tbFileList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSrcPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnUpdateCSSyntax;
        private System.Windows.Forms.Button btnCheckNameRepet;
        private System.Windows.Forms.Button btnOpenCodeDir;
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
        private System.Windows.Forms.CheckBox cb_sql;
    }
}

