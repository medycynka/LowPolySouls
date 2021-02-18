using UnityEngine;
using SzymonPeszek.BaseClasses;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.Misc;
using SzymonPeszek.PlayerScripts.Inventory;


namespace SzymonPeszek.Items.Spells
{
    /// <summary>
    /// Class representing healing type spell
    /// </summary>
    [CreateAssetMenu(menuName = "Spells/Heal Spell")]
    public class HealSpell : SpellItem
    {
        public float healAmount;

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
            playerStats.HealPlayer(healAmount);
        }
    }
}