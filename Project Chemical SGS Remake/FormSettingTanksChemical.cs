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
    public partial class FormSettingTanksChemical : Form
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
        //Database
        public static string Local_Conn;
        public static string Catalog_Local;
        public static string Ip_Addr_Local;
        public static string Sql_usr_Local;
        public static string Sql_pw_Local;

        //Location Config file temp
        public static string Location_File_Tmp;
        public static bool status_sql;


        public FormSettingTanksChemical()
        {
            InitializeComponent();
        }

        private void FormSettingTanksChemical_Load(object sender, EventArgs e)
        {
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file.txt");
            Local_Conn = $"Data Source={Ip_Addr_Local};Initial Catalog={Catalog_Local};User ID={Sql_usr_Local};password={Sql_pw_Local}";
            InitializeDataGridView();
            Create_table();
            ShowGrid();
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

            metroGrid1.Columns[0].HeaderText = "Tanks Number";
            metroGrid1.Columns[1].HeaderText = "Chemical Name";
            metroGrid1.Columns[2].HeaderText = "Weight Min";
            metroGrid1.Columns[3].HeaderText = "Weight Max";
        }
        #endregion

        #region "Function ShowGrid"
        private void ShowGrid()
        {
            try
            {
                metroGrid1.Rows.Clear();
                var dt = new DataTable();
                using (var conn = new SqlConnection(Local_Conn))
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "Select * from Chemical_Address_PLC_Local Where Working = '1'";
                    var sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    metroGrid1.Rows.Add(dr["Tanks_Number"], dr["Chemical"], dr["Weight_Min"], dr["Weight_Max"]);
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormSettingTanksChemical Message: {0}, {err.Message}");
            }
        }
        #endregion

        #region "Button Refresh"
        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            textBoxTanksNumber.Text = "";
            textBoxChemicalName.Text = "";
            textBoxWeightMin.Text = "";
            textBoxWeightMax.Text = "";
            ShowGrid();
        }

        #endregion

        #region "Button Update"
        private void ButtonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                const string message =
        " คุณแน่ใจหรือไม่ ว่าต้องการอัปเดตข้อมูลนี้ไปยังฐานข้อมูล? ";
                const string caption = "Update Data Model to Database";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    if (textBoxTanksNumber.Text == "" || textBoxTanksNumber.Text == null || textBoxChemicalName.Text == "" || textBoxChemicalName.Text == null || textBoxWeightMin.Text == "" || textBoxWeightMin.Text == null || textBoxWeightMax.Text == "" || textBoxWeightMax.Text == null)
                    {
                        MessageBox.Show(" กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        string Chemical_Name = textBoxChemicalName.Text;
                        string Weight_Min = textBoxWeightMin.Text;
                        string Weight_Max = textBoxWeightMax.Text;
                        var cmd = $"Update Chemical_Address_PLC_Local " +
                            $"Set Chemical = '{Chemical_Name}', Weight_Min = '{Weight_Min}', Weight_Max = '{Weight_Max}' " +
                            $"Where Tanks_Number = '{textBoxTanksNumber.Text}'";
                        if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                        {
                            MessageBox.Show(" บันทึกข้อมูลสำเร็จ ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ShowGrid();
                            textBoxTanksNumber.Text = "";
                            textBoxChemicalName.Text = "";
                        }
                    }
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormSettingTanksChemical Message: {0}, {err.Message}");
            }
            
        }
        #endregion

        #region "Gridview Cell MousesUp"
        private void metroGrid1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            textBoxTanksNumber.Text = this.metroGrid1.CurrentRow.Cells[0].Value.ToString();
            textBoxChemicalName.Text = this.metroGrid1.CurrentRow.Cells[1].Value.ToString();
            textBoxWeightMin.Text = this.metroGrid1.CurrentRow.Cells[2].Value.ToString();
            textBoxWeightMax.Text = this.metroGrid1.CurrentRow.Cells[3].Value.ToString();
        }
        #endregion

        #region "Button Keyboard"
        private void labelKeyboard_Click(object sender, EventArgs e)
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
    }
}
