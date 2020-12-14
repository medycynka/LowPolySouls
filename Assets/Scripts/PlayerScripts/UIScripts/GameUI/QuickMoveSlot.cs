using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SP { 
    public class QuickMoveSlot : MonoBehaviour
    {
        public GameObject bonfirePrefab;
        public TextMeshProUGUI locationName;

        public void AddSlot(GameObject bonfire_)
        {
            bonfirePrefab = bonfire_;
            locationName.text = bonfirePrefab.GetComponent<BonfireManager>().locationName;
            gameObject.SetActive(true);
        }

        public void ClearSlot()
        {
            gameObject.SetActive(false);
        }

        public void TeleportHere()
        {
            bonfirePrefab.GetComponent<BonfireInteraction>().QuickMove();
        }
    }
}