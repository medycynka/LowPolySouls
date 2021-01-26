using System.Collections;
using System.Collections.Generic;
using SP;
using UnityEngine;

namespace SP
{
    public class RestManager : MonoBehaviour
    {
        public BonfireInteraction bonfireInteraction;

        public void GetUp()
        {
            bonfireInteraction.GetUp();
        }
    }
}