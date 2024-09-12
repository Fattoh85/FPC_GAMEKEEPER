namespace FPC
{
    partial class FrmSettings
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
            this.button1 = new System.Windows.Forms.Button();
            this.txtDbName = new System.Windows.Forms.TextBox();
            this.txtChTranDelay = new System.Windows.Forms.TextBox();
            this.txtChecStateDelay = new System.Windows.Forms.TextBox();
            this.txtPrinterPath = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblDevStatus = new System.Windows.Forms.Label();
            this.lblDbConState = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTimeDiff = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.comboBoxVatCodes = new System.Windows.Forms.ComboBox();
            this.comboBoxCommodity = new System.Windows.Forms.ComboBox();
            this.txtNdsValue = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(174, 67);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(211, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Указать папку программы";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtDbName
            // 
            this.txtDbName.Enabled = false;
            this.txtDbName.Location = new System.Drawing.Point(174, 28);
            this.txtDbName.Name = "txtDbName";
            this.txtDbName.Size = new System.Drawing.Size(211, 20);
            this.txtDbName.TabIndex = 5;
            // 
            // txtChTranDelay
            // 
            this.txtChTranDelay.Location = new System.Drawing.Point(174, 2);
            this.txtChTranDelay.Name = "txtChTranDelay";
            this.txtChTranDelay.Size = new System.Drawing.Size(211, 20);
            this.txtChTranDelay.TabIndex = 7;
            // 
            // txtChecStateDelay
            // 
            this.txtChecStateDelay.Location = new System.Drawing.Point(174, 160);
            this.txtChecStateDelay.Name = "txtChecStateDelay";
            this.txtChecStateDelay.Size = new System.Drawing.Size(211, 20);
            this.txtChecStateDelay.TabIndex = 8;
            // 
            // txtPrinterPath
            // 
            this.txtPrinterPath.Location = new System.Drawing.Point(174, 186);
            this.txtPrinterPath.Name = "txtPrinterPath";
            this.txtPrinterPath.Size = new System.Drawing.Size(211, 20);
            this.txtPrinterPath.TabIndex = 9;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(552, 336);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(138, 39);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblDevStatus
            // 
            this.lblDevStatus.AutoSize = true;
            this.lblDevStatus.Location = new System.Drawing.Point(391, 193);
            this.lblDevStatus.Name = "lblDevStatus";
            this.lblDevStatus.Size = new System.Drawing.Size(73, 13);
            this.lblDevStatus.TabIndex = 11;
            this.lblDevStatus.Text = "Подождите...";
            // 
            // lblDbConState
            // 
            this.lblDbConState.AutoSize = true;
            this.lblDbConState.Location = new System.Drawing.Point(391, 31);
            this.lblDbConState.Name = "lblDbConState";
            this.lblDbConState.Size = new System.Drawing.Size(73, 13);
            this.lblDbConState.TabIndex = 12;
            this.lblDbConState.Text = "Подождите...";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 189);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Адрес принтера";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblTimeDiff
            // 
            this.lblTimeDiff.AutoSize = true;
            this.lblTimeDiff.Location = new System.Drawing.Point(171, 51);
            this.lblTimeDiff.Name = "lblTimeDiff";
            this.lblTimeDiff.Size = new System.Drawing.Size(73, 13);
            this.lblTimeDiff.TabIndex = 13;
            this.lblTimeDiff.Text = "Подождите...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Обновление транзакций";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Время сервера";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(174, 212);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(211, 23);
            this.button2.TabIndex = 14;
            this.button2.Text = "Проверить доступность";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 163);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(170, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Обновление проверки принтера";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "База";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(391, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(148, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Рекомендуемое значение 3";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(391, 163);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(160, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Рекомендуемое значение 180";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(696, 336);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(138, 39);
            this.button3.TabIndex = 15;
            this.button3.Text = "Отмена";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // comboBoxVatCodes
            // 
            this.comboBoxVatCodes.FormattingEnabled = true;
            this.comboBoxVatCodes.Location = new System.Drawing.Point(177, 279);
            this.comboBoxVatCodes.Name = "comboBoxVatCodes";
            this.comboBoxVatCodes.Size = new System.Drawing.Size(211, 21);
            this.comboBoxVatCodes.TabIndex = 16;
            this.comboBoxVatCodes.SelectedIndexChanged += new System.EventHandler(this.comboBoxVatCodes_SelectedIndexChanged);
            // 
            // comboBoxCommodity
            // 
            this.comboBoxCommodity.FormattingEnabled = true;
            this.comboBoxCommodity.Location = new System.Drawing.Point(177, 306);
            this.comboBoxCommodity.Name = "comboBoxCommodity";
            this.comboBoxCommodity.Size = new System.Drawing.Size(211, 21);
            this.comboBoxCommodity.TabIndex = 16;
            this.comboBoxCommodity.SelectedIndexChanged += new System.EventHandler(this.comboBoxCommodity_SelectedIndexChanged);
            // 
            // txtNdsValue
            // 
            this.txtNdsValue.Location = new System.Drawing.Point(394, 280);
            this.txtNdsValue.Name = "txtNdsValue";
            this.txtNdsValue.Size = new System.Drawing.Size(59, 20);
            this.txtNdsValue.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 287);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "НДС";
            this.label8.Click += new System.EventHandler(this.label1_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 309);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Предмет расчета";
            this.label9.Click += new System.EventHandler(this.label1_Click);
            // 
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 398);
            this.Controls.Add(this.txtNdsValue);
            this.Controls.Add(this.comboBoxCommodity);
            this.Controls.Add(this.comboBoxVatCodes);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lblTimeDiff);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblDbConState);
            this.Controls.Add(this.lblDevStatus);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtPrinterPath);
            this.Controls.Add(this.txtChecStateDelay);
            this.Controls.Add(this.txtChTranDelay);
            this.Controls.Add(this.txtDbName);
            this.Controls.Add(this.button1);
            this.Name = "FrmSettings";
            this.Text = "FrmSettings";
            this.Load += new System.EventHandler(this.FrmSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtDbName;
        private System.Windows.Forms.TextBox txtChTranDelay;
        private System.Windows.Forms.TextBox txtChecStateDelay;
        private System.Windows.Forms.TextBox txtPrinterPath;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblDevStatus;
        private System.Windows.Forms.Label lblDbConState;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTimeDiff;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox comboBoxVatCodes;
        private System.Windows.Forms.ComboBox comboBoxCommodity;
        private System.Windows.Forms.TextBox txtNdsValue;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}