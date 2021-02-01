using UnityEngine;
using TMPro;
using SzymonPeszek.PlayerScripts;


namespace SzymonPeszek.GameUI.WindowsManagers
{
    public class StatWinodowManager : MonoBehaviour
    {
        public PlayerStats playerStats;
        public int levelToAdd = 0;
        public int soulsCost = 0;
        public float strenghtToAdd = 0;
        public float agilityToAdd = 0;
        public float defenceToAdd = 0;
        public float healthToAdd = 0;
        public float staminaToAdd = 0;

        public TextMeshProUGUI visibleLevel;
        public TextMeshProUGUI visibleSouls;
        public TextMeshProUGUI visibleStrength;
        public TextMeshProUGUI visibleAgility;
        public TextMeshProUGUI visibleDefence;
        public TextMeshProUGUI visibleHealth;
        public TextMeshProUGUI visibleStamina;

        private bool _shouldUpdateSouls = true;

        private void Start()
        {
            visibleLevel.color = Color.white;
            visibleSouls.color = Color.white;
            visibleStrength.color = Color.white;
            visibleAgility.color = Color.white;
            visibleDefence.color = Color.white;
            visibleHealth.color = Color.white;
            visibleStamina.color = Color.white;

            UpdateVisibleValues();
        }

        public void UpdateVisibleValues()
        {
            visibleLevel.text = playerStats.playerLevel.ToString();
            visibleSouls.color = Color.white;
            visibleSouls.text = soulsCost.ToString();
            visibleStrength.text = playerStats.Strength.ToString();
            visibleAgility.text = playerStats.Agility.ToString();
            visibleDefence.text = playerStats.Defence.ToString();
            visibleHealth.text = playerStats.bonusHealth.ToString();
            visibleStamina.text = playerStats.bonusStamina.ToString();
        }

        public void UpdateLevel(bool update)
        {
            if (update)
            {
                levelToAdd += 1;
            }
            else
            {
                levelToAdd -= 1;
                if (levelToAdd < 0)
                {
                    levelToAdd = 0;
                }
            }

            visibleLevel.text = (playerStats.playerLevel + levelToAdd).ToString();
        }

        public void UpdateSouls()
        {
            if (levelToAdd > 0)
            {
                soulsCost = playerStats.CalculateSoulsCost(playerStats.playerLevel + levelToAdd);

                if (soulsCost > playerStats.soulsAmount)
                {
                    visibleSouls.color = Color.red;
                }
                else
                {
                    visibleSouls.color = Color.white;
                }
            }
            else
            {
                visibleSouls.color = Color.white;
                soulsCost = 0;
            }

            visibleSouls.text = soulsCost.ToString();
        }

        public void UpdateStrenght(bool update) 
        {
            if (update)
            {
                strenghtToAdd += 1;
                if (playerStats.Strength + strenghtToAdd > 99)
                {
                    _shouldUpdateSouls = false;
                    strenghtToAdd = 99 - playerStats.Strength;
                }
            }
            else
            {
                strenghtToAdd -= 1;
                if (strenghtToAdd < 0)
                {
                    strenghtToAdd = 0;
                }
            }

            if (_shouldUpdateSouls)
            {
                UpdateLevel(update);
                UpdateSouls();
            }

            _shouldUpdateSouls = true;
            visibleStrength.text = (playerStats.Strength + strenghtToAdd).ToString();
        }

        public void UpdateAgility(bool update)
        {
            if (update)
            {
                agilityToAdd += 1;
                if (playerStats.Agility + agilityToAdd > 99)
                {
                    _shouldUpdateSouls = false;
                    agilityToAdd = 99 - playerStats.Agility;
                }
            }
            else
            {
                agilityToAdd -= 1;
                if (agilityToAdd < 0)
                {
                    agilityToAdd = 0;
                }
            }

            if (_shouldUpdateSouls)
            {
                UpdateLevel(update);
                UpdateSouls();
            }

            _shouldUpdateSouls = true;
            visibleAgility.text = (playerStats.Agility + agilityToAdd).ToString();
        }

        public void UpdateDefence(bool update)
        {
            if (update)
            {
                defenceToAdd += 1;
                if (playerStats.Defence + defenceToAdd > 99)
                {
                    _shouldUpdateSouls = false;
                    defenceToAdd = 99 - playerStats.Defence;
                }
            }
            else
            {
                defenceToAdd -= 1;
                if (defenceToAdd < 0)
                {
                    defenceToAdd = 0;
                }
            }

            if (_shouldUpdateSouls)
            {
                UpdateLevel(update);
                UpdateSouls();
            }

            _shouldUpdateSouls = true;
            visibleDefence.text = (playerStats.Defence + defenceToAdd).ToString();
        }

        public void UpdateHealth(bool update)
        {
            if (update)
            {
                healthToAdd += 1;
                if (playerStats.bonusHealth + healthToAdd > 99)
                {
                    _shouldUpdateSouls = false;
                    healthToAdd = 99 - playerStats.bonusHealth;
                }
            }
            else
            {
                healthToAdd -= 1;
                if (healthToAdd < 0)
                {
                    healthToAdd = 0;
                }
            }

            if (_shouldUpdateSouls)
            {
                UpdateLevel(update);
                UpdateSouls();
            }

            _shouldUpdateSouls = true;
            visibleHealth.text = (playerStats.bonusHealth + healthToAdd).ToString();
        }

        public void UpdateStamina(bool update)
        {
            if (update)
            {
                staminaToAdd += 1;
                if (playerStats.bonusStamina + staminaToAdd > 99)
                {
                    _shouldUpdateSouls = false;
                    staminaToAdd = 99 - playerStats.bonusStamina;
                }
            }
            else
            {
                staminaToAdd -= 1;
                if (staminaToAdd < 0)
                {
                    staminaToAdd = 0;
                }
            }

            if (_shouldUpdateSouls)
            {
                UpdateLevel(update);
                UpdateSouls();
            }

            _shouldUpdateSouls = true;
            visibleStamina.text = (playerStats.bonusStamina + staminaToAdd).ToString();
        }

        public void UpgradePlayer()
        {
            if (soulsCost <= playerStats.soulsAmount)
            {
                playerStats.playerLevel += levelToAdd;
                playerStats.soulsAmount -= soulsCost;
                playerStats.Strength += strenghtToAdd;
                playerStats.Agility += agilityToAdd;
                playerStats.Defence += defenceToAdd;
                playerStats.bonusHealth += healthToAdd;
                playerStats.bonusStamina += staminaToAdd;

                playerStats.UpdatePlayerStats();

                ResetAddedStatsValues();
                UpdateVisibleValues();
            }
        }

        public void ResetAddedStatsValues()
        {
            levelToAdd = 0;
            soulsCost = 0;
            strenghtToAdd = 0;
            agilityToAdd = 0;
            defenceToAdd = 0;
            healthToAdd = 0;
            staminaToAdd = 0;
        }
    }

}