using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class Test : MonoBehaviour
    {
        [SerializeField]
        private string m_strObjectiveTrigger = string.Empty;

        public void onClickCanvasButton()
        {
            Debug.LogError("onClickCanvasButton!!");
        }

        public void onWeaponChangedToChainsaw()
        {
            
        }

        void triggerObjective(string a_strObjectiveId)
        {
            ObjectiveManager.TriggerObjective(a_strObjectiveId);
        }

        void Update()
        {
            //if (Input.GetKeyUp(KeyCode.Q))
            //{
            //    WeaponManager.SetPreviousCategory();
            //}
            //else if (Input.GetKeyUp(KeyCode.E))
            //{
            //    WeaponManager.SetNextCategory();
            //}
            //else if (Input.GetKeyUp(KeyCode.C))
            //{
            //    WeaponManager.SetCurrentWeaponInCategory(WEAPON_CATEGORY_TYPE.MELEE, WEAPON_TYPE.CHAINSAW);
            //}
        }
    }
}