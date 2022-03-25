
namespace Project_Chemical_SGS_Remake
{
    partial class FormSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetting));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelChildForm = new System.Windows.Forms.Panel();
            this.ButtonEditFormula = new Project_Chemical_SGS_Remake.RJButton();
            this.ButtonPermission = new Project_Chemical_SGS_Remake.RJButton();
            this.ButtonEditTanks = new Project_Chemical_SGS_Remake.RJButton();
            this.ButtonEditFormula1 = new Project_Chemical_SGS_Remake.RJButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonBackMain = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(684, 88);
            this.panel1.TabIndex = 37;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label13.Location = new System.Drawing.Point(60, 60);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(481, 16);
            this.label13.TabIndex = 18;
            this.label13.Text = "Can edit about formula and chemical or increase chemical. Please select below.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 39);
            this.label1.TabIndex = 17;
            this.label1.Text = "Setting";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.buttonBackMain);
            this.panel2.Location = new System.Drawing.Point(690, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(333, 88);
            this.panel2.TabIndex = 38;
            // 
            // panelChildForm
            // 
            this.panelChildForm.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelChildForm.Location = new System.Drawing.Point(0, 146);
            this.panelChildForm.Name = "panelChildForm";
            this.panelChildForm.Size = new System.Drawing.Size(1025, 550);
            this.panelChildForm.TabIndex = 42;
            // 
            // ButtonEditFormula
            // 
            this.ButtonEditFormula.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonEditFormula.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonEditFormula.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.ButtonEditFormula.BorderRadius = 40;
            this.ButtonEditFormula.BorderSize = 0;
            this.ButtonEditFormula.FlatAppearance.BorderSize = 0;
            this.ButtonEditFormula.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonEditFormula.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.ButtonEditFormula.ForeColor = System.Drawing.Color.White;
            this.ButtonEditFormula.Location = new System.Drawing.Point(0, 90);
            this.ButtonEditFormula.Name = "ButtonEditFormula";
            this.ButtonEditFormula.Size = new System.Drawing.Size(184, 52);
            this.ButtonEditFormula.TabIndex = 41;
            this.ButtonEditFormula.Text = "Item Code";
            this.ButtonEditFormula.TextColor = System.Drawing.Color.White;
            this.ButtonEditFormula.UseVisualStyleBackColor = false;
            this.ButtonEditFormula.Click += new System.EventHandler(this.ButtonEditFormula_Click);
            // 
            // ButtonPermission
            // 
            this.ButtonPermission.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonPermission.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonPermission.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.ButtonPermission.BorderRadius = 40;
            this.ButtonPermission.BorderSize = 0;
            this.ButtonPermission.FlatAppearance.BorderSize = 0;
            this.ButtonPermission.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonPermission.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.ButtonPermission.ForeColor = System.Drawing.Color.White;
            this.ButtonPermission.Location = new System.Drawing.Point(562, 90);
            this.ButtonPermission.Name = "ButtonPermission";
            this.ButtonPermission.Size = new System.Drawing.Size(184, 52);
            this.ButtonPermission.TabIndex = 44;
            this.ButtonPermission.Text = "Accessibility";
            this.ButtonPermission.TextColor = System.Drawing.Color.White;
            this.ButtonPermission.UseVisualStyleBackColor = false;
            this.ButtonPermission.Click += new System.EventHandler(this.ButtonPermission_Click);
            // 
            // ButtonEditTanks
            // 
            this.ButtonEditTanks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonEditTanks.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonEditTanks.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.ButtonEditTanks.BorderRadius = 40;
            this.ButtonEditTanks.BorderSize = 0;
            this.ButtonEditTanks.FlatAppearance.BorderSize = 0;
            this.ButtonEditTanks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonEditTanks.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.ButtonEditTanks.ForeColor = System.Drawing.Color.White;
            this.ButtonEditTanks.Location = new System.Drawing.Point(372, 90);
            this.ButtonEditTanks.Name = "ButtonEditTanks";
            this.ButtonEditTanks.Size = new System.Drawing.Size(184, 52);
            this.ButtonEditTanks.TabIndex = 43;
            this.ButtonEditTanks.Text = "Tanks Chemical";
            this.ButtonEditTanks.TextColor = System.Drawing.Color.White;
            this.ButtonEditTanks.UseVisualStyleBackColor = false;
            this.ButtonEditTanks.Click += new System.EventHandler(this.ButtonEditTanks_Click);
            // 
            // ButtonEditFormula1
            // 
            this.ButtonEditFormula1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonEditFormula1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonEditFormula1.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.ButtonEditFormula1.BorderRadius = 40;
            this.ButtonEditFormula1.BorderSize = 0;
            this.ButtonEditFormula1.FlatAppearance.BorderSize = 0;
            this.ButtonEditFormula1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonEditFormula1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.ButtonEditFormula1.ForeColor = System.Drawing.Color.White;
            this.ButtonEditFormula1.Location = new System.Drawing.Point(186, 90);
            this.ButtonEditFormula1.Name = "ButtonEditFormula1";
            this.ButtonEditFormula1.Size = new System.Drawing.Size(184, 52);
            this.ButtonEditFormula1.TabIndex = 45;
            this.ButtonEditFormula1.Text = "Formula";
            this.ButtonEditFormula1.TextColor = System.Drawing.Color.White;
            this.ButtonEditFormula1.UseVisualStyleBackColor = false;
            this.ButtonEditFormula1.Click += new System.EventHandler(this.ButtonEditFormula1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(125, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(70, 70);
            this.pictureBox1.TabIndex = 25;
            this.pictureBox1.TabStop = false;
            // 
            // buttonBackMain
            // 
            this.buttonBackMain.FlatAppearance.BorderSize = 0;
            this.buttonBackMain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBackMain.Image = ((System.Drawing.Image)(resources.GetObject("buttonBackMain.Image")));
            this.buttonBackMain.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.buttonBackMain.Location = new System.Drawing.Point(239, 5);
            this.buttonBackMain.Name = "buttonBackMain";
            this.buttonBackMain.Size = new System.Drawing.Size(90, 60);
            this.buttonBackMain.TabIndex = 26;
            this.buttonBackMain.UseVisualStyleBackColor = true;
            this.buttonBackMain.Click += new System.EventHandler(this.buttonBackMain_Click);
            // 
            // FormSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1024, 696);
            this.Controls.Add(this.ButtonEditFormula1);
            this.Controls.Add(this.ButtonEditFormula);
            this.Controls.Add(this.ButtonPermission);
            this.Controls.Add(this.ButtonEditTanks);
            this.Controls.Add(this.panelChildForm);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormSetting";
            this.Text = "FormSetting";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonBackMain;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private RJButton ButtonEditFormula;
        private System.Windows.Forms.Panel panelChildForm;
        private RJButton ButtonEditTanks;
        private RJButton ButtonPermission;
        private RJButton ButtonEditFormula1;
    }
}