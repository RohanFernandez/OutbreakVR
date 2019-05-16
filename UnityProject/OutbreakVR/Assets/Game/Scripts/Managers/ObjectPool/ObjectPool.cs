using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ns_Mashmo
{
    [Serializable]
    public class ObjectPool<T> where T : UnityEngine.Object
    {
        /// <summary>
        /// Constructor.
        /// Sets the Prefab from which the objects in the pool are generated
        /// </summary>
        /// <param name="a_ObjPrefab"></param>
        public ObjectPool(UnityEngine.Object a_Obj, int a_iStartPoolObjects = 0)
        {
            if (a_Obj == null)
            {
                Debug.LogError("ObjectPool::ObjectPool:: Object to create an Object pool if is null.");
                return;
            }
            for (int l_iPoolIndex = 0; l_iPoolIndex < a_iStartPoolObjects; l_iPoolIndex++)
            {
                ReturnToPool((a_Obj) as T);
            }
        }

        /// <summary>
        /// The stack that contains the objects of the pool.
        /// </summary>
        private Stack<T> m_Pool = new Stack<T>();

        /// <summary>
        /// Gets an object from the pool.
        /// If the pool is empty then the pool will instantiate a object and return it.
        /// </summary>
        /// <returns></returns>
        public T GetFromPool(T a_ObjPrefab)
        {
            return m_Pool.Count == 0 ? MonoBehaviour.Instantiate(a_ObjPrefab) as T : m_Pool.Pop();
        }

        /// <summary>
        /// Returns the object into the pool.
        /// </summary>
        /// <param name="a_Obj"></param>
        public void ReturnToPool(T a_Obj)
        {
            m_Pool.Push(a_Obj);
        }
    }
}