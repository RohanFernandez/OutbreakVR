using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class ObjectiveKillEnemy : ObjectiveBase
    {
        #region
            private const string ATTRIBUTE_SUB_OBJECTIVE_COUNT = "SubObjectiveCount";
        #endregion

        private int m_iSubObjectiveCount = 0;

        private int m_iCurrentSubObjectiveCompleted = 0;

        public override void onInitialize(Hashtable a_hashtable)
        {
            base.onInitialize(a_hashtable);

            m_iSubObjectiveCount = getInt(ATTRIBUTE_SUB_OBJECTIVE_COUNT);
            if (m_iSubObjectiveCount == 0) { m_iSubObjectiveCount = 1; }

            m_iCurrentSubObjectiveCompleted = 0;
        }

        public override void checkObjectiveCompletion(Hashtable a_Hashtable)
        {
            string l_strObjectiveTriggerID = GeneralUtils.GetString(a_Hashtable, GameEventTypeConst.ID_OBJECTIVE_TRIGGER_ID);
            if (l_strObjectiveTriggerID.Equals(EnemyManager.ENEMY_OBJECTIVE_ID, System.StringComparison.OrdinalIgnoreCase))
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