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


        // Values
        private float mouseSpeed;


        // Constants
        private const float MAX_MOUSE_SPEED = 13.0f;
        


        public InputDevice()
        {
            controller = new Controller();
            simulator = new InputSimulator();

            keyboard = simulator.Keyboard;
            mouse = simulator.Mouse;

        }

        public int pollDevice()
        {
            if (controller.IsConnected)
            {
                status = controller.GetState().Gamepad;
                return 1;
            }
            return 0;
        }

        void handleMouseMovement()
        {
            int lx = status.LeftThumbX;
            int ly = status.LeftThumbY;

            if (lx != 0 || ly != 0)
            {
                int curX = MouseUtils.GetMousePos().x;
                int curY = MouseUtils.GetMousePos().y;

                updateMouseSpeed(1);

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
            float accelerationMultiplier = 0.25f; // At what percent of the max speed does the mouse accelerate by per frame

            if (state == 0)
            {
                mouseSpeed = 0f;
            }
            else if (state == 1)
            {
                mouseSpeed = (MAX_MOUSE_SPEED - mouseSpeed) * accelerationMultiplier;
            }
            else if (state == -1)
            {
                mouseSpeed = MAX_MOUSE_SPEED;
            }
            return mouseSpeed;
        }

        void handleScrolling()
        {
            int rx = status.LeftThumbX;
            int ry = status.LeftThumbY;

            if (ry != 0.0)
            {
                mouse.VerticalScroll(ry);
            }
            else if (rx != 0.0)
            {
                mouse.HorizontalScroll(rx);
            }
        }

        //int rx = status.RightThumbX;
        //int ry = status.RightThumbY;
    }
}
