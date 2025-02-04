﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public enum PLAYER_STATE
    {
        NO_INTERACTION,     //No movement, no pointer
        MENU_SELECTION,     //No movement, menu pointer available
        IN_GAME_MOVEMENT,   //Movement available, in game pointer available
        IN_GAME_HALTED,      //No movement, in game pointer available
        IN_GAME_PAUSED,      //No movement, in game pointer unavailable
        IN_GAME_PARALYSED,   // No movement, no pointer, gravity enabled
        CINEMATIC,          // No movement, no pointer, gravity disabled, helmet enabled
    }

    public enum DAMAGE_INFLICTION_TYPE
    { 
        DEFAULT         =   0,
        STRIKE          =   1,
        GUNFIRE         =   2,
        BLAST           =   3,
        FALL_TO_DEATH   =   4,
        AREA_DAMAGE     =   5
    }

    [Serializable]
    public class DAMAGE_TYPE_SOUND
    {
        [SerializeField]
        private DAMAGE_INFLICTION_TYPE m_DamageInflictionType;
        public DAMAGE_INFLICTION_TYPE DamageInflictionType
        {
            get { return m_DamageInflictionType; }
        }

        [SerializeField]
        private List<string> m_lstAudClips = null;

        public string getRandomAudClipID()
        {
            return (m_lstAudClips.Count > 0) ? m_lstAudClips[UnityEngine.Random.Range(0, m_lstAudClips.Count)] : string.Empty;
        }
    }

    public class PlayerManager : AbsComponentHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static PlayerManager s_Instance = null;

        [SerializeField]
        private PlayerController m_PlayerController = null;

        /// <summary>
        /// The state the player is in
        /// </summary>
        [SerializeField]
        private PLAYER_STATE m_PlayerState;
        public static PLAYER_STATE PlayerState
        {
            get { return s_Instance.m_PlayerState; }
        }

        private PLAYER_STATE m_LastPlayerState = PLAYER_STATE.NO_INTERACTION;
        public static PLAYER_STATE LastPlayerState
        {
            get { return s_Instance.m_LastPlayerState; }
        }

        /// <summary>
        /// Max player life
        /// </summary>
        private const int MAX_PLAYER_HEALTH = 100;

        [SerializeField]
        private int m_iHealthMeter = 100;
        public static int HealthMeter
        {
            get { return s_Instance.m_iHealthMeter; }
            set {
                s_Instance.m_iHealthMeter = Mathf.Clamp(value, 0, MAX_PLAYER_HEALTH);

                EventHash l_EventHash = EventManager.GetEventHashtable();
                l_EventHash.Add(GameEventTypeConst.ID_PLAYER_HEALTH, s_Instance.m_iHealthMeter);
                EventManager.Dispatch(GAME_EVENT_TYPE.ON_PLAYER_HEALTH_UPDATED, l_EventHash);
            }
        }

        [SerializeField]
        private List<DAMAGE_TYPE_SOUND> m_lstDamageTypeSounds = null;

        private string getRandomDamageAudID(DAMAGE_INFLICTION_TYPE a_DamageInflictType)
        {
            int l_iDamageLstCount = m_lstDamageTypeSounds.Count;
            for (int l_iDamageTypeIndex = 0; l_iDamageTypeIndex < l_iDamageLstCount; l_iDamageTypeIndex++)
            {
                if (m_lstDamageTypeSounds[l_iDamageTypeIndex].DamageInflictionType == a_DamageInflictType)
                {
                    return m_lstDamageTypeSounds[l_iDamageTypeIndex].getRandomAudClipID();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// The player audio src used
        /// </summary>
        private int m_iAudSrcIndex = 0;

        /// <summary>
        /// Sets singleton instance
        /// </summary>
        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;
            m_PlayerController.initialize();
            SetPlayerState(m_PlayerState, true );

            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_GAME_PAUSED_TOGGLED, onGamePauseToggled);
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

            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_GAME_PAUSED_TOGGLED, onGamePauseToggled);
            m_PlayerController.destroy();
            s_Instance = null;
        }

        /// <summary>
        /// Gets position of the player
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetPosition()
        {
            return s_Instance.m_PlayerController.transform.position;
        }

        /// <summary>
        /// Gets position of the player
        /// </summary>
        /// <returns></returns>
        public static void SetPosition(Vector3 a_v3Position)
        {
            CharacterController l_CharController = s_Instance.m_PlayerController.CharacterController;
            l_CharController.enabled = false;
            s_Instance.m_PlayerController.transform.position = a_v3Position;
            l_CharController.enabled = true;
        }

        /// <summary>
        /// Gets forward of the player
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetForward()
        {
            return s_Instance.m_PlayerController.transform.forward;
        }

        /// <summary>
        /// The state the player is in
        /// sets its movement
        /// </summary>
        public static void SetPlayerState(PLAYER_STATE a_PlayerState, bool a_bForceEventDispatch = false)
        {
            if (a_PlayerState == s_Instance.m_PlayerState &&
                !a_bForceEventDispatch)
            {
                return;
            }
            PLAYER_STATE l_OldPlayerState = s_Instance.m_PlayerState;
            s_Instance.m_LastPlayerState = l_OldPlayerState;
            s_Instance.m_PlayerState = a_PlayerState;

            EventHash l_EventHash = EventManager.GetEventHashtable();
            l_EventHash.Add(GameEventTypeConst.ID_OLD_PLAYER_STATE, l_OldPlayerState);
            l_EventHash.Add(GameEventTypeConst.ID_NEW_PLAYER_STATE, a_PlayerState);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_PLAYER_STATE_CHANGED, l_EventHash);
        }

        /// <summary>
        /// Enemy inflicting damage on the player
        /// </summary>
        /// <param name="a_iDamage"></param>
        public static void InflictDamage(int a_iDamage, DAMAGE_INFLICTION_TYPE a_DmgInflictionType = DAMAGE_INFLICTION_TYPE.DEFAULT)
        {
            int l_iDamageBefore = HealthMeter;
            HealthMeter -= a_iDamage;

            EventHash l_EventHash = EventManager.GetEventHashtable();
            l_EventHash.Add(GameEventTypeConst.ID_DAMAGE_INFLICTED, a_iDamage);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_DAMAGE_INFLICTED_ON_PLAYER, l_EventHash);

            if (l_iDamageBefore > 0 &&
                HealthMeter <= 0)
            {
                s_Instance.playerKilled();
            }

            if (HealthMeter > 0)
            {
                string l_strDamageAudClipID = s_Instance.getRandomDamageAudID(a_DmgInflictionType);

                s_Instance.m_iAudSrcIndex = (s_Instance.m_iAudSrcIndex + 1) % 2;
                if (!string.IsNullOrEmpty(l_strDamageAudClipID))
                {
                    SoundManager.PlayAudio((s_Instance.m_iAudSrcIndex == 0) ? SoundConst.AUD_SRC_PLAYER_1 : SoundConst.AUD_SRC_PLAYER_2, l_strDamageAudClipID, false, 1.0f, AUDIO_SRC_TYPES.AUD_SRC_SFX);
                }
            }
        }

        /// <summary>
        /// Called when player is killed
        /// </summary>
        private void playerKilled()
        {
            SetPlayerState(PLAYER_STATE.IN_GAME_PARALYSED);
            EventHash l_EventHash = EventManager.GetEventHashtable();
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_PLAYER_KILLED, l_EventHash);
        }

        /// <summary>
        /// event called on game is paused / unpaused
        /// </summary>
        /// <param name="a_EventHash"></param>
        public void onGamePauseToggled(EventHash a_EventHash)
        {
            bool a_bIsGamePaused = (bool)a_EventHash[GameEventTypeConst.ID_GAME_PAUSED];
            if (a_bIsGamePaused)
            {
                SetPlayerState(PLAYER_STATE.IN_GAME_PAUSED);
            }
            else
            {
                SetPlayerState(LastPlayerState);
            }
        }
    }
}