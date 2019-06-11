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
            EventHash l_hash = EventManager.GetEventHashtable();
            l_hash.Add(GameEventTypeConst.ID_OBJECTIVE_TRIGGER_ID, a_strObjectiveId);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_LEVEL_OBJECTIVE_TRIGGERED, l_hash);
            EventManager.ReturnHashtableToPool(l_hash);
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

            if (Input.GetKeyUp(KeyCode.O))
            {
                triggerObjective(m_strObjectiveTrigger);
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                EnemyManager.GetEnemyFromPool(ENEMY_TYPE.SECURITY_OFFICER);
            }
        }
    }
}