
namespace Project_Chemical_SGS_Remake
{
    partial class FormWeight
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWeight));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonBackMain = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxItemcode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxLeadPowder = new System.Windows.Forms.TextBox();
            this.textBoxFormula = new System.Windows.Forms.TextBox();
            this.textBoxJOBNumber = new System.Windows.Forms.TextBox();
            this.textBoxAlarm = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.metroGrid1 = new MetroFramework.Controls.MetroGrid();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelWeighed = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelkg2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelActualWeight = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelKg1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelLocation = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.timerWeightScale = new System.Windows.Forms.Timer(this.components);
            this.timerOpentanks = new System.Windows.Forms.Timer(this.components);
            this.timerStock = new System.Windows.Forms.Timer(this.components);
            this.ButtonKeyboard = new Project_Chemical_SGS_Remake.RJButton();
            this.ButtonManual = new Project_Chemical_SGS_Remake.RJButton();
            this.rjButton3 = new Project_Chemical_SGS_Remake.RJButton();
            this.rjButton2 = new Project_Chemical_SGS_Remake.RJButton();
            this.rjButton1 = new Project_Chemical_SGS_Remake.RJButton();
            this.ButtonReset = new Project_Chemical_SGS_Remake.RJButton();
            this.ButtonConfirm = new Project_Chemical_SGS_Remake.RJButton();
            this.ButtonNextweighing = new Project_Chemical_SGS_Remake.RJButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metroGrid1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            this.panel1.TabIndex = 26;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label13.Location = new System.Drawing.Point(60, 60);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(196, 16);
            this.label13.TabIndex = 18;
            this.label13.Text = "Scan JOB Number for weighing.";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(393, 39);
            this.label1.TabIndex = 17;
            this.label1.Text = "Weighing of Chemicals";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel2.Controls.Add(this.buttonBackMain);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Location = new System.Drawing.Point(690, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(333, 88);
            this.panel2.TabIndex = 32;
            // 
            // buttonBackMain
            // 
            this.buttonBackMain.FlatAppearance.BorderSize = 0;
            this.buttonBackMain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBackMain.Image = ((System.Drawing.Image)(resources.GetObject("buttonBackMain.Image")));
            this.buttonBackMain.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.buttonBackMain.Location = new System.Drawing.Point(240, 5);
            this.buttonBackMain.Name = "buttonBackMain";
            this.buttonBackMain.Size = new System.Drawing.Size(90, 60);
            this.buttonBackMain.TabIndex = 26;
            this.buttonBackMain.UseVisualStyleBackColor = true;
            this.buttonBackMain.Click += new System.EventHandler(this.buttonBackMain_Click);
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
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel3.Controls.Add(this.ButtonKeyboard);
            this.panel3.Controls.Add(this.ButtonManual);
            this.panel3.Controls.Add(this.groupBox4);
            this.panel3.Controls.Add(this.ButtonReset);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.textBoxItemcode);
            this.panel3.Controls.Add(this.ButtonConfirm);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.textBoxLeadPowder);
            this.panel3.Controls.Add(this.textBoxFormula);
            this.panel3.Controls.Add(this.textBoxJOBNumber);
            this.panel3.Controls.Add(this.ButtonNextweighing);
            this.panel3.Location = new System.Drawing.Point(690, 94);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(331, 562);
            this.panel3.TabIndex = 33;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rjButton3);
            this.groupBox4.Controls.Add(this.rjButton2);
            this.groupBox4.Controls.Add(this.rjButton1);
            this.groupBox4.Location = new System.Drawing.Point(13, 199);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(308, 110);
            this.groupBox4.TabIndex = 42;
            this.groupBox4.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label11.Location = new System.Drawing.Point(24, 96);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(95, 20);
            this.label11.TabIndex = 29;
            this.label11.Text = "Item Code : ";
            // 
            // textBoxItemcode
            // 
            this.textBoxItemcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.textBoxItemcode.Location = new System.Drawing.Point(125, 91);
            this.textBoxItemcode.Multiline = true;
            this.textBoxItemcode.Name = "textBoxItemcode";
            this.textBoxItemcode.Size = new System.Drawing.Size(185, 30);
            this.textBoxItemcode.TabIndex = 28;
            this.textBoxItemcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxItemcode.TextChanged += new System.EventHandler(this.textBoxItemcode_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 25);
            this.label3.TabIndex = 18;
            this.label3.Text = "Description";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.DarkBlue;
            this.label2.Location = new System.Drawing.Point(5, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(289, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "_______________________________________________";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(9, 169);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 20);
            this.label7.TabIndex = 24;
            this.label7.Text = "Lead Powder : ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(40, 133);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 20);
            this.label6.TabIndex = 24;
            this.label6.Text = "Formula : ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(7, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 20);
            this.label4.TabIndex = 24;
            this.label4.Text = "JOB Number : ";
            // 
            // textBoxLeadPowder
            // 
            this.textBoxLeadPowder.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBoxLeadPowder.Location = new System.Drawing.Point(125, 163);
            this.textBoxLeadPowder.Multiline = true;
            this.textBoxLeadPowder.Name = "textBoxLeadPowder";
            this.textBoxLeadPowder.Size = new System.Drawing.Size(185, 30);
            this.textBoxLeadPowder.TabIndex = 23;
            this.textBoxLeadPowder.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxFormula
            // 
            this.textBoxFormula.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBoxFormula.Location = new System.Drawing.Point(125, 127);
            this.textBoxFormula.Multiline = true;
            this.textBoxFormula.Name = "textBoxFormula";
            this.textBoxFormula.Size = new System.Drawing.Size(185, 30);
            this.textBoxFormula.TabIndex = 23;
            this.textBoxFormula.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxJOBNumber
            // 
            this.textBoxJOBNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBoxJOBNumber.Location = new System.Drawing.Point(125, 55);
            this.textBoxJOBNumber.Multiline = true;
            this.textBoxJOBNumber.Name = "textBoxJOBNumber";
            this.textBoxJOBNumber.Size = new System.Drawing.Size(185, 30);
            this.textBoxJOBNumber.TabIndex = 23;
            this.textBoxJOBNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxAlarm
            // 
            this.textBoxAlarm.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBoxAlarm.ForeColor = System.Drawing.Color.Red;
            this.textBoxAlarm.Location = new System.Drawing.Point(0, 663);
            this.textBoxAlarm.Multiline = true;
            this.textBoxAlarm.Name = "textBoxAlarm";
            this.textBoxAlarm.Size = new System.Drawing.Size(1024, 33);
            this.textBoxAlarm.TabIndex = 34;
            this.textBoxAlarm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(14, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 29);
            this.label5.TabIndex = 35;
            this.label5.Text = "Weighing";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // metroGrid1
            // 
            this.metroGrid1.AllowUserToResizeRows = false;
            this.metroGrid1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.metroGrid1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.metroGrid1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.metroGrid1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.metroGrid1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.metroGrid1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.metroGrid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.metroGrid1.DefaultCellStyle = dataGridViewCellStyle2;
            this.metroGrid1.EnableHeadersVisualStyles = false;
            this.metroGrid1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.metroGrid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.metroGrid1.Location = new System.Drawing.Point(5, 365);
            this.metroGrid1.Name = "metroGrid1";
            this.metroGrid1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.metroGrid1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.metroGrid1.RowHeadersVisible = false;
            this.metroGrid1.RowHeadersWidth = 10;
            this.metroGrid1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.metroGrid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.metroGrid1.Size = new System.Drawing.Size(679, 288);
            this.metroGrid1.TabIndex = 38;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(14, 337);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(141, 25);
            this.label8.TabIndex = 40;
            this.label8.Text = "Informations";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.OrangeRed;
            this.label10.Location = new System.Drawing.Point(80, -6);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 16);
            this.label10.TabIndex = 27;
            this.label10.Text = "_________";
            // 
            // labelWeighed
            // 
            this.labelWeighed.AutoSize = true;
            this.labelWeighed.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.labelWeighed.ForeColor = System.Drawing.Color.DarkOrange;
            this.labelWeighed.Location = new System.Drawing.Point(25, 55);
            this.labelWeighed.Name = "labelWeighed";
            this.labelWeighed.Size = new System.Drawing.Size(150, 55);
            this.labelWeighed.TabIndex = 28;
            this.labelWeighed.Text = "00.00";
            this.labelWeighed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.groupBox2.Controls.Add(this.labelkg2);
            this.groupBox2.Controls.Add(this.labelWeighed);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.groupBox2.Location = new System.Drawing.Point(246, 135);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 180);
            this.groupBox2.TabIndex = 37;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Weighed";
            // 
            // labelkg2
            // 
            this.labelkg2.AutoSize = true;
            this.labelkg2.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.labelkg2.ForeColor = System.Drawing.Color.DarkOrange;
            this.labelkg2.Location = new System.Drawing.Point(137, 140);
            this.labelkg2.Name = "labelkg2";
            this.labelkg2.Size = new System.Drawing.Size(57, 33);
            this.labelkg2.TabIndex = 28;
            this.labelkg2.Text = "kg.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.DarkBlue;
            this.label9.Location = new System.Drawing.Point(111, -6);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 16);
            this.label9.TabIndex = 26;
            this.label9.Text = "______";
            // 
            // labelActualWeight
            // 
            this.labelActualWeight.AutoSize = true;
            this.labelActualWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.labelActualWeight.ForeColor = System.Drawing.Color.DarkBlue;
            this.labelActualWeight.Location = new System.Drawing.Point(25, 55);
            this.labelActualWeight.Name = "labelActualWeight";
            this.labelActualWeight.Size = new System.Drawing.Size(150, 55);
            this.labelActualWeight.TabIndex = 27;
            this.labelActualWeight.Text = "00.00";
            this.labelActualWeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.groupBox1.Controls.Add(this.labelKg1);
            this.groupBox1.Controls.Add(this.labelActualWeight);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.groupBox1.Location = new System.Drawing.Point(40, 135);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 180);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Weight STD";
            // 
            // labelKg1
            // 
            this.labelKg1.AutoSize = true;
            this.labelKg1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.labelKg1.ForeColor = System.Drawing.Color.DarkBlue;
            this.labelKg1.Location = new System.Drawing.Point(137, 140);
            this.labelKg1.Name = "labelKg1";
            this.labelKg1.Size = new System.Drawing.Size(57, 33);
            this.labelKg1.TabIndex = 28;
            this.labelKg1.Text = "kg.";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.groupBox3.Controls.Add(this.labelLocation);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.groupBox3.Location = new System.Drawing.Point(452, 135);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 180);
            this.groupBox3.TabIndex = 38;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Stock";
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.labelLocation.ForeColor = System.Drawing.Color.DarkGreen;
            this.labelLocation.Location = new System.Drawing.Point(85, 55);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(41, 55);
            this.labelLocation.TabIndex = 28;
            this.labelLocation.Text = "-";
            this.labelLocation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.DarkGreen;
            this.label14.Location = new System.Drawing.Point(78, -6);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(88, 16);
            this.label14.TabIndex = 27;
            this.label14.Text = "__________";
            // 
            // timerWeightScale
            // 
            this.timerWeightScale.Tick += new System.EventHandler(this.timerWeightScale_Tick);
            // 
            // timerStock
            // 
            this.timerStock.Tick += new System.EventHandler(this.timerStock_Tick);
            // 
            // ButtonKeyboard
            // 
            this.ButtonKeyboard.BackColor = System.Drawing.Color.DarkCyan;
            this.ButtonKeyboard.BackgroundColor = System.Drawing.Color.DarkCyan;
            this.ButtonKeyboard.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.ButtonKeyboard.BorderRadius = 40;
            this.ButtonKeyboard.BorderSize = 0;
            this.ButtonKeyboard.FlatAppearance.BorderSize = 0;
            this.ButtonKeyboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonKeyboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.ButtonKeyboard.ForeColor = System.Drawing.Color.White;
            this.ButtonKeyboard.Location = new System.Drawing.Point(172, 315);
            this.ButtonKeyboard.Name = "ButtonKeyboard";
            this.ButtonKeyboard.Size = new System.Drawing.Size(150, 40);
            this.ButtonKeyboard.TabIndex = 44;
            this.ButtonKeyboard.Text = "On Keyboard";
            this.ButtonKeyboard.TextColor = System.Drawing.Color.White;
            this.ButtonKeyboard.UseVisualStyleBackColor = false;
            this.ButtonKeyboard.Click += new System.EventHandler(this.ButtonKeyboard_Click);
            // 
            // ButtonManual
            // 
            this.ButtonManual.BackColor = System.Drawing.Color.DarkCyan;
            this.ButtonManual.BackgroundColor = System.Drawing.Color.DarkCyan;
            this.ButtonManual.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.ButtonManual.BorderRadius = 40;
            this.ButtonManual.BorderSize = 0;
            this.ButtonManual.FlatAppearance.BorderSize = 0;
            this.ButtonManual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonManual.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.ButtonManual.ForeColor = System.Drawing.Color.White;
            this.ButtonManual.Location = new System.Drawing.Point(11, 315);
            this.ButtonManual.Name = "ButtonManual";
            this.ButtonManual.Size = new System.Drawing.Size(150, 40);
            this.ButtonManual.TabIndex = 43;
            this.ButtonManual.Text = "Manual input";
            this.ButtonManual.TextColor = System.Drawing.Color.White;
            this.ButtonManual.UseVisualStyleBackColor = false;
            this.ButtonManual.Click += new System.EventHandler(this.ButtonManual_Click);
            // 
            // rjButton3
            // 
            this.rjButton3.BackColor = System.Drawing.Color.White;
            this.rjButton3.BackgroundColor = System.Drawing.Color.White;
            this.rjButton3.BorderColor = System.Drawing.Color.Black;
            this.rjButton3.BorderRadius = 40;
            this.rjButton3.BorderSize = 1;
            this.rjButton3.FlatAppearance.BorderSize = 0;
            this.rjButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.rjButton3.ForeColor = System.Drawing.Color.Black;
            this.rjButton3.Location = new System.Drawing.Point(206, 13);
            this.rjButton3.Name = "rjButton3";
            this.rjButton3.Size = new System.Drawing.Size(91, 85);
            this.rjButton3.TabIndex = 0;
            this.rjButton3.Text = "900 kg";
            this.rjButton3.TextColor = System.Drawing.Color.Black;
            this.rjButton3.UseVisualStyleBackColor = false;
            this.rjButton3.Click += new System.EventHandler(this.rjButton3_Click);
            // 
            // rjButton2
            // 
            this.rjButton2.BackColor = System.Drawing.Color.White;
            this.rjButton2.BackgroundColor = System.Drawing.Color.White;
            this.rjButton2.BorderColor = System.Drawing.Color.Black;
            this.rjButton2.BorderRadius = 40;
            this.rjButton2.BorderSize = 1;
            this.rjButton2.FlatAppearance.BorderSize = 0;
            this.rjButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.rjButton2.ForeColor = System.Drawing.Color.Black;
            this.rjButton2.Location = new System.Drawing.Point(109, 13);
            this.rjButton2.Name = "rjButton2";
            this.rjButton2.Size = new System.Drawing.Size(91, 85);
            this.rjButton2.TabIndex = 0;
            this.rjButton2.Text = "600 kg";
            this.rjButton2.TextColor = System.Drawing.Color.Black;
            this.rjButton2.UseVisualStyleBackColor = false;
            this.rjButton2.Click += new System.EventHandler(this.rjButton2_Click);
            // 
            // rjButton1
            // 
            this.rjButton1.BackColor = System.Drawing.Color.White;
            this.rjButton1.BackgroundColor = System.Drawing.Color.White;
            this.rjButton1.BorderColor = System.Drawing.Color.Black;
            this.rjButton1.BorderRadius = 40;
            this.rjButton1.BorderSize = 1;
            this.rjButton1.FlatAppearance.BorderSize = 0;
            this.rjButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.rjButton1.ForeColor = System.Drawing.Color.Black;
            this.rjButton1.Location = new System.Drawing.Point(12, 15);
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(91, 85);
            this.rjButton1.TabIndex = 0;
            this.rjButton1.Text = "300 kg";
            this.rjButton1.TextColor = System.Drawing.Color.Black;
            this.rjButton1.UseVisualStyleBackColor = false;
            this.rjButton1.Click += new System.EventHandler(this.rjButton1_Click);
            // 
            // ButtonReset
            // 
            this.ButtonReset.BackColor = System.Drawing.Color.Firebrick;
            this.ButtonReset.BackgroundColor = System.Drawing.Color.Firebrick;
            this.ButtonReset.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.ButtonReset.BorderRadius = 40;
            this.ButtonReset.BorderSize = 0;
            this.ButtonReset.FlatAppearance.BorderSize = 0;
            this.ButtonReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.ButtonReset.ForeColor = System.Drawing.Color.White;
            this.ButtonReset.Image = ((System.Drawing.Image)(resources.GetObject("ButtonReset.Image")));
            this.ButtonReset.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ButtonReset.Location = new System.Drawing.Point(11, 478);
            this.ButtonReset.Name = "ButtonReset";
            this.ButtonReset.Size = new System.Drawing.Size(310, 76);
            this.ButtonReset.TabIndex = 25;
            this.ButtonReset.Text = "   Reset";
            this.ButtonReset.TextColor = System.Drawing.Color.White;
            this.ButtonReset.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ButtonReset.UseVisualStyleBackColor = false;
            this.ButtonReset.Click += new System.EventHandler(this.ButtonReset_Click);
            // 
            // ButtonConfirm
            // 
            this.ButtonConfirm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonConfirm.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonConfirm.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.ButtonConfirm.BorderRadius = 40;
            this.ButtonConfirm.BorderSize = 0;
            this.ButtonConfirm.FlatAppearance.BorderSize = 0;
            this.ButtonConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.ButtonConfirm.ForeColor = System.Drawing.Color.White;
            this.ButtonConfirm.Image = ((System.Drawing.Image)(resources.GetObject("ButtonConfirm.Image")));
            this.ButtonConfirm.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ButtonConfirm.Location = new System.Drawing.Point(11, 362);
            this.ButtonConfirm.Name = "ButtonConfirm";
            this.ButtonConfirm.Size = new System.Drawing.Size(310, 110);
            this.ButtonConfirm.TabIndex = 25;
            this.ButtonConfirm.Text = "   Confirm";
            this.ButtonConfirm.TextColor = System.Drawing.Color.White;
            this.ButtonConfirm.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ButtonConfirm.UseVisualStyleBackColor = false;
            this.ButtonConfirm.Click += new System.EventHandler(this.ButtonConfirm_Click);
            // 
            // ButtonNextweighing
            // 
            this.ButtonNextweighing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonNextweighing.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.ButtonNextweighing.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.ButtonNextweighing.BorderRadius = 40;
            this.ButtonNextweighing.BorderSize = 0;
            this.ButtonNextweighing.FlatAppearance.BorderSize = 0;
            this.ButtonNextweighing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonNextweighing.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.ButtonNextweighing.ForeColor = System.Drawing.Color.White;
            this.ButtonNextweighing.Image = ((System.Drawing.Image)(resources.GetObject("ButtonNextweighing.Image")));
            this.ButtonNextweighing.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ButtonNextweighing.Location = new System.Drawing.Point(11, 362);
            this.ButtonNextweighing.Name = "ButtonNextweighing";
            this.ButtonNextweighing.Size = new System.Drawing.Size(310, 110);
            this.ButtonNextweighing.TabIndex = 25;
            this.ButtonNextweighing.Text = "   Weighing";
            this.ButtonNextweighing.TextColor = System.Drawing.Color.White;
            this.ButtonNextweighing.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ButtonNextweighing.UseVisualStyleBackColor = false;
            this.ButtonNextweighing.Click += new System.EventHandler(this.ButtonNextweighing_Click);
            // 
            // FormWeight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1024, 696);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.metroGrid1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxAlarm);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormWeight";
            this.Text = "FormWeight";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormWeight_FormClosing);
            this.Load += new System.EventHandler(this.FormWeight_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.metroGrid1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonBackMain;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel3;
        private RJButton ButtonConfirm;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxFormula;
        private System.Windows.Forms.TextBox textBoxJOBNumber;
        private System.Windows.Forms.TextBox textBoxAlarm;
        private System.Windows.Forms.Label label5;
        private MetroFramework.Controls.MetroGrid metroGrid1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelWeighed;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelActualWeight;
        private System.Windows.Forms.GroupBox groupBox1;
        private RJButton ButtonNextweighing;
        private System.Windows.Forms.Label labelkg2;
        private System.Windows.Forms.Label labelKg1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label labelLocation;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxItemcode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxLeadPowder;
        private System.Windows.Forms.Timer timerWeightScale;
        private System.Windows.Forms.Timer timerOpentanks;
        private RJButton ButtonReset;
        private System.Windows.Forms.GroupBox groupBox4;
        private RJButton rjButton3;
        private RJButton rjButton2;
        private RJButton rjButton1;
        private System.Windows.Forms.Timer timerStock;
        private RJButton ButtonKeyboard;
        private RJButton ButtonManual;
    }
}