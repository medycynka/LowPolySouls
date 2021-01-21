using System.Collections;
using System.Collections.Generic;
using SP;
using UnityEngine;

namespace SP
{
    public class DeathJumpZone : MonoBehaviour
    {
        public bool isInside;
        public Transform dropPosition;
        
        private PlayerStats playerStats;
        private bool insideReset = true;
        
        private void OnTriggerEnter(Collider other)
        {
            isInside = true;

            if (insideReset)
            {
                if (playerStats == null)
                {
                    playerStats = other.GetComponent<PlayerStats>();
                }

                playerStats.isJumpDeath = true;
                playerStats.jumpDeathDropPosition = dropPosition.position;
                playerStats.TakeDamage(1500f);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (isInside && insideReset)
            {
                insideReset = false;
            }
        }
    }
}