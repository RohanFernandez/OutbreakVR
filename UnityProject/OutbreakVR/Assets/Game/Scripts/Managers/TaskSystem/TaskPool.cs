using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public interface ITaskPool
    {
        void returnToPool(ITask a_TaskBase);
        ITask getTask();
    }

    public class TaskPool: ObjectPool<ITask>, ITaskPool
    {
        public TaskPool(string a_strTaskType, int a_iStartSize = 0)
            : base(SystemConsts.NAMESPACE_MASHMO + a_strTaskType, a_iStartSize)
        {
        }

        public ITask getTask()
        {
            return getObject();
        }
    }
}