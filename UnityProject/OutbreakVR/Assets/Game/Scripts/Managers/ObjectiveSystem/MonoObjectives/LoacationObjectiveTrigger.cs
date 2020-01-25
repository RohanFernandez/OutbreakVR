using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class LoacationObjectiveTrigger : MonoBehaviour
    {
        /// <summary>
        /// The objective trigger ID
        /// </summary>
        [SerializeField]
        private string m_strObjectiveTriggerID = string.Empty;

        /// <summary>
        /// Fire event of trigger id on player entered the collider trigger
        /// </summary>
        /// <param name="a_Collider"></param>
        void OnTriggerEnter(Collider a_Collider)
        {
            if (a_Collider.tag.Equals(GameConsts.TAG_PLAYER, System.StringComparison.OrdinalIgnoreCase))
            {
                ObjectiveManager.TriggerObjective(m_strObjectiveTriggerID);
            }
        }
    }
}