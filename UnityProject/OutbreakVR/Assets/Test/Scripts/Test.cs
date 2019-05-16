using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class Test : MonoBehaviour
    {
        public void onClickCanvasButton()
        {
            Debug.LogError("onClickCanvasButton!!");
        }

        public void onWeaponChangedToChainsaw()
        {
            
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Q))
            {
                WeaponManager.SetPreviousCategory();
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                WeaponManager.SetNextCategory();
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                WeaponManager.SetCurrentWeaponInCategory(WEAPON_CATEGORY_TYPE.MELEE, WEAPON_TYPE.CHAINSAW);
            }
        }
    }
}