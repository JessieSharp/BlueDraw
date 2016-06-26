using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlueDraw_2._0
{
    public partial class Form1 : Form
    {
        public int cursorsize = 0;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        //This simulates a left mouse click
        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }

        private Bitmap image1;
        public Form1()
        {
            InitializeComponent();
            Subscribe();
        }

        private IKeyboardMouseEvents m_GlobalHook;


        public void Subscribe()
        {
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
        }
        Thread painting;
        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)61)
            {
                painting = new Thread(paint);
                painting.Start();
            }
            if (e.KeyChar == (char)45)
            {
                painting.Abort();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cursorsize = trackBar3.Value;
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                //dlg.Title = "Open Image";
                //dlg.Filter = "bmp files (*.bmp)|*.bmp";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    image1 = new Bitmap(dlg.FileName);
                    MessageBox.Show("Loaded");
                    image1 = GrayScale(image1);
                    MessageBox.Show("Converted to grey scale");
                }
            }
        }

        public Bitmap GrayScale(Bitmap Bmp)
        {
            int rgb;
            Color c;
            try
            {
                for (int y = 0; y < Bmp.Height; y++)
                    for (int x = 0; x < Bmp.Width; x++)
                    {
                        c = Bmp.GetPixel(x, y);
                        rgb = (int)((c.R + c.G + c.B) / 3);
                        Bmp.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                    }
                return Bmp;
            }
            catch (Exception)
            {

            }
            return Bmp;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = trackBar1.Value.ToString();
            label4.Text = trackBar2.Value.ToString();
            label8.Text = trackBar3.Value.ToString();
        }

        public void paint()
        {
            int curx = Cursor.Position.X;
            int cury = Cursor.Position.Y;
            try
            {
                Color x;
                for (int i = 0; i < image1.Width; i+=cursorsize)
                {
                    for (int j = 0; j < image1.Height; j++)
                    {
                        x = image1.GetPixel(i, j);
                        this.Invoke((MethodInvoker)delegate ()
                        {
                            if (x.R <= trackBar2.Value && x.G <= trackBar2.Value && x.B <= trackBar2.Value)
                            {
                                try
                                {
                                    this.Invoke((MethodInvoker)delegate ()
                                    {
                                        Thread.Sleep(trackBar1.Value);
                                    });
                                }
                                catch (Exception)
                                {

                                    Application.Exit();
                                }

                                LeftMouseClick(i + curx, j + cury);
                            }
                        });
                    }
                }
            }


            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            MessageBox.Show("done");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("My email: nynov3@gmail.com");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(image1);
            form2.Show();          
        }
    }
}
