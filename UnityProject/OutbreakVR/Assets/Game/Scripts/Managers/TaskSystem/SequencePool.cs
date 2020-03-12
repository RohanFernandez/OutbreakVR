using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public interface ISequencePool
    {
        void returnToPool(ISequence a_Sequence);
        ISequence getSequence();

        int getActiveObjectCount();
        int getPooledObjectCount();
    }

    public class SequencePool : ObjectPool<ISequence>, ISequencePool
    {
        public SequencePool(string a_SequenceType, int a_iStartSize = 0)
            : base(SystemConsts.NAMESPACE_MASHMO + a_SequenceType, a_iStartSize)
        {
        }

        public ISequence getSequence()
        {
            return getObject();
        }

        public int getActiveObjectCount()
        {
            return getActiveList().Count;
        }

        public int getPooledObjectCount()
        {
            return getPooledList().Count;
        }
    }
}