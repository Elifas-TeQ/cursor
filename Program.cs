using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MoveCursorWindowless
{
    static class Program
    {
        [DllImport("User32.Dll")]
        static extern long SetCursorPos(int x, int y);

        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out Point lpPoint);

        static Timer Timer = new Timer { Interval = 30_000 };

        static Point _previousPosition;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            GetCursorPos(out _previousPosition);

            Timer.Tick += OnTimerTick;
            Timer.Start();

            Application.ApplicationExit += OnApplicationExit;

            Application.Run(new MoveCursorAppContext());
        }

        private static void OnTimerTick(object sender, EventArgs e)
        {
            GetCursorPos(out Point currentPosition);

            if (currentPosition == _previousPosition)
            {
                MakeMove();
            }

            _previousPosition = currentPosition;
        }

        private static void MakeMove()
        {
            var x = Cursor.Position.X;
            var y = Cursor.Position.Y;

            var radius = 50;

            for (int i = 0; i < 360; i++)
            {
                var p = PointOnCircle(radius, i, new PointF(x, y));
                SetCursorPos((int)p.X, (int)p.Y);

                System.Threading.Thread.Sleep(2);
            }
        }

        private static PointF PointOnCircle(float radius, float angle, PointF origin)
        {
            // Convert from degrees to radians via multiplication by PI/180        
            var x = (float)(radius * Math.Cos(angle * Math.PI / 180F)) - radius + origin.X;
            var y = (float)(radius * Math.Sin(angle * Math.PI / 180F)) + origin.Y;

            return new PointF(x, y);
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            Application.ApplicationExit -= OnApplicationExit;
            Timer.Tick -= OnTimerTick;
            Timer.Dispose();
        }
    }
}
