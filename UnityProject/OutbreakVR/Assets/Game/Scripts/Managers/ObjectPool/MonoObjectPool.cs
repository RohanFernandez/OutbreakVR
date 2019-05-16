using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class MonoObjectPool<T> where T : MonoBehaviour
    {
        /// <summary>
        /// Constructor that specifies the prefab for which the pool manages
        /// Constructs a stack with the specified capacity
        /// </summary>
        /// <param name="a_Prefab"></param>
        /// <param name="a_iStartPoolCapacity"></param>
        public MonoObjectPool(T a_Prefab, int a_iStartPoolCapacity = 0)
        {
            m_Prefab = a_Prefab;
            m_Pool = new Stack<T>(a_iStartPoolCapacity);
            for (int l_iPoolIndex = 0; l_iPoolIndex < a_iStartPoolCapacity; l_iPoolIndex++)
            {
                T l_newObj = MonoBehaviour.Instantiate(a_Prefab);
                l_newObj.gameObject.SetActive(false);
                m_Pool.Push(l_newObj);
            }
        }

        /// <summary>
        /// The pool will manage the creation of this Prefab.
        /// </summary>
        private T m_Prefab = null;

        /// <summary>
        /// Stack that will act like a pool for the gameobjects stored
        /// </summary>
        public Stack<T> m_Pool = null;

        public List<T> m_lstActivePooledObjects = new List<T>();

        /// <summary>
        /// Returns a pooled object.
        /// Pops the stored pooled object if an object exists in the pool else
        /// instantiates a new obj of type T and returns it.
        /// </summary>
        /// <returns></returns>
        public T getPooledObj(Transform a_transformParent)
        {
            T l_LiveObj = m_Pool.Count == 0 ? instantiatePrefab(a_transformParent) : m_Pool.Pop();
            m_lstActivePooledObjects.Add(l_LiveObj);
            return l_LiveObj;
        }

        /// <summary>
        /// Instantiates the game object from the prefab.
        /// Sets the instantiated gameobject as a child of the given transform arguement.
        /// </summary>
        /// <param name="a_transformParent"></param>
        /// <returns></returns>
        private T instantiatePrefab(Transform a_transformParent)
        {
            T l_InstantiatedPrefab = MonoBehaviour.Instantiate(m_Prefab);
            l_InstantiatedPrefab.transform.SetParent(a_transformParent);
            return l_InstantiatedPrefab;
        }

        /// <summary>
        /// Returns the prefab back into the pool after use.
        /// </summary>
        public void returnToPool(T a_PrefabToReturn)
        {
            if (m_lstActivePooledObjects.Remove(a_PrefabToReturn))
            {
                m_Pool.Push(a_PrefabToReturn);
            }
        }

        /// <summary>
        /// Clears all the gameobjects in the pool
        /// </summary>
        public void clearPool()
        {
            m_Pool.Clear();
        }
    }
}