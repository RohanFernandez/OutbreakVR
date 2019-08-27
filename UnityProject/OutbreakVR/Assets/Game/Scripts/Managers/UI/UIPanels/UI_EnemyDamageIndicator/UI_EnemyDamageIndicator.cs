using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_EnemyDamageIndicator : MonoBehaviour, IReusable
    {
        /// <summary>
        /// The text that displays the damage inflicted on the enemy
        /// </summary>
        [SerializeField]
        private TMPro.TMP_Text m_txtDamage = null;

        /// <summary>
        /// The current time since shown the UI is displayed
        /// </summary>
        private float m_fCurrTimeBeingDisplayed = 0.0f;

        /// <summary>
        /// The max time the UI will be displayed for
        /// </summary>
        private const float MAX_TIME_TO_DISPLAY = 1.5f;

        /// <summary>
        /// The rect transform
        /// </summary>
        [SerializeField]
        private RectTransform m_RectTransform = null;
        public RectTransform RectTransform
        {
            get { return m_RectTransform; }
        }

        #region Interface IReusable
        public void onRetrievedFromPool()
        {
            
        }

        public void onReturnedToPool()
        {
            
        }

        #endregion Interface IReusable

        /// <summary>
        /// Displays the UI with the damage inflicted
        /// </summary>
        /// <param name="a_iDamage"></param>
        public void show(int a_iDamage)
        {
            m_txtDamage.text = a_iDamage.ToString();
            gameObject.SetActive(true);
            m_fCurrTimeBeingDisplayed = 0.0f;
        }

        /// <summary>
        /// Hides the UI with the damage inflicted
        /// </summary>
        /// <param name="a_iDamage"></param>
        public void hide()
        {
            gameObject.SetActive(false);
            EnemyDamageIndicatorManager.HideDamageIndicator(this);
        }

        /// <summary>
        /// Displays the UI panel for a certain amount of time and then hides the UI and returns it back into the pool
        /// </summary>
        private void Update()
        {
            m_fCurrTimeBeingDisplayed += Time.deltaTime;
            if (m_fCurrTimeBeingDisplayed > MAX_TIME_TO_DISPLAY)
            {
                hide();
            }
        }
    }
}