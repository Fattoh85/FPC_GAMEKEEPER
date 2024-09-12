namespace FPC
{
    partial class FrmLicense
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLicense));
            this.btnPaste = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.rtbKey = new System.Windows.Forms.RichTextBox();
            this.txtCopy = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(455, 315);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(84, 32);
            this.btnPaste.TabIndex = 10;
            this.btnPaste.Text = "Вставить";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(456, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(83, 31);
            this.button1.TabIndex = 9;
            this.button1.Text = "Копировать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // rtbKey
            // 
            this.rtbKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.rtbKey.Location = new System.Drawing.Point(12, 47);
            this.rtbKey.Name = "rtbKey";
            this.rtbKey.Size = new System.Drawing.Size(438, 300);
            this.rtbKey.TabIndex = 8;
            this.rtbKey.Text = "";
            // 
            // txtCopy
            // 
            this.txtCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtCopy.Location = new System.Drawing.Point(12, 12);
            this.txtCopy.Name = "txtCopy";
            this.txtCopy.Size = new System.Drawing.Size(438, 29);
            this.txtCopy.TabIndex = 7;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(184, 353);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(120, 38);
            this.button3.TabIndex = 12;
            this.button3.Text = "Ok";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FrmLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 401);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.rtbKey);
            this.Controls.Add(this.txtCopy);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmLicense";
            this.Text = "Установка v1.0.10";
            this.Load += new System.EventHandler(this.FrmLicense_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox rtbKey;
        private System.Windows.Forms.TextBox txtCopy;
        private System.Windows.Forms.Button button3;
    }
}