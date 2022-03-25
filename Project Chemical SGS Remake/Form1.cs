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
using System.IO.Ports;

namespace Project_Chemical_SGS_Remake
{
    public partial class Form1 : Form
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

        //ChildForm
        private Form ActiveForm;

        //Transaction
        public static bool status_sql;

        //Location Config file temp
        public static string Location_File_Tmp;

        //Serial Port
        string Serial_DataIn;

        public static int A = 1;


        public static string ID_Emp;
        public static string Name_Emp;
        public static string Lastname_Emp;


        public static SerialPort mySerialPort;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            groupBoxKeyboard.Hide();
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file.txt");
            Local_Conn = $"Data Source={Ip_Addr_Local};Initial Catalog={Catalog_Local};User ID={Sql_usr_Local};password={Sql_pw_Local}";
            GetSerialPort();
            labelKeyboard.Hide();


            labelRegisterNow.Hide();
            label3.Hide();

        }

        //Scanner
        #region "SerialPort GetPort"
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
                //MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                _ = new LogWriter($" Error FormFillChemical Message: {0}, {err.Message}");
                //MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region "SerialPort Show Data"
        private void ShowData(object sender, EventArgs e)
        {
            try
            {
                string[] input = Serial_DataIn.Split(' ');
                textBoxID.Text = input[0];
            }
            catch (Exception err)
            {
                //MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _ = new LogWriter($" Error FormFillChemical Message: {0}, {err.Message}");
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

        #region "Function OpenChildForm"
        private void OpenChildForm(System.Windows.Forms.Form ChildForm)
        {
            try
            {
                if (ActiveForm != null)
                {
                    ActiveForm.Close();
                }
                ActiveForm = ChildForm;
                ChildForm.TopLevel = false;
                ChildForm.FormBorderStyle = FormBorderStyle.None;
                ChildForm.Dock = DockStyle.Fill;
                panelChildform.Controls.Add(ChildForm);
                ChildForm.BringToFront();
                ChildForm.Show();
            }
            catch (Exception err)
            {
                //MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _ = new LogWriter("Application can't login System_Local.txt, login lost");
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

        #region "Function Call Button Number KeyPress"
        private void CALLBUTTON_NUM(Button BTN)
        {
            textBoxID.Text = textBoxID.Text + BTN.Text;
        }
        #endregion

        #region "Button Number KeyPress 0 to 9 and Delete"
        private void ButtonNum1_Click(object sender, EventArgs e)
        {
            CALLBUTTON_NUM(ButtonNum1);
        }

        private void ButtonNum2_Click(object sender, EventArgs e)
        {
            CALLBUTTON_NUM(ButtonNum2);
        }

        private void ButtonNum3_Click(object sender, EventArgs e)
        {
            CALLBUTTON_NUM(ButtonNum3);
        }
        private void ButtonNum4_Click(object sender, EventArgs e)
        {
            CALLBUTTON_NUM(ButtonNum4);
        }
        private void ButtonNum5_Click(object sender, EventArgs e)
        {
            CALLBUTTON_NUM(ButtonNum5);
        }

        private void ButtonNum6_Click(object sender, EventArgs e)
        {
            CALLBUTTON_NUM(ButtonNum6);
        }

        private void ButtonNum7_Click(object sender, EventArgs e)
        {
            CALLBUTTON_NUM(ButtonNum7);
        }

        private void ButtonNum8_Click(object sender, EventArgs e)
        {
            CALLBUTTON_NUM(ButtonNum8);
        }

        private void ButtonNum9_Click(object sender, EventArgs e)
        {
            CALLBUTTON_NUM(ButtonNum9);
        }
        private void ButtonNum0_Click(object sender, EventArgs e)
        {
            CALLBUTTON_NUM(ButtonNum0);
        }

        private void ButtonNumDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxID.Text == "" || textBoxID.Text == null)
                {
                    textBoxID.Text = textBoxID.Text.Substring(0, textBoxID.Text.Length - 1 + 1);
                }
                else
                {
                    textBoxID.Text = textBoxID.Text.Substring(0, textBoxID.Text.Length - 1);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _ = new LogWriter("Application can't login System_Local.txt, login lost");
            }
        }
        #endregion

        #region "Button Login"
        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxID.Text == "" || textBoxID.Text == null || textBoxID.Text == "ENTER ID")
                {
                    MessageBox.Show("กรุณาใส่ข้อมูลในช่องว่างให้ครบ เพื่อดำเนินการในขั้นตอนต่อไป", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxID.Text = "ENTER ID";
                    groupBoxKeyboard.Hide();
                    return;
                }
                else
                {
                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * from Chemical_Employee_Local where ID_Employee = '{textBoxID.Text}' And Working = 1";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    int Count_row = dt.Rows.Count;
                    if (Count_row == 0)
                    {
                        MessageBox.Show(" ID : " + textBoxID.Text + " ไม่พบข้อมูลในระบบ");
                        textBoxID.Text = "ENTER ID";
                        groupBoxKeyboard.Hide();
                        return;
                    }
                    else
                    {
                        ID_Emp = dt.Rows[0]["ID_Employee"].ToString();
                        Name_Emp = dt.Rows[0]["Firstname_Employee"].ToString();
                        Lastname_Emp = dt.Rows[0]["Lastname_Employee"].ToString();
                        CloseSerialPort();
                        Properties.Settings.Default.ID_Emplyee = Convert.ToInt32(ID_Emp);
                        Properties.Settings.Default.Name_Emplyee = Name_Emp;
                        Properties.Settings.Default.Lastname_Emplyee = Lastname_Emp;
                        Properties.Settings.Default.Check_SerialPort = false;
                        Properties.Settings.Default.StatusScan = false;
                        Properties.Settings.Default.Save();
                        timerScan.Enabled = true;
                        OpenChildForm(new FormMain());
                        groupBoxKeyboard.Hide();
                        textBoxID.Text = "ENTER ID";
                        _ = new LogWriter("ID: " + ID_Emp + " " + Name_Emp + " " + Lastname_Emp +" Login Successful!" + " " +"DateTime: " + DateTime.Now);
                    }
                }
            }
            catch (Exception err)
            {
                textBoxID.Text = "ENTER ID";
                //MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _ = new LogWriter("Application can't login System_Local.txt, login lost");
                Environment.Exit(0);
            }
        }
        #endregion

        #region "Button Exit"
        private void ButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region "Button Keyboard"
        private void labelKeyboard_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception error)
            {
                _ = new LogWriter($"   Error FormWeiht Message: {0}, {error.Message}");
            }   
        }

        #endregion

        private void textBoxID_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxID.Text = "";
            groupBoxKeyboard.Show();
        }

        private void timerCheckSerialPort_Tick(object sender, EventArgs e)
        {
            //try
            //{
            //    timerCheckSerialPort.Enabled = false;
            //    if (Properties.Settings.Default.Check_SerialPort == true)
            //    {
            //        GetSerialPort();
            //    }
            //    timerCheckSerialPort.Enabled = true;
            //}
            //catch (Exception err)
            //{
            //    _ = new LogWriter($" Message: {0}, {err.Message}");
            //}
        }

        private void labelRegisterNow_Click(object sender, EventArgs e)
        {
            FormRegister FormRegis = new FormRegister();
            FormRegis.ShowDialog();
        }

        private void timerScan_Tick(object sender, EventArgs e)
        {
            try
            {
                timerScan.Enabled = false;

                if (Properties.Settings.Default.StatusScan == true)
                {
                    if (mySerialPort.IsOpen == false)
                    {
                        mySerialPort.Open();
                    }
                    else
                    {

                    }
                }

                timerScan.Enabled = true;
            }
            catch
            {

            }

        }
    }
}
