using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public interface ISequencePool
    {
        void returnToPool(ISequence a_Sequence);
        ISequence getSequence();
    }

    public class SequencePool : ObjectPool<ISequence>, ISequencePool
    {
        public SequencePool(string a_SequenceType, int a_iStartSize = 0)
            : base(SystemManager.NAMESPACE_MASHMO + a_SequenceType, a_iStartSize)
        {
        }

        public ISequence getSequence()
        {
            return getObject();
        }
    }
}