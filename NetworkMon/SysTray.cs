using System;
using System.Drawing;
using System.Windows.Forms;

namespace NetworkMon
{
    public class SysTray : IDisposable
    {
        public bool IsRunning = true;
        Icon DownloadIcon;
        Icon NeutralIcon;

        public SysTray()
        {
            m_notifyIcon = new NotifyIcon();
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
                m_notifyIcon.Icon = DownloadIcon;
            }
        }

        public void ClearTrayIcon()
        {
            m_notifyIcon.Icon = NeutralIcon;
        }

        public void Dispose()
        {
            m_notifyIcon.Dispose();
        }

        private NotifyIcon m_notifyIcon;
        private Icon m_DefaultIcon;
    }
}
