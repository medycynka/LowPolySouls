using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class DeathState : State
    {
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            return this;
        }
    }

}
