using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EnemyPatrolPoint : MonoBehaviour
    {
        private void OnEnable()
        {
            PatrolManager.RegisterPatrolPoint(this);
        }

        private void OnDisable()
        {
            PatrolManager.UnregisterPatrolPoint(this);
        }
    }
}