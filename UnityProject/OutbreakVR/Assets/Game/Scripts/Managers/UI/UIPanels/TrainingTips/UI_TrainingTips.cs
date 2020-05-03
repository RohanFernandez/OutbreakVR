using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_TrainingTips : AbsUISingleton
    {
        /// <summary>
        /// singleton instance
        /// </summary>
        private static UI_TrainingTips s_Instance = null;

        /// <summary>
        /// The transform of the camera
        /// </summary>
        [SerializeField]
        private Transform m_CamTransform = null;

        /// <summary>
        /// initializes, sets singleton to this
        /// </summary>
        public override void initialize()
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
        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }
            s_Instance = null;
        }

        private void Update()
        {
            transform.localRotation = Quaternion.Euler(0.0f, m_CamTransform.rotation.eulerAngles.y, 0.0f);
        }

        public static void Show()
        {
            s_Instance.show();
        }

        public static void Hide()
        {
            s_Instance.hide();
        }
    }
}