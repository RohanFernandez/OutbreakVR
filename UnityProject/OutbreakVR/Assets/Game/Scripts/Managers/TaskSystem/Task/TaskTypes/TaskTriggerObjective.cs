using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskTriggerObjective : TaskBase
    {
        #region ATTRIBUTE_KEY
        private const string ATTRIBUTE_OBJECTIVE_TRIGGER_ID = "TriggerID";
        #endregion ATTRIBUTE_KEY

        /// <summary>
        /// The objective trigger to be fired
        /// </summary>
        private string m_strObjectiveTriggerID = string.Empty;

        public override void onInitialize()
        {
            base.onInitialize();
            m_strObjectiveTriggerID = getString(ATTRIBUTE_OBJECTIVE_TRIGGER_ID);
        }

        public override void onExecute()
        {
            base.onExecute();
            ObjectiveManager.TriggerObjective(m_strObjectiveTriggerID);
            onComplete();
        }
    }
}