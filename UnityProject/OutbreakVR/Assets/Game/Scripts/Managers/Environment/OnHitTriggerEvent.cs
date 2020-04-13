using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class OnHitTriggerEvent : MonoBehaviour, IEnvironmentTrigger
    {
        /// <summary>
        /// The sequence to execute on trigger
        /// </summary>
        [SerializeField]
        private string m_strSequenceToTrigger = string.Empty;

        public void onObjectHit()
        {
            TaskManager.ExecuteSequence(m_strSequenceToTrigger);
        }
    }
}