
namespace Project_Chemical_SGS_Remake
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.panelChildForm = new System.Windows.Forms.Panel();
            this.textBoxAlarm = new System.Windows.Forms.TextBox();
            this.rjButton1 = new Project_Chemical_SGS_Remake.RJButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ButtonMixing = new Project_Chemical_SGS_Remake.RJButton();
            this.ButtonWeighingScale = new Project_Chemical_SGS_Remake.RJButton();
            this.ButtonSetting = new Project_Chemical_SGS_Remake.RJButton();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.panelUser = new System.Windows.Forms.Panel();
            this.labelCname = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.labelCid = new System.Windows.Forms.Label();
            this.labelID = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.ButtonLogout = new Project_Chemical_SGS_Remake.RJButton();
            this.timerCheckChemical = new System.Windows.Forms.Timer(this.components);
            this.panelChildForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panelUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // panelChildForm
            // 
            this.panelChildForm.Controls.Add(this.textBoxAlarm);
            this.panelChildForm.Controls.Add(this.rjButton1);
            this.panelChildForm.Controls.Add(this.pictureBox1);
            this.panelChildForm.Controls.Add(this.ButtonMixing);
            this.panelChildForm.Controls.Add(this.ButtonWeighingScale);
            this.panelChildForm.Controls.Add(this.ButtonSetting);
            this.panelChildForm.Location = new System.Drawing.Point(1, 72);
            this.panelChildForm.Name = "panelChildForm";
            this.panelChildForm.Size = new System.Drawing.Size(1024, 696);
            this.panelChildForm.TabIndex = 6;
            // 
            // textBoxAlarm
            // 
            this.textBoxAlarm.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBoxAlarm.ForeColor = System.Drawing.Color.Red;
            this.textBoxAlarm.Location = new System.Drawing.Point(0, 661);
            this.textBoxAlarm.Multiline = true;
            this.textBoxAlarm.Name = "textBoxAlarm";
            this.textBoxAlarm.Size = new System.Drawing.Size(1024, 33);
            this.textBoxAlarm.TabIndex = 7;
            this.textBoxAlarm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // rjButton1
            // 
            this.rjButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.rjButton1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.rjButton1.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rjButton1.BorderRadius = 40;
            this.rjButton1.BorderSize = 0;
            this.rjButton1.FlatAppearance.BorderSize = 0;
            this.rjButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.rjButton1.ForeColor = System.Drawing.Color.White;
            this.rjButton1.Image = ((System.Drawing.Image)(resources.GetObject("rjButton1.Image")));
            this.rjButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rjButton1.Location = new System.Drawing.Point(174, 511);
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(337, 118);
            this.rjButton1.TabIndex = 6;
            this.rjButton1.Text = "   Fill Chemical";
            this.rjButton1.TextColor = System.Drawing.Color.White;
            this.rjButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton1.UseVisualStyleBackColor = false;
            this.rjButton1.Click += new System.EventHandler(this.rjButton1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(108, 33);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(807, 327);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // ButtonMixing
            // 
            this.ButtonMixing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonMixing.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonMixing.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.ButtonMixing.BorderRadius = 40;
            this.ButtonMixing.BorderSize = 0;
            this.ButtonMixing.FlatAppearance.BorderSize = 0;
            this.ButtonMixing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonMixing.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.ButtonMixing.ForeColor = System.Drawing.Color.White;
            this.ButtonMixing.Image = ((System.Drawing.Image)(resources.GetObject("ButtonMixing.Image")));
            this.ButtonMixing.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ButtonMixing.Location = new System.Drawing.Point(517, 387);
            this.ButtonMixing.Name = "ButtonMixing";
            this.ButtonMixing.Size = new System.Drawing.Size(337, 118);
            this.ButtonMixing.TabIndex = 5;
            this.ButtonMixing.Text = "   Mixing";
            this.ButtonMixing.TextColor = System.Drawing.Color.White;
            this.ButtonMixing.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ButtonMixing.UseVisualStyleBackColor = false;
            this.ButtonMixing.Click += new System.EventHandler(this.ButtonMixing_Click);
            // 
            // ButtonWeighingScale
            // 
            this.ButtonWeighingScale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonWeighingScale.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonWeighingScale.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.ButtonWeighingScale.BorderRadius = 40;
            this.ButtonWeighingScale.BorderSize = 0;
            this.ButtonWeighingScale.FlatAppearance.BorderSize = 0;
            this.ButtonWeighingScale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonWeighingScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.ButtonWeighingScale.ForeColor = System.Drawing.Color.White;
            this.ButtonWeighingScale.Image = ((System.Drawing.Image)(resources.GetObject("ButtonWeighingScale.Image")));
            this.ButtonWeighingScale.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ButtonWeighingScale.Location = new System.Drawing.Point(174, 387);
            this.ButtonWeighingScale.Name = "ButtonWeighingScale";
            this.ButtonWeighingScale.Size = new System.Drawing.Size(337, 118);
            this.ButtonWeighingScale.TabIndex = 5;
            this.ButtonWeighingScale.Text = "   Weighing";
            this.ButtonWeighingScale.TextColor = System.Drawing.Color.White;
            this.ButtonWeighingScale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ButtonWeighingScale.UseVisualStyleBackColor = false;
            this.ButtonWeighingScale.Click += new System.EventHandler(this.ButtonWeighingScale_Click);
            // 
            // ButtonSetting
            // 
            this.ButtonSetting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonSetting.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonSetting.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.ButtonSetting.BorderRadius = 40;
            this.ButtonSetting.BorderSize = 0;
            this.ButtonSetting.FlatAppearance.BorderSize = 0;
            this.ButtonSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonSetting.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.ButtonSetting.ForeColor = System.Drawing.Color.White;
            this.ButtonSetting.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSetting.Image")));
            this.ButtonSetting.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ButtonSetting.Location = new System.Drawing.Point(517, 511);
            this.ButtonSetting.Name = "ButtonSetting";
            this.ButtonSetting.Size = new System.Drawing.Size(337, 118);
            this.ButtonSetting.TabIndex = 5;
            this.ButtonSetting.Text = "   Setting";
            this.ButtonSetting.TextColor = System.Drawing.Color.White;
            this.ButtonSetting.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ButtonSetting.UseVisualStyleBackColor = false;
            this.ButtonSetting.Click += new System.EventHandler(this.ButtonSetting_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(756, 11);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(200, 54);
            this.pictureBox3.TabIndex = 6;
            this.pictureBox3.TabStop = false;
            // 
            // panelUser
            // 
            this.panelUser.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panelUser.Controls.Add(this.pictureBox3);
            this.panelUser.Controls.Add(this.labelCname);
            this.panelUser.Controls.Add(this.labelName);
            this.panelUser.Controls.Add(this.labelCid);
            this.panelUser.Controls.Add(this.labelID);
            this.panelUser.Controls.Add(this.pictureBox2);
            this.panelUser.Controls.Add(this.ButtonLogout);
            this.panelUser.Location = new System.Drawing.Point(0, 0);
            this.panelUser.Name = "panelUser";
            this.panelUser.Size = new System.Drawing.Size(1025, 72);
            this.panelUser.TabIndex = 7;
            // 
            // labelCname
            // 
            this.labelCname.AutoSize = true;
            this.labelCname.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.labelCname.ForeColor = System.Drawing.Color.Gray;
            this.labelCname.Location = new System.Drawing.Point(145, 12);
            this.labelCname.Name = "labelCname";
            this.labelCname.Size = new System.Drawing.Size(55, 20);
            this.labelCname.TabIndex = 1;
            this.labelCname.Text = "Name";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.labelName.Location = new System.Drawing.Point(76, 12);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(70, 20);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "Name : ";
            // 
            // labelCid
            // 
            this.labelCid.AutoSize = true;
            this.labelCid.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.labelCid.ForeColor = System.Drawing.Color.Gray;
            this.labelCid.Location = new System.Drawing.Point(145, 37);
            this.labelCid.Name = "labelCid";
            this.labelCid.Size = new System.Drawing.Size(28, 20);
            this.labelCid.TabIndex = 1;
            this.labelCid.Text = "ID";
            // 
            // labelID
            // 
            this.labelID.AutoSize = true;
            this.labelID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.labelID.Location = new System.Drawing.Point(101, 37);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(43, 20);
            this.labelID.TabIndex = 1;
            this.labelID.Text = "ID : ";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(2, 1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(70, 70);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // ButtonLogout
            // 
            this.ButtonLogout.BackColor = System.Drawing.Color.Transparent;
            this.ButtonLogout.BackgroundColor = System.Drawing.Color.Transparent;
            this.ButtonLogout.BorderColor = System.Drawing.Color.Transparent;
            this.ButtonLogout.BorderRadius = 40;
            this.ButtonLogout.BorderSize = 0;
            this.ButtonLogout.FlatAppearance.BorderSize = 0;
            this.ButtonLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonLogout.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonLogout.ForeColor = System.Drawing.Color.White;
            this.ButtonLogout.Image = ((System.Drawing.Image)(resources.GetObject("ButtonLogout.Image")));
            this.ButtonLogout.Location = new System.Drawing.Point(957, 4);
            this.ButtonLogout.Name = "ButtonLogout";
            this.ButtonLogout.Size = new System.Drawing.Size(65, 65);
            this.ButtonLogout.TabIndex = 5;
            this.ButtonLogout.TextColor = System.Drawing.Color.White;
            this.ButtonLogout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ButtonLogout.UseVisualStyleBackColor = false;
            this.ButtonLogout.Click += new System.EventHandler(this.ButtonLogout_Click);
            // 
            // timerCheckChemical
            // 
            this.timerCheckChemical.Tick += new System.EventHandler(this.timerCheckChemical_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.panelUser);
            this.Controls.Add(this.panelChildForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.panelChildForm.ResumeLayout(false);
            this.panelChildForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panelUser.ResumeLayout(false);
            this.panelUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private RJButton ButtonWeighingScale;
        private RJButton ButtonMixing;
        private RJButton ButtonLogout;
        private System.Windows.Forms.Panel panelChildForm;
        private System.Windows.Forms.Panel panelUser;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label labelCname;
        private System.Windows.Forms.Label labelCid;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelID;
        private RJButton rjButton1;
        private System.Windows.Forms.TextBox textBoxAlarm;
        private System.Windows.Forms.Timer timerCheckChemical;
        private RJButton ButtonSetting;
        private System.Windows.Forms.PictureBox pictureBox3;
    }
}