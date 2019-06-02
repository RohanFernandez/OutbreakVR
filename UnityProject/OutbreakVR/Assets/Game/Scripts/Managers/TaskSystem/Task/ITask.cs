using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public interface ITask
    {
        void onStartInitialization(Hashtable a_hashAttributes);
        void onStartExecution(System.Action a_SequenceCallbackTaskComplete);
        void onComplete();
        void onUpdate();
        string getTaskType();
    }
}