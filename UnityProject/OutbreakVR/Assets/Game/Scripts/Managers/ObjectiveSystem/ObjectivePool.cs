using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public interface IObjectivePool
    {
        void returnToPool(IObjective a_Objective);
        IObjective getObjective();
    }

    public class ObjectivePool : ObjectPool<IObjective>, IObjectivePool
    {
        public ObjectivePool(string a_strObjectiveType, int a_iStartSize = 0)
            : base(SystemConsts.NAMESPACE_MASHMO + a_strObjectiveType, a_iStartSize)
        {
        }

        public IObjective getObjective()
        {
            return getObject();
        }
    }
}