using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;


namespace ControllerToMouse.src.Utils
{
    internal static class MouseUtils
    {
        static private InputSimulator isimulator = new InputSimulator();
        static private IMouseSimulator mouse = isimulator.Mouse;

        [StructLayout(LayoutKind.Sequential)]
        public struct ScreenPoint
        {
            public int x;
            public int y;
        }


        // Import GetCursorPos function from user32.dll.
        // This is a windows library function. Will need to modify this logic for any Linux/Mac versions.
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out ScreenPoint lpPoint);


        public static ScreenPoint GetMousePos()
        {
            ScreenPoint point;
            if (GetCursorPos(out point)) {
                return point;
            }
            else
            {
                Console.WriteLine("[ERROR > MOUSE] Could not find mouse location. Setting to default (0,0).");
                point.x = 0;
                point.y = 0;
                return point;
            }
        }

        public static void resetMousePos()
        {
            mouse.MoveMouseTo(0, 0);
        }

    }
}
