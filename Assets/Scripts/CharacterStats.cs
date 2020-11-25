using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{

    public class CharacterStats : MonoBehaviour
    {
        [Header("Health")]
        public float healthLevel = 10;
        public float maxHealth;
        public float currentHealth;

        [Header("Stamina")]
        public float staminaLevel = 10;
        public float maxStamina;
        public float currentStamina;
    }

}
