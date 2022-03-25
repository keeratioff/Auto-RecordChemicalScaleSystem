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
    public partial class FormSettingAccessibility : Form
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


        // Permission
        string Permission;

        //Database
        public static string Local_Conn;
        public static string Catalog_Local;
        public static string Ip_Addr_Local;
        public static string Sql_usr_Local;
        public static string Sql_pw_Local;

        //Location Config file temp
        public static string Location_File_Tmp;
        public static bool status_sql;

        public FormSettingAccessibility()
        {
            InitializeComponent();
            metroGrid1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void FormSettingAccessibility_Load(object sender, EventArgs e)
        {
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file.txt");
            Local_Conn = $"Data Source={Ip_Addr_Local};Initial Catalog={Catalog_Local};User ID={Sql_usr_Local};password={Sql_pw_Local}";
            Create_table();
            ShowGrid();
        }

        #region "Radio Select"
        private void radioButtonAdmin_CheckedChanged(object sender, EventArgs e)
        {
            Permission = "Admin";
        }

        private void radioButtonUser_CheckedChanged(object sender, EventArgs e)
        {
            Permission = "User";
        }
        #endregion

        #region "Function Create table"
        private void Create_table()
        {
            metroGrid1.Columns.Clear();
            metroGrid1.Rows.Clear();
            metroGrid1.ColumnCount = 4;

            metroGrid1.Columns[0].HeaderText = "ID Employee";
            metroGrid1.Columns[1].HeaderText = "First Name";
            metroGrid1.Columns[2].HeaderText = "Last Name";
            metroGrid1.Columns[3].HeaderText = "Permission";
        }
        #endregion

        #region "Function ShowGrid"
        private void ShowGrid()
        {
            metroGrid1.Rows.Clear();
            var dt = new DataTable();
            using (var conn = new SqlConnection(Local_Conn))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "Select * from Chemical_Employee_Local Where Working = '1'";
                var sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            foreach (DataRow dr in dt.Rows)
            {
                metroGrid1.Rows.Add(dr["ID_Employee"], dr["Firstname_Employee"], dr["Lastname_Employee"], dr["Permission"]);
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

        #region "Button Add"
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                const string message =
        "คุณแน่ใจหรือไม่ ว่าต้องการเพิ่มโมเดลนี้ในฐานข้อมูล?";
                const string caption = "Add Model to Database";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (textBoxIDemp.Text == "" || textBoxIDemp.Text == null || textBoxFirstname.Text == "" || textBoxFirstname.Text == null || textBoxLastname.Text == "" || textBoxLastname.Text == null || radioButtonAdmin.Checked == false && radioButtonUser.Checked == false)
                    {
                        MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        string ID_Emp = textBoxIDemp.Text;
                        string FirstName_Emp = textBoxFirstname.Text;
                        string Lastname_Emp = textBoxLastname.Text;

                        var dt = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * From Chemical_Employee_Local Where ID_Employee = '{textBoxIDemp.Text}' and Working = '1'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt);
                        }
                        int Count_row = dt.Rows.Count;
                        if (Count_row >= 1)
                        {
                            MessageBox.Show("พบ ID พนักงานเหมืยนกันกับในฐานข้อมูล โปรดอัปเดตหรือใช้ ID ใหม่เพื่อเพิ่ม", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ShowGrid();
                            this.ActiveControl = metroGrid1;
                        }
                        else
                        {
                            var cmd = $"Insert into Chemical_Employee_Local (ID_Employee, Firstname_Employee, Lastname_Employee, Permission, Status_Update, Working) " +
                                $"Values ('{ID_Emp}', '{FirstName_Emp}', '{Lastname_Emp}', '{Permission}', 'false', '1')";
                            if (ExecuteSqlTransaction(cmd, Local_Conn, "Add"))
                            {
                                MessageBox.Show("เพิ่มข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ShowGrid();
                                this.ActiveControl = metroGrid1;
                                textBoxIDemp.Text = "";
                                textBoxFirstname.Text = "";
                                textBoxLastname.Text = "";
                                radioButtonAdmin.Checked = false;
                                radioButtonUser.Checked = false;
                            }
                            else
                            {
                                MessageBox.Show(" ไม่สามารถเพิ่ม ID พนักงานได้, กรุณาลองใหม่อีกครั้ง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormSettingAccessibility Message: {0}, {err.Message}");
            }
        }
        #endregion

        #region "Button Update"
        private void ButtonUpdate_Click(object sender, EventArgs e)
        {
            const string message =
        "คุณแน่ใจหรือไม่ ว่าต้องการอัปเดตข้อมูลนี้ไปยังฐานข้อมูล?";
            const string caption = "Update Data Model to Database";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                if (textBoxIDemp.Text == "" || textBoxIDemp.Text == null || textBoxFirstname.Text == "" || textBoxFirstname.Text == null || textBoxLastname.Text == "" || textBoxLastname.Text == null || radioButtonAdmin.Checked == false && radioButtonUser.Checked == false)
                {
                    MessageBox.Show(" กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string ID_Emp = textBoxIDemp.Text;
                    string FirstName_Emp = textBoxFirstname.Text;
                    string Lastname_Emp = textBoxLastname.Text;

                    var cmd = $"Update Chemical_Employee_Local " +
                        $"Set Firstname_Employee = '{FirstName_Emp}', Lastname_Employee = '{Lastname_Emp}', Permission = '{Permission}', Status_Update = 'false' " +
                        $"Where ID_Employee = '{textBoxIDemp.Text}'";
                    if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                    {
                        MessageBox.Show("Update Complete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ShowGrid();
                        textBoxIDemp.Text = "";
                        textBoxFirstname.Text = "";
                        textBoxLastname.Text = "";
                        radioButtonAdmin.Checked = false;
                        radioButtonUser.Checked = false;
                    }
                }
            }
        }
        #endregion

        #region "Button Delete"
        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                const string message =
        "Are you sure that you would like to delete of this model?";
                const string caption = "Delete model from database";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    if (textBoxIDemp.Text == "" || textBoxIDemp.Text == null || textBoxFirstname.Text == "" || textBoxFirstname.Text == null || textBoxLastname.Text == "" || textBoxLastname.Text == null)
                    {
                        MessageBox.Show(" กรุณาเลือกชื่อพนักงานก่อนเริ่มกระบวนการต่อไป", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        var cmd = $"Update Chemical_Employee_Local " +
                            $"Set Working = '0' " +
                            $"Where ID_Employee = '{textBoxIDemp.Text}'";
                        if (ExecuteSqlTransaction(cmd, Local_Conn, "Delete"))
                        {
                            MessageBox.Show("ลบข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ShowGrid();
                            this.ActiveControl = metroGrid1;
                            textBoxIDemp.Text = "";
                            textBoxFirstname.Text = "";
                            textBoxLastname.Text = "";
                            radioButtonAdmin.Checked = false;
                            radioButtonUser.Checked = false;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormSettingAccessibility Message: {0}, {err.Message}");
            }
        }
        #endregion

        #region "Button Refresh"
        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            textBoxIDemp.Text = "";
            textBoxFirstname.Text = "";
            textBoxLastname.Text = "";
            radioButtonAdmin.Checked = false;
            radioButtonUser.Checked = false;
            Permission = "";
            ShowGrid();
        }
        #endregion

        #region "Gridview Cell MousesUp"
        private void metroGrid1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            textBoxIDemp.Text = this.metroGrid1.CurrentRow.Cells[0].Value.ToString();
            textBoxFirstname.Text = this.metroGrid1.CurrentRow.Cells[1].Value.ToString();
            textBoxLastname.Text = this.metroGrid1.CurrentRow.Cells[2].Value.ToString();
            string PermissionCheck = this.metroGrid1.CurrentRow.Cells[3].Value.ToString();
            if (PermissionCheck == "Admin")
            {
                radioButtonAdmin.Checked = true;
            }
            if (PermissionCheck == "User")
            {
                radioButtonUser.Checked = true;
            }
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
