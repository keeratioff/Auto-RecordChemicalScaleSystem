using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using EasyModbus;

namespace Project_Chemical_SGS_Remake
{
    public partial class FormMixing : Form
    {
        #region "Keyboard"
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);
        private const UInt32 WM_SYSCOMMAND = 0x112;
        private const UInt32 SC_RESTORE = 0xf120;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        private string OnScreenKeyboadApplication = "osk.exe";
        #endregion


        //Modbus
        ModbusClient ModbusClient = new ModbusClient();

        //Serial Port
        string Serial_DataIn;


        //Database
        public static string Local_Conn;
        public static string Catalog_Local;
        public static string Ip_Addr_Local;
        public static string Sql_usr_Local;
        public static string Sql_pw_Local;

        //Location Config file temp
        public static string Location_File_Tmp;

        public static bool status_sql;

        //Select Pending
        public static int Count_row1;
        public static int Count_row2;
        public static int Count_row3;
        public static int Count_rowShip1;
        public static int Count_rowNight1;
        public static int Count_rowShip2;
        public static int Count_rowNight2;
        public static int Count_rowShip3;
        public static int Count_rowNight3;
        public static int result;

        //Select Complete
        public static int Count_row1_Complete;
        public static int Count_row2_Complete;
        public static int Count_row3_Complete;
        public static int Count_rowShip_Complete1;
        public static int Count_rowNight_Complete1;
        public static int Count_rowShip_Complete2;
        public static int Count_rowNight_Complete2;
        public static int Count_rowShip_Complete3;
        public static int Count_rowNight_Complete3;
        public static int result_Complete;

        public static string Status_Time;

        //Check Location PLC 1 = Full, 0 = empty
        public int Location_PLC_A;
        public int Location_PLC_B;
        public int Location_PLC_C;

        //Machine mixing
        public static string Machine_Mixing;

        //SerialPort
        public static SerialPort mySerialPort;

        //Delay for Write PLC
        public int mydelay = 100;


        //public static string TextIn;

        public FormMixing()
        {
            InitializeComponent();
        }

        private void FormMixing_Load(object sender, EventArgs e)
        {
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file.txt");
            Local_Conn = $"Data Source={Ip_Addr_Local};Initial Catalog={Catalog_Local};User ID={Sql_usr_Local};password={Sql_pw_Local}";
            InitializeDataGridView();
            Modbus_Client_Port502();
            timerCheckSensor.Enabled = true;
            GetSerialPort();
            Create_table();
            ShowGrid();
            //groupBox1.Hide();
            //groupBox2.Hide();
            //groupBox5.Hide();
            //groupBox6.Hide();
            Select_Time();
            SelectLocation1();
            SelectLocation2();
            SelectLocation3();
            Result();
            SelectComplete1();
            SelectComplete2();
            SelectComplete3();
            ResultComplete();
            //Function_CheckLocation_PLC();
            ButtonConfirm.Hide();
            textBoxJobNumber.Enabled = false;
            textBoxFormula.Enabled = false;
            textBoxItemcode.Enabled = false;
            textBoxMachine.Enabled = false;
        }

        private void FormMixing_FormClosing(object sender, FormClosingEventArgs e)
        {
            mySerialPort.Close();
        }

        #region "Select Time"
        private void Select_Time()
        {
            try
            {
                string datecheck_H = DateTime.Now.ToString("HH");
                int convdatecheckH = Convert.ToInt32(datecheck_H);
                string datecheck_M = DateTime.Now.ToString("mm");
                int convdatecheckM = Convert.ToInt32(datecheck_M);
                if (convdatecheckH >= 7 && convdatecheckH <= 19)
                {
                    groupBox1.Show();
                    groupBox2.Show();
                    groupBox5.Hide();
                    groupBox6.Hide();
                    Status_Time = "Daytime";
                }
                else
                {
                    groupBox5.Show();
                    groupBox6.Show();
                    groupBox1.Hide();
                    groupBox2.Hide();
                    
                    Status_Time = "Nighttime";
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter($"   Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "Label Select Pending"
        private void SelectLocation1()
        {
            #region "Hiden Code"
            //try
            //{
            //    var dt = new DataTable();
            //    using (var conn = new SqlConnection(Local_Conn))
            //    {
            //        var check = conn.CreateCommand();
            //        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'A'";
            //        var sda = new SqlDataAdapter(check);
            //        sda.Fill(dt);
            //    }
            //    int Count_row = dt.Rows.Count;
            //    if (Count_row >= 1)
            //    {
            //        Count_row1 = 1;
            //    }
            //}
            //catch (Exception err)
            //{
            //    _ = new LogWriter($" Error FormMixing Message: {0}, {err.Message}");
            //}
            //try
            //{
            //    string datecheck_H = DateTime.Now.ToString("HH");
            //    int convdatecheckH = Convert.ToInt32(datecheck_H);
            //    string datecheck_M = DateTime.Now.ToString("mm");
            //    int convdatecheckM = Convert.ToInt32(datecheck_M);
            //    if (convdatecheckH >= 7 && convdatecheckH <= 19)
            //    {
            //        string getdate = DateTime.Now.ToString("yyyy-MM-dd");
            //        string date1 = (getdate + "T07:30:00.000");
            //        string date1_1 = (getdate + "T19:29:00.000");

            //        var dt = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'A' and DateTime between '{date1}' and '{date1_1}'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt);
            //        }
            //        int count_row = dt.Rows.Count;
            //        if (count_row >= 1)
            //        {
            //            Count_row1 = 1;
            //        }
            //    }
            //    else
            //    {
            //        string getdate2 = DateTime.Now.ToString("yyyy-MM-dd");
            //        string date2 = (getdate2 + "T19:30:00.000");
            //        string date2_2 = (getdate2 + "T23:59:00.000");
            //        string date2_3 = (getdate2 + "T00:00:00.000");
            //        string date2_4 = (getdate2 + "T07:29:00.000");

            //        var dt2 = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'A' and DateTime between '{date2}' and '{date2_2}'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt2);
            //        }
            //        int count_row1 = dt2.Rows.Count;
            //        if (count_row1 >= 1)
            //        {
            //            Count_rowShip = 1;
            //        }
            //        var dt3 = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'A' and DateTime between '{date2_3}' and '{date2_4}'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt3);
            //        }
            //        int count_row2 = dt3.Rows.Count;
            //        if (count_row2 >= 1)
            //        {
            //            Count_rowNight = 1;
            //        }
            //        Count_row1 = Count_rowShip + Count_rowNight;
            //    }
            //}
            //catch (Exception err)
            //{
            //    _ = new LogWriter($" Error FormMixing Message: {0}, {err.Message}");
            //}
            #endregion
            try
            {
                if (Status_Time == "Daytime")
                {
                    string getdate = DateTime.Now.ToString("yyyy-MM-dd");
                    string date1 = (getdate + "T07:30:00.000");
                    string date1_1 = (getdate + "T19:29:00.000");

                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'A' and DateTime between '{date1}' and '{date1_1}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    int count_row = dt.Rows.Count;
                    if (count_row >= 1)
                    {
                        Count_row1 = 1;
                    }
                }
                else if (Status_Time == "Nighttime")
                {
                    string getdate2 = DateTime.Now.ToString("yyyy-MM-dd");
                    string date2 = (getdate2 + "T19:30:00.000");
                    string date2_2 = (getdate2 + "T23:59:00.000");
                    string date2_3 = (getdate2 + "T00:00:00.000");
                    string date2_4 = (getdate2 + "T07:29:00.000");
                    var dt2 = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'A' and DateTime between '{date2}' and '{date2_2}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt2);
                    }
                    int count_row1 = dt2.Rows.Count;
                    if (count_row1 >= 1)
                    {
                        Count_rowShip1 = 1;
                    }
                    var dt3 = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'A' and DateTime between '{date2_3}' and '{date2_4}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt3);
                    }
                    int count_row2 = dt3.Rows.Count;
                    if (count_row2 >= 1)
                    {
                        Count_rowNight1 = 1;
                    }
                    Count_row1 = Count_rowShip1 + Count_rowNight1;
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter($"   Message: {0}, {error.Message}");
            }
        }

        private void SelectLocation2()
        {
            #region "Hiden Code"
            //try
            //{
            //    var dt = new DataTable();
            //    using (var conn = new SqlConnection(Local_Conn))
            //    {
            //        var check = conn.CreateCommand();
            //        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'B'";
            //        var sda = new SqlDataAdapter(check);
            //        sda.Fill(dt);
            //    }
            //    int Count_row = dt.Rows.Count;
            //    if (Count_row >= 1)
            //    {
            //        Count_row2 = 1;
            //    }
            //}
            //catch (Exception err)
            //{
            //    _ = new LogWriter($" Error FormMixing Message: {0}, {err.Message}");
            //}
            //try
            //{
            //    string datecheck_H = DateTime.Now.ToString("hh");
            //    int convdatecheckH = Convert.ToInt32(datecheck_H);
            //    string datecheck_M = DateTime.Now.ToString("mm");
            //    int convdatecheckM = Convert.ToInt32(datecheck_M);
            //    if (convdatecheckH >= 7 && convdatecheckH <= 19)
            //    {
            //        string getdate = DateTime.Now.ToString("yyyy-MM-dd");
            //        string date1 = (getdate + "T07:30:00.000");
            //        string date1_1 = (getdate + "T19:29:00.000");

            //        var dt = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'B' and DateTime between '{date1}' and '{date1_1}'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt);
            //        }
            //        int count_row = dt.Rows.Count;
            //        if (count_row >= 1)
            //        {
            //            Count_row2 = 1;
            //        }
            //    }
            //    else
            //    {
            //        string getdate2 = DateTime.Now.ToString("yyyy-MM-dd");
            //        string date2 = (getdate2 + "T19:30:00.000");
            //        string date2_2 = (getdate2 + "T23:59:00.000");
            //        string date2_3 = (getdate2 + "T00:00:00.000");
            //        string date2_4 = (getdate2 + "T07:29:00.000");

            //        var dt2 = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'B' and DateTime between '{date2}' and '{date2_2}'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt2);
            //        }
            //        int count_row1 = dt2.Rows.Count;
            //        if (count_row1 >= 1)
            //        {
            //            Count_rowShip = 1;
            //        }
            //        var dt3 = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'B' and DateTime between '{date2_3}' and '{date2_4}'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt3);
            //        }
            //        int count_row2 = dt3.Rows.Count;
            //        if (count_row2 >= 1)
            //        {
            //            Count_rowNight = 1;
            //        }
            //        Count_row2 = Count_rowShip + Count_rowNight;
            //    }
            //}
            //catch (Exception err)
            //{
            //    _ = new LogWriter($" Error FormMixing Message: {0}, {err.Message}");
            //}
            #endregion
            try
            {
                if (Status_Time == "Daytime")
                {
                    string getdate = DateTime.Now.ToString("yyyy-MM-dd");
                    string date1 = (getdate + "T07:30:00.000");
                    string date1_1 = (getdate + "T19:29:00.000");

                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'B' and DateTime between '{date1}' and '{date1_1}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    int count_row = dt.Rows.Count;
                    if (count_row >= 1)
                    {
                        Count_row2 = 1;
                    }
                }
                else if (Status_Time == "Nighttime")
                {
                    string getdate2 = DateTime.Now.ToString("yyyy-MM-dd");
                    string date2 = (getdate2 + "T19:30:00.000");
                    string date2_2 = (getdate2 + "T23:59:00.000");
                    string date2_3 = (getdate2 + "T00:00:00.000");
                    string date2_4 = (getdate2 + "T07:29:00.000");
                    var dt2 = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'B' and DateTime between '{date2}' and '{date2_2}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt2);
                    }
                    int count_row1 = dt2.Rows.Count;
                    if (count_row1 >= 1)
                    {
                        Count_rowShip2 = 1;
                    }
                    var dt3 = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'B' and DateTime between '{date2_3}' and '{date2_4}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt3);
                    }
                    int count_row2 = dt3.Rows.Count;
                    if (count_row2 >= 1)
                    {
                        Count_rowNight2 = 1;
                    }
                    Count_row2 = Count_rowShip2 + Count_rowNight2;
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter($"   Message: {0}, {error.Message}");
            }
        }

        private void SelectLocation3()
        {
            #region "Hiden Code"
            //try
            //{
            //    var dt = new DataTable();
            //    using (var conn = new SqlConnection(Local_Conn))
            //    {
            //        var check = conn.CreateCommand();
            //        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'C'";
            //        var sda = new SqlDataAdapter(check);
            //        sda.Fill(dt);
            //    }
            //    int Count_row = dt.Rows.Count;
            //    if (Count_row >= 1)
            //    {
            //        Count_row3 = 1;
            //    }
            //}
            //catch (Exception err)
            //{
            //    _ = new LogWriter($" Error FormMixing Message: {0}, {err.Message}");
            //}
            //try
            //{
            //    string datecheck_H = DateTime.Now.ToString("hh");
            //    int convdatecheckH = Convert.ToInt32(datecheck_H);
            //    string datecheck_M = DateTime.Now.ToString("mm");
            //    int convdatecheckM = Convert.ToInt32(datecheck_M);
            //    if (convdatecheckH >= 7 && convdatecheckH <= 19)
            //    {
            //        string getdate = DateTime.Now.ToString("yyyy-MM-dd");
            //        string date1 = (getdate + "T07:30:00.000");
            //        string date1_1 = (getdate + "T19:29:00.000");

            //        var dt = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'C' and DateTime between '{date1}' and '{date1_1}'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt);
            //        }
            //        int count_row = dt.Rows.Count;
            //        if (count_row >= 1)
            //        {
            //            Count_row3 = 1;
            //        }
            //    }
            //    else
            //    {
            //        string getdate2 = DateTime.Now.ToString("yyyy-MM-dd");
            //        string date2 = (getdate2 + "T19:30:00.000");
            //        string date2_2 = (getdate2 + "T23:59:00.000");
            //        string date2_3 = (getdate2 + "T00:00:00.000");
            //        string date2_4 = (getdate2 + "T07:29:00.000");

            //        var dt2 = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'C' and DateTime between '{date2}' and '{date2_2}'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt2);
            //        }
            //        int count_row1 = dt2.Rows.Count;
            //        if (count_row1 >= 1)
            //        {
            //            Count_rowShip = 1;
            //        }
            //        var dt3 = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'C' and DateTime between '{date2_3}' and '{date2_4}'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt3);
            //        }
            //        int count_row2 = dt3.Rows.Count;
            //        if (count_row2 >= 1)
            //        {
            //            Count_rowNight = 1;
            //        }
            //        Count_row3 = Count_rowShip + Count_rowNight;
            //    }
            //}
            //catch (Exception err)
            //{
            //    _ = new LogWriter($" Error FormMixing Message: {0}, {err.Message}");
            //}
            #endregion
            try
            {
                if (Status_Time == "Daytime")
                {
                    string getdate = DateTime.Now.ToString("yyyy-MM-dd");
                    string date1 = (getdate + "T07:30:00.000");
                    string date1_1 = (getdate + "T19:29:00.000");

                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'C' and DateTime between '{date1}' and '{date1_1}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    int count_row = dt.Rows.Count;
                    if (count_row >= 1)
                    {
                        Count_row3 = 1;
                    }
                }
                else if (Status_Time == "Nighttime")
                {
                    string getdate2 = DateTime.Now.ToString("yyyy-MM-dd");
                    string date2 = (getdate2 + "T19:30:00.000");
                    string date2_2 = (getdate2 + "T23:59:00.000");
                    string date2_3 = (getdate2 + "T00:00:00.000");
                    string date2_4 = (getdate2 + "T07:29:00.000");
                    var dt2 = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'C' and DateTime between '{date2}' and '{date2_2}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt2);
                    }
                    int count_row1 = dt2.Rows.Count;
                    if (count_row1 >= 1)
                    {
                        Count_rowShip3 = 1;
                    }
                    var dt3 = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Pending' and Location_waiting = 'C' and DateTime between '{date2_3}' and '{date2_4}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt3);
                    }
                    int count_row2 = dt3.Rows.Count;
                    if (count_row2 >= 1)
                    {
                        Count_rowNight3 = 1;
                    }
                    Count_row3 = Count_rowShip3 + Count_rowNight3;
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter($"   Message: {0}, {error.Message}");
            }
        }

        private void Result()
        {
            result = Count_row1 + Count_row2 + Count_row3;
            if (Status_Time == "Daytime")
            {
                //labelPendingTasks.Text = result.ToString();
                labelPendingTasks.Text = Convert.ToString(result);
            }
            else if (Status_Time == "Nighttime")
            {
                //label10.Text = result.ToString();
                label10.Text = Convert.ToString(result);
            }
        }
        #endregion

        #region "Label Select Complete"
        private void SelectComplete1()
        {
            #region "Hiden Code"
            //try
            //{
            //    var dt = new DataTable();
            //    using (var conn = new SqlConnection(Local_Conn))
            //    {
            //        var check = conn.CreateCommand();
            //        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'A'";
            //        var sda = new SqlDataAdapter(check);
            //        sda.Fill(dt);
            //    }
            //    int Count_row = dt.Rows.Count;
            //    if (Count_row >= 1)
            //    {
            //        string Date = dt.Rows[0]["DateTime"].ToString();
            //        string Date_Today = DateTime.Today.ToString();
            //        if (Date == Date_Today)
            //        {
            //            Count_row1_Complete = 1;
            //        }
            //    }
            //}
            //catch (Exception err)
            //{
            //    _ = new LogWriter($" Error FormMixing Message: {0}, {err.Message}");
            //}
            //try
            //{
            //    string datecheck_H = DateTime.Now.ToString("hh");

            //    string datecheck_M = DateTime.Now.ToString("mm");
            //    if (Convert.ToInt32(datecheck_H) >= 7 && Convert.ToInt32(datecheck_H) <= 19)
            //    {
            //        string getdate = DateTime.Now.ToString("yyyy-MM-dd");
            //        string date1 = (getdate + "T07:30:00.000");
            //        string date1_1 = (getdate + "T19:29:00.000");

            //        var dt = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'A' and DateTime between '{date1}' and '{date1_1}'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt);
            //        }
            //        int count_row = dt.Rows.Count;
            //        if (count_row >= 1)
            //        {
            //            Count_row1 = 1;
            //        }
            //    }
            //    else
            //    {
            //        string getdate2 = DateTime.Now.ToString("yyyy-MM-dd");
            //        string date2 = (getdate2 + "T19:30:00.000");
            //        string date2_2 = (getdate2 + "T23:59:00.000");
            //        string date2_3 = (getdate2 + "T00:00:00.000");
            //        string date2_4 = (getdate2 + "T07:29:00.000");

            //        var dt2 = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'A' and DateTime between '{date2}' and '{date2_2}'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt2);
            //        }
            //        int count_row1 = dt2.Rows.Count;
            //        var dt3 = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'A' and DateTime between '{date2_3}' and '{date2_4}'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt3);
            //        }
            //        int count_row2 = dt3.Rows.Count;
            //        Count_row1 = count_row1 + count_row2;
            //    }
            //}
            //catch (Exception err)
            //{
            //    _ = new LogWriter($" Error FormMixing Message: {0}, {err.Message}");
            //}
            #endregion
            try
            {
                if (Status_Time == "Daytime")
                {
                    string getdate = DateTime.Now.ToString("yyyy-MM-dd");
                    string date1 = (getdate + "T07:30:00.000");
                    string date1_1 = (getdate + "T19:29:00.000");

                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'A' and DateTime between '{date1}' and '{date1_1}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    int count_row = dt.Rows.Count;
                    if (count_row >= 1)
                    {
                        Count_row1_Complete = 1;
                    }
                }
                else if (Status_Time == "Nighttime")
                {
                    string getdate2 = DateTime.Now.ToString("yyyy-MM-dd");
                    string date2 = (getdate2 + "T19:30:00.000");
                    string date2_2 = (getdate2 + "T23:59:00.000");
                    string date2_3 = (getdate2 + "T00:00:00.000");
                    string date2_4 = (getdate2 + "T07:29:00.000");
                    var dt2 = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'A' and DateTime between '{date2}' and '{date2_2}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt2);
                    }
                    int count_row1 = dt2.Rows.Count;
                    if (count_row1 >= 1)
                    {
                        Count_rowShip_Complete1 = 1;
                    }
                    var dt3 = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'A' and DateTime between '{date2_3}' and '{date2_4}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt3);
                    }
                    int count_row2 = dt3.Rows.Count;
                    if (count_row2 >= 1)
                    {
                        Count_rowNight_Complete1 = 1;
                    }
                    Count_row1_Complete = Count_rowShip_Complete1 + Count_rowNight_Complete1;
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter($"   Message: {0}, {error.Message}");
            }
            
        }
        private void SelectComplete2()
        {
            #region "Hiden Code"
            //try
            //{
            //    var dt = new DataTable();
            //    using (var conn = new SqlConnection(Local_Conn))
            //    {
            //        var check = conn.CreateCommand();
            //        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'B'";
            //        var sda = new SqlDataAdapter(check);
            //        sda.Fill(dt);
            //    }
            //    int Count_row = dt.Rows.Count;
            //    if (Count_row >= 1)
            //    {
            //        string Date = dt.Rows[0]["DateTime"].ToString();
            //        string Date_Today = DateTime.Today.ToString();
            //        if (Date == Date_Today)
            //        {
            //            Count_row2_Complete = 1;
            //        }
            //    }
            //}
            //catch (Exception err)
            //{
            //    _ = new LogWriter($" Error FormMixing Message: {0}, {err.Message}");
            //}
            //try
            //{
            //    string datecheck_H = DateTime.Now.ToString("hh");
            //    string datecheck_M = DateTime.Now.ToString("mm");
            //    if (Convert.ToInt32(datecheck_H) >= 7 && Convert.ToInt32(datecheck_H) <= 19)
            //    {
            //        string getdate = DateTime.Now.ToString("yyyy-MM-dd");
            //        string date1 = (getdate + "T07:30:00.000");
            //        string date1_1 = (getdate + "T19:29:00.000");

            //        var dt = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'B' and DateTime between '{date1}' and '{date1_1}'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt);
            //        }
            //        int count_row = dt.Rows.Count;
            //        if (count_row >= 1)
            //        {
            //            Count_row1 = 1;
            //        }
            //    }
            //    else
            //    {
            //        string getdate2 = DateTime.Now.ToString("yyyy-MM-dd");
            //        string date2 = (getdate2 + "T19:30:00.000");
            //        string date2_2 = (getdate2 + "T23:59:00.000");
            //        string date2_3 = (getdate2 + "T00:00:00.000");
            //        string date2_4 = (getdate2 + "T07:29:00.000");

            //        var dt2 = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'B' and DateTime between '{date2}' and '{date2_2}'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt2);
            //        }
            //        int count_row1 = dt2.Rows.Count;
            //        var dt3 = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'B' and DateTime between '{date2_3}' and '{date2_4}'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt3);
            //        }
            //        int count_row2 = dt3.Rows.Count;
            //        Count_row1 = count_row1 + count_row2;
            //    }
            //}
            //catch (Exception err)
            //{
            //    _ = new LogWriter($" Error FormMixing Message: {0}, {err.Message}");
            //}
            #endregion
            try
            {
                if (Status_Time == "Daytime")
                {
                    string getdate = DateTime.Now.ToString("yyyy-MM-dd");
                    string date1 = (getdate + "T07:30:00.000");
                    string date1_1 = (getdate + "T19:29:00.000");

                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'B' and DateTime between '{date1}' and '{date1_1}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    int count_row = dt.Rows.Count;
                    if (count_row >= 1)
                    {
                        Count_row2_Complete = 1;
                    }
                }
                else if (Status_Time == "Nighttime")
                {
                    string getdate2 = DateTime.Now.ToString("yyyy-MM-dd");
                    string date2 = (getdate2 + "T19:30:00.000");
                    string date2_2 = (getdate2 + "T23:59:00.000");
                    string date2_3 = (getdate2 + "T00:00:00.000");
                    string date2_4 = (getdate2 + "T07:29:00.000");
                    var dt2 = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'B' and DateTime between '{date2}' and '{date2_2}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt2);
                    }
                    int count_row1 = dt2.Rows.Count;
                    if (count_row1 >= 1)
                    {
                        Count_rowShip_Complete2 = 1;
                    }
                    var dt3 = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'B' and DateTime between '{date2_3}' and '{date2_4}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt3);
                    }
                    int count_row2 = dt3.Rows.Count;
                    if (count_row2 >= 1)
                    {
                        Count_rowNight_Complete2 = 1;
                    }
                    Count_row2_Complete = Count_rowShip_Complete2 + Count_rowNight_Complete2;
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter($"   Message: {0}, {error.Message}");
            }
        }
        private void SelectComplete3()
        {
            #region "Hiden Code"
            //try
            //{
            //    var dt = new DataTable();
            //    using (var conn = new SqlConnection(Local_Conn))
            //    {
            //        var check = conn.CreateCommand();
            //        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'C'";
            //        var sda = new SqlDataAdapter(check);
            //        sda.Fill(dt);
            //    }
            //    int Count_row = dt.Rows.Count;
            //    if (Count_row >= 1)
            //    {
            //        string Date = dt.Rows[0]["DateTime"].ToString();
            //        string Date_Today = DateTime.Today.ToString();
            //        if (Date == Date_Today)
            //        {
            //            Count_row3_Complete = 1;
            //        }
            //    }
            //}
            //catch (Exception err)
            //{
            //    _ = new LogWriter($" Error FormMixing Message: {0}, {err.Message}");
            //}
            //try
            //{
            //    string datecheck_H = DateTime.Now.ToString("hh");
            //    string datecheck_M = DateTime.Now.ToString("mm");
            //    if (Convert.ToInt32(datecheck_H) >= 7 && Convert.ToInt32(datecheck_H) <= 19)
            //    {

            //        string getdate = DateTime.Now.ToString("yyyy-MM-dd");
            //        string date1 = (getdate + "T07:30:00.000");
            //        string date1_1 = (getdate + "T19:29:00.000");

            //        var dt = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'C' and DateTime between '{date1}' and '{date1_1}'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt);
            //        }
            //        int count_row = dt.Rows.Count;
            //        if (count_row >= 1)
            //        {
            //            Count_row1 = 1;
            //        }
            //    }
            //    else
            //    {
            //        string getdate2 = DateTime.Now.ToString("yyyy-MM-dd");
            //        string date2 = (getdate2 + "T19:30:00.000");
            //        string date2_2 = (getdate2 + "T23:59:00.000");
            //        string date2_3 = (getdate2 + "T00:00:00.000");
            //        string date2_4 = (getdate2 + "T07:29:00.000");

            //        var dt2 = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'C' and DateTime between '{date2}' and '{date2_2}'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt2);
            //        }
            //        int count_row1 = dt2.Rows.Count;
            //        var dt3 = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'C' and DateTime between '{date2_3}' and '{date2_4}'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt3);
            //        }
            //        int count_row2 = dt3.Rows.Count;
            //        Count_row1 = count_row1 + count_row2;
            //    }
            //}
            //catch (Exception err)
            //{
            //    _ = new LogWriter($" Error FormMixing Message: {0}, {err.Message}");
            //}
            #endregion
            try
            {
                if (Status_Time == "Daytime")
                {
                    string getdate = DateTime.Now.ToString("yyyy-MM-dd");
                    string date1 = (getdate + "T07:30:00.000");
                    string date1_1 = (getdate + "T19:29:00.000");

                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'C' and DateTime between '{date1}' and '{date1_1}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    int count_row = dt.Rows.Count;
                    if (count_row >= 1)
                    {
                        Count_row3_Complete = 1;
                    }
                }
                else if (Status_Time == "Nighttime")
                {
                    string getdate2 = DateTime.Now.ToString("yyyy-MM-dd");
                    string date2 = (getdate2 + "T19:30:00.000");
                    string date2_2 = (getdate2 + "T23:59:00.000");
                    string date2_3 = (getdate2 + "T00:00:00.000");
                    string date2_4 = (getdate2 + "T07:29:00.000");
                    var dt2 = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'C' and DateTime between '{date2}' and '{date2_2}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt2);
                    }
                    int count_row1 = dt2.Rows.Count;
                    if (count_row1 >= 1)
                    {
                        Count_rowShip_Complete3 = 1;
                    }
                    var dt3 = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Status = 'Complete' and Location_waiting = 'C' and DateTime between '{date2_3}' and '{date2_4}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt3);
                    }
                    int count_row2 = dt3.Rows.Count;
                    if (count_row2 >= 1)
                    {
                        Count_rowNight_Complete3 = 1;
                    }
                    Count_row3_Complete = Count_rowShip_Complete3 + Count_rowNight_Complete3;
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter($"   Message: {0}, {error.Message}");
            }
        }

        private void ResultComplete()
        {
            result_Complete = Count_row1_Complete + Count_row2_Complete + Count_row3_Complete;
            if (Status_Time == "Daytime")
            {
                //labelCompleteTasks.Text = result_Complete.ToString();
                labelCompleteTasks.Text = Convert.ToString(result_Complete);

            }
            else if (Status_Time == "Nighttime")
            {
                //label9.Text = result_Complete.ToString();
                label9.Text = Convert.ToString(result_Complete);
            }
        }
        #endregion

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

        #region "Close Serial Port"
        public void CloseSerialPort()
        {
            try
            {
                if (mySerialPort.IsOpen == true)
                {
                    mySerialPort.Close();
                }
            }
            catch
            {

            }
        }
        #endregion

        #region "Transaction"
        private static bool ExecuteSqlTransaction(string cmd, string connectionString, string Transaction)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction(Transaction);
                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    command.CommandText = cmd;
                    status_sql = Convert.ToBoolean(command.ExecuteNonQuery());
                    transaction.Commit();
                    _ = new LogWriter("Data record are written to database.");
                    return true;
                }
                catch (Exception ex)
                {
                    _ = new LogWriter($"Commit Exeption Type: {0}, {ex.GetType()}");
                    _ = new LogWriter($"   Message: {0}, {ex.Message}");
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        _ = new LogWriter($"Rollback Exception Type: {0}, {ex2.GetType()}");
                        _ = new LogWriter($"  Message: {0}, {ex2.Message}");
                    }
                    return false;
                }
            }
        }
        #endregion

        #region "InitializeDataGridView"
        private void InitializeDataGridView()
        {
            metroGrid1.BorderStyle = BorderStyle.Fixed3D;
            metroGrid1.AllowUserToAddRows = false;
            metroGrid1.AllowUserToDeleteRows = false;
            metroGrid1.AllowUserToOrderColumns = true;
            metroGrid1.ReadOnly = true;
            metroGrid1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            metroGrid1.MultiSelect = false;
            metroGrid1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            metroGrid1.AllowUserToResizeColumns = false;
            metroGrid1.AllowUserToResizeRows = false;
            metroGrid1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            metroGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        #endregion

        #region "Function Create table"
        private void Create_table()
        {
            metroGrid1.Columns.Clear();
            metroGrid1.Rows.Clear();
            metroGrid1.ColumnCount = 4;

            metroGrid1.Columns[0].HeaderText = "Job Number";
            metroGrid1.Columns[1].HeaderText = "Item Code";
            metroGrid1.Columns[2].HeaderText = "Formula";
            metroGrid1.Columns[3].HeaderText = "Location";
        }
        #endregion

        #region "Show DataGrid"
        private void ShowGrid()
        {
            
            try
            {
                Create_table();
                metroGrid1.Rows.Clear();
                var dt = new DataTable();
                using (var conn = new SqlConnection(Local_Conn))
                {
                    var cmd = conn.CreateCommand();
                    //cmd.CommandText = "Select Job_No, Item_code, Formula, Location_waiting from Chemical_Record_Local Group by Job_No, Item_code, Formula, Location_waiting";
                    cmd.CommandText = "Select * from Chemical_Record_Local Where Status = 'Pending'";
                    var sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    metroGrid1.Rows.Add(dr["Job_No"], dr["Item_code"], dr["Formula"], dr["Location_waiting"]);
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormMixing Message: {0}, {err.Message}");
            }
            
        }
        #endregion

        #region "Button Back"
        private void buttonBackMain_Click_1(object sender, EventArgs e)
        {
            CloseSerialPort();
            ModbusClient.Disconnect();
            this.Close();
        }

        #endregion

        #region "Button Manual"
        private void ButtonManual_Click(object sender, EventArgs e)
        {
            textBoxJobNumber.Enabled = true;
            textBoxItemcode.Enabled = true;
            textBoxFormula.Enabled = true;
            textBoxMachine.Enabled = true;
        }
        #endregion

        #region "Button Keyboard"
        private void ButtonKeyboard_Click(object sender, EventArgs e)
        {
            string processName = System.IO.Path.GetFileNameWithoutExtension(OnScreenKeyboadApplication);
            var query = from process in Process.GetProcesses()
                        where process.ProcessName == processName
                        select process;
            var keyboardProcess = query.FirstOrDefault();
            if (keyboardProcess == null)
            {
                IntPtr ptr = new IntPtr(); ;
                bool sucessfullyDisabledWow64Redirect = false;

                if (System.Environment.Is64BitOperatingSystem)
                {
                    sucessfullyDisabledWow64Redirect = Wow64DisableWow64FsRedirection(ref ptr);
                }
                using (Process osk = new Process())
                {
                    osk.StartInfo.FileName = OnScreenKeyboadApplication;
                    osk.Start();
                    //osk.WaitForInputIdle(2000);
                }
                if (System.Environment.Is64BitOperatingSystem)
                    if (sucessfullyDisabledWow64Redirect)
                        Wow64RevertWow64FsRedirection(ptr);
            }
            else
            {
                var windowHandle = keyboardProcess.MainWindowHandle;
                SendMessage(windowHandle, WM_SYSCOMMAND, new IntPtr(SC_RESTORE), new IntPtr(0));
            }
        }
        #endregion

        #region "Button Confirm"
        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                const string message = " คุณแน่ใจหรือไม่ ว่าต้องการอัปเดตข้อมูลนี้ไปยังฐานข้อมูล? ";
                const string caption = "Update Data Model to Database";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    if (textBoxJobNumber.Text == "" || textBoxJobNumber == null || textBoxFormula.Text == "" || textBoxFormula.Text == null || textBoxItemcode.Text == "" || textBoxItemcode.Text == null || textBoxMachine.Text == "" || textBoxMachine.Text == null)
                    {
                        MessageBox.Show(" กรุณากรอกข้อมูลก่อนดำเนินการ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        string Location_Check = labelLocation_waiting.Text;
                        //timerCheckSensor.Enabled = true;
                        Function_CheckLocation_PLC();
                        if (Location_PLC_A == 0 && Location_PLC_B == 0 && Location_PLC_C == 0)
                        {
                            MessageBox.Show("ตำแหน่งจุด Stock ถังบรรจุสารเคมีเต็ม กรุณาดำเนินการผสมสารเคมีก่อน");
                        }
                        else
                        {
                            // Location A
                            try
                            {
                                if (Location_Check == "A" && Location_PLC_A == 1)
                                {
                                    if (Machine_Mixing == "Positive")
                                    {
                                        Thread.Sleep(mydelay);
                                        ModbusClient.WriteSingleRegister(30, 1); // Positive
                                        Thread.Sleep(mydelay);
                                        ModbusClient.WriteSingleRegister(37, 1); // Location
                                        textBoxAlarm.Text = "Pending Mixing";
                                        timerCheckStatus.Enabled = true;
                                    }
                                    else if (Machine_Mixing == "Negative")
                                    {
                                        Thread.Sleep(mydelay);
                                        ModbusClient.WriteSingleRegister(31, 1); // Negative
                                        Thread.Sleep(mydelay);
                                        ModbusClient.WriteSingleRegister(37, 1); // Location
                                        textBoxAlarm.Text = "Pending Mixing";
                                        timerCheckStatus.Enabled = true;
                                    }
                                }
                                else if (Location_Check == "B" && Location_PLC_B == 1)
                                {
                                    if (Machine_Mixing == "Positive")
                                    {
                                        Thread.Sleep(mydelay);
                                        ModbusClient.WriteSingleRegister(30, 1); // Positive
                                        Thread.Sleep(mydelay);
                                        ModbusClient.WriteSingleRegister(38, 1); // Location
                                        textBoxAlarm.Text = "Pending Mixing";
                                        timerCheckStatus.Enabled = true;
                                    }
                                    else if (Machine_Mixing == "Negative")
                                    {
                                        Thread.Sleep(mydelay);
                                        ModbusClient.WriteSingleRegister(31, 1); // Negative
                                        Thread.Sleep(mydelay);
                                        ModbusClient.WriteSingleRegister(38, 1); // Location
                                        textBoxAlarm.Text = "Pending Mixing";
                                        timerCheckStatus.Enabled = true;
                                    }
                                }
                                else if (Location_Check == "C" && Location_PLC_C == 1)
                                {
                                    if (Machine_Mixing == "Positive")
                                    {
                                        Thread.Sleep(mydelay);
                                        ModbusClient.WriteSingleRegister(30, 1); // Positive
                                        Thread.Sleep(mydelay);
                                        ModbusClient.WriteSingleRegister(39, 1); // Location
                                        textBoxAlarm.Text = "Pending Mixing";
                                        timerCheckStatus.Enabled = true;
                                    }
                                    else if (Machine_Mixing == "Negative")
                                    {
                                        Thread.Sleep(mydelay);
                                        ModbusClient.WriteSingleRegister(31, 1); // Negative
                                        Thread.Sleep(mydelay);
                                        ModbusClient.WriteSingleRegister(39, 1); // Location
                                        textBoxAlarm.Text = "Pending Mixing";
                                        timerCheckStatus.Enabled = true;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("ตรวจพบว่าถังบรรจุสารเคมีไม่ได้วางตามจุดที่กำหนด กรุณาตรวจสอบอีกครั้ง");
                                }
                            }
                            catch (Exception error)
                            {
                                _ = new LogWriter($"Mixing Location error   Message: {0}, {error.Message}");
                                textBoxAlarm.Text = ($"   Message: {error.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter($"   Message: {0}, {error.Message}");
                textBoxAlarm.Text = ($"   Message: {error.Message}");
            }
            
        }
        #endregion

        #region "Button Reset"
        private void ButtonReset_Click(object sender, EventArgs e)
        {
            try
            {
                const string message = "คุณแน่ใจหรือไม่ ว่าต้องการตั้งค่าเป็นเริ่มต้น?";
                const string caption = "Reset Job for Mixing";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    

                    groupBox1.Hide();
                    groupBox2.Hide();
                    groupBox5.Hide();
                    groupBox6.Hide();
                    Select_Time();
                    SelectLocation1();
                    SelectLocation2();
                    SelectLocation3();
                    Result();
                    SelectComplete1();
                    SelectComplete2();
                    SelectComplete3();
                    ResultComplete();
                    textBoxJobNumber.Text = "";
                    textBoxItemcode.Text = "";
                    textBoxFormula.Text = "";
                    textBoxJobNumber.Enabled = false;
                    textBoxItemcode.Enabled = false;
                    textBoxFormula.Enabled = false;
                    textBoxMachine.Enabled = false;
                    textBoxMachine.Text = "";
                    ButtonMNegative.BackColor = Color.WhiteSmoke;
                    ButtonMNegative.ForeColor = Color.Black;
                    ButtonMPositive.BackColor = Color.WhiteSmoke;
                    ButtonMPositive.ForeColor = Color.Black;
                    Machine_Mixing = "";
                    Thread.Sleep(mydelay);
                    ModbusClient.WriteSingleRegister(40, 1); // Status = Reset
                    Create_table();
                    ShowGrid();
                    Properties.Settings.Default.MonitorChemical = "";
                    Properties.Settings.Default.MonitorFormula = "";
                    Properties.Settings.Default.MonitorStatus = "";
                    Properties.Settings.Default.MonitorSTD = 0;
                    Properties.Settings.Default.MonitorWeight = 0;
                    Properties.Settings.Default.Save();
                }

            }
            catch (Exception error)
            {
                _ = new LogWriter($"   Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "SerialPort Show Data"
        private void ShowData(object sender, EventArgs e)
        {
            string qrTextCode = Serial_DataIn;
            if (qrTextCode.StartsWith("M"))
            {
                if (qrTextCode == "MachinePositive")
                {
                    ButtonMPositive.BackColor = Color.MidnightBlue;
                    ButtonMPositive.ForeColor = Color.WhiteSmoke;
                    ButtonMNegative.BackColor = Color.WhiteSmoke;
                    ButtonMNegative.ForeColor = Color.Black;
                    Machine_Mixing = "Positive";
                    textBoxMachine.Text = "Positive";
                    ButtonConfirm.Show();
                }
                if (qrTextCode == "MachineNegative")
                {
                    ButtonMNegative.BackColor = Color.MidnightBlue;
                    ButtonMNegative.ForeColor = Color.WhiteSmoke;
                    ButtonMPositive.BackColor = Color.WhiteSmoke;
                    ButtonMPositive.ForeColor = Color.Black;
                    Machine_Mixing = "Negative";
                    textBoxMachine.Text = "Negative";
                    ButtonConfirm.Show();
                }
            }
            else
            {
                string[] input = Serial_DataIn.Split('*');
                //TextIn = input[0];
                textBoxJobNumber.Text = input[0];
                
                //textBoxItemcode.Text = input[1];
            }
        }
        #endregion

        #region "SerialPort Data Recived"
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                Serial_DataIn = sp.ReadExisting();
                this.Invoke(new EventHandler(ShowData));
                Serial_DataIn = "";
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region "Get Serial Port"
        public void GetSerialPort()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                //SerialPort mySerialPort = new SerialPort(ports[0]);
                mySerialPort = new SerialPort(ports[1]);
                mySerialPort.BaudRate = 9600;
                mySerialPort.Parity = Parity.None;
                mySerialPort.StopBits = StopBits.One;
                mySerialPort.DataBits = 8;
                mySerialPort.Handshake = Handshake.None;

                mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                mySerialPort.Open();
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Message: {0}, {err.Message}");
            }
        }
        #endregion

        #region "Text Changed of Job Number"
        private void textBoxJobNumber_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Create_table();
                //metroGrid1.Rows.Clear();
                //ShowGrid();
                textBoxFormula.Text = "";
                textBoxItemcode.Text = "";
                textBoxMachine.Text = "";
                textBoxMachine.Text = "";
                ButtonConfirm.Hide();
                if (textBoxJobNumber.TextLength >= 1)
                {

                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Job_No = '{textBoxJobNumber.Text}' And Status = 'Pending'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    int count_row = dt.Rows.Count;
                    if (count_row == 0)
                    {
                        ButtonConfirm.Hide();
                        textBoxFormula.Text = "";
                        textBoxItemcode.Text = "";
                        textBoxMachine.Text = "";
                    }
                    else
                    {
                        textBoxItemcode.Text = dt.Rows[0]["Item_code"].ToString();
                        textBoxFormula.Text = dt.Rows[0]["Formula"].ToString();
                        labelLocation_waiting.Text = dt.Rows[0]["Location_waiting"].ToString();
                        foreach (DataRow dr in dt.Rows)
                        {
                            metroGrid1.Rows.Add(dr["Job_No"], dr["Item_code"], dr["Formula"], dr["Location_waiting"]);
                        }
                        for (int i = 1; i < count_row;)
                        {
                            metroGrid1.Rows[i].Visible = false;
                            i++;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter($"   Message: {0}, {error.Message}");
                textBoxAlarm.Text = ($"   Message: {error.Message}");
            }
        }
        #endregion

        #region "Button Machine Mixing"
        private void ButtonMPositive_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxMachine.Text = "Positive";
                Machine_Mixing = "Positive";
                ButtonMPositive.BackColor = Color.MidnightBlue;
                ButtonMPositive.ForeColor = Color.WhiteSmoke;
                ButtonMNegative.BackColor = Color.WhiteSmoke;
                ButtonMNegative.ForeColor = Color.Black;
                ButtonConfirm.Show();
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormMixing Message: {0}, {err.Message}");
            }
        }

        private void ButtonMNegative_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxMachine.Text = "Negative";
                Machine_Mixing = "Negative";
                ButtonMNegative.BackColor = Color.MidnightBlue;
                ButtonMNegative.ForeColor = Color.WhiteSmoke;
                ButtonMPositive.BackColor = Color.WhiteSmoke;
                ButtonMPositive.ForeColor = Color.Black;
                ButtonConfirm.Show();
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormMixing Message: {0}, {err.Message}");
            }
        }
        #endregion

        #region "Function Clear Text"
        private void Cleartext()
        {
            try
            {
                ////Select Pending
                Count_row1 = 0;
                Count_row2 = 0;
                Count_row3 = 0;
                Count_rowShip1 = 0;
                Count_rowNight1 = 0;
                Count_rowShip2 = 0;
                Count_rowNight2 = 0;
                Count_rowShip3 = 0;
                Count_rowNight3 = 0;
                result = 0;

                ////Select Complete
                Count_row1_Complete = 0;
                Count_row2_Complete = 0;
                Count_row3_Complete = 0;
                Count_rowShip_Complete1 = 0;
                Count_rowNight_Complete1 = 0;
                Count_rowShip_Complete2 = 0;
                Count_rowNight_Complete2 = 0;
                Count_rowShip_Complete3 = 0;
                Count_rowNight_Complete3 = 0;
                result_Complete = 0;
                SelectLocation1();
                SelectLocation2();
                SelectLocation3();
                Result();
                SelectComplete1();
                SelectComplete2();
                SelectComplete3();
                ResultComplete();
                textBoxJobNumber.Text = "";
                textBoxItemcode.Text = "";
                textBoxFormula.Text = "";
                textBoxMachine.Text = "";
                labelLocation_waiting.Text = "-";
                ButtonMNegative.BackColor = Color.WhiteSmoke;
                ButtonMNegative.ForeColor = Color.Black;
                ButtonMPositive.BackColor = Color.WhiteSmoke;
                ButtonMPositive.ForeColor = Color.Black;
                Machine_Mixing = "";
                metroGrid1.Rows.Clear();
                Create_table();
                ShowGrid();
                
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormMixing Message: {0}, {err.Message}");
            }
        }
        #endregion

        #region "Function Modbus Client 502"
        private void Modbus_Client_Port502()
        {
            try
            {
                //ModbusClient.Disconnect();
                ModbusClient.IPAddress = "192.168.3.250";
                ModbusClient.Port = 502;
                ModbusClient.Connect();
            }
            catch (Exception err)
            {
                //MessageBox.Show(err.Message, "Modbus Client Error : ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _ = new LogWriter($"Modbus Client Error : {0}, {err.Message}");
            }
        }
        #endregion

        #region "Timer Checkstatus and location and save"
        private void timerCheckStatus_Tick(object sender, EventArgs e)
        {
            try
            {
                timerCheckStatus.Enabled = false;
                int[] D = ModbusClient.ReadHoldingRegisters(0, 100);
                //Thread.Sleep(mydelay);
                int ReadM_Positive = (D[32]); // อ่านค่า Status
                //Thread.Sleep(mydelay);
                int ReadM_Negative = (D[33]); // อ่านค่า Status
                if (ReadM_Positive == 1)
                {
                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Job_No = '{textBoxJobNumber.Text}' And Status = 'Pending'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    int count_row = dt.Rows.Count;
                    string Location_check = dt.Rows[0]["Location_waiting"].ToString();
                    if (count_row == 0)
                    {
                        MessageBox.Show("ไม่มีงานในระบบ กรุณาตรวจสอบใหม่อีกครั้ง");

                    }
                    else
                    {
                        if (Location_check == "A")
                        {
                            var cmd = $"Update Chemical_Record_Local " +
                                $"Set Status = 'Complete', Location_machine = 'Positive', " +
                                $"DateTime = Getdate(), Status_Update = 'false' " +
                                $"Where Job_No = '{textBoxJobNumber.Text}'";
                            if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                            {
                                
                            }
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(30, 0); // Positive
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(32, 0); // Status Positive
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(22, 0); // Location Waiting Weight
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(34, 0); // Location Waiting Mixing
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(37, 0);
                            Thread.Sleep(mydelay);
                            
                            //Create_table();
                            //ShowGrid();
                            Cleartext();
                            //Select_Time();
                            //SelectLocation1();
                            //SelectLocation2();
                            //SelectLocation3();
                            //Result();
                            //SelectComplete1();
                            //SelectComplete2();
                            //SelectComplete3();
                            //ResultComplete();
                            //timerCheckStatus.Enabled = false;
                            textBoxAlarm.Text = " บันทึกข้อมูลสำเร็จ ";
                            MessageBox.Show("บันทึกข้อมูลสำเร็จ");
                        }
                        else if (Location_check == "B")
                        {
                            var cmd = $"Update Chemical_Record_Local " +
                                $"Set Status = 'Complete', Location_machine = 'Positive', " +
                                $"DateTime = Getdate(), Status_Update = 'false' " +
                                $"Where Job_No = '{textBoxJobNumber.Text}'";
                            if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                            {
                                
                            }
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(30, 0); // Positive
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(32, 0); // Status Positive
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(23, 0); // Location Waiting Weight
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(35, 0); // Location Waiting Mixing
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(38, 0);
                            
                            //Create_table();
                            //ShowGrid();
                            Cleartext();
                            //Select_Time();
                            //SelectLocation1();
                            //SelectLocation2();
                            //SelectLocation3();
                            //Result();
                            //SelectComplete1();
                            //SelectComplete2();
                            //SelectComplete3();
                            //ResultComplete();
                            //timerCheckStatus.Enabled = false;
                            textBoxAlarm.Text = " บันทึกข้อมูลสำเร็จ ";
                            MessageBox.Show("บันทึกข้อมูลสำเร็จ");
                        }
                        else if (Location_check == "C")
                        {
                            var cmd = $"Update Chemical_Record_Local " +
                                $"Set Status = 'Complete', Location_machine = 'Positive', " +
                                $"DateTime = Getdate(), Status_Update = 'false' " +
                                $"Where Job_No = '{textBoxJobNumber.Text}'";
                            if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                            {
                                
                            }
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(30, 0); // Positive
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(32, 0); // Status Positive
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(24, 0); // Location Waiting Weight
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(36, 0); // Location Waiting Mixing
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(39, 0);
                            
                            //Create_table();
                            //ShowGrid();
                            Cleartext();
                            //Select_Time();
                            //SelectLocation1();
                            //SelectLocation2();
                            //SelectLocation3();
                            //Result();
                            //SelectComplete1();
                            //SelectComplete2();
                            //SelectComplete3();
                            //ResultComplete();
                            //timerCheckStatus.Enabled = false;
                            textBoxAlarm.Text = " บันทึกข้อมูลสำเร็จ ";
                            MessageBox.Show("บันทึกข้อมูลสำเร็จ");
                        }
                    }
                }
                else if (ReadM_Negative == 1)
                {
                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Record_Local Where Job_No = '{textBoxJobNumber.Text}' And Status = 'Pending'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    int count_row = dt.Rows.Count;
                    string Location_check = dt.Rows[0]["Location_waiting"].ToString();
                    if (count_row == 0)
                    {
                        MessageBox.Show("ไม่มีงานในระบบ กรุณาตรวจสอบใหม่อีกครั้ง");
                    }
                    else
                    {
                        if (Location_check == "A")
                        {
                            var cmd = $"Update Chemical_Record_Local " +
                                $"Set Status = 'Complete', Location_machine = 'Negative', " +
                                $"DateTime = Getdate(), Status_Update = 'false' " +
                                $"Where Job_No = '{textBoxJobNumber.Text}'";
                            if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                            {
                                
                            }
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(31, 0); // Negative
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(33, 0); // Status Negative
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(22, 0); // Location Waiting Weight
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(34, 0); // Location Waiting Mixing
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(37, 0);
                            
                            //Create_table();
                            //ShowGrid();
                            Cleartext();
                            //Select_Time();
                            //SelectLocation1();
                            //SelectLocation2();
                            //SelectLocation3();
                            //Result();
                            //SelectComplete1();
                            //SelectComplete2();
                            //SelectComplete3();
                            //ResultComplete();
                            //timerCheckStatus.Enabled = false;

                            textBoxAlarm.Text = " บันทึกข้อมูลสำเร็จ ";
                            MessageBox.Show("บันทึกข้อมูลสำเร็จ");
                        }
                        else if (Location_check == "B")
                        {
                            var cmd = $"Update Chemical_Record_Local " +
                                $"Set Status = 'Complete', Location_machine = 'Negative', " +
                                $"DateTime = Getdate(), Status_Update = 'false' " +
                                $"Where Job_No = '{textBoxJobNumber.Text}'";
                            if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                            {
                                
                            }
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(31, 0); // Negative
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(33, 0); // Status Negative
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(23, 0); // Location Waiting Weight
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(35, 0); // Location Waiting Mixing
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(38, 0);

                            //Create_table();
                            //ShowGrid();
                            Cleartext();
                            //Select_Time();
                            //SelectLocation1();
                            //SelectLocation2();
                            //SelectLocation3();
                            //Result();
                            //SelectComplete1();
                            //SelectComplete2();
                            //SelectComplete3();
                            //ResultComplete();
                            //timerCheckStatus.Enabled = false;

                            textBoxAlarm.Text = " บันทึกข้อมูลสำเร็จ ";
                            MessageBox.Show("บันทึกข้อมูลสำเร็จ");
                        }
                        else if (Location_check == "C")
                        {
                            var cmd = $"Update Chemical_Record_Local " +
                                $"Set Status = 'Complete', Location_machine = 'Negative', " +
                                $"DateTime = Getdate(), Status_Update = 'false' " +
                                $"Where Job_No = '{textBoxJobNumber.Text}'";
                            if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                            {
                                
                            }
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(31, 0); // Negative
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(33, 0); // Status Negative
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(24, 0); // Location Waiting Weight
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(36, 0); // Location Waiting Mixing
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(39, 0);
                            
                            //Create_table();
                            //ShowGrid();
                            Cleartext();
                            //Select_Time();
                            //SelectLocation1();
                            //SelectLocation2();
                            //SelectLocation3();
                            //Result();
                            //SelectComplete1();
                            //SelectComplete2();
                            //SelectComplete3();
                            //ResultComplete();
                            //timerCheckStatus.Enabled = false;

                            textBoxAlarm.Text = " บันทึกข้อมูลสำเร็จ ";
                            MessageBox.Show("บันทึกข้อมูลสำเร็จ");
                        }
                    }
                }
                timerCheckStatus.Enabled = true;
            }
            catch (Exception error)
            {
                _ = new LogWriter($"   Message: {0}, {error.Message}");
            }
        }
        #endregion

        private void Function_CheckLocation_PLC()
        {
            try
            {
                int[] D = ModbusClient.ReadHoldingRegisters(0, 100);
                Location_PLC_A = Convert.ToInt32(D[34]);
                Thread.Sleep(mydelay);
                Location_PLC_B = Convert.ToInt32(D[35]);
                Thread.Sleep(mydelay);
                Location_PLC_C = Convert.ToInt32(D[36]);
            }
            catch
            {

            }
        }

        private void timerCheckSensor_Tick(object sender, EventArgs e)
        {
            try
            {
                timerCheckSensor.Enabled = false;
                int[] D = ModbusClient.ReadHoldingRegisters(0, 100);
                Location_PLC_A = Convert.ToInt32(D[34]);
                Thread.Sleep(mydelay);
                Location_PLC_B = Convert.ToInt32(D[35]);
                Thread.Sleep(mydelay);
                Location_PLC_C = Convert.ToInt32(D[36]);
                timerCheckSensor.Enabled = true;
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormMixing Message: {0}, {err.Message}");
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {
            string text = label13.Text;
            if (text.StartsWith("S"))
            {
                label13.Text = "สแกนเลขงาน และกดปุ่มเพื่อยืนยันการบันทึกข้อมูล";
            }
            else
            {
                label13.Text = "Scan JOB Number and pull button confirm for record data.";
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            string text = label5.Text;
            if (text.StartsWith("T"))
            {
                label5.Text = "งาน";
            }
            else
            {
                label5.Text = "Tasks";
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {
            string text = label12.Text;
            if (text.StartsWith("I"))
            {
                label12.Text = "ข้อมูลของงาน";
            }
            else
            {
                label12.Text = "Information";
            }
        }


    }
}
