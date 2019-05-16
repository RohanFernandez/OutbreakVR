using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public enum WEAPON_CATEGORY_TYPE
    {
        MELEE                   = 0,
        SECONDARY               = 1,
        PRIMARY                 = 2,
        MAX_WEAPON_CATEGORIES   = 3
    }

    public enum WEAPON_TYPE
    {
        NONE               = 0,
        UNARMED            = 1,
        CHAINSAW           = 2,
        FN57               = 3,
        AK47               = 4,
    }

    public class WeaponManager : AbsComponentHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static WeaponManager s_Instance = null;

        /// <summary>
        /// List of all weapons
        /// </summary>
        [SerializeField]
        private List<WeaponBase> m_lstWeapons = null;

        /// <summary>
        /// Dictionary of weapon type to Weapon GameObject
        /// </summary>
        private Dictionary<WEAPON_TYPE, WeaponBase> m_dictWeapons = null;

        /// <summary>
        /// Dictionary of weapon categories type to the weapon category that holds all the weapons in that category
        /// </summary>
        private Dictionary<WEAPON_CATEGORY_TYPE, WeaponCategory> m_dictWeaponCategories = null;

        [SerializeField]
        private WEAPON_CATEGORY_TYPE m_CurrentWeaponCategoryType = WEAPON_CATEGORY_TYPE.MELEE;
        public static WEAPON_CATEGORY_TYPE CurrentWeaponCategoryType
        {
            get { return s_Instance.m_CurrentWeaponCategoryType; }
        }

        [SerializeField]
        private WEAPON_TYPE m_CurrentWeaponType = WEAPON_TYPE.NONE;
        public static WEAPON_TYPE CurrentWeaponType
        {
            get {return s_Instance.m_CurrentWeaponType;}
        }

        [SerializeField]
        private bool m_bIsWeaponActive = false;
        public static bool IsWeaponActive
        {
            get { return s_Instance.m_bIsWeaponActive; }
            set
            {
                if (s_Instance.m_bIsWeaponActive == value)
                {
                    return;
                }
                s_Instance.m_bIsWeaponActive = value;
                if (s_Instance.m_bIsWeaponActive)
                {
                    WeaponBase l_Weapon = null;
                    if (s_Instance.m_dictWeapons.TryGetValue(s_Instance.m_CurrentWeaponType, out l_Weapon))
                    {
                        l_Weapon.gameObject.SetActive(true);
                    }
                }
                else
                {
                    s_Instance.disableAllWeapons();
                }
            }
        }

        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;

            m_dictWeapons = new Dictionary<WEAPON_TYPE, WeaponBase>(10);
            int l_iWeaponCount = m_lstWeapons.Count;
            for (int l_iWeaponIndex = 0; l_iWeaponIndex < l_iWeaponCount; l_iWeaponIndex++)
            {
                WeaponBase l_CurrentWeaponBase = m_lstWeapons[l_iWeaponIndex];
                m_dictWeapons.Add(l_CurrentWeaponBase.m_WeaponType, l_CurrentWeaponBase);
            }
            disableAllWeapons();

            m_dictWeaponCategories = new Dictionary<WEAPON_CATEGORY_TYPE, WeaponCategory>(3);

            m_dictWeaponCategories.Add(WEAPON_CATEGORY_TYPE.MELEE,      new WeaponCategory(WEAPON_CATEGORY_TYPE.MELEE));
            m_dictWeaponCategories.Add(WEAPON_CATEGORY_TYPE.SECONDARY,  new WeaponCategory(WEAPON_CATEGORY_TYPE.SECONDARY));
            m_dictWeaponCategories.Add(WEAPON_CATEGORY_TYPE.PRIMARY,    new WeaponCategory(WEAPON_CATEGORY_TYPE.PRIMARY));

            SetCurrentWeaponInCategory(WEAPON_CATEGORY_TYPE.MELEE, WEAPON_TYPE.UNARMED);
            SetCurrentWeaponInCategory(WEAPON_CATEGORY_TYPE.PRIMARY, WEAPON_TYPE.AK47);
            SetCurrentWeaponInCategory(WEAPON_CATEGORY_TYPE.SECONDARY, WEAPON_TYPE.FN57);
            SetCategoryAsCurrent(WEAPON_CATEGORY_TYPE.MELEE);
        }

        /// <summary>
        /// Destroys singleton instance
        /// </summary>
        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }
            s_Instance = null;
        }

        /// <summary>
        /// Deactivates all weapons
        /// </summary>
        private void disableAllWeapons()
        {
            foreach(KeyValuePair<WEAPON_TYPE, WeaponBase> l_Pair in m_dictWeapons)
            {
                l_Pair.Value.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Sets the current weapon in the category as given type.
        /// </summary>
        /// <param name="a_WeaponCategoryType"></param>
        /// <param name="a_WeaponType"></param>
        public static void SetCurrentWeaponInCategory(WEAPON_CATEGORY_TYPE a_WeaponCategoryType, WEAPON_TYPE a_WeaponType)
        {
            WeaponCategory l_WeaponCategory = null;
            s_Instance.m_dictWeaponCategories.TryGetValue(a_WeaponCategoryType, out l_WeaponCategory);
            WeaponBase l_WeaponBase = null;
            s_Instance.m_dictWeapons.TryGetValue(a_WeaponType, out l_WeaponBase);

            if (l_WeaponCategory != null &&
                l_WeaponBase != null && 
                l_WeaponCategory.m_WeaponType != a_WeaponType && // Same weapon is not already set in the weapon category
                l_WeaponBase.m_WeaponCategoryType == a_WeaponCategoryType)  //The category of the weapon type is the same as the category to set
            {
                s_Instance.onDispatchWeaponOrCategoryChanged(a_WeaponCategoryType, a_WeaponType, a_WeaponCategoryType, l_WeaponCategory.m_WeaponType);
            }
        }

        /// <summary>
        /// Sets the category as current
        /// if the category to set is different to the current category
        /// sets the new category and its weapon as current and fires an event
        /// </summary>
        /// <param name="a_WeaponCategoryType"></param>
        /// <returns></returns>
        public static bool SetCategoryAsCurrent(WEAPON_CATEGORY_TYPE a_WeaponCategoryType)
        {
            WeaponCategory l_WeaponCategory = null;
            s_Instance.m_dictWeaponCategories.TryGetValue(a_WeaponCategoryType, out l_WeaponCategory);

            if (l_WeaponCategory == null)
            {
                return false;
            }
            else if ((a_WeaponCategoryType != s_Instance.m_CurrentWeaponCategoryType) &&
                (l_WeaponCategory.m_WeaponType != WEAPON_TYPE.NONE))
            {
                s_Instance.onDispatchWeaponOrCategoryChanged(l_WeaponCategory.m_WeaponCategoryType, l_WeaponCategory.m_WeaponType,
                    s_Instance.m_CurrentWeaponCategoryType, s_Instance.m_CurrentWeaponType);
                return true;
            }
            else if(a_WeaponCategoryType == s_Instance.m_CurrentWeaponCategoryType)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets new weapon and category type on weapon or category changed and 
        /// fires the event if the a different in the same category is changed
        /// </summary>
        /// <param name="a_newWeaponCategoryType"></param>
        /// <param name="a_newWeaponType"></param>
        /// <param name="a_oldWeaponCategoryType"></param>
        /// <param name="a_oldWeaponType"></param>
        private void onDispatchWeaponOrCategoryChanged(WEAPON_CATEGORY_TYPE a_newWeaponCategoryType, WEAPON_TYPE a_newWeaponType,
            WEAPON_CATEGORY_TYPE a_oldWeaponCategoryType, WEAPON_TYPE a_oldWeaponType)
        {
            if (a_newWeaponCategoryType == a_oldWeaponCategoryType)
            {
                WeaponCategory l_WeaponCategory = null;
                if (m_dictWeaponCategories.TryGetValue(a_newWeaponCategoryType, out l_WeaponCategory))
                {
                    l_WeaponCategory.m_WeaponType = a_newWeaponType;
                }
            }

            if (m_CurrentWeaponCategoryType == a_oldWeaponCategoryType)
            {
                m_CurrentWeaponCategoryType = a_newWeaponCategoryType;
                m_CurrentWeaponType = a_newWeaponType;

                WeaponBase l_goOldWeapon = null;
                s_Instance.m_dictWeapons.TryGetValue(a_oldWeaponType, out l_goOldWeapon);
                WeaponBase l_goNewWeapon = null;
                s_Instance.m_dictWeapons.TryGetValue(a_newWeaponType, out l_goNewWeapon);

                if (m_bIsWeaponActive)
                {
                    if (l_goOldWeapon != null) { l_goOldWeapon.gameObject.SetActive(false); }
                    if (l_goNewWeapon != null) { l_goNewWeapon.gameObject.SetActive(true); }
                }

                Hashtable l_Hashtable = new Hashtable(6);
                l_Hashtable.Add(GameEventTypeConst.ID_NEW_WEAPON_CATEGORY_TYPE, a_newWeaponCategoryType);
                l_Hashtable.Add(GameEventTypeConst.ID_NEW_WEAPON_TYPE, a_newWeaponType);
                l_Hashtable.Add(GameEventTypeConst.ID_NEW_WEAPON_BASE, l_goNewWeapon);
                l_Hashtable.Add(GameEventTypeConst.ID_OLD_WEAPON_CATEGORY_TYPE, a_oldWeaponCategoryType);
                l_Hashtable.Add(GameEventTypeConst.ID_OLD_WEAPON_TYPE, a_oldWeaponType);
                l_Hashtable.Add(GameEventTypeConst.ID_OLD_WEAPON_BASE, l_goOldWeapon);
                EventManager.Dispatch(GAME_EVENT_TYPE.ON_CURRENT_WEAPON_OR_CATEGORY_CHANGED, l_Hashtable);
            }
        }

        /// <summary>
        /// Sets the current weapon category to the next weapon category
        /// If the current weapon in the next category in NONE then check the next category until the current weapon in a category is not none
        /// </summary>
        public static void SetNextCategory()
        {
            int l_iCurrentWeaponCategoryIndex = (int)s_Instance.m_CurrentWeaponCategoryType;
            int l_iNextWeaponCategoryIndex = l_iCurrentWeaponCategoryIndex;
            int l_iMaxCategoryIndex = (int)WEAPON_CATEGORY_TYPE.MAX_WEAPON_CATEGORIES;

            bool l_bIsNewCategorySet = false;
            while (!l_bIsNewCategorySet)
            {
                ++l_iNextWeaponCategoryIndex;
                l_iNextWeaponCategoryIndex = l_iNextWeaponCategoryIndex % l_iMaxCategoryIndex;
                l_bIsNewCategorySet = SetCategoryAsCurrent((WEAPON_CATEGORY_TYPE)l_iNextWeaponCategoryIndex);
            }
        }

        /// <summary>
        /// Sets the current weapon category to the previous weapon category
        /// If the current weapon in the previous category in NONE then check the next category until the current weapon in a category is not none
        /// </summary>
        public static void SetPreviousCategory()
        {
            int l_iCurrentWeaponCategoryIndex = (int)s_Instance.m_CurrentWeaponCategoryType;
            int l_iPrevWeaponCategoryIndex = l_iCurrentWeaponCategoryIndex;
            int l_iMaxCategoryIndex = (int)WEAPON_CATEGORY_TYPE.MAX_WEAPON_CATEGORIES;

            bool l_bIsNewCategorySet = false;
            while (!l_bIsNewCategorySet)
            {
                l_iPrevWeaponCategoryIndex = (--l_iPrevWeaponCategoryIndex) % l_iMaxCategoryIndex;
                if (l_iPrevWeaponCategoryIndex == -1) { l_iPrevWeaponCategoryIndex = l_iMaxCategoryIndex - 1; }
                l_bIsNewCategorySet = SetCategoryAsCurrent((WEAPON_CATEGORY_TYPE)l_iPrevWeaponCategoryIndex);
            }
        }
    }
}