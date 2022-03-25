using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
    public partial class FormFillChemical : Form
    {
        //Modbus
        ModbusClient ModbusClient = new ModbusClient();

        //Database
        public static string Local_Conn;
        public static string Catalog_Local;
        public static string Ip_Addr_Local;
        public static string Sql_usr_Local;
        public static string Sql_pw_Local;

        //Location Config file temp
        public static string Location_File_Tmp;

        //String Grid
        string GridName;
        string GridMin;
        string GridMax;

        public static bool status_sql;

        //Delay for Write PLC
        public int mydelay = 150;

        public FormFillChemical()
        {
            InitializeComponent();
        }

        private void FormFillChemical_Load(object sender, EventArgs e)
        {
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file.txt");
            Local_Conn = $"Data Source={Ip_Addr_Local};Initial Catalog={Catalog_Local};User ID={Sql_usr_Local};password={Sql_pw_Local}";
            InitializeDataGridView();
            Create_table();
            DataGrid_Show();
            Cell_Format();
            groupBox1.Hide();
            ButtonConfirm.Hide();
            textBoxFill.Enabled = false;
            textBoxNameChemical.Enabled = false;
            textBoxMaxWeight.Enabled = false;
            Modbus_Client_Port502();
        }

        #region "Function Modbus Client 502"
        private void Modbus_Client_Port502()
        {
            try
            {
                ModbusClient.Disconnect();
                ModbusClient.IPAddress = "192.168.3.250";
                ModbusClient.Port = 502;
                ModbusClient.Connect();
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormFillChemical Message: {0}, {err.Message}");
                //MessageBox.Show(err.Message, "Modbus Client Error : ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region "Function Tanks is close"
        private void Function_Tanks_off()
        {
            try
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
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormFillChemical Message: {0}, {err.Message}");
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

        #region "Button Back"
        private void buttonBackMain_Click(object sender, EventArgs e)
        {
            ModbusClient.Disconnect();
            this.Close();

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
            try
            {
                metroGrid1.Columns.Clear();
                metroGrid1.Rows.Clear();
                metroGrid1.ColumnCount = 3;

                metroGrid1.Columns[0].HeaderText = "Chemical";
                metroGrid1.Columns[1].HeaderText = "Weight_Min";
                metroGrid1.Columns[2].HeaderText = "Weight_Max";
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormFillChemical Message: {0}, {err.Message}");
            }
            
        }
        #endregion

        #region "Function Call Button Number KeyPress"
        private void CALLBUTTON_NUM(Button BTN)
        {
            textBoxFill.Text = textBoxFill.Text + BTN.Text;
        }
        #endregion

        #region "Function Call Button Number KeyPress"
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
                if (textBoxFill.Text == "" || textBoxFill.Text == null)
                {
                    textBoxFill.Text = textBoxFill.Text.Substring(0, textBoxFill.Text.Length - 1 + 1);
                }
                else
                {
                    textBoxFill.Text = textBoxFill.Text.Substring(0, textBoxFill.Text.Length - 1);
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormFillChemical Message: {0}, {err.Message}");
            }
        }
        #endregion

        #region "DataGrid Show"
        private void DataGrid_Show()
        {
            try
            {
                metroGrid1.Rows.Clear();
                var dt = new DataTable();
                using (var conn = new SqlConnection(Local_Conn))
                {
                    var check = conn.CreateCommand();
                    check.CommandText = $"Select * from Chemical_Address_PLC_Local Where Working = 1";
                    var sda = new SqlDataAdapter(check);
                    sda.Fill(dt);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    metroGrid1.Rows.Add(dr["Chemical"], dr["Weight_Min"], dr["Weight_Max"]);
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormFillChemical Message: {0}, {err.Message}");
            }
            
        }
        #endregion

        #region "MetroGrid Cell Format"
        private void metroGrid1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Cell_Format();
        }
        #endregion

        #region "Cell Format"
        private void Cell_Format()
        {
            try
            {
                foreach (DataGridViewRow row in metroGrid1.Rows)
                {
                    if (Convert.ToInt32(row.Cells[1].Value) <= 20)
                    {
                        metroGrid1.Rows[row.Index].DefaultCellStyle.BackColor = Color.Red;
                        metroGrid1.Rows[row.Index].DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormFillChemical Message: {0}, {err.Message}");
            }
            
        }
        #endregion

        #region "Grid Cell Mouse Click"
        private void metroGrid1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                GridName = metroGrid1.CurrentRow.Cells[0].Value.ToString();
                GridMin = metroGrid1.CurrentRow.Cells[1].Value.ToString();
                GridMax = metroGrid1.CurrentRow.Cells[2].Value.ToString();
                textBoxNameChemical.Text = GridName;
                textBoxMaxWeight.Text = GridMax;
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormFillChemical Message: {0}, {err.Message}");
            }
            
        }
        #endregion

        #region "Button Fill Chemical"
        private void ButtonFillChemical_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxNameChemical.Text == "" || textBoxNameChemical.Text == null || textBoxMaxWeight.Text == "" || textBoxMaxWeight.Text == null)
                {
                    MessageBox.Show(" กรุณาเลือกสารเคมีก่อนเริ่มกระบวนการต่อไป ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    groupBox1.Hide();
                }
                else
                {
                    var dt_check = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * from Chemical_Address_PLC_Local Where Chemical = '{textBoxNameChemical.Text}' and Working = 1";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt_check);
                    }
                    string Address = dt_check.Rows[0]["Address"].ToString();
                    Thread.Sleep(mydelay);
                    ModbusClient.WriteSingleRegister(Convert.ToInt32(Address), 1); // Open tanks
                    groupBox1.Show();
                    ButtonConfirm.Show();
                    ButtonFillChemical.Hide();
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter($"   Message: {0}, {error.Message}");
                textBoxAlarm.Text = ($"   Message: {error.Message}");
            }
        }
        #endregion

        #region "Button Confirm"
        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                const string message = " คุณแน่ใจหรือไม่ ว่าต้องการอัปเดตสารเคมีนี้ไปยังฐานข้อมูล?";
                const string caption = "Update Data Chemical to Database";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    if (textBoxNameChemical.Text == "" || textBoxNameChemical.Text == null || textBoxMaxWeight.Text == "" || textBoxMaxWeight.Text == null || textBoxFill.Text == "" || textBoxFill.Text == null)
                    {
                        MessageBox.Show("กรุณาใส่ข้อมูลในช่องว่างให้ครบ เพื่อดำเนินการในขั้นตอนต่อไป", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        groupBox1.Hide();
                    }
                    else
                    {
                        string Qtyfill = textBoxFill.Text;
                        double Result = Convert.ToDouble(GridMin) + Convert.ToDouble(Qtyfill);
                        if (Result > Convert.ToDouble(GridMax))
                        {
                            MessageBox.Show("สารเคมีนี้มีน้ำหนักเกิน กรุณาตรวจสอบอีกครั้ง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //MessageBox.Show("this chemical is overweight");
                            textBoxFill.Text = "";
                            groupBox1.Hide();
                        }
                        else
                        {
                            var dt_check = new DataTable();
                            using (var conn = new SqlConnection(Local_Conn))
                            {
                                var check = conn.CreateCommand();
                                check.CommandText = $"Select * from Chemical_Address_PLC_Local Where Chemical = '{textBoxNameChemical.Text}' and Working = 1";
                                var sda = new SqlDataAdapter(check);
                                sda.Fill(dt_check);
                            }
                            string Address = dt_check.Rows[0]["Address"].ToString();
                            Thread.Sleep(mydelay);
                            ModbusClient.WriteSingleRegister(Convert.ToInt32(Address), 0); // Close tanks

                            var cmd = $"Update Chemical_Address_PLC_Local " +
                                $"Set Weight_Min = '{Result}', Status_Update = 'false', Working = 'true'" +
                                $"Where Chemical = '{textBoxNameChemical.Text}'";
                            if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                            {
                                MessageBox.Show("บันทึกข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.ActiveControl = metroGrid1;
                                DataGrid_Show();
                                Cell_Format();
                                textBoxNameChemical.Text = "";
                                textBoxMaxWeight.Text = "";
                                textBoxFill.Text = "";
                                textBoxFill.Enabled = false;
                                ButtonFillChemical.Show();
                                ButtonConfirm.Hide();
                                groupBox1.Hide();
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

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            textBoxNameChemical.Text = "";
            textBoxFill.Text = "";
            textBoxMaxWeight.Text = "";
            groupBox1.Hide();
            ButtonFillChemical.Hide();
            ButtonConfirm.Hide();
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
    }
}
