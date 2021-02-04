using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.Misc;
using SzymonPeszek.PlayerScripts.Inventory;


namespace SzymonPeszek.Items.Spells
{
    [CreateAssetMenu(menuName = "Spells/Heal Spell")]
    public class HealSpell : SpellItem
    {
        public float healAmount;

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
            playerStats.HealPlayer(healAmount);
        }
    }
}