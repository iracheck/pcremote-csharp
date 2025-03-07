using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.XInput;
using WindowsInput;
using WindowsInput.Native;
using ControllerToMouse.src.Utils;
using SharpDX.Mathematics.Interop;

namespace ControllerToMouse.src.Backend
{
    internal class InputDevice
    {
        // External Library Hooks
        Controller controller;
        Gamepad status;

        InputSimulator simulator;
        IMouseSimulator mouse;
        IKeyboardSimulator keyboard;

        DateTime now = DateTime.Now;
        DateTime lastAction = DateTime.Now;


        // Values
        private float mouseSpeed;


        // Constants
        private const float MAX_MOUSE_SPEED = 2f;
        


        public InputDevice()
        {
            Console.WriteLine("Creating new input device...");
            controller = new Controller(UserIndex.One);
            simulator = new InputSimulator();
            Console.WriteLine(controller.UserIndex.ToString());

            keyboard = simulator.Keyboard;
            mouse = simulator.Mouse;

        }

        public int pollDevice()
        {
            if (controller.IsConnected)
            {
                status = controller.GetState().Gamepad;
                handleMouseMovement();
                handleScrolling();
                return 1;
            }
            return 0;
        }

        void handleMouseMovement()
        {
            int lx = status.LeftThumbX / 1000; 
            int ly = status.LeftThumbY / 1000;


            if (lx != 0 || ly != 0)
            {
                lx = (int)(lx * mouseSpeed); // Must divide by a large number, as raw controller input provides a very large number
                ly = (int)(ly * mouseSpeed) * -1;

                updateMouseSpeed(1);
                Console.WriteLine(lx + " " + ly);

                mouse.MoveMouseBy(lx, ly);
            }
            else
            {
                updateMouseSpeed(0);
            }
        }


        // Mouse Speed uses a state system in order to determine current speed.
        // 0 -> reset
        // 1 -> accelerate
        // -1 -> max speed
        float updateMouseSpeed(int state)
        {
            float accelerationMultiplier = 1.0f / 13.0f; // This is an arbitrary number that slows down the speed of the mouse acceleration, to allow precise movement.

            if (state == 0) // reset
            {
                mouseSpeed = 0f;
            }
            else if (state == 1) // accelerate
            {
                mouseSpeed = (MAX_MOUSE_SPEED - mouseSpeed) * accelerationMultiplier;
            }
            else if (state == -1) // max speed
            {
                mouseSpeed = MAX_MOUSE_SPEED;
            }
            return mouseSpeed;
        }


        void handleScrolling()
        {
            int rx = status.RightThumbX; 
            int ry = status.RightThumbY;

            if (ry != 0.0)
            {
                mouse.VerticalScroll(ry);
            }
            else if (rx != 0.0)
            {
                mouse.HorizontalScroll(rx);
            }
        }

        //void handleControllerSleepMode()
        //{
        //    Timer currentTime 
        //}

        //int rx = status.RightThumbX;
        //int ry = status.RightThumbY;



        float getMouseSpeed()
        {
            return mouseSpeed;
        }
    }

}
