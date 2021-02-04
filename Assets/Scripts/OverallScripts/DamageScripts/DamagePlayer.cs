using UnityEngine;
using SzymonPeszek.PlayerScripts;


namespace SzymonPeszek.Damage
{
    public class DamagePlayer : MonoBehaviour
    {
        [Header("Damage Value")]
        public float damage = 25;

        private void OnTriggerEnter(Collider other)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }
        }
    }

}