using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;

using ControllerToMouse.Settings;
using ControllerToMouse.Utils;

namespace ControllerToMouse.Utils
{
    internal static class MediaUtils
    {
        static private CoreAudioController AudioController = new CoreAudioController();

        // Volume Functionality
        public static void IncreaseVolume()
        {
            if (AudioDeviceExists()) SetRelativeVolume(GlobalSettings.audioStep);
        }

        public static void DecreaseVolume()
        {
            if (AudioDeviceExists()) SetRelativeVolume(-GlobalSettings.audioStep);
        }

        private static void SetRelativeVolume(float step)
        {
            float newVolume = (float)GetVolume() + step;
            newVolume = MathUtils.Clamp(newVolume, 0, 100);

            if (AudioDeviceExists()) AudioController.DefaultPlaybackDevice.Volume = newVolume;
        }

        private static void SetExactVolume(float value)
        {
            value = MathUtils.Clamp(value, 0, 100);
            if (AudioDeviceExists()) AudioController.DefaultPlaybackDevice.Volume = value;
        }

        public static double GetVolume()
        {
            return AudioController.DefaultPlaybackDevice.Volume;
        }


        // Muting Functionality
        public static void ToggleMuteAudio()
        {
            // Sets the mute status to the opposite of what it currently is.
            bool status = !IsMuted();
            SetIsMuted(status);
        }


        public static void SetIsMuted(bool status)
        {
            if (AudioDeviceExists()) AudioController.DefaultPlaybackDevice.Mute(status);
        }

        public static bool IsMuted()
        {
            if (AudioController.DefaultPlaybackDevice.IsMuted) return true;
            else return false;
        }


        // Ensure that the audio device actually exists, for users who somehow don't have one.
        private static bool AudioDeviceExists()
        {
            if (AudioController.DefaultPlaybackDevice != null) return true;
            else return false;
        }


    }
}
