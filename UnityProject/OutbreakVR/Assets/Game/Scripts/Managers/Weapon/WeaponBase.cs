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
        /// The transform that will hold the ray transform game object of the controller
        /// The laser/ line renderer or aim will start from this point
        /// </summary>
        [SerializeField]
        private Transform m_GunRayTransformParent = null;
        public Transform GunRayTransformParent
        {
            get { return m_GunRayTransformParent; }
        }

        /// <summary>
        /// The parent holds the monitor on the arm that indicates the bullet count
        /// </summary>
        [SerializeField]
        protected Transform m_ArmMonitorParent_BulletCount = null;
        public Transform ArmMonitorParent_BulletCount
        {
            get { return m_ArmMonitorParent_BulletCount; }
        }

        /// <summary>
        /// The parent holds the monitor on the arm that indicates the health
        /// </summary>
        [SerializeField]
        protected Transform m_ArmMonitorParent_Health = null;
        public Transform ArmMonitorParent_Health
        {
            get { return m_ArmMonitorParent_Health; }
        }

        /// <summary>
        /// Reloads weapon if it has a capability
        /// </summary>
        public virtual void reload() { }

        /// <summary>
        /// Fires weapon if it has the feature
        /// </summary>
        public virtual void startShootingAnim() { }

        public virtual void shootBullet() { }

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
        /// The game is paused/unpaused
        /// play animation of the arm monitor
        /// </summary>
        /// <param name="a_IsPaused"></param>
        public virtual void onGamePauseToggled(bool a_IsPaused) {  }

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