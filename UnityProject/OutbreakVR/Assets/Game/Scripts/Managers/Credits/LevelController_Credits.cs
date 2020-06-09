using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ns_Mashmo
{
    public class LevelController_Credits : MonoBehaviour
    {
        [SerializeField]
        private Scrollbar m_CreditsScrollbar = null;

        [SerializeField]
        private float m_fScrollSpeed = 0.08f;

        // Update is called once per frame
        void Update()
        {
            m_CreditsScrollbar.value += Time.deltaTime * m_fScrollSpeed;

            if (m_CreditsScrollbar.value >= 1.0f)
            {
                gameObject.SetActive(false);
                onScrollCompleted();
            }
        }

        /// <summary>
        /// On scrolling completed go back to home
        /// </summary>
        private void onScrollCompleted()
        {
            LevelManager.GoToLevel(GameConsts.STATE_NAME_HOME);
        }
    }
}