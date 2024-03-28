namespace Bars.FIAS.Converter
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbConnectionString = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.regionList = new System.Windows.Forms.CheckedListBox();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.buttonUnselectAll = new System.Windows.Forms.Button();
            this.label_ИнформацияОВыделении = new System.Windows.Forms.Label();
            this.buttonPush = new System.Windows.Forms.Button();
            this.label_ИнформацияОПроцессе = new System.Windows.Forms.Label();
            this.buttonSelectFiasPath = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.pathFias = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbTypeOLEDB = new System.Windows.Forms.ComboBox();
            this.TestConnectionBtn = new System.Windows.Forms.Button();
            this.saveConnectionBtn = new System.Windows.Forms.Button();
            this.cbDataBaseType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bwLoader = new System.ComponentModel.BackgroundWorker();
            this.bwRegionList = new System.ComponentModel.BackgroundWorker();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.cbTypeLoad = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbConnectionString
            // 
            this.tbConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbConnectionString.Location = new System.Drawing.Point(167, 46);
            this.tbConnectionString.Name = "tbConnectionString";
            this.tbConnectionString.Size = new System.Drawing.Size(501, 21);
            this.tbConnectionString.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Строка подключения:";
            // 
            // regionList
            // 
            this.regionList.CheckOnClick = true;
            this.regionList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.regionList.FormattingEnabled = true;
            this.regionList.Location = new System.Drawing.Point(3, 46);
            this.regionList.Name = "regionList";
            this.regionList.Size = new System.Drawing.Size(674, 404);
            this.regionList.TabIndex = 6;
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelectAll.Location = new System.Drawing.Point(6, 6);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(112, 23);
            this.buttonSelectAll.TabIndex = 8;
            this.buttonSelectAll.Text = "Выделить все";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.ButtonSelectAllClick);
            // 
            // buttonUnselectAll
            // 
            this.buttonUnselectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUnselectAll.Location = new System.Drawing.Point(124, 6);
            this.buttonUnselectAll.Name = "buttonUnselectAll";
            this.buttonUnselectAll.Size = new System.Drawing.Size(112, 23);
            this.buttonUnselectAll.TabIndex = 9;
            this.buttonUnselectAll.Text = "Убрать выделение";
            this.buttonUnselectAll.UseVisualStyleBackColor = true;
            this.buttonUnselectAll.Click += new System.EventHandler(this.ButtonUnselectAllClick);
            // 
            // label_ИнформацияОВыделении
            // 
            this.label_ИнформацияОВыделении.AutoSize = true;
            this.label_ИнформацияОВыделении.Location = new System.Drawing.Point(9, 420);
            this.label_ИнформацияОВыделении.Name = "label_ИнформацияОВыделении";
            this.label_ИнформацияОВыделении.Size = new System.Drawing.Size(0, 13);
            this.label_ИнформацияОВыделении.TabIndex = 10;
            this.label_ИнформацияОВыделении.Visible = false;
            // 
            // buttonPush
            // 
            this.buttonPush.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPush.Location = new System.Drawing.Point(553, 6);
            this.buttonPush.Name = "buttonPush";
            this.buttonPush.Size = new System.Drawing.Size(121, 23);
            this.buttonPush.TabIndex = 11;
            this.buttonPush.Text = "Загрузить";
            this.buttonPush.UseVisualStyleBackColor = true;
            this.buttonPush.Click += new System.EventHandler(this.ButtonPushFiasClick);
            // 
            // label_ИнформацияОПроцессе
            // 
            this.label_ИнформацияОПроцессе.AutoSize = true;
            this.label_ИнформацияОПроцессе.Location = new System.Drawing.Point(12, 449);
            this.label_ИнформацияОПроцессе.Name = "label_ИнформацияОПроцессе";
            this.label_ИнформацияОПроцессе.Size = new System.Drawing.Size(0, 13);
            this.label_ИнформацияОПроцессе.TabIndex = 13;
            // 
            // buttonSelectFiasPath
            // 
            this.buttonSelectFiasPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectFiasPath.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSelectFiasPath.Location = new System.Drawing.Point(649, 11);
            this.buttonSelectFiasPath.Name = "buttonSelectFiasPath";
            this.buttonSelectFiasPath.Size = new System.Drawing.Size(22, 23);
            this.buttonSelectFiasPath.TabIndex = 16;
            this.buttonSelectFiasPath.Text = "...";
            this.buttonSelectFiasPath.UseVisualStyleBackColor = true;
            this.buttonSelectFiasPath.Click += new System.EventHandler(this.ButtonSelectFiasPath);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Путь к папке с ФИАС:";
            // 
            // pathFias
            // 
            this.pathFias.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathFias.Location = new System.Drawing.Point(142, 12);
            this.pathFias.Name = "pathFias";
            this.pathFias.ReadOnly = true;
            this.pathFias.Size = new System.Drawing.Size(501, 21);
            this.pathFias.TabIndex = 14;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cbTypeOLEDB);
            this.groupBox1.Controls.Add(this.TestConnectionBtn);
            this.groupBox1.Controls.Add(this.saveConnectionBtn);
            this.groupBox1.Controls.Add(this.cbDataBaseType);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbConnectionString);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(674, 132);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Настройки БД";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 106);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(149, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Драйвер OLEDB (32 или 64):";
            // 
            // cbTypeOLEDB
            // 
            this.cbTypeOLEDB.FormattingEnabled = true;
            this.cbTypeOLEDB.Location = new System.Drawing.Point(167, 103);
            this.cbTypeOLEDB.Name = "cbTypeOLEDB";
            this.cbTypeOLEDB.Size = new System.Drawing.Size(225, 21);
            this.cbTypeOLEDB.TabIndex = 9;
            // 
            // TestConnectionBtn
            // 
            this.TestConnectionBtn.Location = new System.Drawing.Point(167, 17);
            this.TestConnectionBtn.Name = "TestConnectionBtn";
            this.TestConnectionBtn.Size = new System.Drawing.Size(115, 23);
            this.TestConnectionBtn.TabIndex = 8;
            this.TestConnectionBtn.Text = "Тест подключения";
            this.TestConnectionBtn.UseVisualStyleBackColor = true;
            this.TestConnectionBtn.Click += new System.EventHandler(this.TestConnectionBtn_Click);
            // 
            // saveConnectionBtn
            // 
            this.saveConnectionBtn.Location = new System.Drawing.Point(12, 17);
            this.saveConnectionBtn.Name = "saveConnectionBtn";
            this.saveConnectionBtn.Size = new System.Drawing.Size(149, 23);
            this.saveConnectionBtn.TabIndex = 7;
            this.saveConnectionBtn.Text = "Сохранить настройки";
            this.saveConnectionBtn.UseVisualStyleBackColor = true;
            this.saveConnectionBtn.Click += new System.EventHandler(this.saveConnectionBtn_Click);
            // 
            // cbDataBaseType
            // 
            this.cbDataBaseType.Enabled = false;
            this.cbDataBaseType.FormattingEnabled = true;
            this.cbDataBaseType.Location = new System.Drawing.Point(167, 76);
            this.cbDataBaseType.Name = "cbDataBaseType";
            this.cbDataBaseType.Size = new System.Drawing.Size(225, 21);
            this.cbDataBaseType.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(115, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Тип БД:";
            // 
            // bwLoader
            // 
            this.bwLoader.WorkerReportsProgress = true;
            this.bwLoader.WorkerSupportsCancellation = true;
            this.bwLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwLoaderDoWork);
            this.bwLoader.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwLoaderProgressChanged);
            this.bwLoader.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwLoaderRunWorkerCompleted);
            // 
            // bwRegionList
            // 
            this.bwRegionList.WorkerReportsProgress = true;
            this.bwRegionList.WorkerSupportsCancellation = true;
            this.bwRegionList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.RegionListDoWork);
            this.bwRegionList.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.RegionListProgressChanged);
            this.bwRegionList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.RegionListRunWorkerCompleted);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(0, 39);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(688, 479);
            this.tabControl1.TabIndex = 21;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.buttonUnselectAll);
            this.tabPage1.Controls.Add(this.regionList);
            this.tabPage1.Controls.Add(this.buttonSelectAll);
            this.tabPage1.Controls.Add(this.buttonPush);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(680, 453);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Регионы";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.cbTypeLoad);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(680, 453);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Настройки";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(74, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Режим загрузки:";
            // 
            // cbTypeLoad
            // 
            this.cbTypeLoad.FormattingEnabled = true;
            this.cbTypeLoad.Items.AddRange(new object[] {
            "Загрузка ФИАС с предварительной очисткой таблицы",
            "Загрузка обновления, без очистки таблицы"});
            this.cbTypeLoad.Location = new System.Drawing.Point(170, 148);
            this.cbTypeLoad.Name = "cbTypeLoad";
            this.cbTypeLoad.Size = new System.Drawing.Size(502, 21);
            this.cbTypeLoad.TabIndex = 20;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 518);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonSelectFiasPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pathFias);
            this.Controls.Add(this.label_ИнформацияОПроцессе);
            this.Controls.Add(this.label_ИнформацияОВыделении);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Конвертер ФИАС";
            this.LocationChanged += new System.EventHandler(this.MainFormLocationChanged);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbConnectionString;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox regionList;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Button buttonUnselectAll;
        private System.Windows.Forms.Label label_ИнформацияОВыделении;
        private System.Windows.Forms.Button buttonPush;
        public System.Windows.Forms.Label label_ИнформацияОПроцессе;
        private System.Windows.Forms.Button buttonSelectFiasPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox pathFias;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.ComponentModel.BackgroundWorker bwLoader;
        private System.ComponentModel.BackgroundWorker bwRegionList;
        private System.Windows.Forms.ComboBox cbDataBaseType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button TestConnectionBtn;
        private System.Windows.Forms.Button saveConnectionBtn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbTypeLoad;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbTypeOLEDB;
    }
}
