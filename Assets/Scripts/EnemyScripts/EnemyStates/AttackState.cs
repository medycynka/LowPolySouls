using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SP
{

    public class AttackState : State
    {
        [Header("Attack State", order = 0)]
        [Header("Possible After States", order = 1)]
        public IdleState idleState;
        public CombatStanceState combatStanceState;
        public DeathState deathState;

        [Header("Enemy Attacks", order = 1)]
        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
        {
            if (enemyStats.currentHealth > 0)
            {
                if(enemyManager.currentTarget.currentHealth <= 0)
                {
                    return idleState;
                }

                Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
                enemyManager.viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (enemyManager.isPreformingAction)
                    return combatStanceState;

                if (currentAttack != null)
                {
                    if (enemyManager.distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
                    {
                        return this;
                    }
                    else if (enemyManager.distanceFromTarget < currentAttack.maximumDistanceNeededToAttack)
                    {
                        if (enemyManager.viewableAngle <= currentAttack.maximumAttackAngle && enemyManager.viewableAngle >= currentAttack.minimumAttackAngle)
                        {
                            if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPreformingAction == false)
                            {
                                enemyAnimationManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                                enemyAnimationManager.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                                enemyAnimationManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                                enemyManager.isPreformingAction = true;
                                enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                                currentAttack = null;

                                return combatStanceState;
                            }
                        }
                    }
                }
                else
                {
                    GetNewAttack(enemyManager);
                }

                return combatStanceState;
            }
            else
            {
                return deathState;
            }
        }

        private void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetsDirection = enemyManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && enemyManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }


            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && enemyManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (currentAttack != null)
                            return;

                        temporaryScore += enemyAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }
    }
}