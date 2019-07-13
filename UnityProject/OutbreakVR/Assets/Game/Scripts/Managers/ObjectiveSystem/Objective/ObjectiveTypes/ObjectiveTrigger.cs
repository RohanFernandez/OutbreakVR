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
        private const string ATTRIBUTE_SUB_OBJECTIVE_COUNT = "SubObjectiveCount";
        #endregion

        private string m_strTriggerID = string.Empty;
        private int m_iSubObjectiveCount = 0;

        private int m_iCurrentSubObjectiveCompleted = 0;

        public override void onInitialize(Hashtable a_hashtable)
        {
            base.onInitialize(a_hashtable);
            m_strTriggerID = getString(ATTRIBUTE_TRIGGER_ID);

            m_iSubObjectiveCount = getInt(ATTRIBUTE_SUB_OBJECTIVE_COUNT);
            if (m_iSubObjectiveCount == 0) { m_iSubObjectiveCount = 1; }

            m_iCurrentSubObjectiveCompleted = 0;
        }

        public override void checkObjectiveCompletion(Hashtable a_Hashtable)
        {
            string l_strObjectiveTriggerID = GeneralUtils.GetString(a_Hashtable, GameEventTypeConst.ID_OBJECTIVE_TRIGGER_ID);
            if (l_strObjectiveTriggerID.Equals(m_strTriggerID, System.StringComparison.OrdinalIgnoreCase))
            {
                m_iCurrentSubObjectiveCompleted++;
                if (m_iCurrentSubObjectiveCompleted >= m_iSubObjectiveCount)
                {
                    onComplete();
                }
            }
        }
    }
}