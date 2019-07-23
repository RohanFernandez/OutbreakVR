using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EnemyPatrolPoint : MonoBehaviour
    {
        private void OnEnable()
        {
            EnemyManager.RegisterPatrolPoint(this);
        }

        private void OnDisable()
        {
            EnemyManager.UnregisterPatrolPoint(this);
        }

        private void OnDrawGizmoSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 5.0f);
        }
    }
}