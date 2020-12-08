using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SP
{

    public class AttackState : State
    {
        public CombatStanceState combatStanceState;
        public DeathState deathState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyStats.currentHealth > 0)
            {
                //Select one of our many attacks based on attack scores
                //if the selected attack is not able to be used because of bad angle or distance, select a new attack
                //if the attack is viable, stop our movement and attack our target
                //set our recovery timer to the attacks recovery time
                //return the combat stance state

                return this;
            }
            else
            {
                return deathState;
            }
        }
    }

}
