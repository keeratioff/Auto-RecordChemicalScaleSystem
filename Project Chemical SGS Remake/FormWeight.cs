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
//using System.IO;
using System.IO.Ports;
using EasyModbus; 


namespace Project_Chemical_SGS_Remake
{
    public partial class FormWeight : Form
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
        public static ModbusClient ModbusClient = new ModbusClient();
        //status of PLC weight เมื่อเสร็จแล้วจะส่ง 1
        public static bool Status_PLC;

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

        //Login
        public int ID_Emp_Main = Properties.Settings.Default.ID_Emplyee;
        public string Name_Emp_Main = Properties.Settings.Default.Name_Emplyee.ToString();
        public string Lastname_Emp_Main = Properties.Settings.Default.Lastname_Emplyee.ToString();
        //Monitor
        
        public string MonitorChemical = Properties.Settings.Default.MonitorChemical.ToString();
        public double MonitorWeight = Properties.Settings.Default.MonitorWeight;
        public double MonitorWeightSTD = Properties.Settings.Default.MonitorSTD;
        public string MonitorFormula = Properties.Settings.Default.MonitorFormula.ToString();
        public string MonitorStatus = Properties.Settings.Default.MonitorStatus.ToString();
        public string MonitorSetpoint = Properties.Settings.Default.MonitorSetPoint.ToString();


        //Get Job#
        public static string Job_Number;

        //Check Location Locel 1 = Full, 0 = empty
        //public int Location_Local_A;
        //public int Location_Local_B;
        //public int Location_Local_C;


        //Check Location PLC 1 = Full, 0 = empty
        public int Location_PLC_A;
        public int Location_PLC_B;
        public int Location_PLC_C;

        DataTable tableWeight = new DataTable("table");


        //Get Data of Weight row
        public int Row_weight = 0;
        public static string NameChemical;
        public double WeightActual;
        public double WeightSetPoint;
        public double Weightscale = 0;
        public double Weight_scale;

        //SerialPort
        public static SerialPort mySerialPort;

        //Radio Select Formula
        public static int Radio_Formula;

        //Delay for Write PLC
        public int mydelay = 100;

        public static Form FormMonitor = new FormMonitor();

        public FormWeight()
        {
            InitializeComponent();
        }

        private void FormWeight_Load(object sender, EventArgs e)
        {
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file.txt");
            Local_Conn = $"Data Source={Ip_Addr_Local};Initial Catalog={Catalog_Local};User ID={Sql_usr_Local};password={Sql_pw_Local}";
            Modbus_Client_Port502();
            GetSerialPort();
            Properties.Settings.Default.MonitorStatus = "0";
            Properties.Settings.Default.Save();
            FunctionMonitor();
            Function_CheckLocation_PLC();
            //Function_Tanks_off();
            Create_table();
            Radio_Formula = 0;
            ButtonNextweighing.Hide();
            ButtonConfirm.Hide();
            textBoxJOBNumber.Enabled = false;
            textBoxItemcode.Enabled = false;
            textBoxFormula.Enabled = false;
            textBoxLeadPowder.Enabled = false;

            //timerOpentanks.Enabled = false;
            timerWeightScale.Enabled = false;
            ModbusClient.WriteSingleRegister(21, 0); // Status = false
        }

        private void FormWeight_FormClosing(object sender, FormClosingEventArgs e)
        {
            //mySerialPort.Close();
        }



        #region "Button Back"
        private void buttonBackMain_Click(object sender, EventArgs e)
        {
            Function_Tanks_off();
            CloseSerialPort();
            ModbusClient.Disconnect();
            this.Close();
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

        #region "Button Keyborad"
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

        #region "Button Lead Powder"
        private void rjButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Radio_Formula = 1;
                textBoxLeadPowder.Text = "300";
                rjButton1.BackColor = Color.MidnightBlue;
                rjButton1.ForeColor = Color.WhiteSmoke;
                rjButton2.BackColor = Color.WhiteSmoke;
                rjButton2.ForeColor = Color.Black;
                rjButton3.BackColor = Color.WhiteSmoke;
                rjButton3.ForeColor = Color.Black;
                metroGrid1.Rows.Clear();
                var dt_Code = new DataTable();
                using (var conn = new SqlConnection(Local_Conn))
                {
                    var check = conn.CreateCommand();
                    check.CommandText = $"Select * from Chemical_CodeFormula_Local where Code = '{textBoxFormula.Text}' and Formula = '{Radio_Formula}' and Working = '1'";
                    var sda = new SqlDataAdapter(check);
                    sda.Fill(dt_Code);
                }
                foreach (DataRow dr in dt_Code.Rows)
                {
                    metroGrid1.Rows.Add(dr["Chemical"], dr["Weight"]);
                    ButtonNextweighing.Text = "Weighing";
                    ButtonNextweighing.Show();
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormWeight Message: {0}, {err.Message}");
            }
        }

        private void rjButton2_Click(object sender, EventArgs e)
        {
            try
            {
                Radio_Formula = 2;
                textBoxLeadPowder.Text = "600";
                rjButton1.BackColor = Color.WhiteSmoke;
                rjButton1.ForeColor = Color.Black;
                rjButton2.BackColor = Color.MidnightBlue;
                rjButton2.ForeColor = Color.WhiteSmoke;
                rjButton3.BackColor = Color.WhiteSmoke;
                rjButton3.ForeColor = Color.Black;
                metroGrid1.Rows.Clear();
                var dt_Code = new DataTable();
                using (var conn = new SqlConnection(Local_Conn))
                {
                    var check = conn.CreateCommand();
                    check.CommandText = $"Select * from Chemical_CodeFormula_Local where Code = '{textBoxFormula.Text}' and Formula = '{Radio_Formula}' and Working = '1'";
                    var sda = new SqlDataAdapter(check);
                    sda.Fill(dt_Code);
                }
                foreach (DataRow dr in dt_Code.Rows)
                {
                    metroGrid1.Rows.Add(dr["Chemical"], dr["Weight"]);
                    ButtonNextweighing.Text = "Weighing";
                    ButtonNextweighing.Show();
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormWeight Message: {0}, {err.Message}");
            }
        }

        private void rjButton3_Click(object sender, EventArgs e)
        {
            try
            {
                Radio_Formula = 3;
                textBoxLeadPowder.Text = "900";
                rjButton1.BackColor = Color.WhiteSmoke;
                rjButton1.ForeColor = Color.Black;
                rjButton2.BackColor = Color.WhiteSmoke;
                rjButton2.ForeColor = Color.Black;
                rjButton3.BackColor = Color.MidnightBlue;
                rjButton3.ForeColor = Color.WhiteSmoke;
                metroGrid1.Rows.Clear();
                var dt_Code = new DataTable();
                using (var conn = new SqlConnection(Local_Conn))
                {
                    var check = conn.CreateCommand();
                    check.CommandText = $"Select * from Chemical_CodeFormula_Local where Code = '{textBoxFormula.Text}' and Formula = '{Radio_Formula}' and Working = '1'";
                    var sda = new SqlDataAdapter(check);
                    sda.Fill(dt_Code);
                }
                foreach (DataRow dr in dt_Code.Rows)
                {
                    metroGrid1.Rows.Add(dr["Chemical"], dr["Weight"]);
                    ButtonNextweighing.Text = "Weighing";
                    ButtonNextweighing.Show();
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormWeight Message: {0}, {err.Message}");
            }
        }
        #endregion

        #region "Button Manual"
        private void ButtonManual_Click(object sender, EventArgs e)
        {
            textBoxJOBNumber.Enabled = true;
            textBoxItemcode.Enabled = true;
            textBoxFormula.Enabled = true;
            textBoxLeadPowder.Enabled = true;
        }
        #endregion

        #region "Radio Select Formula"
        private void radioButtonFormul1_CheckedChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    Radio_Formula = 1;
            //    textBoxLeadPowder.Text = "300";
            //    metroGrid1.Rows.Clear();
            //    var dt_Code = new DataTable();
            //    using (var conn = new SqlConnection(Local_Conn))
            //    {
            //        var check = conn.CreateCommand();
            //        check.CommandText = $"Select * from Chemical_CodeFormula_Local where Code = '{textBoxFormula.Text}' and Formula = '{Radio_Formula}' and Working = '1'";
            //        var sda = new SqlDataAdapter(check);
            //        sda.Fill(dt_Code);
            //    }
            //    foreach (DataRow dr in dt_Code.Rows)
            //    {
            //        metroGrid1.Rows.Add(dr["Chemical"], dr["Weight"]);
            //        ButtonNextweighing.Text = "Weighing";
            //        ButtonNextweighing.Show();
            //    }
            //}
            //catch (Exception err)
            //{
            //    _ = new LogWriter($" Error FormWeight Message: {0}, {err.Message}");
            //}
        }

        private void radioButtonFormul2_CheckedChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    Radio_Formula = 2;
            //    textBoxLeadPowder.Text = "600";
            //    metroGrid1.Rows.Clear();
            //    var dt_Code = new DataTable();
            //    using (var conn = new SqlConnection(Local_Conn))
            //    {
            //        var check = conn.CreateCommand();
            //        check.CommandText = $"Select * from Chemical_CodeFormula_Local where Code = '{textBoxFormula.Text}' and Formula = '{Radio_Formula}' and Working = '1'";
            //        var sda = new SqlDataAdapter(check);
            //        sda.Fill(dt_Code);
            //    }
            //    foreach (DataRow dr in dt_Code.Rows)
            //    {
            //        metroGrid1.Rows.Add(dr["Chemical"], dr["Weight"]);
            //        ButtonNextweighing.Text = "Weighing";
            //        ButtonNextweighing.Show();
            //    }
            //}
            //catch (Exception err)
            //{
            //    _ = new LogWriter($" Error FormWeight Message: {0}, {err.Message}");
            //}
            
        }

        private void radioButtonFormul3_CheckedChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    Radio_Formula = 3;
            //    textBoxLeadPowder.Text = "900";
            //    metroGrid1.Rows.Clear();
            //    var dt_Code = new DataTable();
            //    using (var conn = new SqlConnection(Local_Conn))
            //    {
            //        var check = conn.CreateCommand();
            //        check.CommandText = $"Select * from Chemical_CodeFormula_Local where Code = '{textBoxFormula.Text}' and Formula = '{Radio_Formula}' and Working = '1'";
            //        var sda = new SqlDataAdapter(check);
            //        sda.Fill(dt_Code);
            //    }
            //    foreach (DataRow dr in dt_Code.Rows)
            //    {
            //        metroGrid1.Rows.Add(dr["Chemical"], dr["Weight"]);
            //        ButtonNextweighing.Text = "Weighing";
            //        ButtonNextweighing.Show();
            //    }
            //}
            //catch (Exception err)
            //{
            //    _ = new LogWriter($" Error FormWeight Message: {0}, {err.Message}");
            //}
        }

        #endregion

        #region "SerialPort Show Data"
        private void ShowData(object sender, EventArgs e)
        {
            try
            {
                string[] input = Serial_DataIn.Split('*');
                textBoxJOBNumber.Text = input[0];
                textBoxItemcode.Text = input[1];
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "SerialPort Error : ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #region "Function Monitor"
        private void FunctionMonitor()
        {
            try
            {
                //var FormMonitor = new FormMonitor();
                FormMonitor.StartPosition = FormStartPosition.Manual;
                FormMonitor.Left = 1024;
                FormMonitor.Show();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Function Monitor Error : ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(err.Message, "Modbus Client Error : ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region "Function Create table"
        private void Create_table()
        {
            metroGrid1.Columns.Clear();
            metroGrid1.Rows.Clear();
            metroGrid1.ColumnCount = 3;
            
            metroGrid1.Columns[0].HeaderText = "Chemical";
            metroGrid1.Columns[1].HeaderText = "Weight standards";
            metroGrid1.Columns[2].HeaderText = "Actual weight";
        }
        #endregion

        #region "Function Tanks is close"
        private void Function_Tanks_off()
        {
            Thread.Sleep(mydelay);
            ModbusClient.WriteSingleRegister(11, 0);
            Thread.Sleep(mydelay);
            ModbusClient.WriteSingleRegister(12, 0);
            Thread.Sleep(mydelay);
            ModbusClient.WriteSingleRegister(13, 0);
            Thread.Sleep(mydelay);
            ModbusClient.WriteSingleRegister(14, 0);
            Thread.Sleep(mydelay);
            ModbusClient.WriteSingleRegister(15, 0);
            Thread.Sleep(mydelay);
            ModbusClient.WriteSingleRegister(16, 0);
            Thread.Sleep(mydelay);
            ModbusClient.WriteSingleRegister(17, 0);
            Thread.Sleep(mydelay);
            ModbusClient.WriteSingleRegister(18, 0);
            Thread.Sleep(mydelay);
            ModbusClient.WriteSingleRegister(19, 0);
            Thread.Sleep(mydelay);
            ModbusClient.WriteSingleRegister(20, 0);
        }
        #endregion

        #region "Function Tanks is open and next tanks"
        private void Function_Tanks_Open()
        {
            try
            {
                //Location A
                try
                {
                    
                    if (Location_PLC_A == 0)
                    {
                        //labelActualWeight.Text = "00.00";
                        //Function_Tanks_off();
                        var dt = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Code = '{textBoxFormula.Text}' and Formula = '{Radio_Formula}' and Working = '1'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                        }
                        int Count_row = dt.Rows.Count;
                        if (Row_weight + 1 >= Count_row)
                        {
                            double result_Weight = (Weightscale / 1000);
                            #region "Fill tanks"
                            Weight_scale = result_Weight;
                            #endregion
                            metroGrid1.Rows[Row_weight].Cells[2].Value = result_Weight;
                            labelWeighed.Text = result_Weight.ToString();
                            Properties.Settings.Default.MonitorWeight = result_Weight;
                            Properties.Settings.Default.Save();
                            if (Row_weight >= 0)
                            {
                                NameChemical = metroGrid1.Rows[Row_weight].Cells[0].Value.ToString();
                                WeightActual = Convert.ToDouble(metroGrid1.Rows[Row_weight].Cells[1].Value.ToString());
                                labelActualWeight.Text = WeightActual.ToString();

                                Properties.Settings.Default.MonitorChemical = NameChemical;
                                Properties.Settings.Default.MonitorSTD = WeightActual;
                                Properties.Settings.Default.Save();
                                int Result_WeightActual = Convert.ToInt32(WeightActual * 1000);
                                var dt_address = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                var check = conn.CreateCommand();
                                check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{NameChemical}' and Working = '1'";
                                var sda = new SqlDataAdapter(check);
                                sda.Fill(dt_address);
                                }
                                string Address = dt_address.Rows[0]["Address"].ToString();
                                var dt_setpoint = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                    var check = conn.CreateCommand();
                                    check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Chemical = '{NameChemical}' and Working = '1'";
                                    var sda = new SqlDataAdapter(check);
                                    sda.Fill(dt_setpoint);
                                }
                                double SetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"]);
                                WeightSetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"].ToString());
                                Properties.Settings.Default.MonitorSetPoint = WeightSetPoint;
                                Properties.Settings.Default.Save();
                                int Result_SetPoint = Convert.ToInt32(SetPoint * 1000);
                                //Modbus_Client_Port502();
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(21, 0); // เปลี่ยน Status เป็น 0
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(Convert.ToInt32(Address), 1); // เปิดฝาถังถัดไป
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(25, Result_SetPoint); // น้ำหนัก Set Point
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(26, Result_WeightActual); // น้ำหนัก STD
                                #region "Fill tanks"
                                var dt_fill = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                    var check = conn.CreateCommand();
                                    check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{NameChemical}'";
                                    var sda = new SqlDataAdapter(check);
                                    sda.Fill(dt_fill);
                                }
                                double Weigght_Max = Convert.ToDouble(dt_fill.Rows[0]["Weight_Max"]);
                                double Weight_Min = Convert.ToDouble(dt_fill.Rows[0]["Weight_Min"]);
                                double Weight_result = (Weight_Min - Weight_scale);
                                var cmd = $"Update Chemical_Address_PLC_Local " +
                                    $"Set Weight_Min = {Weight_result} " +
                                    $"Where Chemical = '{NameChemical}'";
                                if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                                {

                                }
                                #endregion
                                labelLocation.Text = "A";
                                //timerWeightScale.Enabled = true;
                                // send Location
                                ButtonConfirm.Show();
                                Properties.Settings.Default.MonitorStatus = "1";
                                Properties.Settings.Default.Save();
                                return;
                            }
                        }
                        else
                        {
                            Function_Tanks_off();
                            double result_Weight = (Weightscale / 1000);
                            #region "Fill tanks"
                            Weight_scale = result_Weight;
                            #endregion
                            metroGrid1.Rows[Row_weight].Cells[2].Value = result_Weight;
                            labelWeighed.Text = result_Weight.ToString();
                            Properties.Settings.Default.MonitorWeight = result_Weight;
                            Properties.Settings.Default.Save();
                            Row_weight++;
                            if (Row_weight >= 0)
                            {
                                NameChemical = metroGrid1.Rows[Row_weight].Cells[0].Value.ToString();
                                WeightActual = Convert.ToDouble(metroGrid1.Rows[Row_weight].Cells[1].Value.ToString());
                                labelActualWeight.Text = WeightActual.ToString();
                                Properties.Settings.Default.MonitorChemical = NameChemical;
                                Properties.Settings.Default.MonitorSTD = WeightActual;
                                Properties.Settings.Default.Save();
                                int Result_WeightActual = Convert.ToInt32(WeightActual * 1000);
                                var dt_address = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                    var check = conn.CreateCommand();
                                    check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{NameChemical}' and Working = '1'";
                                    var sda = new SqlDataAdapter(check);
                                    sda.Fill(dt_address);
                                }
                                string Address = dt_address.Rows[0]["Address"].ToString();

                                var dt_setpoint = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                    var check = conn.CreateCommand();
                                    check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Chemical = '{NameChemical}' and Working = '1'";
                                    var sda = new SqlDataAdapter(check);
                                    sda.Fill(dt_setpoint);
                                }
                                double SetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"]);
                                WeightSetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"].ToString());
                                Properties.Settings.Default.MonitorSetPoint = WeightSetPoint;
                                Properties.Settings.Default.Save();
                                int Result_SetPoint = Convert.ToInt32(SetPoint * 1000);
                                //Modbus_Client_Port502();
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(21, 0); // เปลี่ยน Status เป็น 0
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(Convert.ToInt32(Address), 1); // เปิดฝาถังถัดไป
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(25, Result_SetPoint); // น้ำหนัก Set Point
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(26, Result_WeightActual); // น้ำหนัก STD

                                #region "Fill tanks"
                                var dt_fill = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                    var check = conn.CreateCommand();
                                    check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{NameChemical}'";
                                    var sda = new SqlDataAdapter(check);
                                    sda.Fill(dt_fill);
                                }
                                double Weigght_Max = Convert.ToDouble(dt_fill.Rows[0]["Weight_Max"]);
                                double Weight_Min = Convert.ToDouble(dt_fill.Rows[0]["Weight_Min"]);
                                double Weight_result = (Weight_Min - Weight_scale);
                                var cmd = $"Update Chemical_Address_PLC_Local " +
                                    $"Set Weight_Min = {Weight_result} " +
                                    $"Where Chemical = '{NameChemical}'";
                                if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                                {

                                }
                                #endregion
                                timerWeightScale.Enabled = true;
                                labelWeighed.Text = "00.00";
                                Properties.Settings.Default.MonitorStatus = "1";
                                Properties.Settings.Default.Save();
                                return;
                            }
                        }
                    }
                }
                catch (Exception error)
                {
                    _ = new LogWriter($"   Message: {0}, {error.Message}");
                    textBoxAlarm.Text = ($"   Message: {error.Message}");
                }

                //Location B
                try
                {
                    if (Location_PLC_B == 0)
                    {
                        //labelActualWeight.Text = "00.00";
                        //Function_Tanks_off();
                        var dt = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Code = '{textBoxFormula.Text}' and Formula = '{Radio_Formula}' and Working = '1'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt);
                        }
                        int Count_row = dt.Rows.Count;
                        if (Row_weight + 1 >= Count_row)
                        {
                            double result_Weight = (Weightscale / 1000);
                            #region "Fill tanks"
                            Weight_scale = result_Weight;
                            #endregion
                            metroGrid1.Rows[Row_weight].Cells[2].Value = result_Weight;
                            labelWeighed.Text = result_Weight.ToString();
                            Properties.Settings.Default.MonitorWeight = result_Weight;
                            Properties.Settings.Default.Save();
                            if (Row_weight >= 0)
                            {
                                NameChemical = metroGrid1.Rows[Row_weight].Cells[0].Value.ToString();
                                WeightActual = Convert.ToDouble(metroGrid1.Rows[Row_weight].Cells[1].Value.ToString());
                                labelActualWeight.Text = WeightActual.ToString();
                                Properties.Settings.Default.MonitorChemical = NameChemical;
                                Properties.Settings.Default.MonitorSTD = WeightActual;
                                Properties.Settings.Default.Save();
                                int Result_WeightActual = Convert.ToInt32(WeightActual * 1000);
                                var dt_address = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                    var check = conn.CreateCommand();
                                    check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{NameChemical}' and Working = '1'";
                                    var sda = new SqlDataAdapter(check);
                                    sda.Fill(dt_address);
                                }
                                string Address = dt_address.Rows[0]["Address"].ToString();
                                var dt_setpoint = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                    var check = conn.CreateCommand();
                                    check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Chemical = '{NameChemical}' and Working = '1'";
                                    var sda = new SqlDataAdapter(check);
                                    sda.Fill(dt_setpoint);
                                }
                                double SetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"]);
                                WeightSetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"].ToString());
                                Properties.Settings.Default.MonitorSetPoint = WeightSetPoint;
                                Properties.Settings.Default.Save();
                                int Result_SetPoint = Convert.ToInt32(SetPoint * 1000);
                                //Modbus_Client_Port502();
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(21, 0); // เปลี่ยน Status เป็น 0
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(Convert.ToInt32(Address), 1); // เปิดฝาถังถัดไป
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(25, Result_SetPoint); // น้ำหนัก Set Point
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(26, Result_WeightActual); // น้ำหนัก STD
                                #region "Fill tanks"
                                var dt_fill = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                    var check = conn.CreateCommand();
                                    check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{NameChemical}'";
                                    var sda = new SqlDataAdapter(check);
                                    sda.Fill(dt_fill);
                                }
                                double Weigght_Max = Convert.ToDouble(dt_fill.Rows[0]["Weight_Max"]);
                                double Weight_Min = Convert.ToDouble(dt_fill.Rows[0]["Weight_Min"]);
                                double Weight_result = (Weight_Min - Weight_scale);
                                var cmd = $"Update Chemical_Address_PLC_Local " +
                                    $"Set Weight_Min = {Weight_result} " +
                                    $"Where Chemical = '{NameChemical}'";
                                if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                                {

                                }
                                #endregion
                                labelLocation.Text = "B";
                                //timerWeightScale.Enabled = true;
                                // send Location
                                ButtonConfirm.Show();
                                Properties.Settings.Default.MonitorStatus = "1";
                                Properties.Settings.Default.Save();
                                return;
                            }
                        }
                        else
                        {
                            Function_Tanks_off();
                            double result_Weight = (Weightscale / 1000);
                            #region "Fill tanks"
                            Weight_scale = result_Weight;
                            #endregion
                            metroGrid1.Rows[Row_weight].Cells[2].Value = result_Weight;
                            labelWeighed.Text = result_Weight.ToString();
                            Properties.Settings.Default.MonitorWeight = result_Weight;
                            Properties.Settings.Default.Save();
                            Row_weight++;
                            if (Row_weight >= 0)
                            {
                                NameChemical = metroGrid1.Rows[Row_weight].Cells[0].Value.ToString();
                                WeightActual = Convert.ToDouble(metroGrid1.Rows[Row_weight].Cells[1].Value.ToString());
                                labelActualWeight.Text = WeightActual.ToString();
                                Properties.Settings.Default.MonitorChemical = NameChemical;
                                Properties.Settings.Default.MonitorSTD = WeightActual;
                                Properties.Settings.Default.Save();
                                int Result_WeightActual = Convert.ToInt32(WeightActual * 1000);
                                var dt_address = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                    var check = conn.CreateCommand();
                                    check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{NameChemical}' and Working = '1'";
                                    var sda = new SqlDataAdapter(check);
                                    sda.Fill(dt_address);
                                }
                                string Address = dt_address.Rows[0]["Address"].ToString();

                                var dt_setpoint = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                    var check = conn.CreateCommand();
                                    check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Chemical = '{NameChemical}' and Working = '1'";
                                    var sda = new SqlDataAdapter(check);
                                    sda.Fill(dt_setpoint);
                                }
                                double SetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"]);
                                WeightSetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"].ToString());
                                Properties.Settings.Default.MonitorSetPoint = WeightSetPoint;
                                Properties.Settings.Default.Save();
                                int Result_SetPoint = Convert.ToInt32(SetPoint * 1000);
                                //Modbus_Client_Port502();
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(21, 0); // เปลี่ยน Status เป็น 0
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(Convert.ToInt32(Address), 1); // เปิดฝาถังถัดไป
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(25, Result_SetPoint); // น้ำหนัก Set Point
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(26, Result_WeightActual); // น้ำหนัก STD
                                #region "Fill tanks"
                                var dt_fill = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                    var check = conn.CreateCommand();
                                    check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{NameChemical}'";
                                    var sda = new SqlDataAdapter(check);
                                    sda.Fill(dt_fill);
                                }
                                double Weigght_Max = Convert.ToDouble(dt_fill.Rows[0]["Weight_Max"]);
                                double Weight_Min = Convert.ToDouble(dt_fill.Rows[0]["Weight_Min"]);
                                double Weight_result = (Weight_Min - Weight_scale);
                                var cmd = $"Update Chemical_Address_PLC_Local " +
                                    $"Set Weight_Min = {Weight_result} " +
                                    $"Where Chemical = '{NameChemical}'";
                                if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                                {

                                }
                                #endregion
                                timerWeightScale.Enabled = true;
                                labelWeighed.Text = "00.00";
                                Properties.Settings.Default.MonitorStatus = "1";
                                Properties.Settings.Default.Save();
                                return;
                            }
                        }
                    }
                }
                catch (Exception error)
                {
                    _ = new LogWriter($"   Message: {0}, {error.Message}");
                    textBoxAlarm.Text = ($"   Message: {error.Message}");
                }

                //Location C
                try
                {
                    if (Location_PLC_C == 0)
                    {
                        //labelActualWeight.Text = "00.00";
                        //Function_Tanks_off();
                        var dt = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Code = '{textBoxFormula.Text}' and Formula = '{Radio_Formula}' and Working = '1'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt);
                        }
                        int Count_row = dt.Rows.Count;
                        if (Row_weight + 1 >= Count_row)
                        {
                            double result_Weight = (Weightscale / 1000);
                            #region "Fill tanks"
                            Weight_scale = result_Weight;
                            #endregion
                            metroGrid1.Rows[Row_weight].Cells[2].Value = result_Weight;
                            labelWeighed.Text = result_Weight.ToString();
                            Properties.Settings.Default.MonitorWeight = result_Weight;
                            Properties.Settings.Default.Save();
                            if (Row_weight >= 0)
                            {
                                NameChemical = metroGrid1.Rows[Row_weight].Cells[0].Value.ToString();
                                WeightActual = Convert.ToDouble(metroGrid1.Rows[Row_weight].Cells[1].Value.ToString());
                                labelActualWeight.Text = WeightActual.ToString();
                                Properties.Settings.Default.MonitorChemical = NameChemical;
                                Properties.Settings.Default.MonitorSTD = WeightActual;
                                Properties.Settings.Default.Save();
                                int Result_WeightActual = Convert.ToInt32(WeightActual * 1000);
                                var dt_address = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                    var check = conn.CreateCommand();
                                    check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{NameChemical}' and Working = '1'";
                                    var sda = new SqlDataAdapter(check);
                                    sda.Fill(dt_address);
                                }
                                string Address = dt_address.Rows[0]["Address"].ToString();
                                var dt_setpoint = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                    var check = conn.CreateCommand();
                                    check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Chemical = '{NameChemical}' and Working = '1'";
                                    var sda = new SqlDataAdapter(check);
                                    sda.Fill(dt_setpoint);
                                }
                                double SetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"]);
                                WeightSetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"].ToString());
                                Properties.Settings.Default.MonitorSetPoint = WeightSetPoint;
                                Properties.Settings.Default.Save();
                                int Result_SetPoint = Convert.ToInt32(SetPoint * 1000);
                                //Modbus_Client_Port502();
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(21, 0); // เปลี่ยน Status เป็น 0
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(Convert.ToInt32(Address), 1); // เปิดฝาถังถัดไป
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(25, Result_SetPoint); // น้ำหนัก Set Point
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(26, Result_WeightActual); // น้ำหนัก STD
                                #region "Fill tanks"
                                var dt_fill = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                    var check = conn.CreateCommand();
                                    check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{NameChemical}'";
                                    var sda = new SqlDataAdapter(check);
                                    sda.Fill(dt_fill);
                                }
                                double Weigght_Max = Convert.ToDouble(dt_fill.Rows[0]["Weight_Max"]);
                                double Weight_Min = Convert.ToDouble(dt_fill.Rows[0]["Weight_Min"]);
                                double Weight_result = (Weight_Min - Weight_scale);
                                var cmd = $"Update Chemical_Address_PLC_Local " +
                                    $"Set Weight_Min = {Weight_result} " +
                                    $"Where Chemical = '{NameChemical}'";
                                if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                                {

                                }
                                #endregion
                                labelLocation.Text = "C";
                                //timerWeightScale.Enabled = true;
                                // send Location
                                ButtonConfirm.Show();
                                Properties.Settings.Default.MonitorStatus = "1";
                                Properties.Settings.Default.Save();
                                return;
                            }
                        }
                        else
                        {
                            Function_Tanks_off();
                            double result_Weight = (Weightscale / 1000);
                            #region "Fill tanks"
                            Weight_scale = result_Weight;
                            #endregion
                            metroGrid1.Rows[Row_weight].Cells[2].Value = result_Weight;
                            labelWeighed.Text = result_Weight.ToString();
                            Properties.Settings.Default.MonitorWeight = result_Weight;
                            Properties.Settings.Default.Save();
                            Row_weight++;
                            if (Row_weight >= 0)
                            {
                                NameChemical = metroGrid1.Rows[Row_weight].Cells[0].Value.ToString();
                                WeightActual = Convert.ToDouble(metroGrid1.Rows[Row_weight].Cells[1].Value.ToString());
                                labelActualWeight.Text = WeightActual.ToString();
                                Properties.Settings.Default.MonitorChemical = NameChemical;
                                Properties.Settings.Default.MonitorSTD = WeightActual;
                                Properties.Settings.Default.Save();
                                int Result_WeightActual = Convert.ToInt32(WeightActual * 1000);
                                var dt_address = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                    var check = conn.CreateCommand();
                                    check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{NameChemical}' and Working = '1'";
                                    var sda = new SqlDataAdapter(check);
                                    sda.Fill(dt_address);
                                }
                                string Address = dt_address.Rows[0]["Address"].ToString();

                                var dt_setpoint = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                    var check = conn.CreateCommand();
                                    check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Chemical = '{NameChemical}' and Working = '1'";
                                    var sda = new SqlDataAdapter(check);
                                    sda.Fill(dt_setpoint);
                                }
                                double SetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"]);
                                WeightSetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"].ToString());
                                Properties.Settings.Default.MonitorSetPoint = WeightSetPoint;
                                Properties.Settings.Default.Save();
                                int Result_SetPoint = Convert.ToInt32(SetPoint * 1000);
                                //Modbus_Client_Port502();
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(21, 0); // เปลี่ยน Status เป็น 0
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(Convert.ToInt32(Address), 1); // เปิดฝาถังถัดไป
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(25, Result_SetPoint); // น้ำหนัก Set Point
                                Thread.Sleep(mydelay);
                                ModbusClient.WriteSingleRegister(26, Result_WeightActual); // น้ำหนัก STD
                                #region "Fill tanks"
                                var dt_fill = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                    var check = conn.CreateCommand();
                                    check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{NameChemical}'";
                                    var sda = new SqlDataAdapter(check);
                                    sda.Fill(dt_fill);
                                }
                                double Weigght_Max = Convert.ToDouble(dt_fill.Rows[0]["Weight_Max"]);
                                double Weight_Min = Convert.ToDouble(dt_fill.Rows[0]["Weight_Min"]);
                                double Weight_result = (Weight_Min - Weight_scale);
                                var cmd = $"Update Chemical_Address_PLC_Local " +
                                    $"Set Weight_Min = {Weight_result} " +
                                    $"Where Chemical = '{NameChemical}'";
                                if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                                {

                                }
                                #endregion
                                timerWeightScale.Enabled = true;
                                labelWeighed.Text = "00.00";
                                Properties.Settings.Default.MonitorStatus = "1";
                                Properties.Settings.Default.Save();
                                return;
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
            catch (Exception error)
            {
                _ = new LogWriter($"   Message: {0}, {error.Message}");
                textBoxAlarm.Text = ($"   Message: {error.Message}");
            }
            #region "Code Hiden"
            //try
            //{
            //    int[] D = ModbusClient.ReadHoldingRegisters(0, 100);
            //    int ReadStatus = Convert.ToInt32(D[21]);
            //    if (ReadStatus == 1 && Convert.ToInt32(Weightscale) > 0)
            //    {
            //        Function_Tanks_off();
            //        var dt_Location = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * From Chemical_LocationWaiting_Local";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt_Location);
            //        }
            //        Location_Local_A = Convert.ToInt32(dt_Location.Rows[0]["Status"]);
            //        Location_Local_B = Convert.ToInt32(dt_Location.Rows[1]["Status"]);
            //        Location_Local_C = Convert.ToInt32(dt_Location.Rows[2]["Status"]);
            //        if (Location_Local_A == 0 && Location_PLC_A == 0 || Location_Local_B == 0 && Location_PLC_B == 0 || Location_Local_C == 0 && Location_PLC_C == 0)
            //        {
            //            if (Location_Local_A == 0 && Location_PLC_A == 0)
            //            {
            //                var dt = new DataTable();
            //                using (var conn = new SqlConnection(Local_Conn))
            //                {
            //                    var check = conn.CreateCommand();
            //                    check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Code = '{textBoxFormula.Text}' and Formula = '{Radio_Formula}'";
            //                    var sda = new SqlDataAdapter(check);
            //                    sda.Fill(dt);
            //                }
            //                int Count_row = dt.Rows.Count;
            //                if (Row_weight + 1 >= Count_row)
            //                {
            //                    double result_Weight = (Weightscale / 1000);
            //                    metroGrid1.Rows[Row_weight].Cells[2].Value = result_Weight;
            //                    labelWeighed.Text = result_Weight.ToString();
            //                    if (Row_weight > 0)
            //                    {
            //                        NameChemical = metroGrid1.Rows[Row_weight].Cells[0].Value.ToString();
            //                        WeightActual = Convert.ToDouble(metroGrid1.Rows[Row_weight].Cells[1].Value.ToString());
            //                        labelActualWeight.Text = WeightActual.ToString();
            //                        int Result_WeightActual = Convert.ToInt32(WeightActual * 1000);
            //                        var dt_address = new DataTable();
            //                        using (var conn = new SqlConnection(Local_Conn))
            //                        {
            //                            var check = conn.CreateCommand();
            //                            check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{NameChemical}'";
            //                            var sda = new SqlDataAdapter(check);
            //                            sda.Fill(dt_address);
            //                        }
            //                        string Address = dt_address.Rows[0]["Address"].ToString();

            //                        var dt_setpoint = new DataTable();
            //                        using (var conn = new SqlConnection(Local_Conn))
            //                        {
            //                            var check = conn.CreateCommand();
            //                            check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Chemical = '{NameChemical}'";
            //                            var sda = new SqlDataAdapter(check);
            //                            sda.Fill(dt_setpoint);
            //                        }
            //                        double SetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"]);
            //                        int Result_SetPoint = Convert.ToInt32(SetPoint * 1000);
            //                        Modbus_Client_Port502();
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(Convert.ToInt32(Address), 1); // เปิดฝาถังถัดไป
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(25, Result_SetPoint); // น้ำหนัก Set Point
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(26, Result_WeightActual); // น้ำหนัก STD
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(21, 0); // เปลี่ยน Status เป็น 0
            //                        //labelLocation.Text = "A";
            //                        timerWeightScale.Enabled = true;
            //                        // send Location
            //                        ButtonConfirm.Show();
            //                    }
            //                }
            //                else
            //                {
            //                    double result_Weight = (Weightscale / 1000);
            //                    metroGrid1.Rows[Row_weight].Cells[2].Value = result_Weight;
            //                    labelWeighed.Text = result_Weight.ToString();
            //                    Row_weight++;
            //                    if (Row_weight > 0)
            //                    {
            //                        NameChemical = metroGrid1.Rows[Row_weight].Cells[0].Value.ToString();
            //                        WeightActual = Convert.ToDouble(metroGrid1.Rows[Row_weight].Cells[1].Value.ToString());
            //                        labelActualWeight.Text = WeightActual.ToString();
            //                        int Result_WeightActual = Convert.ToInt32(WeightActual * 1000);
            //                        var dt_address = new DataTable();
            //                        using (var conn = new SqlConnection(Local_Conn))
            //                        {
            //                            var check = conn.CreateCommand();
            //                            check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{NameChemical}'";
            //                            var sda = new SqlDataAdapter(check);
            //                            sda.Fill(dt_address);
            //                        }
            //                        string Address = dt_address.Rows[0]["Address"].ToString();

            //                        var dt_setpoint = new DataTable();
            //                        using (var conn = new SqlConnection(Local_Conn))
            //                        {
            //                            var check = conn.CreateCommand();
            //                            check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Chemical = '{NameChemical}'";
            //                            var sda = new SqlDataAdapter(check);
            //                            sda.Fill(dt_setpoint);
            //                        }
            //                        double SetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"]);
            //                        int Result_SetPoint = Convert.ToInt32(SetPoint * 1000);
            //                        Modbus_Client_Port502();
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(Convert.ToInt32(Address), 1); // เปิดฝาถังถัดไป
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(25, Result_SetPoint); // น้ำหนัก Set Point
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(26, Result_WeightActual); // น้ำหนัก STD
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(21, 0); // เปลี่ยน Status เป็น 0
            //                        timerWeightScale.Enabled = true;
            //                    }
            //                }
            //            }
            //            else if (Location_Local_B == 0 && Location_PLC_B == 0)
            //            {
            //                var dt = new DataTable();
            //                using (var conn = new SqlConnection(Local_Conn))
            //                {
            //                    var check = conn.CreateCommand();
            //                    check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Code = '{textBoxFormula.Text}' and Formula = '{Radio_Formula}'";
            //                    var sda = new SqlDataAdapter(check);
            //                    sda.Fill(dt);
            //                }
            //                int Count_row = dt.Rows.Count;
            //                if (Row_weight + 1 >= Count_row)
            //                {
            //                    double result_Weight = (Weightscale / 1000);
            //                    metroGrid1.Rows[Row_weight].Cells[2].Value = result_Weight;
            //                    labelWeighed.Text = result_Weight.ToString();
            //                    if (Row_weight > 0)
            //                    {
            //                        NameChemical = metroGrid1.Rows[Row_weight].Cells[0].Value.ToString();
            //                        WeightActual = Convert.ToDouble(metroGrid1.Rows[Row_weight].Cells[1].Value.ToString());
            //                        labelActualWeight.Text = WeightActual.ToString();
            //                        int Result_WeightActual = Convert.ToInt32(WeightActual * 1000);
            //                        var dt_address = new DataTable();
            //                        using (var conn = new SqlConnection(Local_Conn))
            //                        {
            //                            var check = conn.CreateCommand();
            //                            check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{NameChemical}'";
            //                            var sda = new SqlDataAdapter(check);
            //                            sda.Fill(dt_address);
            //                        }
            //                        string Address = dt_address.Rows[0]["Address"].ToString();

            //                        var dt_setpoint = new DataTable();
            //                        using (var conn = new SqlConnection(Local_Conn))
            //                        {
            //                            var check = conn.CreateCommand();
            //                            check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Chemical = '{NameChemical}'";
            //                            var sda = new SqlDataAdapter(check);
            //                            sda.Fill(dt_setpoint);
            //                        }
            //                        double SetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"]);
            //                        int Result_SetPoint = Convert.ToInt32(SetPoint * 1000);
            //                        Modbus_Client_Port502();
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(Convert.ToInt32(Address), 1); // เปิดฝาถังถัดไป
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(25, Result_SetPoint); // น้ำหนัก Set Point
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(26, Result_WeightActual); // น้ำหนัก STD
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(21, 0); // เปลี่ยน Status เป็น 0
            //                        //labelLocation.Text = "B";
            //                        timerWeightScale.Enabled = true;
            //                        ButtonConfirm.Show();
            //                    }
            //                }
            //                else
            //                {
            //                    double result_Weight = (Weightscale / 1000);
            //                    metroGrid1.Rows[Row_weight].Cells[2].Value = result_Weight;
            //                    labelWeighed.Text = result_Weight.ToString();
            //                    Row_weight++;
            //                    if (Row_weight > 0)
            //                    {
            //                        NameChemical = metroGrid1.Rows[Row_weight].Cells[0].Value.ToString();
            //                        WeightActual = Convert.ToDouble(metroGrid1.Rows[Row_weight].Cells[1].Value.ToString());
            //                        labelActualWeight.Text = WeightActual.ToString();
            //                        int Result_WeightActual = Convert.ToInt32(WeightActual * 1000);
            //                        var dt_address = new DataTable();
            //                        using (var conn = new SqlConnection(Local_Conn))
            //                        {
            //                            var check = conn.CreateCommand();
            //                            check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{NameChemical}'";
            //                            var sda = new SqlDataAdapter(check);
            //                            sda.Fill(dt_address);
            //                        }
            //                        string Address = dt_address.Rows[0]["Address"].ToString();

            //                        var dt_setpoint = new DataTable();
            //                        using (var conn = new SqlConnection(Local_Conn))
            //                        {
            //                            var check = conn.CreateCommand();
            //                            check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Chemical = '{NameChemical}'";
            //                            var sda = new SqlDataAdapter(check);
            //                            sda.Fill(dt_setpoint);
            //                        }
            //                        double SetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"]);
            //                        int Result_SetPoint = Convert.ToInt32(SetPoint * 1000);
            //                        Modbus_Client_Port502();
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(Convert.ToInt32(Address), 1); // เปิดฝาถังถัดไป
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(25, Result_SetPoint); // น้ำหนัก Set Point
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(26, Result_WeightActual); // น้ำหนัก STD
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(21, 0); // เปลี่ยน Status เป็น 0
            //                        timerWeightScale.Enabled = true;
            //                    }
            //                }
            //            }
            //            else if (Location_Local_C == 0 && Location_PLC_C == 0)
            //            {
            //                var dt = new DataTable();
            //                using (var conn = new SqlConnection(Local_Conn))
            //                {
            //                    var check = conn.CreateCommand();
            //                    check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Code = '{textBoxFormula.Text}' and Formula = '{Radio_Formula}'";
            //                    var sda = new SqlDataAdapter(check);
            //                    sda.Fill(dt);
            //                }
            //                int Count_row = dt.Rows.Count;
            //                if (Row_weight + 1 >= Count_row)
            //                {
            //                    double result_Weight = (Weightscale / 1000);
            //                    metroGrid1.Rows[Row_weight].Cells[2].Value = result_Weight;
            //                    labelWeighed.Text = result_Weight.ToString();
            //                    if (Row_weight > 0)
            //                    {
            //                        NameChemical = metroGrid1.Rows[Row_weight].Cells[0].Value.ToString();
            //                        WeightActual = Convert.ToDouble(metroGrid1.Rows[Row_weight].Cells[1].Value.ToString());
            //                        labelActualWeight.Text = WeightActual.ToString();
            //                        int Result_WeightActual = Convert.ToInt32(WeightActual * 1000);
            //                        var dt_address = new DataTable();
            //                        using (var conn = new SqlConnection(Local_Conn))
            //                        {
            //                            var check = conn.CreateCommand();
            //                            check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{NameChemical}'";
            //                            var sda = new SqlDataAdapter(check);
            //                            sda.Fill(dt_address);
            //                        }
            //                        string Address = dt_address.Rows[0]["Address"].ToString();

            //                        var dt_setpoint = new DataTable();
            //                        using (var conn = new SqlConnection(Local_Conn))
            //                        {
            //                            var check = conn.CreateCommand();
            //                            check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Chemical = '{NameChemical}'";
            //                            var sda = new SqlDataAdapter(check);
            //                            sda.Fill(dt_setpoint);
            //                        }
            //                        double SetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"]);
            //                        int Result_SetPoint = Convert.ToInt32(SetPoint * 1000);
            //                        Modbus_Client_Port502();
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(Convert.ToInt32(Address), 1); // เปิดฝาถังถัดไป
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(25, Result_SetPoint); // น้ำหนัก Set Point
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(26, Result_WeightActual); // น้ำหนัก STD
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(21, 0); // เปลี่ยน Status เป็น 0
            //                        //labelLocation.Text = "C";
            //                        timerWeightScale.Enabled = true;
            //                        ButtonConfirm.Show();
            //                    }
            //                }
            //                else
            //                {
            //                    double result_Weight = (Weightscale / 1000);
            //                    metroGrid1.Rows[Row_weight].Cells[2].Value = result_Weight;
            //                    labelWeighed.Text = result_Weight.ToString();
            //                    Row_weight++;
            //                    if (Row_weight > 0)
            //                    {
            //                        NameChemical = metroGrid1.Rows[Row_weight].Cells[0].Value.ToString();
            //                        WeightActual = Convert.ToDouble(metroGrid1.Rows[Row_weight].Cells[1].Value.ToString());
            //                        labelActualWeight.Text = WeightActual.ToString();
            //                        int Result_WeightActual = Convert.ToInt32(WeightActual * 1000);
            //                        var dt_address = new DataTable();
            //                        using (var conn = new SqlConnection(Local_Conn))
            //                        {
            //                            var check = conn.CreateCommand();
            //                            check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{NameChemical}'";
            //                            var sda = new SqlDataAdapter(check);
            //                            sda.Fill(dt_address);
            //                        }
            //                        string Address = dt_address.Rows[0]["Address"].ToString();

            //                        var dt_setpoint = new DataTable();
            //                        using (var conn = new SqlConnection(Local_Conn))
            //                        {
            //                            var check = conn.CreateCommand();
            //                            check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Chemical = '{NameChemical}'";
            //                            var sda = new SqlDataAdapter(check);
            //                            sda.Fill(dt_setpoint);
            //                        }
            //                        double SetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"]);
            //                        int Result_SetPoint = Convert.ToInt32(SetPoint * 1000);
            //                        Modbus_Client_Port502();
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(Convert.ToInt32(Address), 1); // เปิดฝาถังถัดไป
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(25, Result_SetPoint); // น้ำหนัก Set Point
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(26, Result_SetPoint); // น้ำหนัก STD
            //                        Thread.Sleep(mydelay);
            //                        ModbusClient.WriteSingleRegister(21, 0); // เปลี่ยน Status เป็น 0
            //                        timerWeightScale.Enabled = true;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception error)
            //{
            //    _ = new LogWriter($"   Message: {0}, {error.Message}");
            //    textBoxAlarm.Text = ($"   Message: {error.Message}");

            //}
            #endregion
        }
        #endregion

        #region "Function Check Location of PLC"
        private void Function_CheckLocation_PLC()
        {
            try
            {
                //Modbus_Client_Port502();
                int[] D = ModbusClient.ReadHoldingRegisters(0, 100);
                Thread.Sleep(mydelay);
                Location_PLC_A = D[34];
                Thread.Sleep(mydelay);
                Location_PLC_B = D[35];
                Thread.Sleep(mydelay);
                Location_PLC_C = D[36];
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormWeight Message: {0}, {err.Message}");
            }
        }
        #endregion

        #region "Function Clear Text"
        private void Cleartext()
        {
            try
            {
                labelActualWeight.Text = "00.00";
                labelWeighed.Text = "00.00";
                labelLocation.Text = "-";
                textBoxJOBNumber.Text = "";
                textBoxItemcode.Text = "";
                textBoxFormula.Text = "";
                textBoxLeadPowder.Text = "";
                Radio_Formula = 0;
                textBoxJOBNumber.Enabled = false;
                textBoxItemcode.Enabled = false;
                textBoxFormula.Enabled = false;
                textBoxLeadPowder.Enabled = false;
                rjButton1.Show();
                rjButton2.Show();
                rjButton3.Show();
                metroGrid1.Rows.Clear();
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormWeight Message: {0}, {err.Message}");
            }
        }
        #endregion

        #region "Text Changed of Item code"
        private void textBoxItemcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //////// Funtion Scan and click create Job
                textBoxFormula.Text = "";
                labelActualWeight.Text = "00.00";
                labelWeighed.Text = "00.00";
                ButtonNextweighing.Hide();
                if (textBoxItemcode.TextLength >= 11)
                {
                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * from Chemical_Model_Local Where Item_code = '{textBoxItemcode.Text}' and Working = '1'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    int Count_row = dt.Rows.Count;
                    if (Count_row == 0)
                    {
                        ButtonNextweighing.Hide();
                        ButtonConfirm.Hide();
                        textBoxFormula.Text = "";
                        labelActualWeight.Text = "00.00";
                        labelWeighed.Text = "00.00";
                    }
                    else
                    {
                        textBoxFormula.Text = dt.Rows[0]["Code"].ToString();
                    }
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormWeight Message: {0}, {err.Message}");
            }
        }
        #endregion

        #region "Button Weight"
        private void ButtonNextweighing_Click(object sender, EventArgs e)
        {
            try
            {
                const string message = " คุณแน่ใจหรือไม่ ว่าต้องการชั่งน้ำหนัก Job number นี้? ";
                const string caption = "Create Job for weighing";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (textBoxItemcode.Text == "" || textBoxItemcode.Text == null || textBoxJOBNumber.Text == "" || textBoxJOBNumber.Text == null || textBoxFormula.Text == "" || textBoxFormula.Text == null || textBoxLeadPowder.Text == "" || textBoxLeadPowder.Text == null)
                    {
                        MessageBox.Show(" กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        #region "F_Fill"
                        var dt_select = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * From Chemical_Address_PLC_Local";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt_select);
                        }
                        string Name_tanks1 = dt_select.Rows[0]["Chemical"].ToString();
                        string Name_tanks2 = dt_select.Rows[1]["Chemical"].ToString();
                        string Name_tanks3 = dt_select.Rows[2]["Chemical"].ToString();
                        string Name_tanks4 = dt_select.Rows[3]["Chemical"].ToString();
                        string Name_tanks5 = dt_select.Rows[4]["Chemical"].ToString();
                        string Name_tanks6 = dt_select.Rows[5]["Chemical"].ToString();
                        string Name_tanks7 = dt_select.Rows[6]["Chemical"].ToString();
                        string Name_tanks8 = dt_select.Rows[7]["Chemical"].ToString();
                        string Name_tanks9 = dt_select.Rows[8]["Chemical"].ToString();
                        string Name_tanks10 = dt_select.Rows[9]["Chemical"].ToString();

                        var dt_fill1 = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{Name_tanks1}'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt_fill1);
                        }
                        double tanks_1 = Convert.ToDouble(dt_fill1.Rows[0]["Weight_Min"]);

                        var dt_fill2 = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{Name_tanks2}'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt_fill2);
                        }
                        double tanks_2 = Convert.ToDouble(dt_fill2.Rows[0]["Weight_Min"]);

                        var dt_fill3 = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{Name_tanks3}'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt_fill3);
                        }
                        double tanks_3 = Convert.ToDouble(dt_fill3.Rows[0]["Weight_Min"]);

                        var dt_fill4 = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{Name_tanks4}'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt_fill4);
                        }
                        double tanks_4 = Convert.ToDouble(dt_fill4.Rows[0]["Weight_Min"]);

                        var dt_fill5 = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{Name_tanks5}'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt_fill5);
                        }
                        double tanks_5 = Convert.ToDouble(dt_fill5.Rows[0]["Weight_Min"]);

                        var dt_fill6 = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{Name_tanks6}'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt_fill6);
                        }
                        double tanks_6 = Convert.ToDouble(dt_fill6.Rows[0]["Weight_Min"]);


                        var dt_fill7 = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{Name_tanks7}'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt_fill7);
                        }
                        double tanks_7 = Convert.ToDouble(dt_fill7.Rows[0]["Weight_Min"]);

                        var dt_fill8 = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{Name_tanks8}'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt_fill8);
                        }
                        double tanks_8 = Convert.ToDouble(dt_fill8.Rows[0]["Weight_Min"]);

                        var dt_fill9 = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{Name_tanks9}'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt_fill9);
                        }
                        double tanks_9 = Convert.ToDouble(dt_fill9.Rows[0]["Weight_Min"]);

                        var dt_fill10 = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{Name_tanks10}'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt_fill10);
                        }
                        double tanks_10 = Convert.ToDouble(dt_fill10.Rows[0]["Weight_Min"]);
                        #endregion

                        if (tanks_1 < 2 || tanks_2 < 2 || tanks_3 < 2 || tanks_4 < 2 || tanks_5 < 2 || tanks_6 < 2 || tanks_7 < 2 || tanks_8 < 2 || tanks_9 < 2 || tanks_10 < 2)
                        {
                            MessageBox.Show(" ถังบรรจุสารเคมี มีจำนวนน้อยกว่า 2kg., กรุณาเติมสารเคมี ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            Function_CheckLocation_PLC();
                            if (Location_PLC_A == 1 && Location_PLC_B == 1 && Location_PLC_C == 1)
                            {
                                MessageBox.Show(" พื้นที่ Stock สารเคมีเต็ม, กรุณาตรวจสอบอีกครั้ง ");
                            }
                            else
                            {

                                var dt = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                    var check = conn.CreateCommand();
                                    check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Code = '{textBoxFormula.Text}' and Formula = '{Radio_Formula}' and Working = '1'";
                                    var sda = new SqlDataAdapter(check);
                                    sda.Fill(dt);
                                }
                                int Count_row = dt.Rows.Count;
                                if (Count_row == 0)
                                {
                                    MessageBox.Show(" กรุณาตรวจสอบข้อมูล ");
                                }
                                else
                                {
                                    NameChemical = metroGrid1.Rows[Row_weight].Cells[0].Value.ToString();
                                    WeightActual = Convert.ToDouble(metroGrid1.Rows[Row_weight].Cells[1].Value.ToString());
                                    labelActualWeight.Text = WeightActual.ToString();
                                    Properties.Settings.Default.MonitorChemical = NameChemical;
                                    Properties.Settings.Default.MonitorSTD = WeightActual;
                                    Properties.Settings.Default.MonitorFormula = textBoxFormula.Text;
                                    Properties.Settings.Default.Save();
                                    int Result_WeightActual = Convert.ToInt32(WeightActual * 1000);
                                    var dt_address = new DataTable();
                                    using (var conn = new SqlConnection(Local_Conn))
                                    {
                                        var check = conn.CreateCommand();
                                        check.CommandText = $"Select * From Chemical_Address_PLC_Local Where Chemical = '{NameChemical}' and Working = '1'";
                                        var sda = new SqlDataAdapter(check);
                                        sda.Fill(dt_address);
                                    }
                                    string Address = dt_address.Rows[0]["Address"].ToString();

                                    var dt_setpoint = new DataTable();
                                    using (var conn = new SqlConnection(Local_Conn))
                                    {
                                        var check = conn.CreateCommand();
                                        check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Chemical = '{NameChemical}' and Working = '1'";
                                        var sda = new SqlDataAdapter(check);
                                        sda.Fill(dt_setpoint);
                                    }
                                    double SetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"]);
                                    WeightSetPoint = Convert.ToDouble(dt_setpoint.Rows[0]["Set_Point"].ToString());
                                    Properties.Settings.Default.MonitorSetPoint = WeightSetPoint;
                                    Properties.Settings.Default.Save();
                                    int Result_SetPoint = Convert.ToInt32(SetPoint * 1000);
                                    ButtonNextweighing.Hide();
                                    //Modbus_Client_Port502();
                                    Thread.Sleep(mydelay);
                                    ModbusClient.WriteSingleRegister(Convert.ToInt32(Address), 1); // Open tanks
                                    Thread.Sleep(mydelay);
                                    ModbusClient.WriteSingleRegister(25, Result_SetPoint); // น้ำหนัก Set Point
                                    Thread.Sleep(mydelay);
                                    ModbusClient.WriteSingleRegister(26, Result_WeightActual); // น้ำหนัก STD
                                    Thread.Sleep(mydelay);
                                    ModbusClient.WriteSingleRegister(21, 0); // Status = false
                                    timerWeightScale.Enabled = true;
                                    //timerOpentanks.Enabled = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormWeight Message: {0}, {err.Message}");
                textBoxAlarm.Text = ($"   Message: {err.Message}");
            }
        }
        #endregion

        #region "Button Confirm / Save all to database local"
        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                const string message = " คุณแน่ใจหรือไม่ ว่าต้องการอัปเดตข้อมูลนี้ไปยังฐานข้อมูล? ";
                const string caption = "Add Model to Database";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Function_Tanks_off();
                    int ID_Emp_Save = ID_Emp_Main;
                    string Name_Emp_Save = Name_Emp_Main;
                    string Job_No_Save = textBoxJOBNumber.Text;
                    string Item_Code_Save = textBoxItemcode.Text;
                    string Formula_Save = textBoxFormula.Text;
                    string Location_Save = labelLocation.Text;
                    string Status_Save = "Pending";
                    int count_row = metroGrid1.Rows.Count;
                    for (int i = 0; i + 1 < count_row;)
                    {
                        string Chemical_Save = metroGrid1.Rows[i].Cells[0].Value.ToString();
                        string Weight_Save = metroGrid1.Rows[i].Cells[2].Value.ToString();
                        double Weight_STD_Save = Convert.ToDouble(metroGrid1.Rows[i].Cells[1].Value.ToString());
                        var cmd = $"Insert into Chemical_Record_Local (Name_Employee, ID_Employee, Job_No, Item_code, Formula, Chemical, Weight_STD, Weighed, Location_waiting, Status, Location_machine, DateTime, Status_Update) " +
                        $"Values ('{Name_Emp_Save}', '{ID_Emp_Save}', '{Job_No_Save}', '{Item_Code_Save}', '{Formula_Save}', '{Chemical_Save}', '{Weight_STD_Save}', '{Weight_Save}', '{Location_Save}', '{Status_Save}', '{null}', Getdate(), 'false')";
                        if (ExecuteSqlTransaction(cmd, Local_Conn, "ADD"))
                        {
                            i++;
                        }
                        else
                        {
                            MessageBox.Show("ไม่สามารถบันทึกได้, กรุณาตรวจสอบอีกครั้ง.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    if (Location_Save == "A")
                    {
                        Thread.Sleep(mydelay);
                        ModbusClient.WriteSingleRegister(22, 1); // Location A
                    }
                    else if (Location_Save == "B")
                    {
                        Thread.Sleep(mydelay);
                        ModbusClient.WriteSingleRegister(23, 1); // Location B
                    }
                    else if (Location_Save == "C")
                    {
                        Thread.Sleep(mydelay);
                        ModbusClient.WriteSingleRegister(24, 1); // Location C
                    }
                    Properties.Settings.Default.MonitorStatus = "1";
                    Properties.Settings.Default.Save();
                    MessageBox.Show("บันทึกสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormWeight Message: {0}, {err.Message}");
                textBoxAlarm.Text = ($"   Message: {err.Message}");
            }

            //try
            //{
            //    const string message = " คุณแน่ใจหรือไม่ ว่าต้องการอัปเดตข้อมูลนี้ไปยังฐานข้อมูล? ";
            //    const string caption = "Add Model to Database";
            //    var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    if (result == DialogResult.Yes)
            //    {
            //        Function_Tanks_off();
            //        int ID_Emp_Save = ID_Emp_Main;
            //        string Name_Emp_Save = Name_Emp_Main;
            //        string Job_No_Save = textBoxJOBNumber.Text;
            //        string Item_Code_Save = textBoxItemcode.Text;
            //        string Formula_Save = textBoxFormula.Text;
            //        string Location_Save = labelLocation.Text;
            //        string Status_Save = "Pending";
            //        int count_row = metroGrid1.Rows.Count;
            //        for (int i = 0; i + 1 < count_row;)
            //        {
            //            string Chemical_Save = metroGrid1.Rows[i].Cells[0].Value.ToString();
            //            string Weight_Save = metroGrid1.Rows[i].Cells[2].Value.ToString();
            //            double Weight_STD_Save = Convert.ToDouble(metroGrid1.Rows[i].Cells[1].Value.ToString());
            //            var cmd = $"Insert into Chemical_Record_Local (Name_Employee, ID_Employee, Job_No, Item_code, Formula, Chemical, Weight_STD, Weighed, Location_waiting, Status, Location_machine, DateTime, Status_Update) " +
            //            $"Values ('{Name_Emp_Save}', '{ID_Emp_Save}', '{Job_No_Save}', '{Item_Code_Save}', '{Formula_Save}', '{Chemical_Save}', '{Weight_STD_Save}', '{Weight_Save}', '{Location_Save}', '{Status_Save}', '{null}', Getdate(), 'false')";

            //            if (ExecuteSqlTransaction(cmd, Local_Conn, "ADD"))
            //            {
            //                if (Location_Save == "A")
            //                {
            //                    Thread.Sleep(mydelay);
            //                    ModbusClient.WriteSingleRegister(22, 1); // Location A

            //                    //FonctiontimeStock();

            //                    //timerStock.Enabled = true;
            //                }
            //                else if (Location_Save == "B")
            //                {
            //                    Thread.Sleep(mydelay);
            //                    ModbusClient.WriteSingleRegister(23, 1); // Location B

            //                    //timerStock.Enabled = true;
            //                    //FonctiontimeStock();


            //                }
            //                else if (Location_Save == "C")
            //                {
            //                    Thread.Sleep(mydelay);
            //                    ModbusClient.WriteSingleRegister(24, 1); // Location C

            //                    //timerStock.Enabled = true;
            //                    //FonctiontimeStock();

            //                }
            //                i++;
            //            }
            //            else
            //            {
            //                MessageBox.Show("ไม่สามารถบันทึกได้, กรุณาตรวจสอบอีกครั้ง.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //                return;
            //            }
            //        }
            //        MessageBox.Show("บันทึกสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        Properties.Settings.Default.MonitorStatus = "1";
            //        Properties.Settings.Default.Save();
            //        //Cleartext();
            //    }
            //}
            //catch (Exception err)
            //{
            //    _ = new LogWriter($" Error FormWeight Message: {0}, {err.Message}");
            //    textBoxAlarm.Text = ($"   Message: {err.Message}");
            //}
        }
        #endregion

        #region "Button Reset"
        private void ButtonReset_Click(object sender, EventArgs e)
        {
            const string message = " คุณแน่ใจหรือไม่ ว่าต้องการตั้งค่าเป็นเริ่มต้น? ";
            const string caption = "Reset Job for weighing";
            var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Function_Tanks_off();
                
                Create_table();
                textBoxJOBNumber.Enabled = false;
                textBoxItemcode.Enabled = false;
                textBoxFormula.Enabled = false;
                textBoxLeadPowder.Enabled = false;
                rjButton1.BackColor = Color.WhiteSmoke;
                rjButton1.ForeColor = Color.Black;
                rjButton2.BackColor = Color.WhiteSmoke;
                rjButton2.ForeColor = Color.Black;
                rjButton3.BackColor = Color.WhiteSmoke;
                rjButton3.ForeColor = Color.Black;
                Row_weight = 0;
                Weightscale = 0;
                Radio_Formula = 0;
                Properties.Settings.Default.MonitorChemical = "";
                Properties.Settings.Default.MonitorFormula = "";
                Properties.Settings.Default.MonitorStatus = "";
                Properties.Settings.Default.MonitorSTD = 0;
                Properties.Settings.Default.MonitorWeight = 0;
                Properties.Settings.Default.MonitorSetPoint = 0;
                Properties.Settings.Default.Save();
                textBoxAlarm.Text = "";
                textBoxFormula.Text = "";
                textBoxItemcode.Text = "";
                textBoxJOBNumber.Text = "";
                textBoxLeadPowder.Text = "";
                labelActualWeight.Text = "00.00";
                labelWeighed.Text = "00.00";
                labelLocation.Text = "-";
                ButtonNextweighing.Hide();
                ButtonConfirm.Hide();
                Function_CheckLocation_PLC();
                Thread.Sleep(mydelay);
                ModbusClient.WriteSingleRegister(21, 0); // Status = false
                Location_PLC_A = 0;
                Location_PLC_B = 0;
                Location_PLC_C = 0;
                //ModbusClient.WriteSingleRegister(40, 1); // Status = Reset
            }
        }
        #endregion

        #region "Timer Read Weight scale"
        private void timerWeightScale_Tick(object sender, EventArgs e)
        {
            try
            {
                timerWeightScale.Enabled = false;
                int[] D = ModbusClient.ReadHoldingRegisters(0, 100);
                int ReadStatus = Convert.ToInt32(D[21]);
                int ReadWeightMonitor = D[27];
                Weightscale = Convert.ToDouble(ReadWeightMonitor);
                double result = Weightscale / 1000;
                labelWeighed.Text = result.ToString();
                Properties.Settings.Default.MonitorWeight = result;
                Properties.Settings.Default.Save();
                if (ReadStatus == 1)
                {
                    //Weightscale = 0;
                    int ReadWeight = D[27];
                    //labelWeighed.Text = ReadWeight.ToString();
                    if (ReadWeight > 0)
                    {
                        Weightscale = Convert.ToDouble(ReadWeight);
                        ReadWeight = 0;
                        Properties.Settings.Default.MonitorStatus = "0";
                        Properties.Settings.Default.Save();
                        Function_Tanks_Open();
                    }
                    else
                    {
                        timerWeightScale.Enabled = true;
                    }
                }
                timerWeightScale.Enabled = true;
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormWeight Message: {0}, {err.Message}");
                textBoxAlarm.Text = ($"   Message: {err.Message}");
            }
        }



        #endregion

        private void FonctiontimeStock()
        {
            //try
            //{
            //    timerStock.Enabled = false;
            //    int[] D = ModbusClient.ReadHoldingRegisters(0, 100);
            //    int ReadLocationA = Convert.ToInt32(D[22]);
            //    Thread.Sleep(mydelay);
            //    int ReadLocationB = Convert.ToInt32(D[23]);
            //    Thread.Sleep(mydelay);
            //    int ReadLocationC = Convert.ToInt32(D[24]);
            //    Thread.Sleep(mydelay);
            //    int ReadStockSensorA = Convert.ToInt32(D[34]);
            //    Thread.Sleep(mydelay);
            //    int ReadStockSensorB = Convert.ToInt32(D[35]);
            //    Thread.Sleep(mydelay);
            //    int ReadStockSensorC = Convert.ToInt32(D[36]);
            //    if (ReadLocationA == ReadStockSensorA)
            //    {
            //        ModbusClient.WriteSingleRegister(22, 0);
            //    }
            //    else if (ReadLocationB == ReadStockSensorB)
            //    {
            //        ModbusClient.WriteSingleRegister(23, 0);

            //    }
            //    else if (ReadLocationC == ReadStockSensorC)
            //    {
            //        ModbusClient.WriteSingleRegister(24, 0);
            //    }
            //}
            //catch (Exception err)
            //{
            //    _ = new LogWriter($" Error FormWeight Message: {0}, {err.Message}");
            //}
        }

        private void timerStock_Tick(object sender, EventArgs e)
        {
            try
            {
                timerStock.Enabled = false;
                int[] D = ModbusClient.ReadHoldingRegisters(0, 100);
                int ReadLocationA = Convert.ToInt32(D[22]);
                Thread.Sleep(mydelay);
                int ReadLocationB = Convert.ToInt32(D[23]);
                Thread.Sleep(mydelay);
                int ReadLocationC = Convert.ToInt32(D[24]);
                Thread.Sleep(mydelay);
                int ReadStockSensorA = Convert.ToInt32(D[34]);
                Thread.Sleep(mydelay);
                int ReadStockSensorB = Convert.ToInt32(D[35]);
                Thread.Sleep(mydelay);
                int ReadStockSensorC = Convert.ToInt32(D[36]);
                if (ReadLocationA == ReadStockSensorA)
                {
                    ModbusClient.WriteSingleRegister(22, 0);
                }
                if (ReadLocationB == ReadStockSensorB)
                {
                    ModbusClient.WriteSingleRegister(23, 0);

                }
                if (ReadLocationC == ReadStockSensorC)
                {
                    ModbusClient.WriteSingleRegister(24, 0);
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormWeight Message: {0}, {err.Message}");
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            string text = label5.Text;
            if (text.StartsWith("W"))
            {
                label5.Text = "การชั่งน้ำหนัก";
            }
            else
            {
                label5.Text = "Weighing";
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {
            string text = label13.Text;
            if (text.StartsWith("S"))
            {
                label13.Text = "กรุณา สแกนเลขงานสำหรับการชั่งน้ำหนัก";
            }
            else
            {
                label13.Text = "Scan JOB Number for weighing.";
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {
            string text = label8.Text;
            if (text.StartsWith("I"))
            {
                label8.Text = "ข้อมูลสูตรสารเคมี";
            }
            else
            {
                label8.Text = "Information";
            }
        }


    }
}
