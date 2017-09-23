using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.Windows;

namespace ArkAutoClick
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern Int32 SendMessage(int hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);
        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int cStopDelay = 10000;
        public int Interval { get { return (int)numericUpDown1.Value; } }
        public int StopEvery { get { return (int)numericUpDown2.Value; } }

        Process Shootergame;

        public Form1()
        {
            InitializeComponent();
        } 
        private void Form1_Load(object sender, EventArgs e)
        {
            numericUpDown2.Value = 10;
            Text = "ARK AFK Clicker";
            Process[] pl = Process.GetProcesses();            
            for (int i = 0; i < pl.Length - 1; i++)
            {
                if (pl[i].ProcessName.ToLower() == "shootergame")
                {
                    Shootergame = pl[i];
                    break;
                }
            }
            label3.Text = Shootergame.Responding.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool Terminate = false;
            Thread t;
            if(button1.Text == "Start")
            {
                label3.Text = Shootergame.Responding.ToString();
                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = false;
                button1.Text = "Stop";
                t = new Thread(() =>
                {
                int tick = 0;
                while (!Terminate)
                {
                        /* well aint this just a shame
                    Point tL = new Point();
                    Point bR = new Point();
                    Rectangle rct = new Rectangle();
                    GetWindowRect(Shootergame.MainWindowHandle, out rct);
                    tL.X = rct.Left;
                    tL.Y = rct.Top;
                    bR.X = rct.Right;
                    bR.Y = rct.Bottom;
                    ScreenToClient(Shootergame.MainWindowHandle, ref tL);
                    ScreenToClient(Shootergame.MainWindowHandle, ref bR);

                    int w = bR.X - tL.X;
                    int h = bR.Y - tL.Y;

                    IntPtr wh = (IntPtr)((h << 16) | (w & 0xffff));
                    */
                        IntPtr wh = (IntPtr)0x1;
                    
                        SendMessage((int)Shootergame.MainWindowHandle, WM_LBUTTONDOWN, (IntPtr)0x1, wh);
                        Thread.Sleep(Interval);
                        SendMessage((int)Shootergame.MainWindowHandle, WM_LBUTTONUP, (IntPtr)0x1, wh);
                        if (tick == Math.Floor((decimal)(StopEvery * 1000) / Interval))
                        {
                            Thread.Sleep(cStopDelay);
                        }
                        tick++;
                    }

                })
                {
                    IsBackground = true
                };
                t.Start();
            } else
            {
                Terminate = true;
                numericUpDown1.Enabled = true;
                numericUpDown2.Enabled = true;
                button1.Text = "Start";
            }
            

            
        }

    }
}
