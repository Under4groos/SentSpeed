using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SentSpeed.lib
{
    public static class ColorUnderCursor
    {
        [DllImport("gdi32")]
        public static extern uint GetPixel(IntPtr hDC, int XPos, int YPos);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        /// <summary>
        /// Gets the System.Drawing.Color from under the mouse cursor.
        /// </summary>
        /// <returns>The color value.</returns>
        public static Color Get()
        {
            IntPtr dc = GetWindowDC(IntPtr.Zero);

            POINT p;
            GetCursorPos(out p);

            long color = GetPixel(dc, p.X, p.Y);
            return Color.FromArgb((int)color);
        }
        public static bool Is(Color c1 , Color c2)
        {
            return c1.R == c2.R && c1.G == c2.G && c1.B == c2.B;
        }
        public static string IsS(Color c1 , Color c2)
        {
            return (c1.R == c2.R && c1.G == c2.G && c1.B == c2.B).ToString();
        }
        public static Color Get(int x , int y)
        {
            IntPtr dc = GetWindowDC(IntPtr.Zero);

            POINT p;
            GetCursorPos(out p);

            long color = GetPixel(dc, x, y);
            return Color.FromArgb((int)color);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
