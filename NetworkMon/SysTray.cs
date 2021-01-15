using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Controls;

namespace NetworkMon
{
    public class SysTray : IDisposable
    {
        public bool IsRunning = true;
        Icon DownloadIcon;
        Icon NeutralIcon;

        public SysTray(string text)
        {
            m_notifyIcon = new NotifyIcon();

            m_notifyIcon.Text = text;
            
            // m_notifyIcon.DoubleClick += M_notifyIcon_DoubleClick;
            m_notifyIcon.ContextMenuStrip = new ContextMenuStrip();

            ToolStripButton bt = new ToolStripButton();
            bt.Click += Bt_Click;
            bt.Name = "View";
            bt.Text = "View";
            m_notifyIcon.ContextMenuStrip.Items.Add(bt);

            ToolStripButton btRestart = new ToolStripButton();
            btRestart.Click += BtRestart_Click;
            btRestart.Name = "Restart";
            btRestart.Text = "Restart";
            m_notifyIcon.ContextMenuStrip.Items.Add(btRestart);

            m_font = new Font("segoe ui", 6);

            //m_timer = new Timer();
            //m_timer.Interval = 100;
            //m_timer.Tick += new System.EventHandler(this.m_timer_Tick);

            DownloadIcon = GetImageByName("NetworkMon.Assets.download.ico");
            NeutralIcon = GetImageByName("NetworkMon.Assets.hyphen.ico");
        }

        public void SetDefaultIcons()
        {
            m_notifyIcon.Visible = true;
            m_notifyIcon.Icon = DownloadIcon;
            m_DefaultIcon = DownloadIcon;
        }

        private void BtRestart_Click(object sender, EventArgs e)
        {
            IsRunning = false;

            System.Windows.Application.Current.Shutdown();
        }

        private void Bt_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }

        public void ShowText(string text)
        {
            ShowText(text, m_font, m_col);
        }

        public void RenderText(string text, Color col)
        {
            ShowText(text, m_font, col);
        }

        public void ShowText(string text, Font font)
        {
            ShowText(text, font, m_col);
        }

        public void ShowText(string text, Font font, Color col)
        {
            // Bitmap bitmap = GetImageByName("download.ico");

            //Brush brush = new SolidBrush(col);

            //Graphics graphics = Graphics.FromImage(bitmap);
            //graphics.DrawString("10", m_font, brush, 0, 0);
            //graphics.DrawString("KB", new Font("segoe ui", 5), brush, 0, 9);

            //IntPtr hIcon = bitmap.GetHicon();
            //Icon icon = Icon.FromHandle(hIcon);
            m_notifyIcon.Icon.Dispose();
            m_notifyIcon.Icon = GetImageByName("download.ico");
            // bitmap.Dispose();
        }

        public static Icon GetImageByName(string imageName)
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            string[] names = asm.GetManifestResourceNames();
            var stream = asm.GetManifestResourceStream($"{imageName}");
            var bmp = Bitmap.FromStream(stream);
            var thumb = (Bitmap)bmp.GetThumbnailImage(64, 64, null, IntPtr.Zero);
            thumb.MakeTransparent();
            return Icon.FromHandle(thumb.GetHicon());
        }

        public void ShowTrayDonwloadIcon()
        {
            if (m_notifyIcon.Icon == null || m_notifyIcon.Icon == NeutralIcon)
            {
                // m_notifyIcon.Icon.Dispose();
                m_notifyIcon.Icon = DownloadIcon;
            }
        }

        public void ClearTrayIcon()
        {
            // m_notifyIcon.Icon.Dispose();
            // m_notifyIcon.Icon = GetImageByName("NetworkMon.Assets.download.ico");

            // m_notifyIcon.Icon.Dispose();
            m_notifyIcon.Icon = NeutralIcon;
        }

        public void SetAnimationClip(Icon[] icons)
        {
            m_animationIcons = icons;
        }

        public void SetAnimationClip(Bitmap[] bitmap)
        {
            m_animationIcons = new Icon[bitmap.Length];
            for (int i = 0; i < bitmap.Length; i++)
            {
                m_animationIcons[i] = Icon.FromHandle(bitmap[i].GetHicon());
            }
        }

        public void SetAnimationClip(Bitmap bitmapStrip)
        {
            m_animationIcons = new Icon[bitmapStrip.Width / 16];
            for (int i = 0; i < m_animationIcons.Length; i++)
            {
                Rectangle rect = new Rectangle(i * 16, 0, 16, 16);
                Bitmap bmp = bitmapStrip.Clone(rect, bitmapStrip.PixelFormat);
                m_animationIcons[i] = Icon.FromHandle(bmp.GetHicon());
            }
        }

        public void StartAnimation(int interval, int loopCount)
        {
            if (m_animationIcons == null)
                throw new ApplicationException("Animation clip not set with SetAnimationClip");

            m_loopCount = loopCount;
            m_timer.Interval = interval;
            m_timer.Start();
        }

        public void StopAnimation()
        {
            m_timer.Stop();
        }

        public void Dispose()
        {
            m_notifyIcon.Dispose();
            if (m_font != null)
                m_font.Dispose();
        }

        private void m_timer_Tick(object sender, EventArgs e)
        {
            if (m_currIndex < m_animationIcons.Length)
            {
                m_notifyIcon.Icon = m_animationIcons[m_currIndex];
                m_currIndex++;
            }
            else
            {
                m_currIndex = 0;
                if (m_loopCount <= 0)
                {
                    m_timer.Stop();
                    m_notifyIcon.Icon = m_DefaultIcon;
                }
                else
                {
                    --m_loopCount;
                }
            }
        }

        private NotifyIcon m_notifyIcon;
        private Font m_font;
        private Color m_col = Color.Black;
        private Icon[] m_animationIcons;
        private Timer m_timer;
        private int m_currIndex = 0;
        private int m_loopCount = 0;
        private Icon m_DefaultIcon;
    }
}
