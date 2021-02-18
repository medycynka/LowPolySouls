using UnityEngine;
using SzymonPeszek.Enums;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.PlayerScripts.Inventory;


namespace SzymonPeszek.BaseClasses
{
    /// <summary>
    /// Base class for spell object
    /// </summary>
    public class SpellItem : Item
    {
        public GameObject spellWarmUpFX;
        public GameObject spellCastFX;
        public string spellAnimation;

        [Header("Spell Cost")]
        public float focusPointCost;
        
        [Header("Spell Type")] 
        public CastingType spellType;

        [Header("Spell Description")]
        [TextArea]
        public string spellDescription;

        /// <summary>
        /// Attempt to cast this spell
        /// </summary>
        /// <param name="playerAnimatorHandler">Player animation manager</param>
        /// <param name="playerStats">Player stats</param>
        public virtual void AttemptToCastSpell(PlayerAnimatorHandler playerAnimatorHandler, PlayerStats playerStats)
        {
            
        }

        /// <summary>
        /// Successfully cast this spell
        /// </summary>
        /// <param name="playerAnimatorHandler">Player animation manager</param>
        /// <param name="playerStats">Player stats</param>
        public virtual void SuccessfullyCastSpell(PlayerAnimatorHandler playerAnimatorHandler, PlayerStats playerStats)
        {
            playerStats.TakeFocusDamage(focusPointCost);
        }
    }
}