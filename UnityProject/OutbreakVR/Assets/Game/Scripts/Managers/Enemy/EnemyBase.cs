using System.Collections;
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
        DEAD
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
        protected float m_fAttackRadius = 15.0f;

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

        [SerializeField]
        private List<Transform> m_lstHitTransformPoint = null;

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
            
        }

        public virtual void onRetrievedFromPool()
        {
            m_iCurrentLifeCounter = m_iMaxLifeCapacityCounter;
        }

        /// <summary>
        /// Pauses enemy movement or action
        /// </summary>
        public virtual void pauseEnemy()
        {

        }

        /// <summary>
        /// Unpauses enemy movement and action
        /// </summary>
        public virtual void unpauseEnemy()
        {

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
        public void inflictDamage(int a_iDamage, Vector3 a_v3HitPoint)
        {
            int l_iHitTransformPointCount = m_lstHitTransformPoint.Count;
            if (l_iHitTransformPointCount > 0)
            {
                int l_iRandomIndex = Random.Range(0, l_iHitTransformPointCount);
                int l_iRandomIndexPlus1 = (l_iRandomIndex + 1) % l_iHitTransformPointCount;
                Vector3 l_v3CurrentTransform = m_lstHitTransformPoint[l_iRandomIndex].transform.position;
                Vector3 l_v3CurrentTransformPlus1 = m_lstHitTransformPoint[l_iRandomIndexPlus1].transform.position;

                Vector3 l_v3PosToSpawnDamageindicator = new Vector3(
                    Random.Range(l_v3CurrentTransform.x, l_v3CurrentTransformPlus1.x),
                    Random.Range(l_v3CurrentTransform.y, l_v3CurrentTransformPlus1.y), 
                    Random.Range(l_v3CurrentTransform.z, l_v3CurrentTransformPlus1.z));

                EnemyDamageIndicatorManager.ShowDamageIndicator(l_v3PosToSpawnDamageindicator, a_iDamage);
            }

            int l_iLifeCounterBeforeInflicted = m_iCurrentLifeCounter;
            m_iCurrentLifeCounter -= a_iDamage;

            if (m_iCurrentLifeCounter <= 0 &&
                l_iLifeCounterBeforeInflicted > 0)
            {
                onKilled();
            }
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
        /// 
        /// </summary>
        public virtual void onStrikeAttack() { }

        #endregion ATTACK CALLBACK

        protected virtual void onStateChanged(ENEMY_STATE l_OldNavState, ENEMY_STATE a_NavState)
        {
            /// Specific OLD to NEW state
            if (l_OldNavState == ENEMY_STATE.ALERT)
            {
                EventHash l_EventHash = EventManager.GetEventHashtable();
                l_EventHash.Add(GameEventTypeConst.ID_ENEMY_BASE, this);
                EventManager.Dispatch(GAME_EVENT_TYPE.ON_ENEMY_ALERT_ENDED, l_EventHash);
            }
            else if(a_NavState == ENEMY_STATE.ALERT)
            {
                EventHash l_EventHash = EventManager.GetEventHashtable();
                l_EventHash.Add(GameEventTypeConst.ID_ENEMY_BASE, this);
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