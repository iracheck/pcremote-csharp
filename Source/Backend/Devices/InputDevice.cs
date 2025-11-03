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
using ControllerToMouse.Utils;
using System.ComponentModel;

namespace ControllerToMouse.Devices
{
    public class InputDevice : INotifyPropertyChanged
    {
        // Outward-Facing Controller Information
        public String Name { set; get; } = "Input Device";
        public String SerialNumber { set; get; } = "0";

        public String ProfileName { set; get; } = "Default Profile";

        public bool isBatteryPowered => false;
        public int batteryLevel = 0;


        // External Library Hooks
        Controller Controller;
        Gamepad Status;
        GamepadButtonFlags Buttons;

        BatteryDeviceType BatteryInfo;

        InputSimulator Simulator;
        IMouseSimulator Mouse;
        IKeyboardSimulator Keyboard;

        // Controller Indexing
        UserIndex Index;

        // Sleep Functionalities
        Stopwatch LastAction;

        private bool ControllerActive;
        private bool PollingActive = true;


        // Clicking
        private bool LeftClickPressed = false;
        private bool RightClickPressed = false;
        private bool MiddleClickPressed = false;

        // D-Pad
        private bool DPadUpPressed;
        private bool DPadDownPressed;
        private bool DPadLeftPressed;
        private bool DPadRightPressed;

        // Menu Buttons
        private bool MenuButtonRightPressed;
        private bool MenuButtonLeftPressed;

        // Left Stick
        private bool LeftStickMoved;
        private bool LeftStickPressed;

        // Right Stick
        private bool RightStickMoved;
        private bool RightStickPressed;



        // Mouse Speed Values
        private float MouseSpeed;


        // Constants
        private const float MOUSE_ACCELERATION_RATE = 0.05f;


        public InputDevice(UserIndex index)
        {
            Console.WriteLine("Creating new input device with index {0}...", index);

            Index = index;
            Controller = new Controller(Index);

            if (Controller.IsConnected == false) {
                Console.WriteLine("No controller found.");
                return;
            }

            LastAction = new Stopwatch();

            Simulator = new InputSimulator();

            Keyboard = Simulator.Keyboard;
            Mouse = Simulator.Mouse;
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public void BeginDeviceThread()
        {
            while (PollingActive) PollDevice();
        }

        // This does not actually "shut it down," it lets it finish what it was doing on that given frame and *then* stops!
        public void ShutdownDeviceThread()
        {
            PollingActive = false; // safely stops the thread
        }


        // Polls the device until polling is disabled or program is terminated. 
        // This should be run within its own thread.
        public void PollDevice() 
        {
            if (!Controller.IsConnected)
            {
                InputDeviceManager.RemoveDevice(Index);
            }
            if (LastAction.IsRunning == false) LastAction.Start();

            Status = Controller.GetState().Gamepad; // updates the status of all buttons/axes
            Buttons = Controller.GetState().Gamepad.Buttons;

            HandleInputs();

            Thread.Sleep(CalculateSleepTime());
        }


        // Calls inputs and sets active state based off of user input
        void HandleInputs()
        {
            // Triggers checks for activity, executes actions, and then returns the activity status of each input
            bool leftStick = UpdateLeftStick();
            bool rightStick = UpdateRightStick();
            bool triggers = UpdateTriggers();
            bool dpad = UpdateDPad();
            bool menuR = UpdateMenuButtonRight();
            bool menuL = UpdateMenuButtonLeft();
            bool rb = UpdateMenuButtonRight();
            bool lb = UpdateMenuButtonLeft();
            bool sbr = UpdateRightShoulderButton();
            bool sbl = UpdateLeftShoulderButton();
            bool buttons = PollButtons();


            if (leftStick || rightStick || triggers || dpad || menuR || 
                menuL || rb || lb || sbr || sbl || buttons) SetActive();

            else ResetActive();
        }


        // Handles all mouse movements
        bool UpdateLeftStick()
        {
            int mouseSensitivity = 3000;

            // Get the input from each axis of the thumbstick, and then normalize it based off of the sensitivity given by the user.
            int lx = Status.LeftThumbX;
            int ly = Status.LeftThumbY;

            LeftStickMoved = lx > 0 || ly > 0;

            // Adjust for the sensitivity of the mouse. Then normalize due to the sheer magnitude of lx/ly
            lx *= (mouseSensitivity / 1000);
            ly *= (mouseSensitivity / 1000);

            if (lx != 0 || ly != 0)
            {
                CalculateMouseSpeed(1);

                // Calculate final x and y values of left stick
                lx = (int)(Math.Clamp(lx , -5, 5) * MouseSpeed);
                ly = (int)(Math.Clamp(ly, -5, 5) * -MouseSpeed);

                Mouse.MoveMouseBy(lx, ly);
                Trace.WriteLine(lx.ToString() + " " + ly.ToString());
                return true;
            }
            else
            {
                CalculateMouseSpeed(0);
                return false;
            }
        }


        // Mouse Speed uses a state system in order to determine current speed.
        // Mouse Speed is a percentage--> 0.5 = 50% of maximum
        // 0 -> reset
        // 1 -> accelerate
        // -1 -> max speed

        // Because mouse acceleration is handled on a per-controller basis, it is called without an expected return statement
        void CalculateMouseSpeed(int state)
        {
            float calculatedSpeed = MouseSpeed;

            // Based on state, decide what to do with speed
            if (state == 0) // reset
            {
                calculatedSpeed = 0.0f;
            }
            else if (state == 1) // accelerate
            {
                // Find the normalization (e.g. if battery saver is on, emulate the amount of impact that same input would have if it were off)
                float normalization = (float)CalculateSleepTime() / 10;

                // Calculate acceleration
                calculatedSpeed += (1 - MouseSpeed) * MOUSE_ACCELERATION_RATE * normalization;

            }
            else if (state == -1) // max speed
            {
                calculatedSpeed = 1.0f;
            }

            // Ensures mouse speed does not go above 100% or below 0%
            calculatedSpeed = MathUtils.Clampf(calculatedSpeed, 0, 1);

            MouseSpeed = calculatedSpeed;
        }


        // Returns the amount of time the program should "sleep" for before 
        int CalculateSleepTime()
        {
            float deltaTime = LastAction.ElapsedMilliseconds;

            if (!GetIsActive() && deltaTime > AppSettings.Sleep.TimeBeforeSleep)
            {
                return AppSettings.Sleep.SleepRefreshSpeed;
            }
            else if (AppSettings.Sleep.BatterySaverEnabled && BatteryUtils.IsOnBatterySaver())
            {
                return AppSettings.Sleep.BatterySaverRefreshSpeed;
            }
            else
            {
                return AppSettings.Sleep.ActiveRefreshSpeed;
            }
        }



        // Handles scrolling
        bool UpdateRightStick()
        {
            int rx = GetRightStickXRaw();
            int ry = GetRightStickYRaw();

            

            if ((rx != 0 || ry != 0) && !RightStickMoved)
            {
                RightStickMoved = true;
                if (rx != 0.0) Mouse.HorizontalScroll(rx);
                if (ry != 0.0) Mouse.VerticalScroll(ry);

                return true;
            }
            else if (rx == 0 && ry == 0)
            {
                RightStickMoved = false;
            }
            return false;
        }

        // Handles mouse clicks
        bool UpdateTriggers()
        {
            bool leftClick = Status.LeftTrigger > 0;
            bool rightClick = Status.RightTrigger > 0;
            bool middleClick = leftClick && rightClick;

            // middle click
            if (middleClick && !MiddleClickPressed)
            {
                Mouse.XButtonDown(3);
                MiddleClickPressed = true;
            }
            else if (MiddleClickPressed && !middleClick)
            {
                Mouse.XButtonUp(3);
                MiddleClickPressed = false;
            }

            // left click
            if (leftClick && !LeftClickPressed && !middleClick)
            {
                Mouse.LeftButtonDown();
                LeftClickPressed = true;
            }
            else if (LeftClickPressed && !leftClick)
            {
                Mouse.LeftButtonUp();
                LeftClickPressed = false;
            }

            // right click
            if (rightClick && !RightClickPressed && !middleClick)
            {
                Mouse.RightButtonDown();
                RightClickPressed = true;
            }
            else if (RightClickPressed && !rightClick)
            {
                Mouse.RightButtonUp();
                RightClickPressed = false;
            }

            return leftClick || rightClick || middleClick;
        }


        bool UpdateDPad()
        {
            bool up = (Buttons == GamepadButtonFlags.DPadUp);
            bool down = (Buttons == GamepadButtonFlags.DPadDown);
            bool left = (Buttons == GamepadButtonFlags.DPadLeft);
            bool right = (Buttons == GamepadButtonFlags.DPadRight);

            if (up && !DPadUpPressed)
            {
                MediaUtils.IncreaseVolume();
                DPadUpPressed = true;
            }
            else if (DPadUpPressed && !up)
            {
                DPadUpPressed = false;
            }

            if (down && !DPadDownPressed)
            {
                MediaUtils.DecreaseVolume();
                DPadDownPressed = true;
            }
            else if (DPadDownPressed && !down)
            {
                DPadDownPressed= false;
            }

            if (left && !DPadLeftPressed)
            {
                MediaUtils.ToggleMute();
                DPadLeftPressed = true;
            }
            else if (DPadLeftPressed && !left)
            {
                DPadLeftPressed = false;
            }

            if (right && !DPadRightPressed)
            {
                MediaUtils.TogglePlay();
                DPadRightPressed = true;
            }
            else if (DPadRightPressed && !right)
            {
                DPadRightPressed = false;
            }
            
            return up || down || left || right;
        }


        bool UpdateMenuButtonRight()
        {
            bool menuButton = (Buttons == GamepadButtonFlags.Start);

            if (menuButton && !MenuButtonRightPressed)
            {
                MenuButtonRightPressed = true;
                Keyboard.KeyDown(VirtualKeyCode.LWIN);
                Keyboard.KeyUp(VirtualKeyCode.LWIN);
                return true;
            }
            else if (!menuButton && MenuButtonRightPressed)
            {
                MenuButtonLeftPressed = false;
            }
            return false;
        }


        bool UpdateMenuButtonLeft()
        {
            bool startButton = (Buttons == GamepadButtonFlags.Back);

            if (startButton && !MenuButtonLeftPressed)
            {
                MenuButtonLeftPressed = true;
                return true; 
            }
            else if (!startButton && MenuButtonLeftPressed)
            {
                MenuButtonLeftPressed = false;
            }
            return false;
        }


        bool UpdateLeftShoulderButton()
        {
            bool state = (Buttons == GamepadButtonFlags.LeftShoulder);

            if (state)
            {
                return true;
            }
            return false;
        }

        
        bool UpdateRightShoulderButton()
        {
            bool state = (Buttons == GamepadButtonFlags.LeftShoulder);

            if (state)
            {
                return true;
            }
            return false;
        }

        bool PollButtons()
        {
            return ButtonA() || ButtonB() || ButtonX() || ButtonY();
        }


        
        bool ButtonA()
        {
            bool state = (Buttons == GamepadButtonFlags.A);

            if (state)
            {
                return true;
            }
            return false;
        }

        bool ButtonB()
        {
            bool state = (Buttons == GamepadButtonFlags.B);

            if (state)
            {
                return true;
            }
            return false;
        }

        bool ButtonX()
        {
            bool state = (Buttons == GamepadButtonFlags.X);

            if (state)
            {
                return true;
            }
            return false;
        }

        bool ButtonY()
        {
            bool state = (Buttons == GamepadButtonFlags.Y);

            if (state)
            {
                return true;
            }
            return false;
        }


        void SetActive()
        {
            ControllerActive = true;
            LastAction.Restart(); // reset the last action timer to ensure sleep states are handled correctly
        }


        void ResetActive()
        {
            ControllerActive = false;
        }


        public bool GetIsActive()
        {
            return ControllerActive;
        }

        public bool GetIsAsleep()
        {
            if (CalculateSleepTime() == AppSettings.Sleep.SleepRefreshSpeed) return true;
            else return false;
        }



        public float GetMouseSpeed()
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

        // Get raw input from the right stick
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

        // Get raw input from the left stick
        int GetLeftStickXRaw()
        {
            if (Status.LeftThumbX > 0)
            {
                return 1;
            }
            else if (Status.LeftThumbX < 0)
            {
                return -1;
            }
            return 0;
        }

        // Get raw input from the left stick.
        int GetLeftStickYRaw()
        {
            if (Status.LeftThumbY > 0)
            {
                return 1;
            }
            else if (Status.LeftThumbY < 0)
            {
                return -1;
            }
            return 0;
        }

        // Returns whether the given user index has any controllers connected
        public bool GetIsConnected()
        {
            return Controller.IsConnected;
        }
    }
}