﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SP
{

    public class AttackState : State
    {
        public CombatStanceState combatStanceState;
        public DeathState deathState;

        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyStats.currentHealth > 0)
            {
                //Select one of our many attacks based on attack scores
                //if the selected attack is not able to be used because of bad angle or distance, select a new attack
                //if the attack is viable, stop our movement and attack our target
                //set our recovery timer to the attacks recovery time
                //return the combat stance state
                Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

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
                                enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                                enemyAnimatorManager.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                                enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
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
