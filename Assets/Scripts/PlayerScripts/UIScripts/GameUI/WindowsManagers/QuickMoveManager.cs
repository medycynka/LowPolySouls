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
        public BonfireManager[] bonfireList;
        private QuickMoveSlot[] quickMoveSlots;


        public void UpdateActiveBonfireList()
        {
            int id = 0;
            quickMoveSlots = backgroundScreen.GetComponentsInChildren<QuickMoveSlot>();

            for (var i = 0; i < quickMoveSlots.Length; i++)
            {
                quickMoveSlots[i].ClearSlot();
            }

            for (var i = 0; i < bonfireList.Length; i++)
            {
                if (bonfireList[i].isActivated)
                {
                    bonfireList[i].qucikMoveID = id;
                    Instantiate(buttonPrefab, backgroundScreen.transform);

                    buttonPrefab.GetComponent<QuickMoveSlot>().AddSlot(bonfireList[i].gameObject);
                }

                id++;
            }
        }
    }
}