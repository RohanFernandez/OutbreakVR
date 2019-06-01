using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public interface ISequence
    {
        void onInitialize(string a_strSequenceID);

        void onExecute();

        void onComplete();

        void addTask(ITask a_Task);
    }
}