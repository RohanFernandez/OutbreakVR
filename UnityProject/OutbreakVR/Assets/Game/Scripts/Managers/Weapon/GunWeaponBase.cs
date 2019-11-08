using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class GunWeaponBase : WeaponBase
    {
        #region EFFECTS AND SOUNDS
        [SerializeField]
        private ParticleSystem m_ChamberBulletRelease = null;

        [SerializeField]
        private ParticleSystem m_MuzzleFlash = null;

        #endregion EFFECTS AND SOUNDS

        [SerializeField]
        private int m_iMaxMagazinesAllowed = 10;

        [SerializeField]
        private int m_iMaxSingleMagazineBulletCapacity = 10;

        /// <summary>
        /// Audio clip id to play on fire
        /// </summary>
        [SerializeField]
        private string m_strAudClipIDOnShoot = string.Empty;

        /// <summary>
        /// Audio clip id to play on reload
        /// </summary>
        [SerializeField]
        private string m_strAudClipIDOnReload = string.Empty;

        /// <summary>
        /// The time after which the next shot can be fired.
        /// </summary>
        [SerializeField]
        private float m_fTimeBetweenEachShoot = 0.0f;

        /// <summary>
        /// The time calculated since the last shot
        /// </summary>
        private float m_fTimeSinceLastShot = 0.0f;

        [SerializeField]
        private int m_iBulletCountInFirstMag = 10; 
        public int BulletCountInFirstMag
        {
            get { return m_iBulletCountInFirstMag; }
            set {
                m_iBulletCountInFirstMag = Mathf.Clamp(value, 0, m_iMaxSingleMagazineBulletCapacity);
            }
        }

        /// <summary>
        /// The damage inflicted on an enemy on bullet fired on an enemy
        /// </summary>
        [SerializeField]
        private int m_iDamagePerBullet = 15;
        public int DamagePerBullet
        {
            get { return m_iDamagePerBullet; }
        }

        /// <summary>
        /// animation recoil
        /// </summary>
        [SerializeField]
        protected Animation m_animRecoil = null;

        /// <summary>
        /// The name of the recoil anim state name
        /// </summary>
        private const string RECOIL_ANIM_STATE_NAME = "Recoil";

        public int CurrentMagCount
        {
            get
            {
                int l_iCurrentMagCount = 0;
                if (BulletCountInFirstMag > 0) { l_iCurrentMagCount++; }

                int l_iBulletsNotInFirstMag = getBulletsNotInFirstMag();
                l_iCurrentMagCount += (l_iBulletsNotInFirstMag / m_iMaxSingleMagazineBulletCapacity);

                if ((l_iBulletsNotInFirstMag % m_iMaxSingleMagazineBulletCapacity) > 0) { l_iCurrentMagCount++; }

                return l_iCurrentMagCount;
            }
        }

        /// <summary>
        /// The time taken to reload this weapon
        /// </summary>
        [Range(0.0f, 100.0f)]
        [SerializeField]
        private float m_fWeaponReloadTime = 0.0f;

        /// <summary>
        /// The total bullets currently in the weapon
        /// Total bullets of this weapon should be between 0 and the Max bullets(inclusive)
        /// </summary>
        [SerializeField]
        private int m_iTotalBullets = 0;
        public int TotalBullets
        {
            get { return m_iTotalBullets; }
            private set {
                m_iTotalBullets = Mathf.Clamp(value, 0, getMaxBullets());
            }
        }

        /// <summary>
        /// Is the first mag empty
        /// </summary>
        /// <returns></returns>
        public override bool isReloadRequired()
        {
            return BulletCountInFirstMag == 0 && TotalBullets > 0;
        }

        /// <summary>
        /// Can the current weapon be fired
        /// Checks if there are bullets in the first mag
        /// </summary>
        /// <returns></returns>
        public override bool canCurrentWeaponBeFired()
        {
            return (BulletCountInFirstMag != 0) &&
                (m_fTimeSinceLastShot > m_fTimeBetweenEachShoot);
        }

        /// <summary>
        /// Can the gun be reloaded
        /// </summary>
        /// <returns></returns>
        public override bool isReloadPossible()
        {
            return (BulletCountInFirstMag < m_iMaxSingleMagazineBulletCapacity) &&
                (getBulletsNotInFirstMag() > 0);
        }

        public override void fire()
        {
            ///Reload required
            if (isReloadRequired()) { return; }

            --TotalBullets;
            --BulletCountInFirstMag;
            if (m_ChamberBulletRelease != null) { m_ChamberBulletRelease.Play(); }
            if (m_MuzzleFlash != null)          { m_MuzzleFlash.Play(); }

            SoundManager.PlayAudio(GameConsts.AUD_SRC_GUN_FIRE, m_strAudClipIDOnShoot ,false, 1.0f, AUDIO_SRC_TYPES.AUD_SRC_SFX);

            if (m_animRecoil != null)
            {
                m_animRecoil.Stop();
                m_animRecoil.Play();
            }

            m_fTimeSinceLastShot = 0.0f;
            updateBulletData();
        }

        public override void reload()
        {
            if (!isReloadPossible()) { return; }

            int l_iBulletsEmptyInFirstMag = (m_iMaxSingleMagazineBulletCapacity - BulletCountInFirstMag);

            int l_iBulletsNotInFirstMag = getBulletsNotInFirstMag();

            BulletCountInFirstMag += l_iBulletsNotInFirstMag;

            SoundManager.PlayAudio(GameConsts.AUD_SRC_GUN_RELOAD, m_strAudClipIDOnReload, false, 1.0f, AUDIO_SRC_TYPES.AUD_SRC_SFX);

            updateBulletData();
        }

        /// <summary>
        /// On new weapon is selected
        /// </summary>
        public override void onWeaponSelected()
        {
            base.onWeaponSelected();

            if (m_animRecoil != null)
            {
                AnimationState l_AnimState = m_animRecoil[RECOIL_ANIM_STATE_NAME];
                if (l_AnimState != null)
                {
                    l_AnimState.speed = 1.0f / m_fTimeBetweenEachShoot;
                }
            }

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
            return (TotalBullets - BulletCountInFirstMag);
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
            return getMaxBulletsNotInFirstMag() - getBulletsNotInFirstMag();
        }

        /// <summary>
        /// Adds and manages the bullets to add to the gun
        /// </summary>
        /// <param name="a_iBullets"></param>
        public void addBullets(int a_iBullets)
        {
            int l_iBulletsCanBeAdded = getBulletsThatCanBeAdded();
            int l_iBulletsToAdd = (a_iBullets > l_iBulletsCanBeAdded) ? l_iBulletsCanBeAdded : a_iBullets;
            TotalBullets += l_iBulletsToAdd;

            EventHash l_EventHash = EventManager.GetEventHashtable();
            l_EventHash.Add(GameEventTypeConst.ID_GUN_WEAPON, this);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_BULLETS_ADDED, l_EventHash);

            updateBulletData();
        }

        /// <summary>
        /// Init bullet count, should be called on weapon is acquired for the first time
        /// </summary>
        /// <param name="a_iTotalBulletCount"></param>
        public void initBulletCount(int a_iTotalBulletCount, int a_iBulletsInFirstMag)
        {
            TotalBullets = a_iTotalBulletCount;
            BulletCountInFirstMag = a_iTotalBulletCount;
            updateBulletData();
        }

        /// <summary>
        /// updates the bullet data
        /// </summary>
        public void updateBulletData()
        {

        }

        /// <summary>
        /// The time taken by the gun to reload
        /// </summary>
        /// <returns></returns>
        public override float getReloadWaitTime()
        {
            return m_fWeaponReloadTime;
        }

        void Update()
        {
            if (m_fTimeSinceLastShot < m_fTimeBetweenEachShoot)
            {
                m_fTimeSinceLastShot += Time.deltaTime;
            }
        }
    }
}