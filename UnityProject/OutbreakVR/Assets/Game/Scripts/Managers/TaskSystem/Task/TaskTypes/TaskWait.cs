using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskWait : TaskBase
    {
        private float m_fWaitTime = 0.0f;
        private float m_fTimePassed = 0.0f;

        public override void onInitialize(Hashtable a_hashAttributes)
        {
            base.onInitialize(a_hashAttributes);

            m_fWaitTime = float.Parse(a_hashAttributes["WaitTime"].ToString());
        }

        public override void onExecute()
        {
            base.onExecute();
            m_fTimePassed = 0.0f;
        }

        public override void onUpdate()
        {
            base.onUpdate();
            m_fTimePassed += Time.deltaTime;
            if (m_fTimePassed >= m_fWaitTime)
            {
                onComplete();
            }
        }
    }
}