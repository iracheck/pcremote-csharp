using ControllerToMouse.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ControllerToMouse.Source.GUI.UserControls;
using System.Windows.Threading;
using System.Threading;

namespace ControllerToMouse.Source.GUI
{
    static class StatusBar
    {
        public static TextBlock Target; // this is the actual status bar, to be used 

        static float TimeRemaining;
        public static float UpdateInterval;

        /// <summary>
        /// Set a status message with a given time interval. 
        /// Note that the program refreshes the UI ten times per second, and thus the length of the message should be limited to the tenths place.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="length"></param>
        static public void Message(String message, float length)
        {
            Target.Text = message;
            TimeRemaining = length;
        }

        /// <summary>
        /// Set a status message, overwriting the previous one. Default interval of 1.5s
        /// </summary>
        /// <param name="message"></param>
        /// <param name="length"></param>
        static public void Message(String message)
        {
            Message(message, 1.5f);
        }

        static public void Update(object sender, EventArgs e)
        {
            TimeRemaining -= UpdateInterval;

            if (TimeRemaining < 0)
            {
                Target.Text = "";
                TimeRemaining = 0;
            }
        }
    }
}
