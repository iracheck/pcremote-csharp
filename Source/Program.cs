using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ControllerToMouse.src.Backend;
using System.Threading;

namespace ControllerToMouse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InputDevice test = new InputDevice();
            while (true)
            {
                test.pollDevice();
                Thread.Sleep(10);
            }
        }
    }
}
