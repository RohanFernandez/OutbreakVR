using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class ObjectPool<T> where T : class
    {
        /// <summary>
        /// The stack that contains the objects of the pool.
        /// </summary>
        private Stack<T> m_Pool = null;

        /// <summary>
        /// The type of the pooled object
        /// </summary>
        private System.Type m_Type = null;

        /// <summary>
        /// Constructor.
        /// Sets the Prefab from which the objects in the pool are generated
        /// </summary>
        /// <param name="a_ObjPrefab"></param>
        public ObjectPool(string a_strObjectType, int a_iStartSize = 0)
        {
            m_Type = System.Type.GetType(a_strObjectType);
            m_Pool = new Stack<T>(a_iStartSize);

            for (int l_iIndex = 0; l_iIndex < a_iStartSize; l_iIndex++)
            {
                createObj();
            }
        }

        /// <summary>
        /// Creates object of type T and pushes into the pool
        /// </summary>
        public void createObj()
        {
            T l_CreatedObj = System.Activator.CreateInstance(m_Type) as T;
            
            m_Pool.Push(l_CreatedObj);
        }

        /// <summary>
        /// Returns back into the pool for reuse in the future
        /// </summary>
        /// <param name="a_Obj"></param>
        public void returnToPool(T a_Obj)
        {
            m_Pool.Push(a_Obj);
        }

        /// <summary>
        /// Gets object of type from the pool
        /// sets the task variables
        /// </summary>
        public T getObject()
        {
            if (m_Pool.Count == 0)
            {
                createObj();
            }
            return m_Pool.Pop();
        }
    }
}