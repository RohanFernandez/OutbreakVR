using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_NotificationPanel : AbsUISingleton
    {
        /// <summary>
        /// singleton instance
        /// </summary>
        private static UI_NotificationPanel s_Instance = null;

        /// <summary>
        /// The text component that displays the notification title
        /// </summary>
        [SerializeField]
        private TMPro.TMP_Text m_txtTitle = null;

        /// <summary>
        /// The text component that displays the notification message
        /// </summary>
        [SerializeField]
        private TMPro.TMP_Text m_txtMsg = null;

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

        /// <summary>
        /// Displays the notification UI with a title and a message
        /// </summary>
        /// <param name="a_strTitle"></param>
        /// <param name="a_strMsg"></param>
        public static void Show(string a_strTitle, string a_strMsg)
        {
            s_Instance.m_txtTitle.text = a_strTitle;
            s_Instance.m_txtMsg.text = a_strMsg;
            s_Instance.show();
        }

        public static void Hide()
        {
            if (s_Instance != null)
            {
                s_Instance.hide();
            }
        }
    }
}
