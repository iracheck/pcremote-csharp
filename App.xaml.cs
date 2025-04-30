using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

namespace ControllerToMouse
{
    public partial class App : Application
    {
        Route route;
        LidarCam camera1;
        LidarCam camera2;

        bool droneActive = true;

        public static void InitializeDrone()
        {
            route = getRoute();
            camera1 = getComponent<lidarCamera[0]>();
            camera2 = getComponent<lidarCamera[1]>();

            if (route == null)
            {
                CreateNewRoute();
            }

            if (camera1 == null || camera2 == null)
            {
                Console.WriteLine("Camera is not functioning. Please check the drone");
            }
        }

        public static bool CollisionAvoidance()
        {
            CameraMap cameramap = getCameraMap();

            if (cameramap.getCollisionLocations().length != 0)
            {
                for (int i = 0; i < cameramap.getCollisionLocations().length; i++)
                {
                    if (cameramap.getCollisionLocations()[i].isOnRoute())
                    {
                        return false;
                    }
                    else return true;
                }
            }

            Sleep(0.5);
        }
    }
}
