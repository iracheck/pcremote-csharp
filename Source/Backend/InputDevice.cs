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

        private bool controllerActive;
        private bool leftClickDown = false;
        private bool rightClickDown = false;
        private bool middleClickDown = false;


        // Values
        private float mouseSpeed;


        // Constants
        private const float MAX_MOUSE_SPEED = 1.0f;



        public InputDevice()
        {
            Console.WriteLine("Creating new input device...");
            controller = new Controller(UserIndex.One);
            simulator = new InputSimulator();

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
                handleMouseClicks();
                return 1;
            }
            else
            {
                Console.WriteLine("Controller disconnected.");
            }
            return 0;
        }

        // Handles all mouse movements
        void handleMouseMovement()
        {
            int lx = status.LeftThumbX / 1000; // Must divide by a large number, as raw controller input provides a very large number. Returns num pixels to move by
            int ly = status.LeftThumbY / 1000;


            if (lx != 0 || ly != 0)
            {
                updateMouseSpeed(1);
                setActive();

                lx = (int)(lx * mouseSpeed);
                ly = (int)(ly * mouseSpeed) * -1;

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
            float accelerationMultiplier = 1.0f / 20f; // This is an arbitrary number that slows down the speed of the mouse acceleration, to allow precise movement.

            if (state == 0) // reset
            {
                mouseSpeed = 0f;
            }
            else if (state == 1) // accelerate
            {
                mouseSpeed += (MAX_MOUSE_SPEED - mouseSpeed) * accelerationMultiplier;
            }
            else if (state == -1) // max speed
            {
                mouseSpeed = MAX_MOUSE_SPEED;
            }
            Console.WriteLine("Current speed: " + mouseSpeed);
            return mouseSpeed;
        }


        // Handles scrolling
        void handleScrolling()
        {
            int rx = getRightStickXRaw();
            int ry = getRightStickYRaw();

            if (rx != 0 || ry != 0)
            {
                setActive();
                if (ry != 0.0) mouse.VerticalScroll(ry);
                if (rx != 0.0) mouse.HorizontalScroll(rx);
            }
        }

        void handleMouseClicks()
        {
            bool leftClick = status.LeftTrigger > 0;
            bool rightClick = status.RightTrigger > 0;
            bool middleClick = leftClick && rightClick;

            if (leftClick || rightClick) setActive();

            if (middleClick)
            {
                mouse.XButtonDown(3);

                return;
            }
            else if (middleClick & !middleClickDown)
            {
                mouse.XButtonUp(1);
            }

            if (leftClick && !leftClickDown)
            {
                mouse.LeftButtonDown();
                leftClickDown = true;

                return;
            }
            else if (leftClickDown && !leftClick)
            {
                mouse.LeftButtonUp();
                leftClickDown = false;
            }

            if (rightClick && !rightClickDown)
            {
                mouse.RightButtonDown();
                rightClickDown = true;

                return;
            }
            else if (rightClickDown && !rightClick)
            {
                mouse.RightButtonUp();
                rightClickDown = false;
            }

        }


        void setActive()
        {
            controllerActive = true;
        }

        void resetActive()
        {
            controllerActive = false;
        }


        // A couple getters to make life easier
        float getMouseSpeed()
        {
            return mouseSpeed;
        }

        int getRightStickXRaw()
        {
            if (status.RightThumbX > 0)
            {
                return 1;
            }
            else if (status.RightThumbX < 0)
            {
                return -1;
            }
            return 0;
        }

        int getRightStickYRaw()
        {
            if (status.RightThumbY > 0)
            {
                return 1;
            }
            else if (status.RightThumbY < 0)
            {
                return -1;
            }
            return 0;
        }

        bool getRightClickDown()
        {
            return rightClickDown;
        }

        bool getLeftClickDown()
        {
            return leftClickDown;
        }

        bool getMiddleClickDown()
        {
            return middleClickDown;
        }

        void setRightClickDown(bool state)
        {
            rightClickDown = state;
        }

        void setLeftClickDown(bool state)
        {
            leftClickDown = state;
        }

        void setMiddleClickDown(bool state)
        {
            middleClickDown = state;
        }
    }
}
