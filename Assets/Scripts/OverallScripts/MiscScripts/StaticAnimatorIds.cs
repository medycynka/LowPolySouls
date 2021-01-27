using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public static class StaticAnimatorIds
    {
        #region Animation names
        public const string VerticalName = "Vertical";
        public const string HorizontalName = "Horizontal";
        public const string IsInteractingName = "isInteracting";
        public const string CanDoComboName = "canDoCombo";
        public const string IsInAirName = "isInAir";
        public const string IsUsingRightHandName = "isUsingRightHand";
        public const string IsUsingLeftHandName = "isUsingLeftHand";
        public const string IsInvulnerableName = "isInvulnerable";
        public const string IsDeadName = "isDead";
        public const string EmptyName = "Empty";
        public const string StandUpName = "Stand Up";
        public const string SitName = "Sit Down";
        public const string PickUpName = "Pick_Up_Item";
        public const string EstusName = "Use Estus";
        public const string UseItemName = "Use Item";
        public const string RollName = "Rolling";
        public const string BackStepName = "Backstep";
        public const string JumpName = "Jump";
        public const string FallName = "Falling";
        public const string LandName = "Land";
        public const string Damage01Name = "Damage_01";
        public const string Death01Name = "Dead_01";
        public const string GetUpName = "Get Up";
        public const string SleepName = "Sleep";
        public const string FogRemoveName = "Remove Fog Wall";
        public const string BackStabName = "Back Stab";
        public const string BackStabbedName = "Back Stabbed";
        public const string LayDownName = "Laying Down";
        #endregion
        
        #region Player Animation Ids
        public static int VerticalId;
        public static int HorizontalId;
        public static int IsInteractingId;
        public static int CanDoComboId;
        public static int IsInAirId;
        public static int IsUsingRightHandId;
        public static int IsUsingLeftHandId;
        public static int IsInvulnerableId;
        public static int IsDeadId;
        public static int EmptyId;
        public static int StandUpId;
        public static int SitId;
        public static int PickUpId;
        public static int EstusId;
        public static int UseItemId;
        public static int RollId;
        public static int BackStepId;
        public static int JumpId;
        public static int FallId;
        public static int LandId;
        public static int Damage01Id;
        public static int Death01Id;
        public static int FogRemoveId;
        public static int BackStabId;
        public static int BackStabbedId;
        public static int LayDownId;
        #endregion
        
        #region Enemy Animation Ids
        public static int EnemyVerticalId;
        public static int EnemyHorizontalId;
        public static int EnemyIsInteractingId;
        public static int EnemyIsDeadId;
        public static int EnemyEmptyId;
        public static int EnemyDamage01Id;
        public static int EnemyDeath01Id;
        public static int EnemyGetUpId;
        public static int EnemySleepId;
        public static int EnemyBackStabId;
        public static int EnemyBackStabbedId;
        public static int EnemyLayDownId;
        #endregion
    }
}