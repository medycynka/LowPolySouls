using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{

    public class CharacterStats : MonoBehaviour
    {
        [Header("Health")]
        public float healthLevel = 10f;
        public float maxHealth;
        public float currentHealth;

        [Header("Stamina")]
        public float staminaLevel = 10f;
        public float maxStamina;
        public float currentStamina;

        [Header("Atributes")]
        public float baseArmor = 5f;
        public float Strength = 4f;      // 1 Strenght = +1 attack damage and +2.5 max health
        public float Agility = 1f;       // 1 agility = +2.5 stamina and +0.5 armor
        public float Defence = 3f;       // +2.5 to defence
        public float bonusHealth = 2f;   // +10 to maxHealth
        public float bonusStamina = 2f;  // +10 to maxStamina
    }

}
