using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Globalization;
using System.IO;

namespace pomodoro
{
    public partial class FrmMain : Form
    {

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        const  uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOMOVE = 0x0002;
        const uint TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        private bool _locked = false;
        private Timer _timer;
        private string _timeType;
        private int _breakCount = 0;
        private int _ticksAvai = 0;
        private AppState _appState;
        private int _oldWorkTimeSetting = 0;
        [DllImport("user32.dll")]

        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        public FrmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.InitConfig();
            this.CreateContextMenu();
        }

        private void InitConfig()
        {
            this._appState = AppState.Load();
            this._locked = this._appState.locked;
            Screen[] screens = Screen.AllScreens;
            foreach (Screen screen in screens)
            {
                Point formTopLeft = new Point(this._appState.x_pos, this._appState.y_pos);

                if (screen.WorkingArea.Contains(formTopLeft))
                {
                    this.Top = this._appState.y_pos;
                    this.Left = this._appState.x_pos;
                }
            }
            this.Opacity = this._appState.opacity;
            this._ticksAvai = this._appState.work_time * 60;
            this._timeType = "work";
            label1.Text = ConvertToString(this._ticksAvai);
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            this._timer = new Timer {Interval = 1000};
            this._timer.Tick += new EventHandler(TimerTick);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || this._locked) return;
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        private void CreateContextMenu()
        {
            ContextMenuStrip menuStrip = new ContextMenuStrip();
            ToolStripMenuItem startBtn = new ToolStripMenuItem("Start", null, StartClick, "Start");
            ToolStripMenuItem pauseBtn = new ToolStripMenuItem("Pause", null, PauseClick, "Pause");
            ToolStripMenuItem stopBtn = new ToolStripMenuItem("Stop", null, StopClick, "Stop");
            ToolStripMenuItem lockBtn = new ToolStripMenuItem("Lock", null, Lock_Click, "Lock");
            ToolStripMenuItem exitBtn = new ToolStripMenuItem("Exit", null, Exit_Click, "Exit");
            ToolStripMenuItem optionsBtn = new ToolStripMenuItem("Options", null, Options_Click, "Options");
            pauseBtn.Enabled = false;
            stopBtn.Enabled = false;
            lockBtn.Checked = this._locked;
            menuStrip.Items.Add(startBtn);
            menuStrip.Items.Add(pauseBtn);
            menuStrip.Items.Add(stopBtn);
            menuStrip.Items.Add(optionsBtn);
            menuStrip.Items.Add(lockBtn);
            menuStrip.Items.Add(exitBtn);
            this.ContextMenuStrip = menuStrip;

        }

        private void Options_Click(object sender, EventArgs e)
        {
            this._oldWorkTimeSetting = this._appState.work_time;
            var frm = new FrmSetting(this._appState.SerializeJSON());
            frm.Save += new SaveEventHandler(SaveConfigEvent);
            frm.Show();
        }

        private void SaveConfigEvent(object sender, object e)
        {
         
            var appState = (AppState)e;
            if (_timeType == "work" && this._ticksAvai == this._oldWorkTimeSetting * 60)
            {
                this._ticksAvai = appState.work_time * 60;
                label1.Text = ConvertToString(this._ticksAvai);
                
            }
            _appState = appState;
            this.Opacity = _appState.opacity;
            _appState.Save();
        }

        private void Lock_Click(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;
            if (menuItem.Name != "Lock") return;
            var tmpLocked = ((ToolStripMenuItem)menuItem).Checked;
            tmpLocked = !tmpLocked;
            this._locked = tmpLocked;
            ((ToolStripMenuItem)menuItem).Checked = tmpLocked;

            this._appState.locked = tmpLocked;
            this._appState.Save();
        }

        private static void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void StartClick(object sender, EventArgs e)
        {
            StartSound(_appState.work_ring_tone);
            this._timer.Start();
            this.SetColor();
            foreach (var item in this.ContextMenuStrip.Items)
            {
                if (item.GetType().Name == "ToolStripMenuItem")
                {
                    ToolStripMenuItem menuItem = (ToolStripMenuItem)item;
                    switch (menuItem.Name)
                    {
                        case "Start": menuItem.Enabled = false; break;
                        case "Pause": menuItem.Enabled = true; break;
                        case "Stop": menuItem.Enabled = true; break;
                        default: break;
                    }
                }
            }

        }

        private void PauseClick(object sender, EventArgs e)
        {
            this._timer.Stop();
            this.BackColor = Color.White;
            foreach (var item in this.ContextMenuStrip.Items)
            {
                if (item.GetType().Name == "ToolStripMenuItem")
                {
                    ToolStripMenuItem menuItem = (ToolStripMenuItem)item;
                    switch (menuItem.Name)
                    {
                        case "Start": menuItem.Enabled = true; break;
                        case "Pause": menuItem.Enabled = false; break;
                        case "Stop": menuItem.Enabled = true; break;
                        default: break;
                    }
                }
            }
        }

        private void StopClick(object sender, EventArgs e)
        {
            this._timer.Stop();
            this._ticksAvai = this._appState.work_time * 60;
            this._timeType = "work";
            label1.Text = ConvertToString(this._ticksAvai);
            this.BackColor = Color.White;
            foreach (var item in this.ContextMenuStrip.Items)
            {
                if (item.GetType().Name == "ToolStripMenuItem")
                {
                    ToolStripMenuItem menuItem = (ToolStripMenuItem)item;
                    switch (menuItem.Name)
                    {
                        case "Start": menuItem.Enabled = true; break;
                        case "Pause": menuItem.Enabled = false; break;
                        case "Stop": menuItem.Enabled = false; break;
                        default: break;
                    }
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var y = this.Top;
            var x = this.Left;
            this._appState.x_pos = x;
            this._appState.y_pos = y;
            this._appState.Save();
        }


        private static string ConvertToString(int tickAvai)
        {
            var val = "00:00";
            var minute = tickAvai / 60;
            var tick = tickAvai - (minute * 60);
            val = minute.ToString().PadLeft(2, '0') + ":" + tick.ToString().PadLeft(2, '0');
            return val;
        }
        private void TimerTick(object sender, EventArgs e)
        {
            if (this._ticksAvai > 1)
            {
                this._ticksAvai -= 1;
                label1.Text = ConvertToString(this._ticksAvai);
            }
            else
            {
                SetNextTime();
            }

        }

        private void SetNextTime()
        {
            if (this._timeType == "work")
            {
                this._breakCount += 1;
                if (_breakCount > this._appState.long_break_interval)
                {
                    this._timeType = "long_break";
                    this._ticksAvai = this._appState.long_break_time * 60;
                    this.SetColor();
                }
                else
                {
                    this._timeType = "break";
                    this._ticksAvai = this._appState.break_time * 60;
                    this.SetColor();
                }
                StartSound(_appState.break_ring_tone);
            }
            else
            {
                StartSound(_appState.work_ring_tone);
                if (this._timeType == "long_break")
                {
                    this._breakCount = 0;
                }
                this._timeType = "work";
                this._ticksAvai = this._appState.work_time * 60;
                this.SetColor();
            }
        }

        private void SetColor()
        {
            var colorConverter = new ColorConverter();
            var color = Color.White;
            switch (this._timeType)
            {
                case "work":
                    color = (Color)colorConverter.ConvertFromString(_appState.work_color);
                    break;
                case "break":
                    color = (Color)colorConverter.ConvertFromString(_appState.break_color);
                    break;
                case "long_break":
                    color = (Color)colorConverter.ConvertFromString(_appState.long_break_color);
                    break;
                default: this.BackColor = Color.White; break;
            }
            color = Color.FromArgb(255, color.R, color.G, color.B);
            this.BackColor = color;
        }
        private static void StartSound(string filePath)
        {
            if (File.Exists(filePath))
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                player.SoundLocation = filePath;
                player.Play();
            }
        }

        private void label1_DoubleClick(object sender, EventArgs e)
        {
          
        }
    }
}
