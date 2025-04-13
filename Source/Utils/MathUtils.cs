using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerToMouse.Utils
{
    internal static class MathUtils
    {
        public static float Clampf(float value, float min, float max)
        {
            if (value < min) return min;
            else if (value > max) return max;
            else return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            else if (value > max) return max;
            else return value;
        }
    }
}
