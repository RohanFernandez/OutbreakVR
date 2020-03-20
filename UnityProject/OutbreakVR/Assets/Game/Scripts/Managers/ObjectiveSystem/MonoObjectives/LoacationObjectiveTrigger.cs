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
            if (a_Collider.gameObject.layer == LayerMask.NameToLayer(GameConsts.LAYER_NAME_PLAYER))
            {
                ObjectiveManager.TriggerObjective(m_strObjectiveTriggerID);
            }
        }
    }
}