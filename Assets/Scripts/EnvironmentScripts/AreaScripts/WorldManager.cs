using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class WorldManager : MonoBehaviour
    {
        public BossAreaManager[] bossAreaManagers;
        public BonfireManager[] bonfireManagers;

        public WeaponItem[] weaponsHolder;
        public WeaponItem[] shieldsHolder;
        public EquipmentItem[] helmetsHolder;
        public EquipmentItem[] chestsHolder;
        public EquipmentItem[] shouldersHolder;
        public EquipmentItem[] handsHolder;
        public EquipmentItem[] legsHolder;
        public EquipmentItem[] feetHolder;
        public EquipmentItem[] ringsHolder;
        public ConsumableItem[] consumableHolder;
        private void Awake()
        {
            DataManager dataManager = SaveManager.LoadGame();

            bossAreaManagers = GetComponentsInChildren<BossAreaManager>();
            bonfireManagers = GetComponentsInChildren<BonfireManager>();

            if (dataManager != null)
            {
                if (!dataManager.isFirstStart)
                {
                    #region Boss Initializetion
                    for (int i = 0; i < bossAreaManagers.Length; i++)
                    {
                        bossAreaManagers[i].isBossAlive = dataManager.areaBossesAlive[i];
                    }
                    #endregion

                    #region Bonfire Initialization
                    for (int i = 0; i < bonfireManagers.Length; i++)
                    {
                        bonfireManagers[i].isActivated = dataManager.bonfireActivators[i];
                        bonfireManagers[i].showRestPopUp = dataManager.bonfireActivators[i];
                    }
                    #endregion
                }
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < bossAreaManagers.Length; i++)
            {
                SettingsHolder.bossAreaAlive[i] = bossAreaManagers[i].isBossAlive;
            }

            for (int i = 0; i < bonfireManagers.Length; i++)
            {
                SettingsHolder.bonfiresAcrivation[i] = bonfireManagers[i].isActivated;
            }
        }
    }
}