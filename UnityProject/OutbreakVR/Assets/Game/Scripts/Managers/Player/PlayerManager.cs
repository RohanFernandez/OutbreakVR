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
        IN_GAME_HALTED      //No movement, in game pointer available
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

        /// <summary>
        /// Max player life
        /// </summary>
        private const int MAX_PLAYER_LIFE_METER = 100;

        [SerializeField]
        private int m_iLifeMeter = 100;
        public int LifeMeter
        {
            get { return m_iLifeMeter; }
            set {
                if(value <= 0)
                {
                    m_iLifeMeter = 0;
                }
                else if (value >= MAX_PLAYER_LIFE_METER)
                {
                    m_iLifeMeter = MAX_PLAYER_LIFE_METER;
                }
                else
                {
                    m_iLifeMeter = value;
                }
            }
        }

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
            s_Instance.m_PlayerController.transform.position = a_v3Position;
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
        public static void InflictDamage(int a_iDamage)
        {
            int l_iDamageBefore = s_Instance.LifeMeter;
            s_Instance.LifeMeter -= a_iDamage;

            if (l_iDamageBefore > 0 &&
                s_Instance.LifeMeter <= 0)
            {
                s_Instance.playerKilled();
            }
        }

        /// <summary>
        /// Called when player is killed
        /// </summary>
        private void playerKilled()
        {
            EventHash l_EventHash = EventManager.GetEventHashtable();
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_PLAYER_KILLED, l_EventHash);
        }
    }
}