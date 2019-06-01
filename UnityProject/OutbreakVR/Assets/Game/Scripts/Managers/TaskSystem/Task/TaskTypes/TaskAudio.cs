using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskAudio : TaskBase
    {
        private float m_fWaitTime = 0.0f;

        public override void onInitialize(Hashtable a_hashAttributes)
        {
            base.onInitialize(a_hashAttributes);
            m_fWaitTime = float.Parse(a_hashAttributes["WaitTime"].ToString());

        }

        public override void onExecute()
        {
            base.onExecute();
        }

        public override void onComplete()
        {
            base.onComplete();
        }

        float timePassed = 0.0f;
        public override void onUpdate()
        {
            Debug.Log("Executing : " + m_fWaitTime);
            timePassed += Time.deltaTime;
            if (timePassed >= m_fWaitTime)
            {
                Debug.LogError("Completed : " + m_fWaitTime);
                onComplete();
            }
        }
    }
}