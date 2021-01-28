using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SP
{
    public class EnemyManager : CharacterManager
    {
        [HideInInspector] public EnemyLocomotionManager enemyLocomotionManager;
        [HideInInspector] public EnemyAnimationManager enemyAnimationManager;
        EnemyStats enemyStats;
        EnemyDrops enemyDrops; 
        List<Material> characterMaterials;
        int edgeWidthId;
        int noiceScaleId;
        int fresnelPowerId;
        int glowId;
        int aliveId;
        int disolveId;

        [Header("Manager Properties", order = 1)]
        [Header("Bools", order = 2)]
        public bool isPreformingAction;
        public bool isInteracting;
        public bool shouldDrop = true;
        public bool isAlive = true;
        public bool deadFromBackStab;
        public bool shouldFollowTarget = false;

        [Header("Current Target", order = 2)]
        public CharacterStats currentTarget;

        [Header("Current State from FSM", order = 2)]
        public State currentState;

        [Header("A.I Settings", order = 2)]
        public float distanceFromTarget;
        public float viewableAngle;
        public float detectionRadius = 15;
        public float maximumAttackRange = 1.5f;
        public float maximumDetectionAngle = 75;
        public float minimumDetectionAngle = -75;

        [Header("Recovery Timer", order = 2)]
        public float currentRecoveryTime = 0;

        [Header("Death Disolve Effect", order = 2)]
        public float disolveEdgeWidth = 0.015f;
        public float disolveNoiseScale = 30.0f;
        public float disolveFresnelPower = 15.0f;
        public float currentDisolveTime = 0.0f;
        public float disolveDurationTime = 4.0f;
        public bool shouldGlow = false;
        public float objectDestructionDuration = 5.0f;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
            enemyStats = GetComponent<EnemyStats>();
            enemyDrops = GetComponent<EnemyDrops>();
            backStabCollider = GetComponentInChildren<BackStabCollider>();

            characterMaterials = new List<Material>();
            Renderer[] renders = GetComponentsInChildren<Renderer>();

            edgeWidthId = Shader.PropertyToID("_EdgeWidth");
            noiceScaleId = Shader.PropertyToID("_NoiceScale");
            fresnelPowerId = Shader.PropertyToID("_FresnelPower");
            glowId = Shader.PropertyToID("_ShouldBlink");
            aliveId = Shader.PropertyToID("_IsAlive");
            disolveId = Shader.PropertyToID("_DisolveValue");

            foreach (var r in renders)
            {
                r.material.SetFloat(edgeWidthId, disolveEdgeWidth);
                r.material.SetFloat(noiceScaleId, disolveNoiseScale);
                r.material.SetFloat(fresnelPowerId, disolveFresnelPower);
                characterMaterials.Add(r.material);
            }
        }

        private void Update()
        {
            HandleRecoveryTimer();

            isInteracting = enemyAnimationManager.anim.GetBool(StaticAnimatorIds.EnemyAnimationIds[StaticAnimatorIds.IsInteractingName]);
            enemyAnimationManager.anim.SetBool(StaticAnimatorIds.EnemyAnimationIds[StaticAnimatorIds.IsDeadName], enemyStats.currentHealth <= 0.0f);
        }

        private void FixedUpdate()
        {
            HandleStateMachine();
        }

        private void HandleStateMachine()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimationManager);

                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPreformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPreformingAction = false;
                }
            }
        }

        public void HandleGettingBackStabbed(float damage)
        {
            enemyStats.TakeDamage(damage, true);
        }

        public void HandleDeath()
        {
            isAlive = false;
            enemyStats.currentHealth = 0;
            enemyStats.animator.PlayTargetAnimation(deadFromBackStab ? StaticAnimatorIds.EnemyAnimationIds[StaticAnimatorIds.BackStabbedName] : StaticAnimatorIds.EnemyAnimationIds[StaticAnimatorIds.Death01Name], true);

            #region Disolve Effect
            foreach (var cM in characterMaterials)
            {
                cM.SetInt(aliveId, 0);
            }

            if (shouldGlow)
            {
                foreach (var cM in characterMaterials)
                {
                    cM.SetInt(glowId, 1);
                }
            }

            StartCoroutine(DisolveAfterDeath());
            #endregion

            enemyStats.playerStats.soulsAmount += enemyStats.soulsGiveAmount;
            enemyStats.playerStats.uiManager.currentSoulsAmount.text = enemyStats.playerStats.soulsAmount.ToString();

            if (shouldDrop)
            {
                enemyDrops.DropPickUp();
                shouldDrop = false;
            }

            Destroy(enemyStats.enemyObject, deadFromBackStab ? objectDestructionDuration + 1.5f : objectDestructionDuration);
        }

        private IEnumerator DisolveAfterDeath()
        {
            if (deadFromBackStab)
            {
                yield return CoroutineYielder.disolveAfterBackStabWaiter;
            }
            
            while(currentDisolveTime < disolveDurationTime)
            {
                foreach (var cM in characterMaterials)
                {
                    cM.SetFloat(disolveId, Mathf.Lerp(-0.1f, 1.0f, currentDisolveTime / disolveDurationTime));
                }
                
                currentDisolveTime += Time.deltaTime;

                yield return null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.blue;
            Quaternion leftRayRotation = Quaternion.AngleAxis(minimumDetectionAngle, Vector3.up);
            Quaternion rightRayRotation = Quaternion.AngleAxis(maximumDetectionAngle, Vector3.up);
            Vector3 leftRayDirection = leftRayRotation * transform.forward;
            Vector3 rightRayDirection = rightRayRotation * transform.forward;
            Gizmos.DrawRay(transform.position, leftRayDirection * detectionRadius);
            Gizmos.DrawRay(transform.position, rightRayDirection * detectionRadius);
        }
    }
}