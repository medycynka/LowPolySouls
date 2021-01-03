using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public static class SettingsHolder
    {
        // Settings
        public static int resolutionID = 0;
        public static bool isFullscreen = false;
        public static int qualityID = 2;
        public static float mouseSensibility = 25;
        public static float soundVolume = 1;

        // Main Menu
        public static bool isCharacterCreated = false;
        public static string playerName = "";
        public static bool isMale = true;
        public static int headID = 0;
        public static int hairID = -1;
        public static int eyebrowID = -1;
        public static int earID = 1;
        public static int facialHairID = -1;

        // Main menu
    }
}