using UnityEngine;
using SzymonPeszek.EnemyScripts;
using SzymonPeszek.EnemyScripts.Animations;


namespace SzymonPeszek.BaseClasses
{
    public abstract class State : MonoBehaviour
    {
        public abstract State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager);
    }

}