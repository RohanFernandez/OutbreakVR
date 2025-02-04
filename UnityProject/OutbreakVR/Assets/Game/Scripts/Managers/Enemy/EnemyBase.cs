﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public enum ENEMY_STATE
    {
        NONE,
        IDLE,
        PATROL,
        ALERT,
        DEAD,
        SUFFER,
        DAMAGE_INFLICTED
    }

    public abstract class EnemyBase : MonoBehaviour, IEnemy
    {
        /// <summary>
        /// The type of the enemy
        /// Can be identified
        /// </summary>
        [SerializeField]
        protected ENEMY_TYPE m_EnemyType;

        /// <summary>
        /// The ID to refer this enemy
        /// </summary>
        [SerializeField]
        private string m_strEnemyID = string.Empty;

        /// <summary>
        /// The radius inside, if the player comes within this radius the enemy will start attacking
        /// </summary>
        [SerializeField]
        protected float m_fMaxDamagePlayerDamageRadius = 15.0f;

        [SerializeField]
        protected int m_iMaxLifeCapacityCounter = 100;

        [SerializeField]
        protected int m_iCurrentLifeCounter = 100;

        [SerializeField]
        protected float m_fWaitTimeAfterKilled = 2.0f;

        /// <summary>
        /// Coroutine that is called to wait for time after killed and then deactivate
        /// </summary>
        private Coroutine m_coDeactiveOnKilled = null;

        /// <summary>
        /// The state of the enemy
        /// </summary>
        private ENEMY_STATE m_NavState = ENEMY_STATE.NONE;

        /// <summary>
        /// The ray that detects from the transform to the player to check if the player is in the line of sight
        /// </summary>
        protected Ray m_RayDetector = new Ray();

        /// <summary>
        /// The layer mask that a ray from the enemy pointed to the player will be detected
        /// </summary>
        [SerializeField]
        protected LayerMask m_AttackLayerMask;

        /// <summary>
        /// Returns the tyoe of the enemy
        /// </summary>
        /// <returns></returns>
        public ENEMY_TYPE getEnemyType()
        {
            return m_EnemyType;
        }

        /// <summary>
        /// Returns the type of attack the enemy will possess
        /// </summary>
        /// <returns></returns>
        public abstract ENEMY_ATTACK_TYPE getEnemyAttackType();

        /// <summary>
        /// Action called on update, will update the current state the enemy is in
        /// </summary>
        protected System.Action m_actNavStateUpdate = null;

        /// <summary>
        /// Activates use of enemy
        /// </summary>
        public virtual void activateEnemy()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Deactivates use of enemy
        /// </summary>
        public virtual void deactivateEnemy()
        {
            if (m_coDeactiveOnKilled != null)
            {
                StopCoroutine(m_coDeactiveOnKilled);
            }
            gameObject.SetActive(false);
        }

        public virtual void Update()
        {
            if (m_actNavStateUpdate != null)
            {
                m_actNavStateUpdate();
            }
        }

        public virtual void onReturnedToPool()
        {
            NavState = ENEMY_STATE.NONE;
        }

        public virtual void onRetrievedFromPool()
        {
            m_iCurrentLifeCounter = m_iMaxLifeCapacityCounter;
        }

        public string getID()
        {
            return m_strEnemyID;
        }

        /// <summary>
        /// Sets ID
        /// </summary>
        /// <param name="a_strID"></param>
        /// <returns></returns>
        public void setID(string a_strID)
        {
            m_strEnemyID = a_strID;
        }

        /// <summary>
        /// reduces the life counter of the enemy
        /// </summary>
        /// <param name="a_iDamage"></param>
        public void inflictDamage(int a_iDamage, Vector3 a_v3HitPoint, ENEMY_HIT_COLLISION a_EnemyHitCollision = ENEMY_HIT_COLLISION.HIT_COLLISION_DEFAULT)
        {
            int l_iLifeCounterBeforeInflicted = m_iCurrentLifeCounter;
            m_iCurrentLifeCounter -= a_iDamage;

            if (m_iCurrentLifeCounter <= 0 &&
                l_iLifeCounterBeforeInflicted > 0)
            {
                onKilled();
            }
            else if(m_iCurrentLifeCounter > 0)
            {
                onDamageInflictedNotKilled(a_iDamage, a_EnemyHitCollision);
            }
            // else ALREADY DEAD
        }

        /// <summary>
        /// Called on damage is inflicted on the enemy but the enemy is still alive
        /// </summary>
        /// <param name="a_iDamage"></param>
        protected virtual void onDamageInflictedNotKilled(int a_iDamage, ENEMY_HIT_COLLISION a_EnemyHitCollision = ENEMY_HIT_COLLISION.HIT_COLLISION_DEFAULT)
        {

        }

        /// <summary>
        /// called on killed fire event 
        /// </summary>
        protected virtual void onKilled()
        {
            NavState = ENEMY_STATE.DEAD;
            EnemyManager.OnEnemyKilled(this);
            m_coDeactiveOnKilled = StartCoroutine(startEnemyDeactivateAfterTimer());
        }

        /// <summary>
        /// Waits for time and then deactivates and returns enemy back to pool
        /// </summary>
        /// <returns></returns>
        private IEnumerator startEnemyDeactivateAfterTimer()
        {
            yield return new WaitForSeconds(m_fWaitTimeAfterKilled);
            EnemyManager.ReturnActiveEnemyToPool(m_EnemyType, m_strEnemyID);
            m_coDeactiveOnKilled = null;
        }

        #region ATTACK CALLBACK

        /// <summary>
        /// the callback on starting to strike
        /// </summary>
        public virtual void onStrikeAttackStart(int a_iStrikeAttackIndex = 0) { }

        /// <summary>
        /// the callback on strikeing the player and detecting the point of hit, check if player is within range
        /// </summary>
        public virtual void onStrikeAttackHitDetection(int a_iStrikeAttackIndex = 0) { }

        public virtual void onGunFired() { }

        #endregion ATTACK CALLBACK

        protected virtual void onStateChanged(ENEMY_STATE l_OldNavState, ENEMY_STATE a_NavState)
        {
            /// Specific OLD to NEW state
            if ((l_OldNavState == ENEMY_STATE.ALERT ||
                l_OldNavState == ENEMY_STATE.SUFFER ||
                l_OldNavState == ENEMY_STATE.DAMAGE_INFLICTED)
                && 
                (a_NavState != ENEMY_STATE.ALERT &&
                a_NavState != ENEMY_STATE.SUFFER &&
                a_NavState != ENEMY_STATE.DAMAGE_INFLICTED))
            {
                EventHash l_EventHash = EventManager.GetEventHashtable();
                l_EventHash.Add(GameEventTypeConst.ID_ENEMY_BASE, this);
                EventManager.Dispatch(GAME_EVENT_TYPE.ON_ENEMY_ALERT_ENDED, l_EventHash);
            }
            else if(a_NavState == ENEMY_STATE.ALERT ||
                a_NavState == ENEMY_STATE.SUFFER ||
                a_NavState == ENEMY_STATE.DAMAGE_INFLICTED)
            {
                EventHash l_EventHash = EventManager.GetEventHashtable();
                l_EventHash.Add(GameEventTypeConst.ID_ENEMY_BASE, this);
                l_EventHash.Add(GameEventTypeConst.ID_FORCED_ENEMY_ALERT, false);
                EventManager.Dispatch(GAME_EVENT_TYPE.ON_ENEMY_ALERT_STARTED, l_EventHash);
            }
        }

        /// <summary>
        /// Setting the nav state should be only done via this setter
        /// </summary>
        protected ENEMY_STATE NavState
        {
            get { return m_NavState; }
            set
            {
                if (m_NavState == value)
                {
                    return;
                }
                ENEMY_STATE l_OldNavState = m_NavState;
                m_NavState = value;
                onStateChanged(l_OldNavState, m_NavState);
            }
        }
    }

}