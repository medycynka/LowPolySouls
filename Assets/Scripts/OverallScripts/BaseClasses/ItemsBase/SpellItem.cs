using UnityEngine;
using SzymonPeszek.Enums;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Animations;
using SzymonPeszek.PlayerScripts.Inventory;


namespace SzymonPeszek.BaseClasses
{
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

        public virtual void AttemptToCastSpell(PlayerAnimatorHandler playerAnimatorHandler, PlayerStats playerStats)
        {
            
        }

        public virtual void SuccessfullyCastSpell(PlayerAnimatorHandler playerAnimatorHandler, PlayerStats playerStats)
        {
            playerStats.TakeFocusDamage(focusPointCost);
        }
    }
}