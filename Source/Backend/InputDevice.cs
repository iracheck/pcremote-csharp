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

using ControllerToMouse.Settings;

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
        DateTime Now = DateTime.Now;
        DateTime LastAction = DateTime.Now;

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
        private int BATTERY_UPDATE_DURATION = GlobalSettings.BatterySaverRefreshSpeed;


        public InputDevice()
        {
            Console.WriteLine("Creating new input device...");

            Controller = new Controller(UserIndex.One);
            Index = Controller.UserIndex;

            Simulator = new InputSimulator();

            Keyboard = Simulator.Keyboard;
            Mouse = Simulator.Mouse;
        }

        public int PollDevice()
        {
            while (Controller.IsConnected) // This while loop is temporary before a more advanced method can be written
            {
                Status = Controller.GetState().Gamepad; // gets the state of all controller buttons/axes

                HandleMouseMovement();
                HandleScrolling();
                HandleMouseClicks();

                HandleControllerSleep();
            }
            return 0;
        }


        // Handles all mouse movements
        void HandleMouseMovement()
        {
            int lx = Status.LeftThumbX / MOUSE_SENSITIVITY; // Normalize input
            int ly = Status.LeftThumbY / MOUSE_SENSITIVITY;


            if (lx != 0 || ly != 0)
            {
                UpdateMouseSpeed(1);
                SetActive();

                lx = (int)(lx * MouseSpeed);
                ly = (int)(ly * MouseSpeed) * -1;

                Mouse.MoveMouseBy(lx, ly);
            }
            else
            {
                UpdateMouseSpeed(0);
            }
        }


        // Mouse Speed uses a state system in order to determine current speed.
        // Mouse Speed is a percentage-- 0.5 = 50% of maximum
        // 0 -> reset
        // 1 -> accelerate
        // -1 -> max speed
        float UpdateMouseSpeed(int state)
        {
            float accelerationMultiplier = 1.0f / 20f; // This is an arbitrary number that slows down the speed of the mouse acceleration, to allow precise movement.

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
        void HandleScrolling()
        {
            int rx = GetRightStickXRaw();
            int ry = GetRightStickYRaw();

            if (rx != 0 || ry != 0)
            {
                SetActive(); // notes that the controller is active
                if (rx != 0.0) Mouse.HorizontalScroll(rx);
                if (ry != 0.0) Mouse.VerticalScroll(ry);
            }
        }


        void HandleMouseClicks()
        {
            bool leftClick = Status.LeftTrigger > 0;
            bool rightClick = Status.RightTrigger > 0;
            bool middleClick = leftClick && rightClick;

            if (leftClick || rightClick) SetActive();

            // middle click
            if (middleClick)
            {
                Mouse.XButtonDown(3);

                return;
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

                return;
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

                return;
            }
            else if (RightClickDown && !rightClick)
            {
                Mouse.RightButtonUp();
                RightClickDown = false;
            }

        }


        int HandleControllerSleep()
        {
            //deltaTime deltaTime = Now.CompareTo(LastAction);
            if (IsActive())
            {
                Console.WriteLine("Controller " + Index + " sleeping.");
                return ACTIVE_UPDATE_DURATION;
            }
            return 100;
        }


        void SetActive()
        {
            ControllerActive = true;
        }


        void ResetActive()
        {
            ControllerActive = false;
        }


        public bool IsActive()
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