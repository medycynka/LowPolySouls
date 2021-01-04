using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SP
{
    public class LocationManager : MonoBehaviour
    {
        [Header("Area Name", order = 1)]
        public string areaName = "";

        [Header("Location Screen Properties", order = 1)]
        public GameObject locationScreen;
        public TextMeshProUGUI locationScreenText;

        [Header("Bonfires in Area", order = 1)]
        public BonfireManager[] bonfiresInArea;

        [Header("Bonfires in Area", order = 1)]
        public AnimationSoundManager playerSoundManager;

        [Header("Area Sounds", order = 1)]
        public AudioClip areaBgMusic;
        public AudioClip[] footSteps;

        [Header("Bools", order = 1)]
        public bool isInside = false;
        public bool isPlayerDead = false;

        [Header("Player Stats", order = 1)]
        public PlayerStats playerStats;
    }
}