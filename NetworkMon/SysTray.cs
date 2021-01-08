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
        public SysTray(string text, Icon icon/*, ContextMenu menu*/)
        {
            m_notifyIcon = new NotifyIcon();

            m_notifyIcon.Text = text;
            m_notifyIcon.Visible = true;
            m_notifyIcon.Icon = icon;
            m_DefaultIcon = icon;
            m_notifyIcon.DoubleClick += M_notifyIcon_DoubleClick;
            // m_notifyIcon.ContextMenu = menu;
            m_font = new Font("segoe ui", 6);

            m_timer = new Timer();
            m_timer.Interval = 100;
            m_timer.Tick += new System.EventHandler(this.m_timer_Tick);
        }

        private void M_notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            
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
            Bitmap bitmap = new Bitmap(18, 20);//, System.Drawing.Imaging.PixelFormat.Max);

            Brush brush = new SolidBrush(col);

            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.DrawString("10", m_font, brush, 0, 0);
            graphics.DrawString("KB", new Font("segoe ui", 5), brush, 0, 9);

            IntPtr hIcon = bitmap.GetHicon();
            Icon icon = Icon.FromHandle(hIcon);
            m_notifyIcon.Icon.Dispose();
            m_notifyIcon.Icon = icon;
            bitmap.Dispose();
        }

        public void RenderMultiLineText(string speed, string unit, Color col)
        {
            Bitmap bitmap = new Bitmap(18, 20);//, System.Drawing.Imaging.PixelFormat.Max);

            Brush brush = new SolidBrush(col);

            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.DrawString(speed, m_font, brush, 0, 0);
            graphics.DrawString(unit, new Font("segoe ui", 5), brush, 0, 11);

            IntPtr hIcon = bitmap.GetHicon();
            Icon icon = Icon.FromHandle(hIcon);
            m_notifyIcon.Icon.Dispose();
            m_notifyIcon.Icon = icon;
            bitmap.Dispose();
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
