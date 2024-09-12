namespace FPC
{
    partial class FrmMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panelMain = new System.Windows.Forms.Panel();
            this.btnSettings = new System.Windows.Forms.Button();
            this.panelKey = new System.Windows.Forms.Panel();
            this.cmbKey = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.rtbKey = new System.Windows.Forms.RichTextBox();
            this.lsbWsStatuses = new System.Windows.Forms.ListBox();
            this.btnGetXReport = new System.Windows.Forms.Button();
            this.btnGetFDByNumber = new System.Windows.Forms.Button();
            this.txtFdNumber = new System.Windows.Forms.TextBox();
            this.btnGetLastFD = new System.Windows.Forms.Button();
            this.btnCloseShift = new System.Windows.Forms.Button();
            this.btnOpenShift = new System.Windows.Forms.Button();
            this.btnDeviceStatus = new System.Windows.Forms.Button();
            this.rtbResponse = new System.Windows.Forms.RichTextBox();
            this.cmbUsers = new System.Windows.Forms.ComboBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.panelMain.SuspendLayout();
            this.panelKey.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panelMain.Controls.Add(this.btnSettings);
            this.panelMain.Controls.Add(this.panelKey);
            this.panelMain.Controls.Add(this.lsbWsStatuses);
            this.panelMain.Controls.Add(this.btnGetXReport);
            this.panelMain.Controls.Add(this.btnGetFDByNumber);
            this.panelMain.Controls.Add(this.txtFdNumber);
            this.panelMain.Controls.Add(this.btnGetLastFD);
            this.panelMain.Controls.Add(this.btnCloseShift);
            this.panelMain.Controls.Add(this.btnOpenShift);
            this.panelMain.Controls.Add(this.btnDeviceStatus);
            this.panelMain.Controls.Add(this.rtbResponse);
            this.panelMain.Controls.Add(this.cmbUsers);
            this.panelMain.Location = new System.Drawing.Point(3, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(768, 491);
            this.panelMain.TabIndex = 6;
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(741, 0);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(27, 22);
            this.btnSettings.TabIndex = 7;
            this.btnSettings.Text = "...";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.button2_Click);
            // 
            // panelKey
            // 
            this.panelKey.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panelKey.Controls.Add(this.cmbKey);
            this.panelKey.Controls.Add(this.button3);
            this.panelKey.Controls.Add(this.btnPaste);
            this.panelKey.Controls.Add(this.button1);
            this.panelKey.Controls.Add(this.rtbKey);
            this.panelKey.Location = new System.Drawing.Point(0, 3);
            this.panelKey.Name = "panelKey";
            this.panelKey.Size = new System.Drawing.Size(771, 488);
            this.panelKey.TabIndex = 7;
            this.panelKey.Paint += new System.Windows.Forms.PaintEventHandler(this.panelKey_Paint);
            // 
            // cmbKey
            // 
            this.cmbKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.cmbKey.FormattingEnabled = true;
            this.cmbKey.Location = new System.Drawing.Point(122, 53);
            this.cmbKey.Name = "cmbKey";
            this.cmbKey.Size = new System.Drawing.Size(438, 32);
            this.cmbKey.TabIndex = 7;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(294, 396);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(120, 38);
            this.button3.TabIndex = 17;
            this.button3.Text = "Ok";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(565, 358);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(84, 32);
            this.btnPaste.TabIndex = 16;
            this.btnPaste.Text = "Вставить";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(566, 53);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(83, 31);
            this.button1.TabIndex = 15;
            this.button1.Text = "Копировать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // rtbKey
            // 
            this.rtbKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.rtbKey.Location = new System.Drawing.Point(122, 90);
            this.rtbKey.Name = "rtbKey";
            this.rtbKey.Size = new System.Drawing.Size(438, 300);
            this.rtbKey.TabIndex = 14;
            this.rtbKey.Text = "";
            // 
            // lsbWsStatuses
            // 
            this.lsbWsStatuses.FormattingEnabled = true;
            this.lsbWsStatuses.Location = new System.Drawing.Point(34, 369);
            this.lsbWsStatuses.Name = "lsbWsStatuses";
            this.lsbWsStatuses.Size = new System.Drawing.Size(718, 108);
            this.lsbWsStatuses.TabIndex = 15;
            // 
            // btnGetXReport
            // 
            this.btnGetXReport.Location = new System.Drawing.Point(591, 30);
            this.btnGetXReport.Name = "btnGetXReport";
            this.btnGetXReport.Size = new System.Drawing.Size(161, 34);
            this.btnGetXReport.TabIndex = 6;
            this.btnGetXReport.Text = "Печать (X-отчет)";
            this.btnGetXReport.UseVisualStyleBackColor = true;
            this.btnGetXReport.Click += new System.EventHandler(this.btnGetXReport_Click_1);
            // 
            // btnGetFDByNumber
            // 
            this.btnGetFDByNumber.Location = new System.Drawing.Point(591, 74);
            this.btnGetFDByNumber.Name = "btnGetFDByNumber";
            this.btnGetFDByNumber.Size = new System.Drawing.Size(161, 36);
            this.btnGetFDByNumber.TabIndex = 10;
            this.btnGetFDByNumber.Text = "Печать по номеру ";
            this.btnGetFDByNumber.UseVisualStyleBackColor = true;
            this.btnGetFDByNumber.Click += new System.EventHandler(this.btnGetFDByNumber_Click_1);
            // 
            // txtFdNumber
            // 
            this.txtFdNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F);
            this.txtFdNumber.Location = new System.Drawing.Point(388, 78);
            this.txtFdNumber.Name = "txtFdNumber";
            this.txtFdNumber.Size = new System.Drawing.Size(197, 32);
            this.txtFdNumber.TabIndex = 7;
            // 
            // btnGetLastFD
            // 
            this.btnGetLastFD.Location = new System.Drawing.Point(391, 30);
            this.btnGetLastFD.Name = "btnGetLastFD";
            this.btnGetLastFD.Size = new System.Drawing.Size(194, 38);
            this.btnGetLastFD.TabIndex = 8;
            this.btnGetLastFD.Text = "Печать последнего фискального документа";
            this.btnGetLastFD.UseVisualStyleBackColor = true;
            this.btnGetLastFD.Click += new System.EventHandler(this.btnGetLastFD_Click_1);
            // 
            // btnCloseShift
            // 
            this.btnCloseShift.Location = new System.Drawing.Point(214, 72);
            this.btnCloseShift.Name = "btnCloseShift";
            this.btnCloseShift.Size = new System.Drawing.Size(168, 38);
            this.btnCloseShift.TabIndex = 14;
            this.btnCloseShift.Text = "Закрыть смену";
            this.btnCloseShift.UseVisualStyleBackColor = true;
            this.btnCloseShift.Click += new System.EventHandler(this.btnCloseShift_Click_1);
            // 
            // btnOpenShift
            // 
            this.btnOpenShift.Location = new System.Drawing.Point(214, 30);
            this.btnOpenShift.Name = "btnOpenShift";
            this.btnOpenShift.Size = new System.Drawing.Size(168, 38);
            this.btnOpenShift.TabIndex = 12;
            this.btnOpenShift.Text = "Открыть смену";
            this.btnOpenShift.UseVisualStyleBackColor = true;
            this.btnOpenShift.Click += new System.EventHandler(this.btnOpenShift_Click_1);
            // 
            // btnDeviceStatus
            // 
            this.btnDeviceStatus.Location = new System.Drawing.Point(31, 74);
            this.btnDeviceStatus.Name = "btnDeviceStatus";
            this.btnDeviceStatus.Size = new System.Drawing.Size(168, 36);
            this.btnDeviceStatus.TabIndex = 9;
            this.btnDeviceStatus.Text = "Проверить соединение";
            this.btnDeviceStatus.UseVisualStyleBackColor = true;
            this.btnDeviceStatus.Click += new System.EventHandler(this.btnDeviceStatus_Click_1);
            // 
            // rtbResponse
            // 
            this.rtbResponse.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.rtbResponse.ForeColor = System.Drawing.SystemColors.Window;
            this.rtbResponse.Location = new System.Drawing.Point(34, 123);
            this.rtbResponse.Name = "rtbResponse";
            this.rtbResponse.Size = new System.Drawing.Size(718, 240);
            this.rtbResponse.TabIndex = 13;
            this.rtbResponse.Text = "";
            // 
            // cmbUsers
            // 
            this.cmbUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F);
            this.cmbUsers.FormattingEnabled = true;
            this.cmbUsers.Location = new System.Drawing.Point(31, 34);
            this.cmbUsers.Name = "cmbUsers";
            this.cmbUsers.Size = new System.Drawing.Size(168, 34);
            this.cmbUsers.TabIndex = 11;
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 60000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 487);
            this.Controls.Add(this.panelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            //this.Text = "Fiscal Printer Module RGuard 1.0.23";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panelKey.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelKey;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox rtbKey;
        private System.Windows.Forms.ListBox lsbWsStatuses;
        private System.Windows.Forms.Button btnGetXReport;
        private System.Windows.Forms.Button btnGetFDByNumber;
        private System.Windows.Forms.TextBox txtFdNumber;
        private System.Windows.Forms.Button btnGetLastFD;
        private System.Windows.Forms.Button btnCloseShift;
        private System.Windows.Forms.Button btnOpenShift;
        private System.Windows.Forms.Button btnDeviceStatus;
        private System.Windows.Forms.RichTextBox rtbResponse;
        private System.Windows.Forms.ComboBox cmbUsers;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.ComboBox cmbKey;
    }
}

