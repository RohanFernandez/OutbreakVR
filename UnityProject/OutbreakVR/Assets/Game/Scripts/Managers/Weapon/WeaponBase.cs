using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [SerializeField]
        public WEAPON_CATEGORY_TYPE m_WeaponCategoryType;

        [SerializeField]
        public WEAPON_TYPE m_WeaponType;

        /// <summary>
        /// The weapon item-pickup type
        /// </summary>
        [SerializeField]
        private ITEM_TYPE m_WeaponItemType;
        public ITEM_TYPE WeaponItemType
        {
            get { return m_WeaponItemType; }
        }

        /// <summary>
        /// The bullet item-pickup type
        /// </summary>
        [SerializeField]
        private ITEM_TYPE m_BulletItemType;
        public ITEM_TYPE BulletItemType
        {
            get { return m_BulletItemType; }
        }

        /// <summary>
        /// Reloads weapon if it has a capability
        /// </summary>
        public virtual void reload() { }

        /// <summary>
        /// Fires weapon if it has the feature
        /// </summary>
        public virtual void fire() { }

        /// <summary>
        /// Called on weapon selected to use
        /// </summary>
        public virtual void onWeaponSelected() { }

        /// <summary>
        /// is a reload required
        /// </summary>
        /// <returns></returns>
        public virtual bool isReloadRequired() { return false; }

        /// <summary>
        /// Can the current weapon be reloaded
        /// </summary>
        /// <returns></returns>
        public virtual bool isReloadPossible()
        {
            return false;
        }

        /// <summary>
        /// The time taken to reload the weapon
        /// </summary>
        /// <returns></returns>
        public virtual float getReloadWaitTime()
        {
            return 0.0f;
        }

        /// <summary>
        /// Can the current weapon be fired.
        /// If its a gun then checks the number of bullets currently
        /// </summary>
        /// <returns></returns>
        public virtual bool canCurrentWeaponBeFired()
        {
            return true;
        }
    }
}