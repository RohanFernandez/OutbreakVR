using ns_Mashmo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_mashmo
{
    public class LevelController_Training : MonoBehaviour
    {
        /// <summary>
        /// the objective trigger that will be fired on reloading during the training for primary weapon
        /// </summary>
        [SerializeField]
        string m_strObjTriggerOnReload_Primary = string.Empty;

        /// <summary>
        /// the objective trigger that will be fired on reloading during the training for secondary weapon
        /// </summary>
        [SerializeField]
        string m_strObjTriggerOnReload_Secondary = string.Empty;

        /// <summary>
        /// the objective trigger that will be fired swithcing the weapon category
        /// </summary>
        [SerializeField]
        string m_strObjTriggerWeaponCategoryChanged = string.Empty;

        void Awake()
        {
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_WEAPON_RELOADED, onWeaponReloaded);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_CURRENT_WEAPON_OR_CATEGORY_CHANGED, onWeaponOrCategoryChanged);
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
            WEAPON_TYPE l_WeaponType = (WEAPON_TYPE)a_Event[GameEventTypeConst.ID_WEAPON_TYPE];
            WEAPON_CATEGORY_TYPE l_WeaponCategory = WeaponManager.GetCategory(l_WeaponType);

            if (l_WeaponCategory == WEAPON_CATEGORY_TYPE.PRIMARY)
            {
                ObjectiveManager.TriggerObjective(m_strObjTriggerOnReload_Primary);
            }
            else if (l_WeaponCategory == WEAPON_CATEGORY_TYPE.SECONDARY)
            {
                ObjectiveManager.TriggerObjective(m_strObjTriggerOnReload_Secondary);
            }
        }

        /// <summary>
        /// event callback on weapon or category changed
        /// </summary>
        /// <param name="a_Event"></param>
        void onWeaponOrCategoryChanged(EventHash a_Event)
        {
            WEAPON_CATEGORY_TYPE l_WeaponNewCategoryType = (WEAPON_CATEGORY_TYPE)a_Event[GameEventTypeConst.ID_NEW_WEAPON_CATEGORY_TYPE];
            WEAPON_CATEGORY_TYPE l_WeaponOldCategoryType = (WEAPON_CATEGORY_TYPE)a_Event[GameEventTypeConst.ID_OLD_WEAPON_CATEGORY_TYPE];

            if (l_WeaponOldCategoryType != l_WeaponNewCategoryType)
            {
                ObjectiveManager.TriggerObjective(m_strObjTriggerWeaponCategoryChanged);
            }
        }
    }
}