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

        private const int frameCheckRate = 5;
        private const int bossCheckVal = 0;
        private const int bonfireCheckVal = 1;
        
        private void Awake()
        {
            SettingsHolder.worldManager = this;
            
            bossAreaManagers = GetComponentsInChildren<BossAreaManager>();
            bonfireManagers = GetComponentsInChildren<BonfireManager>();

            DataManager dataManager = SettingsHolder.dataManager;

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
                
                #region Current Equipment Initialization
                for (int i = 0; i < SettingsHolder.partsID.Length; i++)
                {
                    SettingsHolder.partsID[i] = dataManager.partsID[i];
                    SettingsHolder.partsArmor[i] = dataManager.partsArmor[i];
                }
                
                CurrentEquipments player = GameObject.FindObjectOfType<CurrentEquipments>();
                if (player != null)
                {
                    player.InitializeCurrentEquipment();
                    player.EquipPlayerWithCurrentItems();
                    player.UpdateArmorValue();
                }
                #endregion
            }
        }

        private void FixedUpdate()
        {
            if (Time.frameCount % frameCheckRate == bossCheckVal)
            {
                for (int i = 0; i < bossAreaManagers.Length; i++)
                {
                    SettingsHolder.bossAreaAlive[i] = bossAreaManagers[i].isBossAlive;
                }
            }
            else if (Time.frameCount % frameCheckRate == bonfireCheckVal)
            {
                for (int i = 0; i < bonfireManagers.Length; i++)
                {
                    SettingsHolder.bonfiresAcrivation[i] = bonfireManagers[i].isActivated;
                }
            }
        }
    }
}