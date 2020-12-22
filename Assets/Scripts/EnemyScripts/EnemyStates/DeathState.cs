using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class DeathState : State
    {
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
        {
            if (enemyManager.isAlive)
            {
                enemyManager.isAlive = false;
                enemyManager.HandleDeath();
            }
            
            return this;
        }
    }

}
