namespace BrawlhallaAfker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.StartButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.StateLabel = new System.Windows.Forms.Label();
            this.StateTextBox = new System.Windows.Forms.TextBox();
            this.TimerLabel = new System.Windows.Forms.Label();
            this.TimerTextBox = new System.Windows.Forms.TextBox();
            this.LastPressedKeyLabel = new System.Windows.Forms.Label();
            this.LastPressedKeyTextBox = new System.Windows.Forms.TextBox();
            this.InstructionsTextBox = new System.Windows.Forms.RichTextBox();
            this.DebugLabel = new System.Windows.Forms.Label();
            this.DebugTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(12, 12);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(166, 45);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(12, 63);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(166, 45);
            this.StopButton.TabIndex = 1;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // StateLabel
            // 
            this.StateLabel.AutoSize = true;
            this.StateLabel.Location = new System.Drawing.Point(184, 12);
            this.StateLabel.Name = "StateLabel";
            this.StateLabel.Size = new System.Drawing.Size(32, 13);
            this.StateLabel.TabIndex = 2;
            this.StateLabel.Text = "State";
            // 
            // StateTextBox
            // 
            this.StateTextBox.Location = new System.Drawing.Point(184, 28);
            this.StateTextBox.Name = "StateTextBox";
            this.StateTextBox.Size = new System.Drawing.Size(100, 20);
            this.StateTextBox.TabIndex = 3;
            // 
            // TimerLabel
            // 
            this.TimerLabel.AutoSize = true;
            this.TimerLabel.Location = new System.Drawing.Point(184, 63);
            this.TimerLabel.Name = "TimerLabel";
            this.TimerLabel.Size = new System.Drawing.Size(33, 13);
            this.TimerLabel.TabIndex = 4;
            this.TimerLabel.Text = "Timer";
            // 
            // TimerTextBox
            // 
            this.TimerTextBox.Location = new System.Drawing.Point(184, 79);
            this.TimerTextBox.Name = "TimerTextBox";
            this.TimerTextBox.Size = new System.Drawing.Size(100, 20);
            this.TimerTextBox.TabIndex = 5;
            // 
            // LastPressedKeyLabel
            // 
            this.LastPressedKeyLabel.AutoSize = true;
            this.LastPressedKeyLabel.Location = new System.Drawing.Point(290, 12);
            this.LastPressedKeyLabel.Name = "LastPressedKeyLabel";
            this.LastPressedKeyLabel.Size = new System.Drawing.Size(102, 13);
            this.LastPressedKeyLabel.TabIndex = 6;
            this.LastPressedKeyLabel.Text = "Last Pressed Button";
            // 
            // LastPressedKeyTextBox
            // 
            this.LastPressedKeyTextBox.Location = new System.Drawing.Point(293, 28);
            this.LastPressedKeyTextBox.Name = "LastPressedKeyTextBox";
            this.LastPressedKeyTextBox.Size = new System.Drawing.Size(100, 20);
            this.LastPressedKeyTextBox.TabIndex = 7;
            // 
            // InstructionsTextBox
            // 
            this.InstructionsTextBox.Location = new System.Drawing.Point(12, 123);
            this.InstructionsTextBox.Name = "InstructionsTextBox";
            this.InstructionsTextBox.Size = new System.Drawing.Size(381, 270);
            this.InstructionsTextBox.TabIndex = 8;
            this.InstructionsTextBox.Text = resources.GetString("InstructionsTextBox.Text");
            // 
            // DebugLabel
            // 
            this.DebugLabel.AutoSize = true;
            this.DebugLabel.Location = new System.Drawing.Point(290, 63);
            this.DebugLabel.Name = "DebugLabel";
            this.DebugLabel.Size = new System.Drawing.Size(39, 13);
            this.DebugLabel.TabIndex = 9;
            this.DebugLabel.Text = "Debug";
            // 
            // DebugTextBox
            // 
            this.DebugTextBox.Location = new System.Drawing.Point(293, 79);
            this.DebugTextBox.Name = "DebugTextBox";
            this.DebugTextBox.Size = new System.Drawing.Size(100, 20);
            this.DebugTextBox.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 404);
            this.Controls.Add(this.DebugTextBox);
            this.Controls.Add(this.DebugLabel);
            this.Controls.Add(this.InstructionsTextBox);
            this.Controls.Add(this.LastPressedKeyTextBox);
            this.Controls.Add(this.LastPressedKeyLabel);
            this.Controls.Add(this.TimerTextBox);
            this.Controls.Add(this.TimerLabel);
            this.Controls.Add(this.StateTextBox);
            this.Controls.Add(this.StateLabel);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.StartButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Label StateLabel;
        private System.Windows.Forms.TextBox StateTextBox;
        private System.Windows.Forms.Label TimerLabel;
        private System.Windows.Forms.TextBox TimerTextBox;
        private System.Windows.Forms.Label LastPressedKeyLabel;
        private System.Windows.Forms.TextBox LastPressedKeyTextBox;
        private System.Windows.Forms.RichTextBox InstructionsTextBox;
        private System.Windows.Forms.Label DebugLabel;
        private System.Windows.Forms.TextBox DebugTextBox;
    }
}

