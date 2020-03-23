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
        /// The particle system that displays the tracer on shoot
        /// </summary>
        [SerializeField]
        private ParticleSystem m_TracerParticleSystem = null;

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
        /// The audio src index of the gun fire sound
        /// </summary>
        private bool m_bGunFireAudSrcIndex1 = true;

        [SerializeField]
        private int m_iBulletCountInFirstMag = 10; 
        public int BulletCountInFirstMag
        {
            get { return m_iBulletCountInFirstMag; }
            private set {
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
        /// The animator that handles the hands of this gun
        /// </summary>
        [SerializeField]
        protected Animator m_animatorHands = null;

        /// <summary>
        /// The name of the shoot anim state name
        /// </summary>
        private const string ANIM_STATE_SHOOT = "shoot";

        /// <summary>
        /// The name of the anim state that opens the menu
        /// </summary>
        private const string ANIM_STATE_OPEN_MENU = "Menu_1";

        /// <summary>
        /// The name of the anim state that closes the menu
        /// </summary>
        private const string ANIM_STATE_CLOSE_MENU = "Menu_2";

        /// <summary>
        /// The name of the anim state that closes the menu
        /// </summary>
        private const string ANIM_STATE_IDLE_HANDS = "idle";

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

        #region RECOIL GUN MOVEMENT

        /// <summary>
        /// The maximum the gun is allowed to rotate in the X axis on shoot
        /// </summary>
        [SerializeField]
        private float m_fMaxAngleGunRecoilRotX = 13.0f;

        /// <summary>
        /// The gun should rotate in the X axis per shot
        /// </summary>
        [SerializeField]
        private float m_fPerShotGunRecoilRotXAngle = 4.0f;

        /// <summary>
        /// The gun should reset its X rot angle at this angle/sec
        /// </summary>
        [SerializeField]
        private float m_fRotResetVelocityAnglePerSec = 13.0f;

        #endregion RECOIL GUN MOVEMENT

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
            return (BulletCountInFirstMag != 0);
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

        public override void startShootingAnim()
        {
            ///Reload required
            if (isReloadRequired()) { return; }

            if (m_animatorHands != null)
            {
                m_animatorHands.SetBool(ANIM_STATE_SHOOT, true);
            }
        }

        public override void shootBullet()
        {
            --TotalBullets;
            --BulletCountInFirstMag;
            if (m_ChamberBulletRelease != null) { m_ChamberBulletRelease.Play(); }
            if (m_MuzzleFlash != null) { m_MuzzleFlash.Play(); }

            if (m_TracerParticleSystem != null)
            {
                m_TracerParticleSystem.Play();
            }

            //rotate gun upwards on show to imitate recoil
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(m_fMaxAngleGunRecoilRotX, 0.0f, 0.0f), m_fPerShotGunRecoilRotXAngle);

            //The audio src index that moves between 0 - 1
            // if 0 then play AUD_SRC_GUN_FIRE else 1 then play AUD_SRC_GUN_FIRE_1 else 
            m_bGunFireAudSrcIndex1 = !m_bGunFireAudSrcIndex1;
            SoundManager.PlayAudio(m_bGunFireAudSrcIndex1 ? GameConsts.AUD_SRC_GUN_FIRE : GameConsts.AUD_SRC_GUN_FIRE_1, m_strAudClipIDOnShoot, false, 1.0f, AUDIO_SRC_TYPES.AUD_SRC_SFX);

        }

        /// <summary>
        /// stops the shooting animation manually
        /// </summary>
        public override void stopShootingAnim()
        {
            if (m_animatorHands != null)
            {
                m_animatorHands.SetBool(ANIM_STATE_SHOOT, false);
            }
        }

        public override void reload()
        {
            if (!isReloadPossible()) { return; }

            BulletCountInFirstMag += getBulletsNotInFirstMag();

            SoundManager.PlayAudio(GameConsts.AUD_SRC_GUN_RELOAD, m_strAudClipIDOnReload, false, 1.0f, AUDIO_SRC_TYPES.AUD_SRC_SFX);
        }

        /// <summary>
        /// Sets the custom animation speed of the gun shoot animation
        /// </summary>
        public override void initialize()
        {
            base.initialize();
        }

        /// <summary>
        /// On new weapon is selected
        /// </summary>
        public override void onWeaponSelected()
        {
            base.onWeaponSelected();

            transform.localRotation = Quaternion.identity;

            if (m_animatorHands != null)
            {
                m_animatorHands.SetBool(ANIM_STATE_SHOOT, false);
                m_animatorHands.SetTrigger(ANIM_STATE_IDLE_HANDS);
            }
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
        }

        /// <summary>
        /// Init bullet count, should be called on weapon is acquired for the first time
        /// </summary>
        /// <param name="a_iTotalBulletCount"></param>
        public void initBulletCount(int a_iTotalBulletCount, int a_iBulletsInFirstMag)
        {
            TotalBullets = a_iTotalBulletCount;
            BulletCountInFirstMag = a_iBulletsInFirstMag;
        }

        /// <summary>
        /// The time taken by the gun to reload
        /// </summary>
        /// <returns></returns>
        public override float getReloadWaitTime()
        {
            return m_fWeaponReloadTime;
        }

        /// <summary>
        /// The game is paused/unpaused
        /// play animation of the arm monitor
        /// </summary>
        /// <param name="a_IsPaused"></param>
        public override void onGamePauseToggled(bool a_IsPaused) 
        {
            base.onGamePauseToggled(a_IsPaused);

            if (m_animatorHands != null)
            {
                m_animatorHands.SetTrigger(a_IsPaused ? ANIM_STATE_OPEN_MENU : ANIM_STATE_CLOSE_MENU);
            }
        }

        private void Update()
        {
            //reset crosshair from the recoil movement to point forward
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, transform.parent.localRotation, m_fRotResetVelocityAnglePerSec * Time.deltaTime);
        }
    }
}