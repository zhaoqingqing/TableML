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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCompileSelect = new System.Windows.Forms.Button();
            this.tbFileList = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbFileDir = new System.Windows.Forms.TextBox();
            this.btnCompileAll = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnUpdateCSSyntax = new System.Windows.Forms.Button();
            this.txtCodePath = new System.Windows.Forms.TextBox();
            this.btnSyncCode = new System.Windows.Forms.Button();
            this.btnSyncTml = new System.Windows.Forms.Button();
            this.txtTmlPath = new System.Windows.Forms.TextBox();
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
            this.cbGenCS = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnCompileSelect
            // 
            this.btnCompileSelect.Location = new System.Drawing.Point(46, 443);
            this.btnCompileSelect.Name = "btnCompileSelect";
            this.btnCompileSelect.Size = new System.Drawing.Size(300, 50);
            this.btnCompileSelect.TabIndex = 0;
            this.btnCompileSelect.Text = "编译上面框中的Excel";
            this.btnCompileSelect.UseVisualStyleBackColor = true;
            this.btnCompileSelect.Click += new System.EventHandler(this.btnCompileSelect_Click);
            // 
            // tbFileList
            // 
            this.tbFileList.AllowDrop = true;
            this.tbFileList.Location = new System.Drawing.Point(3, 36);
            this.tbFileList.Multiline = true;
            this.tbFileList.Name = "tbFileList";
            this.tbFileList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbFileList.Size = new System.Drawing.Size(447, 401);
            this.tbFileList.TabIndex = 1;
            this.tbFileList.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbFileList_DragDrop);
            this.tbFileList.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbFileList_DragEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(347, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "请拖入需要编译的单个或多个Excel文件(可手动增加或删除路径)";
            // 
            // tbFileDir
            // 
            this.tbFileDir.AllowDrop = true;
            this.tbFileDir.Location = new System.Drawing.Point(12, 596);
            this.tbFileDir.Multiline = true;
            this.tbFileDir.Name = "tbFileDir";
            this.tbFileDir.Size = new System.Drawing.Size(438, 30);
            this.tbFileDir.TabIndex = 3;
            this.tbFileDir.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbFileDir_DragDrop);
            this.tbFileDir.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbFileDir_DragEnter);
            // 
            // btnCompileAll
            // 
            this.btnCompileAll.Location = new System.Drawing.Point(12, 639);
            this.btnCompileAll.Name = "btnCompileAll";
            this.btnCompileAll.Size = new System.Drawing.Size(140, 40);
            this.btnCompileAll.TabIndex = 0;
            this.btnCompileAll.Text = "编译并插入到SQLite中";
            this.btnCompileAll.UseVisualStyleBackColor = true;
            this.btnCompileAll.Click += new System.EventHandler(this.btnCompileAll_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 563);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "编译这个目录下的Excel";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.Location = new System.Drawing.Point(320, 704);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(233, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "任务栏点击我选TableGUI，可查看输出日志";
            // 
            // btnUpdateCSSyntax
            // 
            this.btnUpdateCSSyntax.Location = new System.Drawing.Point(660, 422);
            this.btnUpdateCSSyntax.Name = "btnUpdateCSSyntax";
            this.btnUpdateCSSyntax.Size = new System.Drawing.Size(169, 40);
            this.btnUpdateCSSyntax.TabIndex = 0;
            this.btnUpdateCSSyntax.Text = "批量改表的前端字段类型";
            this.btnUpdateCSSyntax.UseVisualStyleBackColor = true;
            this.btnUpdateCSSyntax.Click += new System.EventHandler(this.btnUpdateCSSyntax_Click);
            // 
            // txtCodePath
            // 
            this.txtCodePath.Location = new System.Drawing.Point(474, 36);
            this.txtCodePath.Name = "txtCodePath";
            this.txtCodePath.Size = new System.Drawing.Size(398, 21);
            this.txtCodePath.TabIndex = 1;
            // 
            // btnSyncCode
            // 
            this.btnSyncCode.Location = new System.Drawing.Point(474, 73);
            this.btnSyncCode.Name = "btnSyncCode";
            this.btnSyncCode.Size = new System.Drawing.Size(169, 40);
            this.btnSyncCode.TabIndex = 0;
            this.btnSyncCode.Text = "生成代码同步到客户端";
            this.btnSyncCode.UseVisualStyleBackColor = true;
            this.btnSyncCode.Click += new System.EventHandler(this.btnSyncCode_Click);
            // 
            // btnSyncTml
            // 
            this.btnSyncTml.Location = new System.Drawing.Point(474, 182);
            this.btnSyncTml.Name = "btnSyncTml";
            this.btnSyncTml.Size = new System.Drawing.Size(169, 40);
            this.btnSyncTml.TabIndex = 0;
            this.btnSyncTml.Text = "编译后表同步到客户端";
            this.btnSyncTml.UseVisualStyleBackColor = true;
            this.btnSyncTml.Click += new System.EventHandler(this.btnSyncTml_Click);
            // 
            // txtTmlPath
            // 
            this.txtTmlPath.Location = new System.Drawing.Point(474, 145);
            this.txtTmlPath.Name = "txtTmlPath";
            this.txtTmlPath.Size = new System.Drawing.Size(398, 21);
            this.txtTmlPath.TabIndex = 1;
            // 
            // btnCheckNameRepet
            // 
            this.btnCheckNameRepet.Location = new System.Drawing.Point(660, 240);
            this.btnCheckNameRepet.Name = "btnCheckNameRepet";
            this.btnCheckNameRepet.Size = new System.Drawing.Size(169, 40);
            this.btnCheckNameRepet.TabIndex = 0;
            this.btnCheckNameRepet.Text = "检查前端字段名重复";
            this.btnCheckNameRepet.UseVisualStyleBackColor = true;
            this.btnCheckNameRepet.Click += new System.EventHandler(this.btnCheckNameRepet_Click);
            // 
            // btnOpenCodeDir
            // 
            this.btnOpenCodeDir.Location = new System.Drawing.Point(474, 365);
            this.btnOpenCodeDir.Name = "btnOpenCodeDir";
            this.btnOpenCodeDir.Size = new System.Drawing.Size(169, 40);
            this.btnOpenCodeDir.TabIndex = 0;
            this.btnOpenCodeDir.Text = "打开生成的代码目录";
            this.btnOpenCodeDir.UseVisualStyleBackColor = true;
            this.btnOpenCodeDir.Click += new System.EventHandler(this.btnOpenCodeDir_Click);
            // 
            // btnOpenTmlDir
            // 
            this.btnOpenTmlDir.Location = new System.Drawing.Point(660, 365);
            this.btnOpenTmlDir.Name = "btnOpenTmlDir";
            this.btnOpenTmlDir.Size = new System.Drawing.Size(169, 40);
            this.btnOpenTmlDir.TabIndex = 0;
            this.btnOpenTmlDir.Text = "打开编译后的表目录";
            this.btnOpenTmlDir.UseVisualStyleBackColor = true;
            this.btnOpenTmlDir.Click += new System.EventHandler(this.btnOpenTmlDir_Click);
            // 
            // btnCheckNameEmpty
            // 
            this.btnCheckNameEmpty.Location = new System.Drawing.Point(474, 301);
            this.btnCheckNameEmpty.Name = "btnCheckNameEmpty";
            this.btnCheckNameEmpty.Size = new System.Drawing.Size(169, 40);
            this.btnCheckNameEmpty.TabIndex = 0;
            this.btnCheckNameEmpty.Text = "检查前端字段名空白";
            this.btnCheckNameEmpty.UseVisualStyleBackColor = true;
            this.btnCheckNameEmpty.Click += new System.EventHandler(this.btnCheckNameEmpty_Click);
            // 
            // cbSimpleRule
            // 
            this.cbSimpleRule.AutoSize = true;
            this.cbSimpleRule.Location = new System.Drawing.Point(12, 524);
            this.cbSimpleRule.Name = "cbSimpleRule";
            this.cbSimpleRule.Size = new System.Drawing.Size(204, 16);
            this.cbSimpleRule.TabIndex = 6;
            this.cbSimpleRule.Text = "是否 name|type|comment 三行TSV";
            this.cbSimpleRule.UseVisualStyleBackColor = true;
            // 
            // btnCheckCSKW
            // 
            this.btnCheckCSKW.Location = new System.Drawing.Point(660, 301);
            this.btnCheckCSKW.Name = "btnCheckCSKW";
            this.btnCheckCSKW.Size = new System.Drawing.Size(169, 40);
            this.btnCheckCSKW.TabIndex = 0;
            this.btnCheckCSKW.Text = "检查前端字段名是否C#关键字";
            this.btnCheckCSKW.UseVisualStyleBackColor = true;
            this.btnCheckCSKW.Click += new System.EventHandler(this.btnCheckCSKW_Click);
            // 
            // btnUpdateSelectCSSyntax
            // 
            this.btnUpdateSelectCSSyntax.Location = new System.Drawing.Point(474, 422);
            this.btnUpdateSelectCSSyntax.Name = "btnUpdateSelectCSSyntax";
            this.btnUpdateSelectCSSyntax.Size = new System.Drawing.Size(169, 40);
            this.btnUpdateSelectCSSyntax.TabIndex = 0;
            this.btnUpdateSelectCSSyntax.Text = "改框中表的前端字段类型";
            this.btnUpdateSelectCSSyntax.UseVisualStyleBackColor = true;
            this.btnUpdateSelectCSSyntax.Click += new System.EventHandler(this.btnUpdateSelectCSSyntax_Click);
            // 
            // btnCompileExcel
            // 
            this.btnCompileExcel.Location = new System.Drawing.Point(170, 639);
            this.btnCompileExcel.Name = "btnCompileExcel";
            this.btnCompileExcel.Size = new System.Drawing.Size(140, 40);
            this.btnCompileExcel.TabIndex = 0;
            this.btnCompileExcel.Text = "仅编译Excel";
            this.btnCompileExcel.UseVisualStyleBackColor = true;
            this.btnCompileExcel.Click += new System.EventHandler(this.btnCompileExcel_Click);
            // 
            // btnSqlite
            // 
            this.btnSqlite.Location = new System.Drawing.Point(474, 482);
            this.btnSqlite.Name = "btnSqlite";
            this.btnSqlite.Size = new System.Drawing.Size(169, 40);
            this.btnSqlite.TabIndex = 0;
            this.btnSqlite.Text = "测试Sqlite";
            this.btnSqlite.UseVisualStyleBackColor = true;
            this.btnSqlite.Click += new System.EventHandler(this.btnSqlite_Click);
            // 
            // btnUpdateDB
            // 
            this.btnUpdateDB.Location = new System.Drawing.Point(322, 639);
            this.btnUpdateDB.Name = "btnUpdateDB";
            this.btnUpdateDB.Size = new System.Drawing.Size(140, 40);
            this.btnUpdateDB.TabIndex = 0;
            this.btnUpdateDB.Text = "仅插入到Sqlite";
            this.btnUpdateDB.UseVisualStyleBackColor = true;
            this.btnUpdateDB.Click += new System.EventHandler(this.btnUpdateDB_Click);
            // 
            // cbGenCS
            // 
            this.cbGenCS.AutoSize = true;
            this.cbGenCS.Location = new System.Drawing.Point(342, 524);
            this.cbGenCS.Name = "cbGenCS";
            this.cbGenCS.Size = new System.Drawing.Size(108, 16);
            this.cbGenCS.TabIndex = 6;
            this.cbGenCS.Text = "生成CSharp代码";
            this.cbGenCS.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 728);
            this.Controls.Add(this.cbGenCS);
            this.Controls.Add(this.cbSimpleRule);
            this.Controls.Add(this.txtTmlPath);
            this.Controls.Add(this.btnOpenTmlDir);
            this.Controls.Add(this.btnOpenCodeDir);
            this.Controls.Add(this.btnSyncTml);
            this.Controls.Add(this.txtCodePath);
            this.Controls.Add(this.btnSyncCode);
            this.Controls.Add(this.btnCheckCSKW);
            this.Controls.Add(this.btnCheckNameEmpty);
            this.Controls.Add(this.btnCheckNameRepet);
            this.Controls.Add(this.btnUpdateDB);
            this.Controls.Add(this.btnSqlite);
            this.Controls.Add(this.btnUpdateSelectCSSyntax);
            this.Controls.Add(this.btnUpdateCSSyntax);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbFileDir);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbFileList);
            this.Controls.Add(this.btnCompileExcel);
            this.Controls.Add(this.btnCompileAll);
            this.Controls.Add(this.btnCompileSelect);
            this.Name = "MainForm";
            this.Text = "Excel配置表编译 For C#";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCompileSelect;
        private System.Windows.Forms.TextBox tbFileList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbFileDir;
        private System.Windows.Forms.Button btnCompileAll;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnUpdateCSSyntax;
        private System.Windows.Forms.TextBox txtCodePath;
        private System.Windows.Forms.Button btnSyncCode;
        private System.Windows.Forms.Button btnSyncTml;
        private System.Windows.Forms.TextBox txtTmlPath;
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
        private System.Windows.Forms.CheckBox cbGenCS;
    }
}

