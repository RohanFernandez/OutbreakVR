using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class ObjectivePoolManager
    {
        /// <summary>
        /// Dictionary of objective group type name to the ObjectiveGroupBase
        /// </summary>
        private Dictionary<string, IObjectiveGroupPool> m_dictObjectivePoolGroup = null;

        /// <summary>
        /// Dictionary of objective type name to the Objective
        /// </summary>
        private Dictionary<string, IObjectivePool> m_dictObjectivePool = null;

        public ObjectivePoolManager()
        {
            m_dictObjectivePool = new Dictionary<string, IObjectivePool>(5);
            m_dictObjectivePoolGroup = new Dictionary<string, IObjectiveGroupPool>(5);
        }

        /// <summary>
        /// Gets objective of type in arguement from its type's pool
        /// </summary>
        /// <returns></returns>
        public IObjective getObjectiveFromPool(ScriptableObjective a_ScriptableObjective)
        {
            IObjectivePool l_ObjectivePool = null;
            if (!m_dictObjectivePool.TryGetValue(a_ScriptableObjective.m_strType, out l_ObjectivePool))
            {
                l_ObjectivePool = new ObjectivePool(a_ScriptableObjective.m_strType, 3);
                m_dictObjectivePool.Add(a_ScriptableObjective.m_strType, l_ObjectivePool);
            }
            IObjective l_Objective = l_ObjectivePool.getObjective();
            l_Objective.onInitialize(a_ScriptableObjective.m_hashAttributes);

            return l_Objective;
        }

        /// <summary>
        /// Returns the objective back into its respective pool
        /// </summary>
        public void returnObjectiveToPool(IObjective a_Objective)
        {
            IObjectivePool l_ObjectivePool = null;
            if (m_dictObjectivePool.TryGetValue(a_Objective.getObjectiveType(), out l_ObjectivePool))
            {
                l_ObjectivePool.returnToPool(a_Objective);
            }
        }

        /// <summary>
        /// Gets objective of type in arguement from its type's pool
        /// Sets the objectives as described in the objective group
        /// </summary>
        /// <returns></returns>
        public IObjectiveGroup getObjectiveGroupFromPool(ScriptableObjectiveGroup a_ScriptableObjectiveGroup)
        {
            IObjectiveGroupPool l_ObjGroupPool = null;
            if (!m_dictObjectivePoolGroup.TryGetValue(a_ScriptableObjectiveGroup.m_strType, out l_ObjGroupPool))
            {
                l_ObjGroupPool = new ObjectiveGroupPool(a_ScriptableObjectiveGroup.m_strType, 1);
                m_dictObjectivePoolGroup.Add(a_ScriptableObjectiveGroup.m_strType, l_ObjGroupPool);
            }

            IObjectiveGroup l_ObjectiveGroup = l_ObjGroupPool.getObjectiveGroup();
            for (int l_iObjIndex = 0; l_iObjIndex < a_ScriptableObjectiveGroup.m_iObjectiveCount; l_iObjIndex++)
            {
                l_ObjectiveGroup.addObjective((ObjectiveBase)getObjectiveFromPool(a_ScriptableObjectiveGroup.m_lstScriptibeObjective[l_iObjIndex]));
            }
            
            l_ObjectiveGroup.onInitialize(a_ScriptableObjectiveGroup.m_strID, a_ScriptableObjectiveGroup.m_strType);

            return l_ObjectiveGroup;
        }

        /// <summary>
        /// Returns the objective group and all objectives contained within it back into its respective pool
        /// </summary>
        /// <param name="a_ObjectiveGroup"></param>
        public void returnObjectiveGroupToPool(IObjectiveGroup a_ObjectiveGroup)
        {
            IObjectiveGroupPool l_ObjGroupPool = null;
            if (m_dictObjectivePoolGroup.TryGetValue(a_ObjectiveGroup.getObjGroupType(), out l_ObjGroupPool))
            {
                ObjectiveGroupBase l_ObjGroup = (ObjectiveGroupBase)a_ObjectiveGroup;
                int l_iObjCount = l_ObjGroup.m_lstObjectives.Count;
                for (int l_iObjIndex = 0; l_iObjIndex < l_iObjCount; l_iObjIndex++)
                {
                    returnObjectiveToPool(l_ObjGroup.m_lstObjectives[l_iObjIndex]);
                }
                a_ObjectiveGroup.onReturnedToPool();
                l_ObjGroupPool.returnToPool(a_ObjectiveGroup);
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// logs all alive/dead objective group pools and objectives
        /// </summary>
        /// <returns></returns>
        public static void LogGroupPools(System.Text.StringBuilder a_StrBuilder, ObjectivePoolManager a_ObjectivePoolManager)
        {
            a_StrBuilder.AppendLine("<color=BLUE>ObjectiveGroupPools:</color> \n");
            foreach (KeyValuePair<string, IObjectiveGroupPool> l_ObjectiveGroup in a_ObjectivePoolManager.m_dictObjectivePoolGroup)
            {
                a_StrBuilder.AppendLine(l_ObjectiveGroup.Key.ToString() + "\t Pooled : "+ l_ObjectiveGroup.Value.getPooledObjectCount() + "\t Unpooled : "+ l_ObjectiveGroup.Value.getActiveObjectCount());   
            }

            a_StrBuilder.AppendLine("<color=BLUE>\nObjectives: \n</color>");
            foreach (KeyValuePair<string, IObjectivePool> l_ObjectivePool in a_ObjectivePoolManager.m_dictObjectivePool)
            {
                a_StrBuilder.AppendLine(l_ObjectivePool.Key.ToString() + "\t Pooled : " + l_ObjectivePool.Value.getPooledObjectCount() + "\t Unpooled : " + l_ObjectivePool.Value.getActiveObjectCount());
            }
        }
#endif
    }
}