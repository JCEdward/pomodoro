using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace pomodoro
{
    public delegate void SaveEventHandler(object obj1, object obj2);
    public partial class FrmSetting : Form
    {
        private readonly AppState _appState;
        public event SaveEventHandler Save;
        public FrmSetting(string model)
        {
            this._appState = AppState.LoadFromJSON(model);
            InitializeComponent();

            this.trbOpacity.Maximum = 20;
            this.trbOpacity.Value = Convert.ToInt32(this._appState.opacity * 20);
            this.txtWorkTime.Text = this._appState.work_time.ToString();
            this.txtBreakTime.Text = this._appState.break_time.ToString();
            this.txtLongBreakTime.Text = this._appState.long_break_time.ToString();
            this.txtLongTimeInterval.Text = this._appState.long_break_interval.ToString();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtWorkTime.Text))
            {
                txtWorkTime.Text = "25";
            }
            if (string.IsNullOrEmpty(this.txtBreakTime.Text))
            {
                txtBreakTime.Text = "5";
            }
            if (string.IsNullOrEmpty(this.txtLongBreakTime.Text))
            {
                txtLongBreakTime.Text = "15";
            }
            if (string.IsNullOrEmpty(this.txtLongTimeInterval.Text))
            {
                txtLongTimeInterval.Text = "4";
            }
            _appState.work_time = int.Parse(txtWorkTime.Text);
            _appState.break_time = int.Parse(txtBreakTime.Text);
            _appState.long_break_time = int.Parse(txtLongBreakTime.Text);
            _appState.long_break_interval = int.Parse(txtLongTimeInterval.Text);
            _appState.opacity = trbOpacity.Value / 20.0;
            Save?.Invoke(this, _appState);
            this.Close();
        }

        private void OnlyNumber(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }
    }
}
