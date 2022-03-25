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
    public partial class FormSettingFormula1 : Form
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
        //tbl formula code
        //Database
        public static string Local_Conn;
        public static string Catalog_Local;
        public static string Ip_Addr_Local;
        public static string Sql_usr_Local;
        public static string Sql_pw_Local;

        //Location Config file temp
        public static string Location_File_Tmp;
        public static bool status_sql;



        public FormSettingFormula1()
        {
            InitializeComponent();
        }

        private void FormSettingFormula1_Load(object sender, EventArgs e)
        {
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file.txt");
            Local_Conn = $"Data Source={Ip_Addr_Local};Initial Catalog={Catalog_Local};User ID={Sql_usr_Local};password={Sql_pw_Local}";
            ShowGrid_Item();
        }

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

        #region "Function Create table"
        private void Create_table()
        {
            metroGrid1.Columns.Clear();
            metroGrid1.Rows.Clear();
            metroGrid1.ColumnCount = 6;

            metroGrid1.Columns[0].HeaderText = "Code";
            metroGrid1.Columns[1].HeaderText = "Formula";
            metroGrid1.Columns[2].HeaderText = "Chemical";
            metroGrid1.Columns[3].HeaderText = "Weight";
            metroGrid1.Columns[4].HeaderText = "Tolerance";
            metroGrid1.Columns[5].HeaderText = "Remark";

        }
        #endregion

        #region "Function ShowGrid ItemCode"
        private void ShowGrid_Item()
        {
            metroGrid1.Rows.Clear();
            Create_table();
            
            var dt = new DataTable();
            using (var conn = new SqlConnection(Local_Conn))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "Select * from Chemical_CodeFormula_Local Where Working = '1'";
                var sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            foreach (DataRow dr in dt.Rows)
            {
                metroGrid1.Rows.Add(dr["Code"], dr["Formula"], dr["Chemical"], dr["Weight"], dr["Set_point"], dr["Remark"]);
            }
        }
        #endregion

        #region "Button Add"
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                const string message = "คุณแน่ใจหรือไม่ ว่าต้องการเพิ่มข้อมูลนี้ในฐานข้อมูล?";
                const string caption = "Update Data Model to Database";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    if (textBoxCode.Text == "" || textBoxCode.Text == null || textBoxChemical.Text == "" || textBoxChemical.Text == null || textBoxWeight.Text == "" || textBoxWeight.Text == null || textBoxSetpoint.Text == "" || textBoxSetpoint.Text == "" || textBoxRemark.Text == "" || textBoxRemark.Text == null || comboBox1.Text == "" || comboBox1.Text == null)
                    {
                        MessageBox.Show(" กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        var dt = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Code = '{textBoxCode.Text}' And Working = '1'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt);
                        }
                        int count_row = dt.Rows.Count;
                        if (count_row >= 1)
                        {
                            MessageBox.Show("ตรวจพบข้อมูลมีซ้ำในระบบ ไม่สามารถเพิ่มได้ กรุณารวจสอบอีกครั้ง", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ShowGrid_Item();
                            textBoxCode.Text = "";
                            textBoxChemical.Text = "";
                            comboBox1.Text = "";
                            textBoxWeight.Text = "";
                            textBoxSetpoint.Text = "";
                            textBoxRemark.Text = "";
                        }
                        else
                        {
                            string Code_add = textBoxCode.Text;
                            int Formula_add = Convert.ToInt32(comboBox1.Text);
                            string Chemical_add = textBoxChemical.Text;
                            double Weight_STD_add = Convert.ToDouble(textBoxWeight.Text);
                            double Set_point_add = Convert.ToDouble(textBoxSetpoint.Text);
                            string Remark_add = textBoxRemark.Text;
                            var cmd = $"Insert into Chemical_CodeFormula_Local (Code, Formula, Chemical, Weight, Set_point, Remark, Status_Update, Working) " +
                                $"Values ('{Code_add}', '{Formula_add}', '{Chemical_add}', '{Weight_STD_add}', '{Set_point_add}', '{Remark_add}', 'false', '1')";
                            if (ExecuteSqlTransaction(cmd, Local_Conn, "ADD"))
                            {
                                MessageBox.Show("เพิ่มข้อมูลเข้าระบบสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ShowGrid_Item();
                                textBoxCode.Text = "";
                                textBoxChemical.Text = "";
                                comboBox1.Text = "";
                                textBoxWeight.Text = "";
                                textBoxSetpoint.Text = "";
                                textBoxRemark.Text = "";
                            }
                            else
                            {
                                MessageBox.Show("ไม่สามารถเพิ่มข้อมูลนี้ได้ โปรดตรวจสอบอีกครั้ง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            
                        }
                    }
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormSettingFormula1 Message: {0}, {err.Message}");
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
                    if (textBoxCode.Text == "" || textBoxCode.Text == null || textBoxChemical.Text == "" || textBoxChemical.Text == null || textBoxWeight.Text == "" || textBoxWeight.Text == null || textBoxSetpoint.Text == "" || textBoxSetpoint.Text == "" || textBoxRemark.Text == "" || textBoxRemark.Text == null || comboBox1.Text == "" || comboBox1.Text == null)
                    {
                        MessageBox.Show("กรุณาเติ่มข้อมูลในช่องว่างให้ถูกต้อง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        var dt = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Code = ''{textBoxCode.Text} And Working = '1'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt);
                        }
                        int count_row = dt.Rows.Count;
                        if (count_row >= 0)
                        {
                            MessageBox.Show("ไม่มีข้อมูลในระบบ ไม่สามารถแก้ไขข้อมูลได้ กรุณาตรวจสอบอีกครั้ง", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ShowGrid_Item();
                            textBoxCode.Text = "";
                            textBoxChemical.Text = "";
                            comboBox1.Text = "";
                            textBoxWeight.Text = "";
                            textBoxSetpoint.Text = "";
                            textBoxRemark.Text = "";
                        }
                        else
                        {
                            var cmd = $"Update Chemical_CodeFormula_Local " +
                                $"Set Chemical = '{textBoxChemical.Text}', Formula = '{comboBox1.Text}', Weight = '{textBoxWeight.Text}', Set_point = '{textBoxSetpoint.Text}', Remark = '{textBoxRemark.Text}', Status_Update = 'false', Working = '1' " +
                                $"Where Code = '{textBoxCode.Text}'";
                            if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                            {
                                MessageBox.Show("บันทึกข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ShowGrid_Item();
                                textBoxCode.Text = "";
                                textBoxChemical.Text = "";
                                comboBox1.Text = "";
                                textBoxWeight.Text = "";
                                textBoxSetpoint.Text = "";
                                textBoxRemark.Text = "";
                            }

                        }
                    }
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormSettingFormula1 Message: {0}, {err.Message}");
            }
        }
        #endregion

        #region "Button Delete"
        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                const string message = "คุณแน่ใจหรือไม่ ว่าต้องการลบข้อมูลนี้ในฐานข้อมูล?";
                const string caption = "Update Data Model to Database";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    if (textBoxCode.Text == "" || textBoxCode.Text == null || textBoxChemical.Text == "" || textBoxChemical.Text == null || comboBox1.Text == "" || comboBox1.Text == null || textBoxWeight.Text == "" || textBoxWeight.Text == null || textBoxSetpoint.Text == "" || textBoxSetpoint.Text == null || textBoxRemark.Text == "" || textBoxRemark.Text == null)
                    {
                        MessageBox.Show("กรุณาเติ่มข้อมูลในช่องว่างให้ถูกต้อง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        var cmd = $"Update Chemical_CodeFormula_Local " +
                            $"Set Working = '0' " +
                            $"Where Code = '{textBoxCode.Text}'";
                        if (ExecuteSqlTransaction(cmd, Local_Conn, "Delete"))
                        {
                            MessageBox.Show("ลบข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ShowGrid_Item();
                            textBoxCode.Text = "";
                            textBoxChemical.Text = "";
                            comboBox1.Text = "";
                            textBoxWeight.Text = "";
                            textBoxSetpoint.Text = "";
                            textBoxRemark.Text = "";
                        }
                    }
                    
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormSettingFormula1 Message: {0}, {err.Message}");
            }
        }
        #endregion

        #region "Button Reset"
        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            
            textBoxCode.Text = "";
            textBoxChemical.Text = "";
            comboBox1.Text = "";
            textBoxWeight.Text = "";
            textBoxSetpoint.Text = "";
            textBoxRemark.Text = "";
            ShowGrid_Item();
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

        #region "GridView Cell Mouse Up"
        private void metroGrid1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            textBoxCode.Text = this.metroGrid1.CurrentRow.Cells[0].Value.ToString();
            comboBox1.Text = this.metroGrid1.CurrentRow.Cells[1].Value.ToString();
            textBoxChemical.Text = this.metroGrid1.CurrentRow.Cells[2].Value.ToString();
            textBoxWeight.Text = this.metroGrid1.CurrentRow.Cells[3].Value.ToString();
            textBoxSetpoint.Text = this.metroGrid1.CurrentRow.Cells[4].Value.ToString();
            textBoxRemark.Text = this.metroGrid1.CurrentRow.Cells[5].Value.ToString();
        }
        #endregion

        private void textBoxCode_TextChanged(object sender, EventArgs e)
        {
            Create_table();
            textBoxChemical.Text = "";
            textBoxWeight.Text = "";
            textBoxSetpoint.Text = "";
            textBoxRemark.Text = "";
            if (textBoxCode.TextLength >= 1)
            {
                var dt = new DataTable();
                using (var conn = new SqlConnection(Local_Conn))
                {
                    var check = conn.CreateCommand();
                    check.CommandText = $"Select * From Chemical_CodeFormula_Local Where Code = '{textBoxCode.Text}' And Working = '1'";
                    var sda = new SqlDataAdapter(check);
                    sda.Fill(dt);
                }
                int count_row = dt.Rows.Count;
                if (count_row == 0)
                {
                    textBoxChemical.Text = "";
                    textBoxWeight.Text = "";
                    textBoxSetpoint.Text = "";
                    textBoxRemark.Text = "";
                }
                else
                {
                    //textBoxCode.Text = dt.Rows[0]["Code"].ToString();
                    //comboBox1.Text = dt.Rows[0]["Formula"].ToString();
                    //textBoxChemical.Text = dt.Rows[0]["Chemical"].ToString();
                    foreach (DataRow dr in dt.Rows)
                    {
                        metroGrid1.Rows.Add(dr["Code"], dr["Formula"], dr["Chemical"], dr["Weight"], dr["Set_point"], dr["Remark"]);
                    }
                }
            }

        }
    }
}
