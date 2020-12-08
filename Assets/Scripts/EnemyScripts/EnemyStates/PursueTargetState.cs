using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP {

    public class PursueTargetState : State
    {
        public CombatStanceState combatStanceState;
        public DeathState deathState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyStats.currentHealth > 0)
            {
                //Chase the target
                //If within attack range, return combat stance state
                //if target is out of range, return this state and continue to chase target

                return this;
            }
            else
            {
                return deathState;
            }
        }
    }

}