using ns_Mashmo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_mashmo
{
    public class LevelController_Training : MonoBehaviour
    {
        /// <summary>
        /// the objective trigger that will be fired on reloading during the training
        /// </summary>
        [SerializeField]
        string m_strObjTriggerOnReload = string.Empty;

        void Awake()
        {
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_WEAPON_RELOADED, onWeaponReloaded);
        }

        void OnDestroy()
        {
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_WEAPON_RELOADED, onWeaponReloaded);
        }

        /// <summary>
        /// event callback on weapon reloaded
        /// </summary>
        /// <param name="a_Event"></param>
        void onWeaponReloaded(EventHash a_Event)
        {
            ObjectiveManager.TriggerObjective(m_strObjTriggerOnReload);
        }
    }
}