namespace Swagger
{
    partial class Form1
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
            this.StartAntiAfk = new System.Windows.Forms.Button();
            this.StopAntiAfk = new System.Windows.Forms.Button();
            this.CurrentStatus = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // StartAntiAfk
            // 
            this.StartAntiAfk.Location = new System.Drawing.Point(12, 12);
            this.StartAntiAfk.Name = "StartAntiAfk";
            this.StartAntiAfk.Size = new System.Drawing.Size(159, 45);
            this.StartAntiAfk.TabIndex = 0;
            this.StartAntiAfk.Text = "Start Anti Afk";
            this.StartAntiAfk.UseVisualStyleBackColor = true;
            this.StartAntiAfk.Click += new System.EventHandler(this.StartAntiAfk_Click);
            // 
            // StopAntiAfk
            // 
            this.StopAntiAfk.Location = new System.Drawing.Point(12, 63);
            this.StopAntiAfk.Name = "StopAntiAfk";
            this.StopAntiAfk.Size = new System.Drawing.Size(159, 45);
            this.StopAntiAfk.TabIndex = 1;
            this.StopAntiAfk.Text = "Stop Anti Afk";
            this.StopAntiAfk.UseVisualStyleBackColor = true;
            this.StopAntiAfk.Click += new System.EventHandler(this.StopAntiAfk_Click);
            // 
            // CurrentStatus
            // 
            this.CurrentStatus.Enabled = false;
            this.CurrentStatus.Location = new System.Drawing.Point(12, 155);
            this.CurrentStatus.Name = "CurrentStatus";
            this.CurrentStatus.Size = new System.Drawing.Size(159, 20);
            this.CurrentStatus.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 139);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Status";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(183, 187);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CurrentStatus);
            this.Controls.Add(this.StopAntiAfk);
            this.Controls.Add(this.StartAntiAfk);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartAntiAfk;
        private System.Windows.Forms.Button StopAntiAfk;
        private System.Windows.Forms.TextBox CurrentStatus;
        private System.Windows.Forms.Label label1;
    }
}

