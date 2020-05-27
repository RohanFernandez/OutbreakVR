using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class SmashableDummyTarget : SmashableBase
    {
        /// <summary>
        /// The objective trigger to fire on smashed
        /// </summary>
        [SerializeField]
        private string m_strObjectiveTriggerOnSmash = string.Empty;

        /// <summary>
        /// Sets the unbroken object as active and the broken as deactivated
        /// </summary>
        public override void resetValues()
        {
            base.resetValues();
        }

        /// <summary>
        /// Breaks the crate, by deactivating the unbroken gameobject and activating the broken parent
        /// </summary>
        public override void smash()
        {
            base.smash();
            ObjectiveManager.TriggerObjective(m_strObjectiveTriggerOnSmash);
        }
    }
}