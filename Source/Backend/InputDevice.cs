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
        Stopwatch Stopwatch;

        private bool ControllerActive;

        // Clicking
        private bool LeftClickDown = false;
        private bool RightClickDown = false;
        private bool MiddleClickDown = false;


        // Values
        private float MouseSpeed;


        // Constants
        private int MOUSE_SENSITIVITY = GlobalSettings.MouseSensitivity; // controller version of DPI

        // For controller sleep
        private int ACTIVE_UPDATE_DURATION = GlobalSettings.RefreshSpeed;
        private int SLEEP_UPDATE_DURATION = GlobalSettings.SleepRefreshSpeed;
        private int BATTERY_SAVER_UPDATE_DURATION = GlobalSettings.BatterySaverRefreshSpeed;

        private int TIME_BEFORE_SLEEP = 500; // in milliseconds


        public InputDevice()
        {
            Console.WriteLine("Creating new input device...");

            Controller = new Controller(UserIndex.One);
            Index = Controller.UserIndex;
            Stopwatch = new Stopwatch();

            Simulator = new InputSimulator();

            Keyboard = Simulator.Keyboard;
            Mouse = Simulator.Mouse;
        }

        public void PollDevice()
        {
            while (Controller.IsConnected) // This while loop is temporary before a more advanced method can be written
            {
                Stopwatch.StartNew();

                Status = Controller.GetState().Gamepad; // gets the state of all controller buttons/axe

                HandleInputs();

                CalculateSleepTime();
                Console.WriteLine(CalculateSleepTime());

                Sleep();
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
            }
        }


        // Handles all mouse movements
        bool HandleMouseMovement()
        {
            int lx = Status.LeftThumbX / MOUSE_SENSITIVITY; // Normalize input
            int ly = Status.LeftThumbY / MOUSE_SENSITIVITY;


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
            if (middleClick)
            {
                Mouse.XButtonDown(3);

                return true;
            }
            else if (middleClick & !MiddleClickDown)
            {
                Mouse.XButtonUp(3);
            }

            // left click
            if (leftClick && !LeftClickDown)
            {
                Mouse.LeftButtonDown();
                LeftClickDown = true;

                return true;
            }
            else if (LeftClickDown && leftClick)
            {
                Mouse.LeftButtonUp();
                LeftClickDown = false;
            }

            // right click
            if (rightClick && !RightClickDown)
            {
                Mouse.RightButtonDown();
                RightClickDown = true;

                return true;
            }
            else if (RightClickDown && !rightClick)
            {
                Mouse.RightButtonUp();
                RightClickDown = false;
            }

            return false;
        }


        float CalculateSleepTime()
        {
            float delta = Stopwatch.ElapsedMilliseconds;

            if (GetIsActive() && GlobalSettings.BatterySaverEnabled && BatteryUtils.IsOnBatterySaver()) // This is currently not fully functional. :. Will need to be integrated at a later date
            {
                // Console.WriteLine("Battery Saver on");
                return BATTERY_SAVER_UPDATE_DURATION;
            }
            else if (!GetIsActive() && delta > TIME_BEFORE_SLEEP)
            {
                // Console.WriteLine("Sleeping...");
                return SLEEP_UPDATE_DURATION;
            }
            else
            {
                // Console.WriteLine("Not sleeping...");
                return ACTIVE_UPDATE_DURATION;
            }
        }

        void Sleep()
        {
            float deltaTime;
            Stopwatch sleepTimer = new Stopwatch();

            sleepTimer.Start();

            do { 
                deltaTime = sleepTimer.ElapsedMilliseconds;
                Console.WriteLine(deltaTime);
                Console.WriteLine(CalculateSleepTime());
            } while (deltaTime < CalculateSleepTime());
            Console.WriteLine("Done Sleeping");
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