using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.FileIO;
using System.Data.SqlClient;
using excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;

namespace Project_Chemical_SGS_Remake
{
    public partial class FormReport : Form
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

        //Location Config file temp
        public static string Location_File_Tmp;

        //Login
        string ID_Emp_Main = Properties.Settings.Default.ID_Emplyee.ToString();
        string Name_Emp_Main = Properties.Settings.Default.Name_Emplyee.ToString();
        string Lastname_Emp_Main = Properties.Settings.Default.Lastname_Emplyee.ToString();

        public FormReport()
        {
            InitializeComponent();
        }

        private void FormReport_Load(object sender, EventArgs e)
        {
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file.txt");
            Local_Conn = $"Data Source={Ip_Addr_Local};Initial Catalog={Catalog_Local};User ID={Sql_usr_Local};password={Sql_pw_Local}";
            InitializeDataGridView();
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

        #region "InitializeDataGridView"
        private void InitializeDataGridView()
        {
            metroGridReport.BorderStyle = BorderStyle.Fixed3D;
            metroGridReport.AllowUserToAddRows = false;
            metroGridReport.AllowUserToDeleteRows = false;
            metroGridReport.AllowUserToOrderColumns = true;
            metroGridReport.ReadOnly = true;
            metroGridReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            metroGridReport.MultiSelect = false;
            metroGridReport.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            metroGridReport.AllowUserToResizeColumns = false;
            metroGridReport.AllowUserToResizeRows = false;
            metroGridReport.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            metroGridReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        #endregion

        #region "Show DataGrid"
        private void ShowGrid()
        {
            try
            {
                var dt = new DataTable();
                using (var conn = new SqlConnection(Local_Conn))
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "Select * from tbl_Record_Chemical";
                    var sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                }
                metroGridReport.DataSource = dt;
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormReport Message: {0}, {err.Message}");
            }
            
        }
        #endregion

        #region "Button Back"
        private void buttonBackMain_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region "Button Submit Query DatePicker1 and DatePicker2"
        private void ButtonSubmit_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(Local_Conn))
            {
                var cmd = conn.CreateCommand();
                var sda = new SqlDataAdapter("Select * from tbl_Record_Chemical where DateTime between '"+DateTimePickerStart.Value.ToString()+"' and '"+DateTimePickerEnd.Value.ToString()+"'", conn);
                var dt = new DataTable();
                sda.Fill(dt);
                metroGridReport.DataSource = dt;
            }
        }
        #endregion

        #region "Button Clear"
        private void ButtonClear_Click(object sender, EventArgs e)
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(Local_Conn))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "Select * From tbl_Record_Chemical";
                var sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            metroGridReport.DataSource = dt;
        }
        #endregion

        #region "Show Keyborad"
        public void Show_Keyborad()
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                excel.Application app = new excel.Application();
                excel.Workbook workbook = app.Workbooks.Add();
                excel.Worksheet worksheet = null;
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                for (int i = 0; i < metroGridReport.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = metroGridReport.Columns[i].HeaderText;
                }
                for (int j = 0; j < metroGridReport.Rows.Count - 1; j++)
                {
                    for (int i = 0; i < metroGridReport.Columns.Count; i++)
                    {
                        worksheet.Cells[j + 2, i + 1] = metroGridReport.Rows[j].Cells[i].Value.ToString();
                    }
                }
                worksheet.Columns.AutoFit();
                var saveFileDialoge = new SaveFileDialog();
                saveFileDialoge.FileName = "Chemical_Report" + "_" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                saveFileDialoge.Filter = "CSV|*.csv";
                if (saveFileDialoge.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(saveFileDialoge.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    MessageBox.Show("Export is Successful");
                    _ = new LogWriter("Exportfile : " + DateTime.Now);
                    workbook.Close();
                    app.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                }
                else
                {
                    return;
                }
                
            }
            catch (Exception error)
            {
                _ = new LogWriter($"   Export is fail: ");
                _ = new LogWriter($"   Message: {0}, {error.Message}");
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
