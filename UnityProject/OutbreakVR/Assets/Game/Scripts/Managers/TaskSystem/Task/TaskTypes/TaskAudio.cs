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

        IEnumerator waitForAudio()
        {
            yield return new WaitForSeconds(m_fWaitTime);
            Debug.LogError("AudioCompleted : Wait Time :"+ m_fWaitTime);
            onComplete();
        }

        public override void onExecute()
        {
            base.onExecute();
            GameManager.StartCoroutineExecution(waitForAudio());
        }

        public override void onComplete()
        {
            base.onComplete();
        }
    }
}