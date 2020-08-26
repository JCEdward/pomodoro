using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Globalization;

namespace pomodoro
{
    public partial class Form1 : Form
    {

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private bool Locked = false;
        private Timer _timer;
        private string time_Type;
        private int break_count = 0;
        private int ticks_avai = 0;
        private AppState _appState;
        [DllImport("user32.dll")]

        [return: MarshalAs(UnmanagedType.Bool)]

        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        public Form1()
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
            this.Locked = this._appState.locked;
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
            this.ticks_avai = this._appState.work_time * 60;
            this.time_Type = "work";
            label1.Text = this.ConvertToString(this.ticks_avai);
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            this._timer = new Timer();
            this._timer.Interval = 1000;
            this._timer.Tick += new EventHandler(TimerTick);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !this.Locked)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void CreateContextMenu()
        {
            ContextMenuStrip menuStrip = new ContextMenuStrip();
            ToolStripMenuItem Startbtn = new ToolStripMenuItem("Start", null, StartClick, "Start");
            ToolStripMenuItem Pausebtn = new ToolStripMenuItem("Pause", null, PauseClick, "Pause");
            ToolStripMenuItem Stopbtn = new ToolStripMenuItem("Stop", null, StopClick, "Stop");
            ToolStripMenuItem Lockbtn = new ToolStripMenuItem("Lock", null, Lock_Click, "Lock");
            ToolStripMenuItem Exitbtn = new ToolStripMenuItem("Exit", null, Exit_Click, "Exit");
            Pausebtn.Enabled = false;
            Stopbtn.Enabled = false;
            Lockbtn.Checked = this.Locked;
            menuStrip.Items.Add(Startbtn);
            menuStrip.Items.Add(Pausebtn);
            menuStrip.Items.Add(Stopbtn);
            menuStrip.Items.Add(Lockbtn);
            menuStrip.Items.Add(Exitbtn);
            this.ContextMenuStrip = menuStrip;

        }
        void Lock_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            if (menuItem.Name == "Lock")
            {
                var tmp_locked = ((ToolStripMenuItem)menuItem).Checked;
                tmp_locked = !tmp_locked;
                this.Locked = tmp_locked;
                ((ToolStripMenuItem)menuItem).Checked = tmp_locked;

                this._appState.locked = tmp_locked;
                this._appState.Save();

            }
        }
        void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        void StartClick(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = this._appState.work_ring_tone;
            player.Play();
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
        void PauseClick(object sender, EventArgs e)
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
        void StopClick(object sender, EventArgs e)
        {
            this._timer.Stop();
            this.ticks_avai = this._appState.work_time * 60;
            this.time_Type = "work";
            label1.Text = this.ConvertToString(this.ticks_avai);
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


        public string ConvertToString(int tick_avai)
        {
            string val = "00:00";
            int minute = tick_avai / 60;
            int tick = tick_avai - (minute * 60);
            val = minute.ToString().PadLeft(2, '0') + ":" + tick.ToString().PadLeft(2, '0');
            return val;
        }
        private void TimerTick(object sender, EventArgs e)
        {
            if (this.ticks_avai > 1)
            {
                this.ticks_avai -= 1;
                label1.Text = this.ConvertToString(this.ticks_avai);
            }
            else
            {
                SetNextTime();
            }

        }

        private void SetNextTime()
        {
            if (this.time_Type == "work")
            {
                this.break_count += 1;
                if (break_count > this._appState.long_break_interval)
                {
                    this.time_Type = "long_break";
                    this.ticks_avai = this._appState.long_break_time * 60;
                    this.SetColor();
                }
                else
                {
                    this.time_Type = "break";
                    this.ticks_avai = this._appState.break_time * 60;
                    this.SetColor();
                }
                System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                player.SoundLocation = this._appState.break_ring_tone;
                player.Play();
            }
            else
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                player.SoundLocation = this._appState.work_ring_tone;
                player.Play();
                if (this.time_Type == "long_break")
                {
                    this.break_count = 0;
                }
                this.time_Type = "work";
                this.ticks_avai = this._appState.work_time * 60;
                this.SetColor();
            }
        }

        private void SetColor()
        {
            ColorConverter colorConverter = new ColorConverter();
            switch (this.time_Type)
            {
                case "work":
                    Color color = (Color)colorConverter.ConvertFromString(_appState.work_color);
                    color = Color.FromArgb(255, color.R, color.G, color.B);
                    this.BackColor = color; break;
                case "break":
                    Color color2 = (Color)colorConverter.ConvertFromString(_appState.break_color);
                    color2= Color.FromArgb(255, color2.R, color2.G, color2.B);
                    this.BackColor = color2; break;
                case "long_break":
                    Color color3 = (Color)colorConverter.ConvertFromString(_appState.long_break_color);
                    color3 = Color.FromArgb(255, color3.R, color3.G, color3.B);
                    this.BackColor = color3; break;
                default: this.BackColor = Color.White; break;
            }
        }

        private void label1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("test");
        }
    }
}
