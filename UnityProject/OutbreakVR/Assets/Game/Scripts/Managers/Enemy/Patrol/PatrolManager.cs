using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class PatrolManager : IComponentHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static PatrolManager s_Instance = null;

        [SerializeField]
        private List<EnemyPatrolPoint> m_lstEnemyPatrolPoints = new List<EnemyPatrolPoint>(10);

        /// <summary>
        /// The minimum distance the next patrol point should be from the current position
        /// </summary>
        private const float MIN_PATROL_POINT_DISTANCE = 50.0f;

        /// <summary>
        /// The patrol points currently valid which are withing distance
        /// </summary>
        private List<EnemyPatrolPoint> m_lstCurrentValidPatrolPoints = new List<EnemyPatrolPoint>(15);

        /// <summary>
        /// The nav mesh path of this body to manage movement towards the player
        /// </summary>
        protected UnityEngine.AI.NavMeshPath m_NavMeshPath = null;

        /// <summary>
        /// sets singleton to this
        /// </summary>
        public void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;
            if (m_NavMeshPath == null) { m_NavMeshPath = new UnityEngine.AI.NavMeshPath(); }
        }

        /// <summary>
        /// sets singleton to null
        /// </summary>
        public void destroy()
        {
            if (s_Instance == this)
            {
                return;
            }
            s_Instance = null;
        }

        /// <summary>
        /// Returns the sum of the distance of individual corners
        /// </summary>
        /// <param name="a_NavmeshPath"></param>
        /// <returns></returns>
        public static float GetNavDistanceToTarget(UnityEngine.AI.NavMeshPath a_NavmeshPath)
        {
            if (a_NavmeshPath == null) { return 0.0f; }

            int l_iCornersCount = a_NavmeshPath.corners.Length;
            if (l_iCornersCount < 2) { return 0.0f; }

            Vector3 l_v3previousCorner = a_NavmeshPath.corners[0];
            float l_fLengthSoFar = 0.0F;

            for (int l_iCornerIndex = 0; l_iCornerIndex < l_iCornersCount; l_iCornerIndex++)
            {
                Vector3 l_v3CurrentCorner = a_NavmeshPath.corners[l_iCornerIndex];
                l_fLengthSoFar += Vector3.Distance(l_v3previousCorner, l_v3CurrentCorner);
                l_v3previousCorner = l_v3CurrentCorner;
            }
            return l_fLengthSoFar;
        }

        /// <summary>
        /// Registers the Patrol point
        /// </summary>
        public static void RegisterPatrolPoint(EnemyPatrolPoint a_PatrolPoint)
        {
            if (!s_Instance.m_lstEnemyPatrolPoints.Contains(a_PatrolPoint))
            {
                s_Instance.m_lstEnemyPatrolPoints.Add(a_PatrolPoint);
            }
        }

        /// <summary>
        /// Unregisters the Patrol point
        /// </summary>
        public static void UnregisterPatrolPoint(EnemyPatrolPoint a_PatrolPoint)
        {
            if (s_Instance != null &&
                s_Instance.m_lstEnemyPatrolPoints.Contains(a_PatrolPoint))
            {
                s_Instance.m_lstEnemyPatrolPoints.Remove(a_PatrolPoint);
            }
        }

        /// <summary>
        /// Returns the next patrol point
        /// </summary>
        /// <param name="a_NonStaticEnemy"></param>
        /// <param name="a_CurrentPatrolPoint"></param>
        /// <returns></returns>
        public static EnemyPatrolPoint GetNextPatrolPoint(NonStaticEnemy a_NonStaticEnemy, EnemyPatrolPoint a_CurrentPatrolPoint)
        {
            s_Instance.m_lstCurrentValidPatrolPoints.Clear();
            EnemyPatrolPoint l_NextPatrolPoint = null;

            List<EnemyPatrolPoint> l_lstPatrolPointsInRange = a_NonStaticEnemy.getPatrolPointsWithinRange();

            UnityEngine.AI.NavMeshAgent l_NavMeshAgent = a_NonStaticEnemy.NavMeshAgent;

            int l_iPatrolPointCount = l_lstPatrolPointsInRange.Count;
            for (int l_iCurrentPatrolPoint = 0; l_iCurrentPatrolPoint < l_iPatrolPointCount; l_iCurrentPatrolPoint++)
            {
                EnemyPatrolPoint l_CurrentEnemyPatrolPoint = l_lstPatrolPointsInRange[l_iCurrentPatrolPoint];
                if (a_CurrentPatrolPoint != null && l_CurrentEnemyPatrolPoint == a_CurrentPatrolPoint)
                {
                    continue;
                }

                l_NavMeshAgent.CalculatePath(l_CurrentEnemyPatrolPoint.transform.position, s_Instance.m_NavMeshPath);
                
                if (s_Instance.m_NavMeshPath.status == UnityEngine.AI.NavMeshPathStatus.PathComplete)
                {
                    if (GetNavDistanceToTarget(s_Instance.m_NavMeshPath) < MIN_PATROL_POINT_DISTANCE)
                    {
                        s_Instance.m_lstCurrentValidPatrolPoints.Add(l_CurrentEnemyPatrolPoint);
                    }
                }
            }

            if (s_Instance.m_lstCurrentValidPatrolPoints.Count > 0)
            {
                l_NextPatrolPoint = s_Instance.m_lstCurrentValidPatrolPoints[Random.Range(0, s_Instance.m_lstCurrentValidPatrolPoints.Count)];
            }
            
            return l_NextPatrolPoint;
        }
    }
}
