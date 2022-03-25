
namespace Project_Chemical_SGS_Remake
{
    partial class FormMonitor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMonitor));
            this.labelWeighed = new System.Windows.Forms.Label();
            this.labelActualWeight = new System.Windows.Forms.Label();
            this.labelChemical = new System.Windows.Forms.Label();
            this.labelFormula = new System.Windows.Forms.Label();
            this.timerCheckStatus = new System.Windows.Forms.Timer(this.components);
            this.rjButton1 = new Project_Chemical_SGS_Remake.RJButton();
            this.label2 = new System.Windows.Forms.Label();
            this.rjButton2 = new Project_Chemical_SGS_Remake.RJButton();
            this.rjButton3 = new Project_Chemical_SGS_Remake.RJButton();
            this.rjButton4 = new Project_Chemical_SGS_Remake.RJButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ButtonStatus = new Project_Chemical_SGS_Remake.RJButton();
            this.rjButton5 = new Project_Chemical_SGS_Remake.RJButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.rjButton6 = new Project_Chemical_SGS_Remake.RJButton();
            this.label8 = new System.Windows.Forms.Label();
            this.labelSetpoint = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelWeighed
            // 
            this.labelWeighed.AutoSize = true;
            this.labelWeighed.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelWeighed.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.labelWeighed.ForeColor = System.Drawing.Color.DarkBlue;
            this.labelWeighed.Location = new System.Drawing.Point(736, 501);
            this.labelWeighed.Name = "labelWeighed";
            this.labelWeighed.Size = new System.Drawing.Size(235, 108);
            this.labelWeighed.TabIndex = 28;
            this.labelWeighed.Text = "0.00";
            this.labelWeighed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelActualWeight
            // 
            this.labelActualWeight.AutoSize = true;
            this.labelActualWeight.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelActualWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.labelActualWeight.ForeColor = System.Drawing.Color.DarkBlue;
            this.labelActualWeight.Location = new System.Drawing.Point(67, 501);
            this.labelActualWeight.Name = "labelActualWeight";
            this.labelActualWeight.Size = new System.Drawing.Size(235, 108);
            this.labelActualWeight.TabIndex = 27;
            this.labelActualWeight.Text = "0.00";
            this.labelActualWeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelChemical
            // 
            this.labelChemical.AutoSize = true;
            this.labelChemical.BackColor = System.Drawing.Color.LightSalmon;
            this.labelChemical.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.labelChemical.ForeColor = System.Drawing.Color.DarkBlue;
            this.labelChemical.Location = new System.Drawing.Point(315, 338);
            this.labelChemical.Name = "labelChemical";
            this.labelChemical.Size = new System.Drawing.Size(159, 55);
            this.labelChemical.TabIndex = 27;
            this.labelChemical.Text = "SUSA";
            this.labelChemical.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFormula
            // 
            this.labelFormula.AutoSize = true;
            this.labelFormula.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.labelFormula.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.labelFormula.ForeColor = System.Drawing.Color.White;
            this.labelFormula.Location = new System.Drawing.Point(91, 190);
            this.labelFormula.Name = "labelFormula";
            this.labelFormula.Size = new System.Drawing.Size(310, 108);
            this.labelFormula.TabIndex = 40;
            this.labelFormula.Text = "L6-5C";
            // 
            // timerCheckStatus
            // 
            this.timerCheckStatus.Tick += new System.EventHandler(this.timerCheckStatus_Tick);
            // 
            // rjButton1
            // 
            this.rjButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.rjButton1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.rjButton1.BorderColor = System.Drawing.Color.White;
            this.rjButton1.BorderRadius = 40;
            this.rjButton1.BorderSize = 1;
            this.rjButton1.FlatAppearance.BorderSize = 0;
            this.rjButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton1.ForeColor = System.Drawing.Color.White;
            this.rjButton1.Location = new System.Drawing.Point(12, 111);
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(521, 210);
            this.rjButton1.TabIndex = 45;
            this.rjButton1.TextColor = System.Drawing.Color.White;
            this.rjButton1.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.LightSalmon;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(44, 338);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(232, 55);
            this.label2.TabIndex = 46;
            this.label2.Text = "Chemical";
            // 
            // rjButton2
            // 
            this.rjButton2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton2.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.rjButton2.BorderRadius = 40;
            this.rjButton2.BorderSize = 1;
            this.rjButton2.FlatAppearance.BorderSize = 0;
            this.rjButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton2.ForeColor = System.Drawing.Color.White;
            this.rjButton2.Location = new System.Drawing.Point(348, 410);
            this.rjButton2.Name = "rjButton2";
            this.rjButton2.Size = new System.Drawing.Size(330, 270);
            this.rjButton2.TabIndex = 47;
            this.rjButton2.TextColor = System.Drawing.Color.White;
            this.rjButton2.UseVisualStyleBackColor = false;
            // 
            // rjButton3
            // 
            this.rjButton3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton3.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.rjButton3.BorderRadius = 40;
            this.rjButton3.BorderSize = 1;
            this.rjButton3.FlatAppearance.BorderSize = 0;
            this.rjButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton3.ForeColor = System.Drawing.Color.White;
            this.rjButton3.Location = new System.Drawing.Point(682, 410);
            this.rjButton3.Name = "rjButton3";
            this.rjButton3.Size = new System.Drawing.Size(330, 270);
            this.rjButton3.TabIndex = 48;
            this.rjButton3.TextColor = System.Drawing.Color.White;
            this.rjButton3.UseVisualStyleBackColor = false;
            // 
            // rjButton4
            // 
            this.rjButton4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton4.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.rjButton4.BorderRadius = 40;
            this.rjButton4.BorderSize = 1;
            this.rjButton4.FlatAppearance.BorderSize = 0;
            this.rjButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton4.ForeColor = System.Drawing.Color.White;
            this.rjButton4.Location = new System.Drawing.Point(12, 410);
            this.rjButton4.Name = "rjButton4";
            this.rjButton4.Size = new System.Drawing.Size(330, 270);
            this.rjButton4.TabIndex = 49;
            this.rjButton4.TextColor = System.Drawing.Color.White;
            this.rjButton4.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(43, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(207, 55);
            this.label3.TabIndex = 51;
            this.label3.Text = "Formula";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bahnschrift Condensed", 51.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(871, 83);
            this.label1.TabIndex = 52;
            this.label1.Text = "Auto-Record Chemical Scale System";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 32.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(45, 423);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(264, 51);
            this.label4.TabIndex = 54;
            this.label4.Text = "Weight STD";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 32.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(708, 423);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(289, 51);
            this.label5.TabIndex = 55;
            this.label5.Text = "Weight Scale";
            // 
            // ButtonStatus
            // 
            this.ButtonStatus.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ButtonStatus.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.ButtonStatus.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonStatus.BorderRadius = 40;
            this.ButtonStatus.BorderSize = 1;
            this.ButtonStatus.FlatAppearance.BorderSize = 0;
            this.ButtonStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.ButtonStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonStatus.Location = new System.Drawing.Point(10, 686);
            this.ButtonStatus.Name = "ButtonStatus";
            this.ButtonStatus.Size = new System.Drawing.Size(1002, 77);
            this.ButtonStatus.TabIndex = 56;
            this.ButtonStatus.Text = "Pending";
            this.ButtonStatus.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonStatus.UseVisualStyleBackColor = false;
            // 
            // rjButton5
            // 
            this.rjButton5.BackColor = System.Drawing.Color.Transparent;
            this.rjButton5.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton5.BorderColor = System.Drawing.Color.Black;
            this.rjButton5.BorderRadius = 40;
            this.rjButton5.BorderSize = 1;
            this.rjButton5.FlatAppearance.BorderSize = 0;
            this.rjButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton5.ForeColor = System.Drawing.Color.White;
            this.rjButton5.Location = new System.Drawing.Point(53, 59);
            this.rjButton5.Name = "rjButton5";
            this.rjButton5.Size = new System.Drawing.Size(946, 46);
            this.rjButton5.TabIndex = 57;
            this.rjButton5.TextColor = System.Drawing.Color.White;
            this.rjButton5.UseVisualStyleBackColor = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(666, 126);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(250, 187);
            this.pictureBox1.TabIndex = 58;
            this.pictureBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label6.Location = new System.Drawing.Point(256, 626);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 42);
            this.label6.TabIndex = 59;
            this.label6.Text = "Kg.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label7.Location = new System.Drawing.Point(920, 626);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 42);
            this.label7.TabIndex = 60;
            this.label7.Text = "Kg.";
            // 
            // rjButton6
            // 
            this.rjButton6.BackColor = System.Drawing.Color.LightSalmon;
            this.rjButton6.BackgroundColor = System.Drawing.Color.LightSalmon;
            this.rjButton6.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.rjButton6.BorderRadius = 40;
            this.rjButton6.BorderSize = 1;
            this.rjButton6.FlatAppearance.BorderSize = 0;
            this.rjButton6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton6.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.rjButton6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.rjButton6.Location = new System.Drawing.Point(12, 327);
            this.rjButton6.Name = "rjButton6";
            this.rjButton6.Size = new System.Drawing.Size(1002, 77);
            this.rjButton6.TabIndex = 61;
            this.rjButton6.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.rjButton6.UseVisualStyleBackColor = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 32.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(406, 423);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(223, 51);
            this.label8.TabIndex = 62;
            this.label8.Text = "Tolerance";
            // 
            // labelSetpoint
            // 
            this.labelSetpoint.AutoSize = true;
            this.labelSetpoint.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelSetpoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.labelSetpoint.ForeColor = System.Drawing.Color.DarkBlue;
            this.labelSetpoint.Location = new System.Drawing.Point(397, 501);
            this.labelSetpoint.Name = "labelSetpoint";
            this.labelSetpoint.Size = new System.Drawing.Size(235, 108);
            this.labelSetpoint.TabIndex = 63;
            this.labelSetpoint.Text = "0.00";
            this.labelSetpoint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label10.Location = new System.Drawing.Point(585, 626);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 42);
            this.label10.TabIndex = 64;
            this.label10.Text = "Kg.";
            // 
            // FormMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.labelSetpoint);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ButtonStatus);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelWeighed);
            this.Controls.Add(this.labelActualWeight);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelChemical);
            this.Controls.Add(this.labelFormula);
            this.Controls.Add(this.rjButton3);
            this.Controls.Add(this.rjButton2);
            this.Controls.Add(this.rjButton4);
            this.Controls.Add(this.rjButton1);
            this.Controls.Add(this.rjButton5);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.rjButton6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMonitor";
            this.Text = "FormMonitor";
            this.Load += new System.EventHandler(this.FormMonitor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelWeighed;
        private System.Windows.Forms.Label labelActualWeight;
        private System.Windows.Forms.Label labelChemical;
        private System.Windows.Forms.Label labelFormula;
        private System.Windows.Forms.Timer timerCheckStatus;
        private RJButton rjButton1;
        private System.Windows.Forms.Label label2;
        private RJButton rjButton2;
        private RJButton rjButton3;
        private RJButton rjButton4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private RJButton ButtonStatus;
        private RJButton rjButton5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private RJButton rjButton6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelSetpoint;
        private System.Windows.Forms.Label label10;
    }
}