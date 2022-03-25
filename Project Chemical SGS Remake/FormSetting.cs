using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;

namespace Project_Chemical_SGS_Remake
{
    public partial class FormSetting : Form
    {
        //Login
        string ID_Emp_Main = Properties.Settings.Default.ID_Emplyee.ToString();
        string Name_Emp_Main = Properties.Settings.Default.Name_Emplyee.ToString();
        string Lastname_Emp_Main = Properties.Settings.Default.Lastname_Emplyee.ToString();

        //ChildForm
        private Form ActiveForm;

        public FormSetting()
        {
            InitializeComponent();
        }


        #region "Button Back"
        private void buttonBackMain_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        #endregion

        #region "Function OpenChildForm"
        private void OpenChildForm(System.Windows.Forms.Form ChildForm)
        {
            if (ActiveForm != null)
            {
                ActiveForm.Close();
            }
            ActiveForm = ChildForm;
            ChildForm.TopLevel = false;
            ChildForm.FormBorderStyle = FormBorderStyle.None;
            ChildForm.Dock = DockStyle.Fill;
            panelChildForm.Controls.Add(ChildForm);
            ChildForm.BringToFront();
            ChildForm.Show();
        }
        #endregion

        private void ButtonEditFormula_Click(object sender, EventArgs e)
        {
            ButtonEditFormula.BackColor = Color.Blue;
            ButtonEditFormula1.BackColor = Color.FromArgb(2, 48, 71);
            ButtonEditTanks.BackColor = Color.FromArgb(2, 48, 71);
            ButtonPermission.BackColor = Color.FromArgb(2, 48, 71);
            OpenChildForm(new FormSettingFormula());
        }

        private void ButtonEditTanks_Click(object sender, EventArgs e)
        {
            ButtonEditTanks.BackColor = Color.Blue;
            ButtonEditFormula1.BackColor = Color.FromArgb(2, 48, 71);
            ButtonEditFormula.BackColor = Color.FromArgb(2, 48, 71);
            ButtonPermission.BackColor = Color.FromArgb(2, 48, 71);
            OpenChildForm(new FormSettingTanksChemical());
        }

        private void ButtonPermission_Click(object sender, EventArgs e)
        {
            ButtonPermission.BackColor = Color.Blue;
            ButtonEditFormula1.BackColor = Color.FromArgb(2, 48, 71);
            ButtonEditFormula.BackColor = Color.FromArgb(2, 48, 71);
            ButtonEditTanks.BackColor = Color.FromArgb(2, 48, 71);
            OpenChildForm(new FormSettingAccessibility());
        }

        private void ButtonEditFormula1_Click(object sender, EventArgs e)
        {
            ButtonEditFormula1.BackColor = Color.Blue;
            ButtonEditFormula.BackColor = Color.FromArgb(2, 48, 71);
            ButtonEditTanks.BackColor = Color.FromArgb(2, 48, 71);
            ButtonPermission.BackColor = Color.FromArgb(2, 48, 71);
            OpenChildForm(new FormSettingFormula1());
        }
    }
}
