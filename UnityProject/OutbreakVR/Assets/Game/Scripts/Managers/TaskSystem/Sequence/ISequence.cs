using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public interface ISequence
    {
        void execute();
        void onComplete();

        void executeJob();
    }
}