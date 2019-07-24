using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EnemyRangeDetector : MonoBehaviour
    {
        [SerializeField]
        public List<EnemyPatrolPoint> m_lstPatrolPointsWithinRange = new List<EnemyPatrolPoint>(20);

        /// <summary>
        /// On enemy activated, clears the list of all the patrol points withineange
        /// </summary>
        public void onActivated()
        {
            m_lstPatrolPointsWithinRange.Clear();
        }

        public void onDeactivated()
        {
            m_lstPatrolPointsWithinRange.Clear();
        }

        /// <summary>
        /// Adds patrol point within range
        /// </summary>
        /// <param name="a_Collider"></param>
        private void OnTriggerEnter(Collider a_Collider)
        {
            EnemyPatrolPoint l_EnemyPatrolPoint = a_Collider.GetComponent<EnemyPatrolPoint>();

            if (l_EnemyPatrolPoint != null)
            {
                m_lstPatrolPointsWithinRange.Add(l_EnemyPatrolPoint);
            }
        }

        /// <summary>
        /// Adds patrol point within range
        /// </summary>
        /// <param name="a_Collider"></param>
        private void OnTriggerExit(Collider a_Collider)
        {
            EnemyPatrolPoint l_EnemyPatrolPoint = a_Collider.GetComponent<EnemyPatrolPoint>();
            if (l_EnemyPatrolPoint != null)
            {
                m_lstPatrolPointsWithinRange.Remove(l_EnemyPatrolPoint);
            }
        }
    }
}