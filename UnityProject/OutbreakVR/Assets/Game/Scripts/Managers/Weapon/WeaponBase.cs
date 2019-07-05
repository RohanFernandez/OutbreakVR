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
    }
}