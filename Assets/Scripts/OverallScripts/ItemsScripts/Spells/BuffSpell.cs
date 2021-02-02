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
        
        public override void AttemptToCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats);
            
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, animatorHandler.transform);
            animatorHandler.PlayTargetAnimation(StaticAnimatorIds.animationIds[spellAnimation], true);
        }

        public override void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats);
            
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, animatorHandler.transform); 
            playerStats.BuffPlayer(buffType, buffRang, buffAmount);
        }
    }
}