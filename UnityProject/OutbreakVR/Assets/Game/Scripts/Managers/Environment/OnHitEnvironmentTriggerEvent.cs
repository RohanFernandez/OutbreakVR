using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class OnHitEnvironmentTriggerEvent : MonoBehaviour, IEnvironmentTrigger
    {
        /// <summary>
        /// The sequence to execute on trigger
        /// </summary>
        [SerializeField]
        private string m_strSequenceToTrigger = string.Empty;

        [SerializeField]
        private UnityEngine.Events.UnityEvent m_OnTrigger = null;

        public void onObjectHit()
        {
            TaskManager.ExecuteSequence(m_strSequenceToTrigger);
            if (m_OnTrigger != null)
            {
                m_OnTrigger.Invoke();
            }
        }

        public void onPointerEnter()
        {
            
        }

        public void onPointerExit()
        {
            
        }

        public void onPointerInteract()
        {
            
        }
    }
}