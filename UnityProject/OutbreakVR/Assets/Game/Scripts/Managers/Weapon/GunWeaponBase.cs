using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class GunWeaponBase : WeaponBase
    {
        [SerializeField]
        private int m_iMaxMagazinesAllowed = 10;

        [SerializeField]
        private int m_iMaxSingleMagazineBulletCapacity = 10;

        [SerializeField]
        private int m_iBulletCountInFirstMag = 10;  

        public int CurrentMagCount
        {
            get
            {
                int l_iCurrentMagCount = 0;
                if (m_iBulletCountInFirstMag > 0) { l_iCurrentMagCount++; }

                int l_iBulletsNotInFirstMag = getBulletsNotInFirstMag();
                l_iCurrentMagCount += (l_iBulletsNotInFirstMag / m_iMaxSingleMagazineBulletCapacity);

                if ((l_iBulletsNotInFirstMag % m_iMaxSingleMagazineBulletCapacity) > 0) { l_iCurrentMagCount++; }

                return l_iCurrentMagCount;
            }
        }

        [SerializeField]
        private int m_iTotalBullets = 0;

        /// <summary>
        /// Is the first mag empty
        /// </summary>
        /// <returns></returns>
        public bool isReloadRequired()
        {
            return m_iBulletCountInFirstMag == 0;
        }

        /// <summary>
        /// Can the gun be reloaded
        /// </summary>
        /// <returns></returns>
        public bool isReloadPossible()
        {
            return (m_iBulletCountInFirstMag < m_iMaxSingleMagazineBulletCapacity) &&
                (getBulletsNotInFirstMag() > 0);
        }

        public override void fire()
        {
            ///Reload required
            if (isReloadRequired()) { return; }

            m_iTotalBullets--;
            m_iBulletCountInFirstMag--;
            updateBulletData();
        }

        public override void reload()
        {
            if (!isReloadPossible()) { return; }

            int l_iBulletsEmptyInFirstMag = (m_iMaxSingleMagazineBulletCapacity - m_iBulletCountInFirstMag);

            int l_iBulletsNotInFirstMag = getBulletsNotInFirstMag();

            m_iBulletCountInFirstMag += ((l_iBulletsNotInFirstMag > l_iBulletsEmptyInFirstMag) ?
                l_iBulletsEmptyInFirstMag : l_iBulletsNotInFirstMag);

            updateBulletData();
        }

        /// <summary>
        /// On new weapon is selected
        /// </summary>
        public override void onWeaponSelected()
        {
            base.onWeaponSelected();
            updateBulletData();
        }

        /// <summary>
        /// Returns max number of bullets in this weapon
        /// </summary>
        /// <returns></returns>
        public int getMaxBullets()
        {
            return m_iMaxMagazinesAllowed * m_iMaxSingleMagazineBulletCapacity;
        }

        /// <summary>
        /// Returns he max bullets that are in all the mags except for the first mag.
        /// </summary>
        /// <returns></returns>
        public int getMaxBulletsNotInFirstMag()
        {
            return getMaxBullets() - m_iMaxSingleMagazineBulletCapacity;
        }

        /// <summary>
        /// Returns the current number of bullets not in the first mag
        /// </summary>
        /// <returns></returns>
        public int getBulletsNotInFirstMag()
        {
            return (m_iTotalBullets - m_iBulletCountInFirstMag);
        }

        /// <summary>
        /// Can add more bullets to this gun
        /// </summary>
        /// <returns></returns>
        public bool canAddBullets()
        {
            return getBulletsThatCanBeAdded() > 0;
        }

        /// <summary>
        /// returns the number of bullets that can be added to the total number of bullets
        /// The bullets in the first mag is not counted, so if the remaining mags are not full return the empty bullet count in those mags
        /// </summary>
        /// <returns></returns>
        public int getBulletsThatCanBeAdded()
        {
            return getMaxBulletsNotInFirstMag() - getMaxBulletsNotInFirstMag();
        }

        /// <summary>
        /// Adds and manages the bullets to add to the gun
        /// </summary>
        /// <param name="a_iBullets"></param>
        public void addBullets(int a_iBullets)
        {
            int l_iBulletsCanBeAdded = getBulletsThatCanBeAdded();
            int l_iBulletsToAdd = (a_iBullets > l_iBulletsCanBeAdded) ? l_iBulletsCanBeAdded : a_iBullets;
            m_iTotalBullets += l_iBulletsToAdd;
            updateBulletData();
        }

        public void resetBulletCount()
        {
            m_iTotalBullets = 0;
            m_iBulletCountInFirstMag = 0;
            updateBulletData();
        }

        /// <summary>
        /// updates the bullet data
        /// </summary>
        public void updateBulletData()
        {

        }
    }
}