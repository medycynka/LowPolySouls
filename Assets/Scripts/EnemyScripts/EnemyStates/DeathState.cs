using SzymonPeszek.BaseClasses;
using SzymonPeszek.EnemyScripts.Animations;


namespace SzymonPeszek.EnemyScripts.States
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
