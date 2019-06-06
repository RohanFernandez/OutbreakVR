using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public interface IObjectiveGroupPool
    {
        void returnToPool(IObjectiveGroup a_ObjectiveGroup);
        IObjectiveGroup getObjectiveGroup();
    }

    public class ObjectiveGroupPool : ObjectPool<IObjectiveGroup>, IObjectiveGroupPool
    {
        public ObjectiveGroupPool(string a_strObjectiveGroupType, int a_iStartSize = 0)
            : base(SystemConsts.NAMESPACE_MASHMO + a_strObjectiveGroupType, a_iStartSize)
        {
        }

        public IObjectiveGroup getObjectiveGroup()
        {
            return getObject();
        }
    }
}