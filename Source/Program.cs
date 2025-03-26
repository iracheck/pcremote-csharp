using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Threading;

using ControllerToMouse.Backend;
using ControllerToMouse.Utils;
using WindowsInput.Native;

namespace ControllerToMouse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InputDevice test = new InputDevice();
            test.PollDevice();
        }
    }
}
