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
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace Project_Chemical_SGS_Remake
{
    public partial class FormMain : Form
    {
        //Connect Database Local
        public static string Local_Conn;
        public static string Catalog_Local;
        public static string Ip_Addr_Local;
        public static string Sql_usr_Local;
        public static string Sql_pw_Local;
        SqlConnection conn;
        SqlCommand com;
        DataTable dt;
        SqlDataAdapter adpt;

        //Transaction
        public static bool status_sql;

        //Location Config file temp
        public static string Location_File_Tmp;

        //ChildForm
        private Form ActiveForm;

        //Login
        public string ID_Emp_Main = Properties.Settings.Default.ID_Emplyee.ToString();
        public string Name_Emp_Main = Properties.Settings.Default.Name_Emplyee.ToString();
        public string Lastname_Emp_Main = Properties.Settings.Default.Lastname_Emplyee.ToString();

        int Count_row;
        int Alarm;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            timerCheckChemical.Enabled = true;
            labelCid.Text = Properties.Settings.Default.ID_Emplyee.ToString();
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file.txt");
            Local_Conn = $"Data Source={Ip_Addr_Local};Initial Catalog={Catalog_Local};User ID={Sql_usr_Local};password={Sql_pw_Local}";
            
            labelCid.Text = ID_Emp_Main;
            labelCname.Text = Name_Emp_Main + " " + Lastname_Emp_Main;
            timerCheckChemical.Enabled = false;
        }

        #region "Read System File"
        public static void Read_Systemfile(string Location)
        {
            string[] currentrow;
            try
            {
                TextFieldParser parser = new TextFieldParser(Location, Encoding.GetEncoding("utf-8"))
                {
                    TextFieldType = FieldType.Delimited
                };
                parser.SetDelimiters(";");
                while (parser.EndOfData == false)
                {
                    currentrow = parser.ReadFields();
                    Ip_Addr_Local = currentrow[1];
                    currentrow = parser.ReadFields();
                    Catalog_Local = currentrow[1];
                    currentrow = parser.ReadFields();
                    Sql_usr_Local = currentrow[1];
                    currentrow = parser.ReadFields();
                    Sql_pw_Local = currentrow[1];
                }
            }
            catch
            {
                _ = new LogWriter("Application can't open System_Local.txt, Maybe lost");
                Environment.Exit(0);
            }
        }
        #endregion

        #region "Logout and back"
        private void ButtonLogout_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ID_Emplyee = Convert.ToInt32(null);
            Properties.Settings.Default.Name_Emplyee = null;
            Properties.Settings.Default.Lastname_Emplyee = null;
            Properties.Settings.Default.Date_Login = null;
            Properties.Settings.Default.Check_SerialPort = true;
            Properties.Settings.Default.StatusScan = true;
            Properties.Settings.Default.Save();
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

        #region "Open Form"
        private void ButtonSetting_Click(object sender, EventArgs e)
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(Local_Conn))
            {
                var check = conn.CreateCommand();
                check.CommandText = $"Select * from Chemical_Employee_Local Where ID_Employee ='{ID_Emp_Main}' and Working = 1";
                var sda = new SqlDataAdapter(check);
                sda.Fill(dt);
            }
            string Permis = dt.Rows[0]["Permission"].ToString();
            if (Permis == "Admin")
            {
                OpenChildForm(new FormSetting());
            }
            else
            {
                MessageBox.Show("คุณไม่มีสิทธิในการเข้าถึงข้อมูล โปรดติดต่อผู้ดูแลระบบ", "Warning");
                //MessageBox.Show("You don't have Permission to access ...", "Warning");

            }
        }


        private void ButtonMixing_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormMixing());
        }

        private void rjButton1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormFillChemical());
        }

        private void ButtonWeighingScale_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.MonitorChemical = "";
            Properties.Settings.Default.MonitorFormula = "";
            Properties.Settings.Default.MonitorStatus = "";
            Properties.Settings.Default.MonitorSTD = 0;
            Properties.Settings.Default.MonitorWeight = 0;
            Properties.Settings.Default.Save();
            OpenChildForm(new FormWeight());
        }

        #endregion

        #region "Timer Check Chemical <20"
        private void timerCheckChemical_Tick(object sender, EventArgs e)
        {
            try
            {
                timerCheckChemical.Enabled = false;
                var dt = new DataTable();
                using (var conn = new SqlConnection(Local_Conn))
                {
                    var check = conn.CreateCommand();
                    check.CommandText = $"Select * from Chemical_Address_PLC_Local where Weight_Min <= 20";
                    var sda = new SqlDataAdapter(check);
                    sda.Fill(dt);
                }
                Count_row = dt.Rows.Count;
                if (Count_row >= 1)
                {
                    textBoxAlarm.Text = "There are chemicals that are out. please fill a chemicals.";
                }
                else
                {
                    textBoxAlarm.Text = "";
                }
                timerCheckChemical.Enabled = true;
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormMain Message: {0}, {err.Message}");
            }
            
        }

        #endregion

    }
}
