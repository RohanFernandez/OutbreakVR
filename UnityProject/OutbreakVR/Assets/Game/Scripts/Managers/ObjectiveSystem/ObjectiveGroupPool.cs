using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public interface IObjectiveGroupPool
    {
        void returnToPool(IObjectiveGroup a_ObjectiveGroup);
        IObjectiveGroup getObjectiveGroup();

        int getActiveObjectCount();
        int getPooledObjectCount();
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