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
        NONE               = 0, //PRIMARY, SECONDARY
        UNARMED            = 1, //MELEE
        CHAINSAW           = 2, //MELEE
        FN57               = 3, //SECONDARY
        AK47               = 4, //PRIMARY
        SHOTGUN1           = 5, //PRIMARY
        REVOLVER           = 6, //SECONDARY
    }

    public enum BULLETS_TYPE
    {
        SECONDARY,
        PRIMARY_ASSAULT,
        PRIMARY_SHOTGUN
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
        /// The transform that holds all the weapons
        /// </summary>
        [SerializeField]
        private Transform m_WeaponHolder = null;

        /// <summary>
        /// The custom position of the weapon holder when the current controller is the headset
        /// </summary>
        [SerializeField]
        private Vector3 m_v3HeadsetWeaponHolderOffset = Vector3.zero;

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

                s_Instance.setRayTransformParent();
            }
        }

        /// <summary>
        /// The current time taken to reload the weapon
        /// </summary>
        [SerializeField]
        private float m_fCurrentReloadWaitTime = 0.0f;
        private float CurrentReloadWaitTime
        {
            get { return m_fCurrentReloadWaitTime; }
            set
            {
                if (m_fCurrentReloadWaitTime == 0.0f &&
                    value > 0.0f)
                {
                    m_bIsReloadInProgress = true;
                    UI_PlayerHelmet.ToggleReloadProgressBar(m_bIsReloadInProgress);
                }
                else if (m_fCurrentReloadWaitTime > 0.0f &&
                    value == 0.0f)
                {
                    m_bIsReloadInProgress = false;
                    UI_PlayerHelmet.ToggleReloadProgressBar(m_bIsReloadInProgress);
                }
                m_fCurrentReloadWaitTime = value;
            }
        }

        /// <summary>
        /// Is the reload of the current weapon in progress
        /// </summary>
        private bool m_bIsReloadInProgress = false;

        /// <summary>
        /// Interaction layer mask on raycast a gun weapon is fired.
        /// </summary>
        [SerializeField]
        private LayerMask m_GunHitInteractionLayer;

        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;

            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_CONTROLLER_CHANGED, onControllerChanged);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_PLAYER_STATE_CHANGED, onPlayerStateChanged);

            m_dictWeaponCategories = new Dictionary<WEAPON_CATEGORY_TYPE, WeaponCategory>(3);

            m_dictWeaponCategories.Add(WEAPON_CATEGORY_TYPE.MELEE,      new WeaponCategory(WEAPON_CATEGORY_TYPE.MELEE));
            m_dictWeaponCategories.Add(WEAPON_CATEGORY_TYPE.SECONDARY,  new WeaponCategory(WEAPON_CATEGORY_TYPE.SECONDARY));
            m_dictWeaponCategories.Add(WEAPON_CATEGORY_TYPE.PRIMARY,    new WeaponCategory(WEAPON_CATEGORY_TYPE.PRIMARY));

            m_dictWeapons = new Dictionary<WEAPON_TYPE, WeaponBase>(10);
            int l_iWeaponCount = m_lstWeapons.Count;
            for (int l_iWeaponIndex = 0; l_iWeaponIndex < l_iWeaponCount; l_iWeaponIndex++)
            {
                WeaponBase l_CurrentWeaponBase = m_lstWeapons[l_iWeaponIndex];
                m_dictWeapons.Add(l_CurrentWeaponBase.m_WeaponType, l_CurrentWeaponBase);

                WeaponCategory l_FoundWeaponCategory = null;
                if (m_dictWeaponCategories.TryGetValue(l_CurrentWeaponBase.m_WeaponCategoryType, out l_FoundWeaponCategory))
                {
                    l_FoundWeaponCategory.addWeaponTypeToCategory(l_CurrentWeaponBase.m_WeaponType);
                }
            }

            SetCurrentWeaponInCategory(WEAPON_CATEGORY_TYPE.MELEE, WEAPON_TYPE.UNARMED);

            disableAllWeapons();
            onControllerChanged(null);
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
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_CONTROLLER_CHANGED, onControllerChanged);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_PLAYER_STATE_CHANGED, onPlayerStateChanged);

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
            
            if (l_WeaponCategory != null &&
                l_WeaponCategory.m_WeaponType != a_WeaponType && // Same weapon is not already set in the weapon category
                l_WeaponCategory.isWeaponTypeInCategoryExist(a_WeaponType))  //The category of the weapon type is the same as the category to set
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
            /// Change weapon in category
            if (a_newWeaponCategoryType == a_oldWeaponCategoryType)
            {
                WeaponCategory l_WeaponCategory = null;
                if (m_dictWeaponCategories.TryGetValue(a_newWeaponCategoryType, out l_WeaponCategory))
                {
                    l_WeaponCategory.m_WeaponType = a_newWeaponType;
                }
            }

            /// Set new weapon category as current
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
                    if (l_goOldWeapon != null)
                    {
                        l_goOldWeapon.gameObject.SetActive(false);
                    }

                    if (l_goNewWeapon != null)
                    {
                        l_goNewWeapon.gameObject.SetActive(true);
                        l_goNewWeapon.onWeaponSelected();
                    }
                }
                /// on weapon changed set the reload time to 0
                CurrentReloadWaitTime = 0.0f;

                EventHash l_hash = EventManager.GetEventHashtable();
                l_hash.Add(GameEventTypeConst.ID_NEW_WEAPON_CATEGORY_TYPE, a_newWeaponCategoryType);
                l_hash.Add(GameEventTypeConst.ID_NEW_WEAPON_TYPE, a_newWeaponType);
                l_hash.Add(GameEventTypeConst.ID_NEW_WEAPON_BASE, l_goNewWeapon);
                l_hash.Add(GameEventTypeConst.ID_OLD_WEAPON_CATEGORY_TYPE, a_oldWeaponCategoryType);
                l_hash.Add(GameEventTypeConst.ID_OLD_WEAPON_TYPE, a_oldWeaponType);
                l_hash.Add(GameEventTypeConst.ID_OLD_WEAPON_BASE, l_goOldWeapon);
                EventManager.Dispatch(GAME_EVENT_TYPE.ON_CURRENT_WEAPON_OR_CATEGORY_CHANGED, l_hash);

                if (m_CurrentWeaponType != WEAPON_TYPE.NONE)
                {
                    setRayTransformParent();
                }
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

        /// <summary>
        /// Returns the weapon category of the given weapon type
        /// </summary>
        /// <param name="a_WeaponType"></param>
        /// <returns></returns>
        public static WEAPON_CATEGORY_TYPE GetCategory(WEAPON_TYPE a_WeaponType)
        {
            WEAPON_CATEGORY_TYPE l_Return = WEAPON_CATEGORY_TYPE.MELEE;
            WeaponBase l_WeaponBase = null;
            if (s_Instance.m_dictWeapons.TryGetValue(a_WeaponType, out l_WeaponBase))
            {
                l_Return = l_WeaponBase.m_WeaponCategoryType;
            }
            return l_Return;
        }

        /// <summary>
        /// Returns the weapon type in the given category
        /// </summary>
        /// <param name="a_WeaponType"></param>
        /// <returns></returns>
        public static WEAPON_TYPE GetWeaponInCategory(WEAPON_CATEGORY_TYPE a_WeaponCategoryType)
        {
            WEAPON_TYPE l_Return = WEAPON_TYPE.NONE;
            WeaponCategory l_WeaponCategory = null;
            if (s_Instance.m_dictWeaponCategories.TryGetValue(a_WeaponCategoryType, out l_WeaponCategory))
            {
                l_Return = l_WeaponCategory.m_WeaponType;
            }
            return l_Return;
        }

        /// <summary>
        /// Get weapon whose WeaponItemType is a_WeaponItemType
        /// </summary>
        /// <param name="a_WeaponItemType"></param>
        /// <returns></returns>
        private WeaponBase getWeaponBaseByItem(ITEM_TYPE a_WeaponItemType)
        {
            WeaponBase l_Return = null;
            foreach (KeyValuePair<WEAPON_TYPE, WeaponBase> l_Pair in m_dictWeapons)
            {
                if (l_Pair.Value.WeaponItemType == a_WeaponItemType)
                {
                    l_Return = l_Pair.Value;
                    break;
                }
            }

            return l_Return;
        }

        /// <summary>
        /// Gets weapon base by weapon type
        /// </summary>
        /// <param name="a_WeaponType"></param>
        /// <returns></returns>
        private WeaponBase getWeaponBaseByWeaponType(WEAPON_TYPE a_WeaponType)
        {
            WeaponBase l_Return = null;
            if (m_dictWeapons.ContainsKey(a_WeaponType))
            {
                l_Return = m_dictWeapons[a_WeaponType];
            }
            return l_Return;
        }

        /// <summary>
        /// Get weaponwhose currently acquired by the player whose BulletItemType is a_BulletItemType
        /// </summary>
        /// <param name="a_BulletItemType"></param>
        /// <returns></returns>
        private WeaponBase getAcquiredWeaponBaseByBullet(ITEM_TYPE a_BulletItemType)
        {
            WeaponBase l_Return = null;

            foreach (KeyValuePair<WEAPON_CATEGORY_TYPE, WeaponCategory> l_Pair in m_dictWeaponCategories)
            {
                WEAPON_TYPE l_WeaponType = l_Pair.Value.m_WeaponType;
                if (m_dictWeapons.ContainsKey(l_WeaponType))
                {
                    WeaponBase l_WeaponBase = m_dictWeapons[l_WeaponType];
                    if (l_WeaponBase.BulletItemType == a_BulletItemType)
                    {
                        l_Return = l_WeaponBase;
                        break;
                    }
                }
            }
            return l_Return;
        }

        /// <summary>
        /// Picks up the weapon and replaces the one in the current slot
        /// Sets the replaced gun in the environment
        /// If the gun is already possessed with the player then only take its bullets if it can
        /// </summary>
        /// <returns></returns>
        public static bool PickupWeapon(WeaponDropBase a_WeaponDrop)
        {
            bool l_bIsWeaponPickedUp = false;

            ///Get picked up weapon data
            ITEM_CATEGORY l_PickedUpItemCategory = a_WeaponDrop.getItemCategoryType();
            ITEM_TYPE l_PickedUpItemType = a_WeaponDrop.getItemType();
            WeaponBase l_PickedUpWeaponBase = s_Instance.getWeaponBaseByItem(l_PickedUpItemType);
            WEAPON_TYPE l_PickedUpWeaponType = l_PickedUpWeaponBase.m_WeaponType;
            WEAPON_CATEGORY_TYPE l_PickedUpWeaponCategoryType = l_PickedUpWeaponBase.m_WeaponCategoryType;
            int l_iBulletsInPickup = 0;
            if (l_PickedUpItemCategory == ITEM_CATEGORY.GUN)
            {
                l_iBulletsInPickup = ((GunWeaponDrop)a_WeaponDrop).BulletCount;
            }

            ///Get current weapon data in category
            WEAPON_TYPE l_CurrentWeaponType = GetWeaponInCategory(l_PickedUpWeaponCategoryType);
            WeaponBase l_CurrentWeaponBase = s_Instance.getWeaponBaseByWeaponType(l_CurrentWeaponType);

            ///Picks up weapon of the same type
            ///if is gun type weapon, then check if you can add the bullets from the pickup else dont pickup
            ///else dont pick up at all
            if (l_CurrentWeaponType == l_PickedUpWeaponType)
            {
                GunWeaponBase l_GunWeaponBase = (GunWeaponBase)l_CurrentWeaponBase;
                if (l_iBulletsInPickup > 0 &&
                    l_GunWeaponBase != null &&
                    l_GunWeaponBase.canAddBullets()
                    )
                {
                    l_GunWeaponBase.addBullets(l_iBulletsInPickup);
                    l_bIsWeaponPickedUp = true;
                    SetCategoryAsCurrent(l_PickedUpWeaponCategoryType);
                }
            }
            else
            {
                SetCurrentWeaponInCategory(l_PickedUpWeaponCategoryType, l_PickedUpWeaponType);
                GunWeaponBase l_GunWeaponBase = (GunWeaponBase)l_PickedUpWeaponBase;
                if (l_GunWeaponBase != null)
                {
                    l_GunWeaponBase.initBulletCount(l_iBulletsInPickup, l_iBulletsInPickup);
                }

                SetCategoryAsCurrent(l_PickedUpWeaponCategoryType);

                // Replace item with the weapon that the player had
                if (l_CurrentWeaponType != WEAPON_TYPE.NONE &&
                    l_CurrentWeaponType != WEAPON_TYPE.UNARMED)
                {

                    ItemDropBase l_ItemDropBase = ItemDropManager.GetItemDrop(l_CurrentWeaponBase.WeaponItemType);
                    l_ItemDropBase.transform.SetPositionAndRotation(a_WeaponDrop.transform.position, a_WeaponDrop.transform.rotation);

                    ///set the bullet count of the previously acquired gun to the item drop if its a gun
                    GunWeaponBase l_CurrentGunWeaponBase = (GunWeaponBase)l_CurrentWeaponBase;
                    GunWeaponDrop l_GunWeaponDrop = (GunWeaponDrop)l_ItemDropBase;
                    if(l_GunWeaponDrop != null &&
                        l_CurrentGunWeaponBase != null)
                    {
                        l_GunWeaponDrop.BulletCount = l_CurrentGunWeaponBase.TotalBullets;
                    }
                }

                l_bIsWeaponPickedUp = true;
            }

            return l_bIsWeaponPickedUp;
        }

        /// <summary>
        /// Picks up bullets and sets it into the correct gun
        /// If the gun that takes the bullets exists with the player or bullets can be added to the gun
        /// then add the bullets into the gun and returns true
        /// else returns false 
        /// </summary>
        public static bool PickupBullets(BulletDrop a_BulletDrop)
        {
            bool l_bIsBulletsPickedUp = false;

            ITEM_TYPE l_PickedUpItemType = a_BulletDrop.getItemType();
            ITEM_CATEGORY l_PickedUpCategoryType = a_BulletDrop.getItemCategoryType();
            int l_iBulletsToAdd = a_BulletDrop.BulletCount;
            
            WeaponBase l_CurrentWeapon = s_Instance.getAcquiredWeaponBaseByBullet(l_PickedUpItemType);

            if (l_CurrentWeapon != null)
            {
                GunWeaponBase l_GunWeaponBase = (GunWeaponBase)l_CurrentWeapon;
                if (l_GunWeaponBase != null &&
                    l_GunWeaponBase.canAddBullets())
                {
                    l_GunWeaponBase.addBullets(l_iBulletsToAdd);
                    l_bIsBulletsPickedUp = true;
                }
            }

            return l_bIsBulletsPickedUp;
        }

        /// <summary>
        /// Sets the bullet count into the weapon if the possessed by the player
        /// </summary>
        /// <param name="a_WeaponType"></param>
        public static void SetBulletCountInWeapon(WEAPON_TYPE a_WeaponType, int a_iTotalBullets, int a_iBulletsInFirstMag)
        {
            if (a_WeaponType == WEAPON_TYPE.NONE) { return; }

            WeaponBase l_WeaponBase = s_Instance.getWeaponBaseByWeaponType(a_WeaponType);
            GunWeaponBase l_GunWeaponBase = (GunWeaponBase)l_WeaponBase;
            if (l_GunWeaponBase != null)
            {
                l_GunWeaponBase.initBulletCount(a_iTotalBullets, a_iBulletsInFirstMag);
            }
        }

        /// <summary>
        /// Fires the current weapon
        /// </summary>
        private void fireWeapon()
        {
            WeaponBase l_WeaponBase = m_dictWeapons[s_Instance.m_CurrentWeaponType];
            l_WeaponBase.fire();

            bool l_bIsWeaponAGun = (l_WeaponBase.m_WeaponCategoryType != WEAPON_CATEGORY_TYPE.MELEE);
            if (l_bIsWeaponAGun)
            {
                GunWeaponBase l_GunWeaponBase = (GunWeaponBase)l_WeaponBase;

                manageRaycastHitOnGunFire(l_GunWeaponBase);

                //Dispatch weapon fire evetn
                EventHash l_EventHash = EventManager.GetEventHashtable();
                l_EventHash.Add(GameEventTypeConst.ID_WEAPON_TYPE, l_GunWeaponBase.m_WeaponType);
                l_EventHash.Add(GameEventTypeConst.ID_FIRST_MAG_COUNT, l_GunWeaponBase.BulletCountInFirstMag);
                l_EventHash.Add(GameEventTypeConst.ID_TOTAL_BULLETS, l_GunWeaponBase.TotalBullets);
                EventManager.Dispatch(GAME_EVENT_TYPE.ON_WEAPON_FIRED, l_EventHash);
            }
        }

        /// <summary>
        /// reloads the current weapon
        /// </summary>
        private void reloadWeapon()
        {
            WeaponBase l_WeaponBase = m_dictWeapons[s_Instance.m_CurrentWeaponType];
            l_WeaponBase.reload();

            bool l_bIsWeaponAGun = (l_WeaponBase.m_WeaponCategoryType != WEAPON_CATEGORY_TYPE.MELEE);
            if (l_bIsWeaponAGun)
            {
                GunWeaponBase l_GunWeaponBase = (GunWeaponBase)l_WeaponBase;
                EventHash l_EventHash = EventManager.GetEventHashtable();
                l_EventHash.Add(GameEventTypeConst.ID_WEAPON_TYPE, l_GunWeaponBase.m_WeaponType);
                l_EventHash.Add(GameEventTypeConst.ID_FIRST_MAG_COUNT, l_GunWeaponBase.BulletCountInFirstMag);
                l_EventHash.Add(GameEventTypeConst.ID_TOTAL_BULLETS, l_GunWeaponBase.TotalBullets);
                EventManager.Dispatch(GAME_EVENT_TYPE.ON_WEAPON_RELOADED, l_EventHash);
            }
        }

        /// <summary>
        /// Can the current weapon be reloaded, is there more bullets that can be put in the first mag
        /// </summary>
        /// <returns></returns>
        private bool canCurrentWeaponBeReloaded()
        {
            WeaponBase l_WeaponBase = m_dictWeapons[s_Instance.m_CurrentWeaponType];
            return l_WeaponBase.isReloadPossible();
        }

        /// <summary>
        /// Returns the current weapon reload time
        /// </summary>
        /// <returns></returns>
        public float getCurrentWeaponReloadTime()
        {
            WeaponBase l_WeaponBase = m_dictWeapons[s_Instance.m_CurrentWeaponType];
            return l_WeaponBase.getReloadWaitTime();
        }
        /// <summary>
        /// Can the current weapon be fired on click, melee weapon can be fired always
        /// </summary>
        /// <returns></returns>
        private bool canCurrentWeaponBeFired()
        {
            WeaponBase l_WeaponBase = m_dictWeapons[s_Instance.m_CurrentWeaponType];
            return l_WeaponBase.canCurrentWeaponBeFired();
        }

        /// <summary>
        /// Returns the weapon base of the current selected weapon base
        /// </summary>
        /// <returns></returns>
        public static WeaponBase GetCurrentWeaponBase()
        {
            return s_Instance.getWeaponBaseByWeaponType(CurrentWeaponType);
        }

        /// <summary>
        /// Callback called on controller changed
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onControllerChanged(EventHash a_EventHash)
        {
            m_WeaponHolder.localPosition = ControllerManager.IsRemoteAttached ?
                Vector3.zero : m_v3HeadsetWeaponHolderOffset;
            
            m_WeaponHolder.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// Returns a raycast hit on weapon fire
        /// </summary>
        private void manageRaycastHitOnGunFire(GunWeaponBase a_GunWeaponBase)
        {
            RaycastHit l_RaycastHit;

            Transform l_transCurrentControllerAnchor = ControllerManager.RayTransform;
            Ray l_ray = new Ray(l_transCurrentControllerAnchor.position, l_transCurrentControllerAnchor.forward);
            Physics.Raycast(l_ray, out l_RaycastHit, ControllerManager.MAX_CURSOR_DISTANCE, m_GunHitInteractionLayer);

            if (l_RaycastHit.collider != null)
            {
                EnemyHitCollider l_EnemyHitCollider = l_RaycastHit.collider.GetComponent<EnemyHitCollider>();
                if (l_EnemyHitCollider != null)
                {
                    l_EnemyHitCollider.inflictDamage(a_GunWeaponBase.DamagePerBullet, l_RaycastHit.point);
                }
            }
        }

        /// <summary>
        /// Sets the pointer ray transform start to start at the local origin of the a_transParent
        /// Is set as the child of the current active weapon if the weapon is on
        /// Sets it as the child of the weapon holder if the weapon is off
        /// resets the position and rotation
        /// </summary>
        private void setRayTransformParent()
        {
            Transform l_transRayPointer = ControllerManager.RayTransform;
            
            Transform l_transNewParent = IsWeaponActive ? GetCurrentWeaponBase().GunRayTransformParent : m_WeaponHolder;

            l_transRayPointer.SetParent(l_transNewParent);
            l_transRayPointer.localPosition = Vector3.zero;
            l_transRayPointer.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// Sets the current weapons in the category loaded
        /// </summary>
        public static void SetCurrentWeaponInventory(WeaponInventoryStructure a_WeaponInventoryStructure)
        {
            SetCurrentWeaponInCategory(WEAPON_CATEGORY_TYPE.MELEE, a_WeaponInventoryStructure.m_MeleeWeaponInfo.m_WeaponType);

            SetCurrentWeaponInCategory(WEAPON_CATEGORY_TYPE.PRIMARY, a_WeaponInventoryStructure.m_PrimaryWeaponInfo.m_WeaponType);
            SetBulletCountInWeapon(a_WeaponInventoryStructure.m_PrimaryWeaponInfo.m_WeaponType, a_WeaponInventoryStructure.m_PrimaryWeaponInfo.m_iTotalBulletsCount, a_WeaponInventoryStructure.m_PrimaryWeaponInfo.m_iBulletInFirstMag);

            SetCurrentWeaponInCategory(WEAPON_CATEGORY_TYPE.SECONDARY, a_WeaponInventoryStructure.m_SecondaryWeaponInfo.m_WeaponType);
            SetBulletCountInWeapon(a_WeaponInventoryStructure.m_SecondaryWeaponInfo.m_WeaponType, a_WeaponInventoryStructure.m_SecondaryWeaponInfo.m_iTotalBulletsCount, a_WeaponInventoryStructure.m_SecondaryWeaponInfo.m_iBulletInFirstMag);

            SetCategoryAsCurrent(a_WeaponInventoryStructure.m_CurrentWeaponCateogoryType);
        }

        /// <summary>
        /// Retrieves the current weapon info
        /// </summary>
        /// <param name="a_WeaponInfo"></param>
        public static void RetrieveWeaponInfo(ref WeaponInfo a_WeaponInfo)
        {
            WEAPON_TYPE l_WeaponType = GetWeaponInCategory(a_WeaponInfo.m_CategoryType);
            WeaponBase l_WeaponBase = s_Instance.getWeaponBaseByWeaponType(l_WeaponType);

            a_WeaponInfo.m_WeaponType = l_WeaponType;

            if (l_WeaponBase != null && 
                a_WeaponInfo.m_CategoryType != WEAPON_CATEGORY_TYPE.MELEE)
            {
                GunWeaponBase l_GunWeaponBase = (GunWeaponBase)l_WeaponBase;
                a_WeaponInfo.m_iBulletInFirstMag = l_GunWeaponBase.BulletCountInFirstMag;
                a_WeaponInfo.m_iTotalBulletsCount = l_GunWeaponBase.TotalBullets;
            }
        }

        /// <summary>
        /// Event callback on player state changed
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onPlayerStateChanged(EventHash a_EventHash)
        {
            CurrentReloadWaitTime = 0.0f;
            PLAYER_STATE l_NewPlayerState = (PLAYER_STATE)a_EventHash[GameEventTypeConst.ID_NEW_PLAYER_STATE];
            IsWeaponActive = l_NewPlayerState == PLAYER_STATE.IN_GAME_MOVEMENT || l_NewPlayerState == PLAYER_STATE.IN_GAME_HALTED;
        }

        /// <summary>
        /// Updates weapon management with reload
        /// </summary>
        private void Update()
        {
            if (IsWeaponActive)
            {
                manageWeaponAttack();
            }
        }

        /// <summary>
        /// Fires/Reloads the weapon
        /// </summary>
        public void manageWeaponAttack()
        {
            ///Fire weapon
            if (ControllerManager.IsPrimaryTriggerBtnDown()
#if UNITY_EDITOR
                || Input.GetKey(KeyCode.Mouse0)
#endif
                )
            {
                if (canCurrentWeaponBeFired() &&
                    !m_bIsReloadInProgress)
                {
                    fireWeapon();
                }
                else
                {
                    /// Indicate weapon cannot be fired
                }
            }

            ///Reload weapon
            float l_fDotFacingDown = Vector3.Dot(ControllerManager.GetPrimaryControllerDirection(), Vector3.down);
            if (canCurrentWeaponBeReloaded())
            {
                if (
                    (l_fDotFacingDown > 0.85f)
                )
                {
                    CurrentReloadWaitTime += Time.deltaTime;
                    float l_fCurrentWeaponReloadTime = getCurrentWeaponReloadTime();
                    UI_PlayerHelmet.UpdateReloadProgressBar(CurrentReloadWaitTime, l_fCurrentWeaponReloadTime);

                    if (CurrentReloadWaitTime > l_fCurrentWeaponReloadTime)
                    {
                        CurrentReloadWaitTime = 0.0f;
                        reloadWeapon();
                    }
                }
                else
                {
                    CurrentReloadWaitTime = 0.0f;
                }
            }
            else
            {
                CurrentReloadWaitTime = 0.0f;
            }
        }
    }
}