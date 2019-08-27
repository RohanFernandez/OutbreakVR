using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EnemyDamageIndicatorManager : MonoBehaviour, IComponentHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static EnemyDamageIndicatorManager s_Instance = null;

        /// <summary>
        /// The pool that will hold the Damage UI to be reused
        /// </summary>
        private EnemyDamageUIPool m_EnemyDamageUIPool = null;

        /// <summary>
        /// The UI prefab to use as the UI
        /// </summary>
        [SerializeField]
        private UI_EnemyDamageIndicator m_UIEnemyDamageIndicator = null;

        /// <summary>
        /// The rect transform of the screen space canvas
        /// </summary>
        [SerializeField]
        private RectTransform m_ScreenSpaceCanvasRectTransform = null;

        /// <summary>
        /// Sets singleton instance 
        /// </summary>
        public void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;

            m_EnemyDamageUIPool = new EnemyDamageUIPool(m_UIEnemyDamageIndicator, gameObject);
        }

        /// <summary>
        /// Destorys singleton instance
        /// </summary>
        public void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }
            s_Instance = null; ;
        }

        /// <summary>
        /// Displays the damage UI
        /// </summary>
        /// <param name="a_v3Position"></param>
        public static void ShowDamageIndicator(Vector3 a_v3Position, int a_iDamage)
        {
            UI_EnemyDamageIndicator l_UIDamageIndicator = s_Instance.m_EnemyDamageUIPool.getObject();
            l_UIDamageIndicator.transform.position = a_v3Position;
            l_UIDamageIndicator.transform.LookAt(PlayerManager.GetPosition());
            l_UIDamageIndicator.show(a_iDamage);
        }

        /// <summary>
        /// Hides and returns the UI_EnemyDamageIndicator back into the pool
        /// </summary>
        /// <param name="a_UIEnemyDamageIndicator"></param>
        public static void HideDamageIndicator(UI_EnemyDamageIndicator a_UIEnemyDamageIndicator)
        {
            s_Instance.m_EnemyDamageUIPool.returnToPool(a_UIEnemyDamageIndicator);
        }
    }
}