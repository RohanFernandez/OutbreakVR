﻿using System.Collections;
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

        ///// <summary>
        ///// The patrol points currently valid which are withing distance
        ///// </summary>
        //private List<EnemyPatrolPoint> m_lstCurrentValidPatrolPoints = new List<EnemyPatrolPoint>(15);

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
        }

        /// <summary>
        /// sets singleton to null
        /// </summary>
        public void destroy()
        {
            if (s_Instance != this)
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
        public static EnemyPatrolPoint GetNextPatrolPoint(NonStaticEnemy a_NonStaticEnemy, EnemyPatrolPoint a_CurrentPatrolPoint, EnemyPatrolPoint a_LastPatrolPoint)
        {
            EnemyPatrolPoint l_NearestPatrolPoint = null;
            EnemyPatrolPoint l_NextPatrolPoint = null;
            float l_fNearestDistance = 10000.0f;

            int l_iPatrolPointsCount = s_Instance.m_lstEnemyPatrolPoints.Count;
            for (int l_iPatrolPointIndex = 0; l_iPatrolPointIndex < l_iPatrolPointsCount; l_iPatrolPointIndex++)
            {
                EnemyPatrolPoint l_PatrolPoint = s_Instance.m_lstEnemyPatrolPoints[l_iPatrolPointIndex];
                float l_PatrolPointNearestDistance = Vector3.Distance(l_PatrolPoint.transform.position, a_NonStaticEnemy.transform.position);

                if ((a_CurrentPatrolPoint != l_PatrolPoint) &&
                    (l_fNearestDistance > l_PatrolPointNearestDistance))
                {
                    l_fNearestDistance = l_PatrolPointNearestDistance;
                    if ((a_LastPatrolPoint != l_PatrolPoint))
                    {
                        l_NextPatrolPoint = l_PatrolPoint;
                    }
                    l_NearestPatrolPoint = l_PatrolPoint;
                }
            }

            //Only 1 patrol point exist then choose that
            if ((l_NextPatrolPoint == null) &&
                (l_iPatrolPointsCount == 1))
            {
                l_NextPatrolPoint = a_CurrentPatrolPoint;
            }
            //Choose the nearest point, if 2 patrol points exists where a_LastPatrolPoint is 1 and a_CurrentPatrolPoint is the other
            else if (l_NextPatrolPoint == null)
            {
                l_NextPatrolPoint = l_NearestPatrolPoint;
            }

            return l_NextPatrolPoint;
        }
    }
}
