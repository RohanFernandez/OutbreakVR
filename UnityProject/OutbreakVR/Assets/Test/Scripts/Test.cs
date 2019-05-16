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
        }
    }
}