using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;
using WindowsInput.Native;

using ControllerToMouse.Settings;
using ControllerToMouse.Utils;
using WindowsInput;
using System.Runtime.InteropServices;

namespace ControllerToMouse.Utils
{
    internal static class MediaUtils
    {
        static private CoreAudioController AudioController = new CoreAudioController();

        static private InputSimulator InputSimulator = new InputSimulator();

        // Volume Functionality

        // Increase and Decrease volume may seem counterintuitive because you can't specify the amount, however controllers do not have this feature.
        public static void IncreaseVolume()
        {
            if (AudioDeviceExists()) SetRelativeVolume(GlobalSettings.audioStep);
        }

        public static void DecreaseVolume()
        {
            if (AudioDeviceExists()) SetRelativeVolume(-GlobalSettings.audioStep);
        }

        // Changes the relative volume according to its current value [volume (50) + step (5) = newVolume (55)]
        private static void SetRelativeVolume(float step)
        {
            float newVolume = (float)GetVolume() + step;
            newVolume = MathUtils.Clamp(newVolume, 0, 100);

            if (AudioDeviceExists()) AudioController.DefaultPlaybackDevice.Volume = newVolume;
        }

        // Sets the volume to this exact value.
        private static void SetExactVolume(float newVolume)
        {
            newVolume = MathUtils.Clamp(newVolume, 0, 100);
            if (AudioDeviceExists()) AudioController.DefaultPlaybackDevice.Volume = newVolume;
        }

        public static double GetVolume()
        {
            return AudioController.DefaultPlaybackDevice.Volume;
        }


        // Muting Functionality

        // Sets the mute status to the opposite of what it currently is.
        public static void ToggleMute()
        {
            bool status = !IsMuted();
            SetIsMuted(status);
        }

        public static void SetIsMuted(bool status)
        {
            if (AudioDeviceExists()) AudioController.DefaultPlaybackDevice.Mute(status);
        }

        // Returns if the current audio device is muted
        public static bool IsMuted()
        {
            return (AudioController.DefaultPlaybackDevice.IsMuted);
        }


        // Ensure that the audio device actually exists, for users who somehow don't have one.
        private static bool AudioDeviceExists()
        {
            try
            {
                return (AudioController.DefaultPlaybackDevice != null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }


        // Play / Pause

        // Should be rewritten at some point, using winAPI and more robust control but not necessary for now.
        public static void TogglePlay()
        {
            InputSimulator.Keyboard.KeyPress(VirtualKeyCode.MEDIA_PLAY_PAUSE);
        }
    }
}
