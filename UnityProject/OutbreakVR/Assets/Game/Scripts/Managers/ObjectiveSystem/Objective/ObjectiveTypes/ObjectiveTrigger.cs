using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class ObjectiveTrigger : ObjectiveBase
    {
        #region
        private const string ATTRIBUTE_TRIGGER_ID = "TriggerID";
        #endregion

        private string m_strTriggerID = string.Empty;

        public override void onInitialize(Hashtable a_hashtable)
        {
            base.onInitialize(a_hashtable);
            m_strTriggerID = getString(ATTRIBUTE_TRIGGER_ID);
        }

        public override void checkObjectiveCompletion(Hashtable a_Hashtable)
        {
            string l_strObjectiveTriggerID = GeneralUtils.GetString(a_Hashtable, GameEventTypeConst.ID_OBJECTIVE_TRIGGER_ID);
            if (l_strObjectiveTriggerID.Equals(m_strTriggerID, System.StringComparison.OrdinalIgnoreCase))
            {
                onComplete();
            }
        }
    }
}