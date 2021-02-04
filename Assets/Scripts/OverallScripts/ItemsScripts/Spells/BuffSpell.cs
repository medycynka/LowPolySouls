using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.Enums;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.Misc;


namespace SzymonPeszek.Items.Spells
{
    [CreateAssetMenu(menuName = "Spells/Buff Spell")]
    public class BuffSpell : SpellItem
    {
        public StatsBuffType buffType;
        public BuffRang buffRang;
        public float buffAmount;
        
        public override void AttemptToCastSpell(PlayerAnimatorHandler playerAnimatorHandler, PlayerStats playerStats)
        {
            base.AttemptToCastSpell(playerAnimatorHandler, playerStats);
            
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, playerAnimatorHandler.transform);
            playerAnimatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[spellAnimation], true);
        }

        public override void SuccessfullyCastSpell(PlayerAnimatorHandler playerAnimatorHandler, PlayerStats playerStats)
        {
            base.SuccessfullyCastSpell(playerAnimatorHandler, playerStats);
            
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, playerAnimatorHandler.transform); 
            playerStats.BuffPlayer(buffType, buffRang, buffAmount);
        }
    }
}