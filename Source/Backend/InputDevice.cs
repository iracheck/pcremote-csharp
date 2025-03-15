using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.XInput;
using WindowsInput;
using WindowsInput.Native;
using SharpDX.Mathematics.Interop;
using System.Threading;
using System.Diagnostics;

using ControllerToMouse.Settings;
using ControllerToMouse.Source.Utils;

namespace ControllerToMouse.Backend
{
    internal class InputDevice
    {
        // External Library Hooks
        Controller Controller;
        Gamepad Status;

        InputSimulator Simulator;
        IMouseSimulator Mouse;
        IKeyboardSimulator Keyboard;

        // Controller Indexing
        UserIndex Index;

        // Sleep Functionalities
        Stopwatch LastAction;

        private bool ControllerActive;

        // Clicking
        private bool LeftClickDown = false;
        private bool RightClickDown = false;
        private bool MiddleClickDown = false;


        // Mouse Speed Values
        private float MouseSpeed;

        // Constants
        private const float MOUSE_ACCELERATION_RATE = 0.05f;


        public InputDevice()
        {
            Console.WriteLine("Creating new input device...");

            Controller = new Controller(UserIndex.One);
            Index = Controller.UserIndex;
            LastAction = new Stopwatch();

            Simulator = new InputSimulator();

            Keyboard = Simulator.Keyboard;
            Mouse = Simulator.Mouse;
        }

        public void PollDevice()
        {
            while (Controller.IsConnected) // This while loop is temporary before a more advanced method can be written
            {
                if (LastAction.IsRunning == false) LastAction.Start();

                Status = Controller.GetState().Gamepad; // updates the status of all buttons/axes

                HandleInputs();
                Thread.Sleep(CalculateSleepTime());
            }
        }

        public void HandleInputs()
        {
            bool mouseMovement = HandleMouseMovement();
            bool scrolling = HandleScrolling();
            bool clicking = HandleMouseClicks();

            if (mouseMovement || scrolling || clicking)
            {
                SetActive();
                LastAction.Restart(); // reset the last action timer to ensure sleep states are handled correctly
            }
        }


        // Handles all mouse movements
        bool HandleMouseMovement()
        {
            int mouseSensitivity = GlobalSettings.MouseSensitivity;
            int lx = Status.LeftThumbX / mouseSensitivity; // Normalize input
            int ly = Status.LeftThumbY / mouseSensitivity;


            if (lx != 0 || ly != 0)
            {
                UpdateMouseSpeed(1);

                lx = (int)(lx * MouseSpeed);
                ly = (int)(ly * MouseSpeed) * -1;

                Mouse.MoveMouseBy(lx, ly);
                return true;
            }
            else
            {
                UpdateMouseSpeed(0);
                return false;
            }
        }


        // Mouse Speed uses a state system in order to determine current speed.
        // Mouse Speed is a percentage--> 0.5 = 50% of maximum
        // 0 -> reset
        // 1 -> accelerate
        // 2 --> limited acceleration
        // -1 -> max speed
        float UpdateMouseSpeed(int state)
        {
            // The "Acceleration Multiplier" is the percentage of the max speed that the mouse will accelerate by every time this method is called. 0.05 is 5%/s
            float accelerationMultiplier = 0.05f; 

            if (state == 0) // reset
            {
                MouseSpeed = 0f;
            }
            else if (state == 1) // accelerate
            {
                MouseSpeed += (1 - MouseSpeed) * accelerationMultiplier;
            }
            else if (state == -1) // max speed
            {
                MouseSpeed = 1;
            }
            return MouseSpeed;
        }


        // Handles scrolling
        bool HandleScrolling()
        {
            int rx = GetRightStickXRaw();
            int ry = GetRightStickYRaw();

            if (rx != 0 || ry != 0)
            {
                if (rx != 0.0) Mouse.HorizontalScroll(rx);
                if (ry != 0.0) Mouse.VerticalScroll(ry);

                return true;
            }
            return false;
        }

        // Handles mouse clicks
        bool HandleMouseClicks()
        {
            bool leftClick = Status.LeftTrigger > 0;
            bool rightClick = Status.RightTrigger > 0;
            bool middleClick = leftClick && rightClick;

            // middle click
            if (middleClick && !MiddleClickDown)
            {
                Mouse.XButtonDown(3);
                MiddleClickDown = true;
            }
            else if (MiddleClickDown && !middleClick)
            {
                Mouse.XButtonUp(3);
            }

            // left click
            if (leftClick && !LeftClickDown && !middleClick)
            {
                Mouse.LeftButtonDown();
                LeftClickDown = true;
            }
            else if (LeftClickDown && !leftClick)
            {
                Mouse.LeftButtonUp();
                LeftClickDown = false;
            }

            // right click
            if (rightClick && !RightClickDown && !middleClick)
            {
                Mouse.RightButtonDown();
                RightClickDown = true;
            }
            else if (RightClickDown && !rightClick)
            {
                Mouse.RightButtonUp();
                RightClickDown = false;
            }

            return leftClick || rightClick || middleClick;
        }

        // Returns the amount of time the program should "sleep" for before 
        int CalculateSleepTime()
        {
            float deltaTime = LastAction.ElapsedMilliseconds;

            if (!GetIsActive() && deltaTime > GlobalSettings.TimeBeforeSleep) 
            {
                //Console.WriteLine("Sleeping...");
                return GlobalSettings.SleepRefreshSpeed;
            }
            else if (GlobalSettings.BatterySaverEnabled && BatteryUtils.IsOnBatterySaver())
            {
                //Console.WriteLine(GetIsActive() + " " + GlobalSettings.BatterySaverEnabled + " " + BatteryUtils.IsOnBatterySaver());
                //Console.WriteLine("Battery Saver on");
                return GlobalSettings.BatterySaverRefreshSpeed;
            }
            else
            {
                //Console.WriteLine("Not sleeping...");
                return GlobalSettings.ActiveRefreshSpeed;
            }
        }


        void SetActive()
        {
            ControllerActive = true;
        }


        void ResetActive()
        {
            ControllerActive = false;
        }


        public bool GetIsActive()
        {
            return ControllerActive;
        }



        float GetMouseSpeed()
        {
            return MouseSpeed;
        }


        // Get raw input from the right stick
        int GetRightStickXRaw()
        {
            if (Status.RightThumbX > 0)
            {
                return 1;
            }
            else if (Status.RightThumbX < 0)
            {
                return -1;
            }
            return 0;
        }

        int GetRightStickYRaw()
        {
            if (Status.RightThumbY > 0)
            {
                return 1;
            }
            else if (Status.RightThumbY < 0)
            {
                return -1;
            }
            return 0;
        }
    }
}