using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Threading.Thread;

namespace MoveCursorWindowless
{
    static class Program
    {
        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("User32.Dll")]
        private static extern long SetCursorPos(int x, int y);

        private static Point LastCursorPos;

        private static Timer Timer = new Timer { Interval = 5_000 };

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            GetCursorPos(out LastCursorPos);

            Timer.Tick += OnTimerTick;
            Timer.Start();

            Application.ApplicationExit += OnApplicationExit;

            Application.Run(new MoveCursorAppContext());
        }

        private static void OnTimerTick(object sender, EventArgs e)
        {
            GetCursorPos(out Point currentCursorPos);

            if (currentCursorPos == LastCursorPos)
            {
                MoveCursorAsArrow(currentCursorPos);
            }

            LastCursorPos = currentCursorPos;
        }

        private static void MoveCursorAsArrow(Point currentCursorPos)
        {
            MoveCursor(ref currentCursorPos, 72, 0, -1);
            MoveCursor(ref currentCursorPos, 30, -1, 1);
            MoveCursor(ref currentCursorPos, 30, 1, -1);
            MoveCursor(ref currentCursorPos, 30, 1, 1);
            MoveCursor(ref currentCursorPos, 30, -1, -1);
            MoveCursor(ref currentCursorPos, 72, 0, 1);
        }

        private static void MoveCursor(ref Point originPos, int length, int stepX, int stepY)
        {
            for (int i = 1; i < length; i++)
            {
                var newX = GetNewCoordinate(originPos.X, i, stepX);
                var newY = GetNewCoordinate(originPos.Y, i, stepY);

                SetCursorPos(newX, newY);
                Sleep(1);
            }
            originPos.X = GetNewCoordinate(originPos.X, length, stepX);
            originPos.Y = GetNewCoordinate(originPos.Y, length, stepY);
        }

        private static int GetNewCoordinate(int oldCoordinate, int length, int step)
            => oldCoordinate + length * step;

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            Application.ApplicationExit -= OnApplicationExit;
            Timer.Tick -= OnTimerTick;
            Timer.Dispose();
        }
    }
}