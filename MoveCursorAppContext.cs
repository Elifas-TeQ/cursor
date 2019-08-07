using System;
using System.Drawing;
using System.Windows.Forms;

namespace MoveCursorWindowless
{
    public class MoveCursorAppContext : ApplicationContext
    {
        private NotifyIcon trayIcon;

        public MoveCursorAppContext()
        {
            trayIcon = new NotifyIcon()
            {
                Icon = new Icon("arrow.ico"),
                ContextMenu = new ContextMenu(new MenuItem[]
                {
                    new MenuItem("Exit", Exit)
                }),
                Visible = true,
            };
        }

        void Exit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;

            Application.Exit();
        }
    }
}