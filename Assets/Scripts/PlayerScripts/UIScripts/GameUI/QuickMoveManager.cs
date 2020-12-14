using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SP
{
    public class QuickMoveManager : MonoBehaviour
    {
        public GameObject backgroundScreen;
        public GameObject buttonPrefab;
        public GameObject[] bonfireList;
        List<QuickMoveSlot> quickMoveSlots;


        public void UpdateActivBonfireList()
        {
            int id_ = 0;
            quickMoveSlots = new List<QuickMoveSlot>(backgroundScreen.GetComponentsInChildren<QuickMoveSlot>());
            
            foreach(var e_ in quickMoveSlots)
            {
                e_.ClearSlot();
            }

            foreach(var bonfire_ in bonfireList)
            {
                if (bonfire_.GetComponent<BonfireManager>().isActivated)
                {
                    bonfire_.GetComponent<BonfireManager>().qucikMoveID = id_;
                    Instantiate(buttonPrefab, backgroundScreen.transform);

                    buttonPrefab.GetComponent<QuickMoveSlot>().AddSlot(bonfire_);
                }

                id_++;
            }
        }
    }
}