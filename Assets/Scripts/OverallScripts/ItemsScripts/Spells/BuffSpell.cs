using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Enums;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.Misc;


namespace SzymonPeszek.Items.Spells
{
    /// <summary>
    /// Class representing buff type spell
    /// </summary>
    [CreateAssetMenu(menuName = "Spells/Buff Spell")]
    public class BuffSpell : SpellItem
    {
        public StatsBuffType buffType;
        public BuffRang buffRang;
        public float buffAmount;
        
        /// <summary>
        /// Attempt to cast this spell
        /// </summary>
        /// <param name="playerAnimatorHandler">Player animation manager</param>
        /// <param name="playerStats">Player stats</param>
        public override void AttemptToCastSpell(PlayerAnimatorHandler playerAnimatorHandler, PlayerStats playerStats)
        {
            base.AttemptToCastSpell(playerAnimatorHandler, playerStats);
            
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, playerAnimatorHandler.transform);
            playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[spellAnimation], true);
        }

        /// <summary>
        /// Successfully cast this spell
        /// </summary>
        /// <param name="playerAnimatorHandler">Player animation manager</param>
        /// <param name="playerStats">Player stats</param>
        public override void SuccessfullyCastSpell(PlayerAnimatorHandler playerAnimatorHandler, PlayerStats playerStats)
        {
            base.SuccessfullyCastSpell(playerAnimatorHandler, playerStats);
            
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, playerAnimatorHandler.transform); 
            playerStats.BuffPlayer(buffType, buffRang, buffAmount);
        }
    }
}