using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Chemical_SGS_Remake
{
    public partial class FormMonitor : Form
    {
        public string MonitorChemical = Properties.Settings.Default.MonitorChemical.ToString();
        public double MonitorWeight = Properties.Settings.Default.MonitorWeight;
        public double MonitorWeightSTD = Properties.Settings.Default.MonitorSTD;
        public string MonitorFormula = Properties.Settings.Default.MonitorFormula.ToString();
        public string MonitorStatus = Properties.Settings.Default.MonitorStatus.ToString();
        public string MonitorSetpoint = Properties.Settings.Default.MonitorSetPoint.ToString();

        public FormMonitor()
        {
            InitializeComponent();
        }

        private void FormMonitor_Load(object sender, EventArgs e)
        {
            timerCheckStatus.Enabled = true;
            ButtonStatus.Text = "Pending";
            labelFormula.Text = "-";
            labelChemical.Text = "-";
            labelActualWeight.Text = "00.00";
            labelWeighed.Text = "00.00";
        }

        private void FunctionCheckOK()
        {
            try
            {
                double We = Convert.ToDouble(Properties.Settings.Default.MonitorWeight);
                double STD = Convert.ToDouble(Properties.Settings.Default.MonitorSTD);
                double SP = Convert.ToDouble(Properties.Settings.Default.MonitorSetPoint);
                double Min = STD - SP;
                double Max = STD + SP;
                if (We < Max && We > Min)
                {
                    rjButton3.BackgroundColor = Color.Green;
                    labelWeighed.BackColor = Color.Green;
                    label5.BackColor = Color.Green;
                    label7.BackColor = Color.Green;
                }
                if (We > Max)
                {
                    rjButton3.BackgroundColor = Color.Red;
                    labelWeighed.BackColor = Color.Red;
                    label5.BackColor = Color.Red;
                    label7.BackColor = Color.Red;
                }
                if (We < Min)
                {
                    rjButton3.BackgroundColor = Color.WhiteSmoke;
                    labelWeighed.BackColor = Color.WhiteSmoke;
                    label5.BackColor = Color.WhiteSmoke;
                    label7.BackColor = Color.WhiteSmoke;
                }
            }
            catch
            {

            }
            
        }

        private void timerCheckStatus_Tick(object sender, EventArgs e)
        {
            try
            {
                timerCheckStatus.Enabled = false;
                labelFormula.Text = Properties.Settings.Default.MonitorFormula.ToString();
                labelChemical.Text = Properties.Settings.Default.MonitorChemical.ToString();
                labelActualWeight.Text = Properties.Settings.Default.MonitorSTD.ToString();
                labelWeighed.Text = Properties.Settings.Default.MonitorWeight.ToString();
                labelSetpoint.Text = Properties.Settings.Default.MonitorSetPoint.ToString();
                Properties.Settings.Default.Save();
                FunctionCheckOK();
                if (MonitorStatus == "1")
                {
                    ButtonStatus.Text = "Complete";
                    labelFormula.Text = "-";
                    labelChemical.Text = "-";
                    labelActualWeight.Text = "00.00";
                    labelWeighed.Text = "00.00";
                    labelSetpoint.Text = "00.00";
                    MonitorStatus = "0";
                    Properties.Settings.Default.MonitorStatus = "0";
                    Properties.Settings.Default.Save();
                    timerCheckStatus.Enabled = true;

                }
                timerCheckStatus.Enabled = true;
            }
            catch (Exception err)
            {
                _ = new LogWriter($" Error FormMonitor Message: {0}, {err.Message}");
            }
            
        }

    }
}
