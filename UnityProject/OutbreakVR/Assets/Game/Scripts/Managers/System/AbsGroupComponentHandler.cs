using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class AbsGroupComponentHandler : AbsComponentHandler
    {
        /// <summary>
        /// List of all components that are initialized and destroy by this
        /// </summary>
        [SerializeField]
        List<AbsComponentHandler> m_lstComponentsManaged = null;

        /// <summary>
        /// Initializes all components managed by this
        /// </summary>
        public override void initialize()
        {
            int l_iManagedComponentCount = m_lstComponentsManaged.Count;
            for (int l_iComponentIndex = 0; l_iComponentIndex < l_iManagedComponentCount; l_iComponentIndex++)
            {
                m_lstComponentsManaged[l_iComponentIndex].initialize();
            }
        }

        /// <summary>
        /// Destroys all components managed by this
        /// </summary>
        public override void destroy()
        {
            int l_iCurrentComponentIndex = m_lstComponentsManaged.Count - 1;
            while (l_iCurrentComponentIndex >= 0)
            {
                m_lstComponentsManaged[l_iCurrentComponentIndex].destroy();
                --l_iCurrentComponentIndex;
            }
        }
    }
}