using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class EnemyDependantBase : MonoBehaviour
    {
        /// <summary>
        /// The enmey ID of the enemy this object is a dependant of
        /// </summary>
        [SerializeField]
        private string m_strEnemyID = string.Empty;
        public string EnemyID
        {
            get { return m_strEnemyID; }
        }

        void Awake()
        {
            EnemyManager.RegisterUnregisterEnemyDependant(true, this);
        }

        void OnDestroy()
        {
            EnemyManager.RegisterUnregisterEnemyDependant(false, this);
        }

        /// <summary>
        /// on dependant activated
        /// </summary>
        public virtual void onActivate()
        {
            
        }

        /// <summary>
        /// on dependant deactivated
        /// </summary>
        public virtual void onDeactivate()
        {

        }

        /// <summary>
        /// on interacted with dependant
        /// </summary>
        public virtual void onInteract()
        {

        }
    }
}