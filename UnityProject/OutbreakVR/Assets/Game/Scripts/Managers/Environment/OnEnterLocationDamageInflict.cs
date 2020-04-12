using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class OnEnterLocationDamageInflict : MonoBehaviour
    {
        /// <summary>
        /// Is the player inside the trigger area
        /// </summary>
        [SerializeField]
        private bool m_bIsPlayerInside = false;

        /// <summary>
        /// The damage will be inflicted on the player after this amount of time when the player is inside the trigger area
        /// </summary>
        [SerializeField]
        private float m_fDamageAfterTime = 1.0f;

        /// <summary>
        /// the amount of damage that will be inflicted onto the player everytime after the time
        /// </summary>
        [Range(1, 100)]
        [SerializeField]
        private int m_iDamageToInflictAfterTime = 2;

        /// <summary>
        /// The time leading upto the damage to inflict
        /// </summary>
        private float m_fTimeCounterCompletedInTriggerArea = 0.0f;

        private void Awake()
        {
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_LEVEL_RESTARTED, resetPlayerDetection);
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_GAMEPLAY_ENDED, resetPlayerDetection);
        }

        private void OnDestroy()
        {
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_LEVEL_RESTARTED, resetPlayerDetection);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_GAMEPLAY_ENDED, resetPlayerDetection);
        }

        private void OnTriggerEnter(Collider a_Collider)
        {
            if (a_Collider.gameObject.layer == LayerMask.NameToLayer(GameConsts.LAYER_NAME_PLAYER))
            { 
                m_bIsPlayerInside = true;
                m_fTimeCounterCompletedInTriggerArea = 0.0f;
            }
        }

        private void OnTriggerExit(Collider a_Collider)
        {
            if (a_Collider.gameObject.layer == LayerMask.NameToLayer(GameConsts.LAYER_NAME_PLAYER))
            {
                resetPlayerDetection(null);
            }
        }

        /// <summary>
        /// Resets the player to be outside the trigger area
        /// a_EventHash can be null if called from OnTriggerExit
        /// </summary>
        private void resetPlayerDetection(EventHash a_EventHash)
        {
            m_bIsPlayerInside = false;
            m_fTimeCounterCompletedInTriggerArea = 0.0f;
        }

        private void Update()
        {
            if (m_bIsPlayerInside )
            {
                m_fTimeCounterCompletedInTriggerArea += Time.deltaTime;
                if (m_fTimeCounterCompletedInTriggerArea >= m_fDamageAfterTime)
                {
                    m_fTimeCounterCompletedInTriggerArea = 0.0f;
                    PlayerManager.InflictDamage(m_iDamageToInflictAfterTime);
                }
            }
        }
    }
}