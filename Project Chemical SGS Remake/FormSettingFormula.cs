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
    public partial class FormSettingFormula : Form
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


        //tbl Model
        //Database
        public static string Local_Conn;
        public static string Catalog_Local;
        public static string Ip_Addr_Local;
        public static string Sql_usr_Local;
        public static string Sql_pw_Local;

        //Location Config file temp
        public static string Location_File_Tmp;
        public static bool status_sql;

        public FormSettingFormula()
        {
            InitializeComponent();
        }

        private void FormSettingFormula_Load(object sender, EventArgs e)
        {
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file.txt");
            Local_Conn = $"Data Source={Ip_Addr_Local};Initial Catalog={Catalog_Local};User ID={Sql_usr_Local};password={Sql_pw_Local}";
            ShowGrid_Item();
            comboBoxPlatetype();
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

        #region "Function Create table"
        private void Create_table()
        {
            metroGrid1.Columns.Clear();
            metroGrid1.Rows.Clear();
            metroGrid1.ColumnCount = 4;

            metroGrid1.Columns[0].HeaderText = "Item Code";
            metroGrid1.Columns[1].HeaderText = "Model";
            metroGrid1.Columns[2].HeaderText = "Plate Type";
            metroGrid1.Columns[3].HeaderText = "Formula";
        }
        #endregion

        #region "Combo box"
        private void comboBoxPlatetype()
        {
            comboBoxPlate.Items.Add("Positive");
            comboBoxPlate.Items.Add("Negative");
        }
        #endregion

        #region "Function ShowGrid ItemCode"
        private void ShowGrid_Item()
        {
            try
            {
                Create_table();
                metroGrid1.Rows.Clear();
                var dt = new DataTable();
                using (var conn = new SqlConnection(Local_Conn))
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "Select * from Chemical_Model_Local Where Working = '1'";
                    var sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    metroGrid1.Rows.Add(dr["Item_code"], dr["Model"], dr["Plate_type"], dr["Code"]);
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormSettingFormula Message: {0}, {err.Message}");
            }
        }
        #endregion

        #region "Textchang search item code"
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBoxItemCode.Text = "";
                textBoxFormula.Text = "";
                textBoxModel.Text = "";
                comboBoxPlate.Text = "";
                Create_table();
                if (textBoxSearch.TextLength >= 11)
                {
                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From Chemical_Model_Local Where Item_code = '{textBoxSearch.Text}' and Working = '1'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    int count_row = dt.Rows.Count;
                    if (count_row == 0)
                    {
                        textBoxSearch.Text = "";
                        textBoxItemCode.Text = "";
                        textBoxModel.Text = "";
                        textBoxFormula.Text = "";
                        comboBoxPlate.Text = "";
                    }
                    else
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            metroGrid1.Rows.Add(dr["Item_code"], dr["Model"], dr["Plate_type"], dr["Code"]);
                        }
                        textBoxItemCode.Text = metroGrid1.Rows[0].Cells[0].Value.ToString();
                        textBoxModel.Text = metroGrid1.Rows[0].Cells[1].Value.ToString();
                        comboBoxPlate.Text = metroGrid1.Rows[0].Cells[2].Value.ToString();
                        textBoxFormula.Text = metroGrid1.Rows[0].Cells[3].Value.ToString();
                    }
                }
                else
                {
                    Create_table();
                    ShowGrid_Item();
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormSettingFormula Message: {0}, {err.Message}");
            }
        }
        #endregion

        #region "GridView Cell Mouse Up"
        private void metroGrid1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            textBoxItemCode.Text = this.metroGrid1.CurrentRow.Cells[0].Value.ToString();
            textBoxModel.Text = this.metroGrid1.CurrentRow.Cells[1].Value.ToString();
            comboBoxPlate.Text = this.metroGrid1.CurrentRow.Cells[2].Value.ToString();
            textBoxFormula.Text = this.metroGrid1.CurrentRow.Cells[3].Value.ToString();
        }
        #endregion

        #region "Button Add"
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                const string message = " คุณแน่ใจหรือไม่ ว่าต้องการเพิ่มข้อมูลนี้ในฐานข้อมูล? ";
                const string caption = "Add Data Model to Database";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    if (textBoxItemCode.Text == "" || textBoxItemCode.Text == null || textBoxFormula.Text == "" || textBoxFormula.Text == null || textBoxModel.Text == "" || textBoxModel.Text == null)
                    {
                        MessageBox.Show(" กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        var dt = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * From Chemical_Model_Local Where Item_code = {textBoxItemCode.Text} and Working = '1'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt);
                        }
                        int count_row = dt.Rows.Count;
                        if (count_row >= 1)
                        {
                            MessageBox.Show(" มีข้อมูลซ้ำในระบบ, กรูณาตรวจสอบอีกครั้ง ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ShowGrid_Item();
                            textBoxItemCode.Text = "";
                            textBoxFormula.Text = "";
                            comboBoxPlate.Text = "";
                            textBoxModel.Text = "";
                        }
                        else
                        {
                            string Item_code_add = textBoxItemCode.Text;
                            string Model_add = textBoxModel.Text;
                            string PlateType_add = comboBoxPlate.Text;
                            string Formula_add = textBoxFormula.Text;
                            var cmd = $"Insert into Chemical_Model_Local (Item_code, Model, Plate_type, Code, Status_Update, Working) " +
                                $"Values ('{Item_code_add}', '{Model_add}', '{PlateType_add}', '{Formula_add}', 'false', '1')";
                            if (ExecuteSqlTransaction(cmd, Local_Conn, "ADD"))
                            {
                                MessageBox.Show(" เพิ่มข้อมูลสำเร็จ ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ShowGrid_Item();
                                textBoxItemCode.Text = "";
                                textBoxFormula.Text = "";
                                comboBoxPlate.Text = "";
                                textBoxModel.Text = "";
                            }
                            else
                            {
                                MessageBox.Show(" ไม่สามารถเพิ่มข้อมูลได้, กรุณาตรวจสอบอีกครั้ง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormSettingFormula Message: {0}, {err.Message}");
            }
        }
        #endregion

        #region "Button Update"
        private void ButtonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                const string message = "คุณแน่ใจหรือไม่ ว่าต้องการอัปเดตข้อมูลนี้ไปยังฐานข้อมูล?";
                const string caption = "Update Data Model to Database";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    if (textBoxItemCode.Text == "" || textBoxItemCode.Text == null || textBoxFormula.Text == "" || textBoxFormula.Text == null || textBoxModel.Text == "" || textBoxModel.Text == null)
                    {
                        MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        var dt = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * From Chemical_Model_Local Where Item_code = {textBoxItemCode.Text} and Working = '1'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt);
                        }
                        int count_check = dt.Rows.Count;
                        if (count_check >= 0)
                        {
                            MessageBox.Show("ไม่มีข้อมูลในระบบ ไม่สามารถแก้ไขข้อมูลได้ กรุณาตรวจสอบอีกครั้ง", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ShowGrid_Item();
                            textBoxItemCode.Text = "";
                            textBoxFormula.Text = "";
                            comboBoxPlate.Text = "";
                            textBoxModel.Text = "";
                        }
                        else
                        {
                            var cmd = $"Update Chemical_Model_Local " +
                                      $"Set Model = '{textBoxModel.Text}', Plate_type = '{comboBoxPlate.Text}', Code = '{textBoxFormula.Text}', Status_Update = 'false' " +
                                      $"Where Item_code = '{textBoxItemCode.Text}'";

                            if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                            {
                                MessageBox.Show("บันทึกข้อมูลสำเร็จ");
                                ShowGrid_Item();
                                textBoxItemCode.Text = "";
                                textBoxFormula.Text = "";
                                comboBoxPlate.Text = "";
                                textBoxModel.Text = "";
                            }
                        }

                    }
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormSettingFormula Message: {0}, {err.Message}");
            }
        }
        #endregion

        #region "Button Reset"
        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            textBoxSearch.Text = "";
            textBoxFormula.Text = "";
            textBoxItemCode.Text = "";
            textBoxModel.Text = "";
            comboBoxPlate.Text = "";
            ShowGrid_Item();
        }

        #endregion

        #region " Button Delete"
        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                const string message =
        "คุณแน่ใจหรือไม่ ว่าต้องการลบข้อมูลนี้ในฐานข้อมูล?";
                const string caption = "Delete model from database";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    if (textBoxItemCode.Text == "" || textBoxItemCode.Text == null || textBoxFormula.Text == "" || textBoxFormula.Text == null || textBoxModel.Text == "" || textBoxModel.Text == null || comboBoxPlate.Text == "" || comboBoxPlate.Text == null)
                    {
                        MessageBox.Show("Please select empolyee before next operation.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        //var cmd = $"Delete from Chemical_Employee_Local Where ID_Employee = '{textBoxIDemp.Text}'";
                        var cmd = $"Update Chemical_Employee_Local " +
                            $"Set Working = '0' " +
                            $"Where ID_Employee = '{textBoxItemCode.Text}'";
                        if (ExecuteSqlTransaction(cmd, Local_Conn, "Delete"))
                        {
                            MessageBox.Show("ลบข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ShowGrid_Item();
                            this.ActiveControl = metroGrid1;
                            textBoxItemCode.Text = "";
                            textBoxFormula.Text = "";
                            textBoxModel.Text = "";
                            comboBoxPlate.Text = "";
                        }
                    }
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormSettingFormula Message: {0}, {err.Message}");
            }
        }
        #endregion

        #region "Button Ketboard"
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
