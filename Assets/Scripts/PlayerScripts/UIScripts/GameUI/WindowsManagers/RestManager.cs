using UnityEngine;
using SzymonPeszek.Items.Bonfire;


namespace SzymonPeszek.GameUI.WindowsManagers
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