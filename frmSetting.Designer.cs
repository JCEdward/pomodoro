namespace pomodoro
{
    partial class FrmSetting
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trbOpacity = new System.Windows.Forms.TrackBar();
            this.txtWorkTime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBreakTime = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtLongBreakTime = new System.Windows.Forms.TextBox();
            this.txtLongTimeInterval = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trbOpacity)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Opacity";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Work time";
            // 
            // trbOpacity
            // 
            this.trbOpacity.Location = new System.Drawing.Point(90, 9);
            this.trbOpacity.Name = "trbOpacity";
            this.trbOpacity.Size = new System.Drawing.Size(256, 45);
            this.trbOpacity.TabIndex = 1;
            // 
            // txtWorkTime
            // 
            this.txtWorkTime.Location = new System.Drawing.Point(90, 61);
            this.txtWorkTime.Name = "txtWorkTime";
            this.txtWorkTime.Size = new System.Drawing.Size(70, 23);
            this.txtWorkTime.TabIndex = 2;
            this.txtWorkTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyNumber);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Break time";
            // 
            // txtBreakTime
            // 
            this.txtBreakTime.Location = new System.Drawing.Point(90, 95);
            this.txtBreakTime.Name = "txtBreakTime";
            this.txtBreakTime.Size = new System.Drawing.Size(70, 23);
            this.txtBreakTime.TabIndex = 2;
            this.txtBreakTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyNumber);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(166, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Long break time";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(166, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Long time interval";
            // 
            // txtLongBreakTime
            // 
            this.txtLongBreakTime.Location = new System.Drawing.Point(276, 61);
            this.txtLongBreakTime.Name = "txtLongBreakTime";
            this.txtLongBreakTime.Size = new System.Drawing.Size(70, 23);
            this.txtLongBreakTime.TabIndex = 2;
            this.txtLongBreakTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyNumber);
            // 
            // txtLongTimeInterval
            // 
            this.txtLongTimeInterval.Location = new System.Drawing.Point(276, 95);
            this.txtLongTimeInterval.Name = "txtLongTimeInterval";
            this.txtLongTimeInterval.Size = new System.Drawing.Size(70, 23);
            this.txtLongTimeInterval.TabIndex = 2;
            this.txtLongTimeInterval.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyNumber);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(122, 148);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(99, 37);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 197);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtLongTimeInterval);
            this.Controls.Add(this.txtLongBreakTime);
            this.Controls.Add(this.txtBreakTime);
            this.Controls.Add(this.txtWorkTime);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.trbOpacity);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmSetting";
            this.Text = "Options";
            ((System.ComponentModel.ISupportInitialize)(this.trbOpacity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trbOpacity;
        private System.Windows.Forms.TextBox txtWorkTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBreakTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtLongBreakTime;
        private System.Windows.Forms.TextBox txtLongTimeInterval;
        private System.Windows.Forms.Button btnSave;
    }
}