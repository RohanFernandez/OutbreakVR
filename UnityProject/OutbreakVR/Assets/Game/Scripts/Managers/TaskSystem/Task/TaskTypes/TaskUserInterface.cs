using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskUserInterface : TaskBase
    {
        #region ATTRIBUTE_KEY
        private const string ATTRIBUTE_COMMAND = "Command";
        private const string ATTRIBUTE_UI_ID = "UI";
        #endregion ATTRIBUTE_KEY

        #region ATTRIBUTE_VALUE
        private const string ATTRIBUTE_VALUE_SHOW = "Show";
        private const string ATTRIBUTE_VALUE_HIDE = "Hide";
        #endregion ATTRIBUTE_VALUE

        private string m_strUIName = string.Empty;
        private string m_strCommand = string.Empty;

        public override void onInitialize()
        {
            base.onInitialize();
            m_strUIName = getString(ATTRIBUTE_UI_ID);
            m_strCommand = getString(ATTRIBUTE_COMMAND);
        }

        public override void onExecute()
        {
            base.onExecute();
            AbsUIPanel.UI_TYPE l_UIType;
            if (System.Enum.TryParse(m_strUIName, out l_UIType))
            {
                switch (m_strCommand)
                {
                    case ATTRIBUTE_VALUE_SHOW:
                        {
                            UIManager.ToggleUI(l_UIType, true);
                            break;
                        }
                    case ATTRIBUTE_VALUE_HIDE:
                        {
                            UIManager.ToggleUI(l_UIType, false);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            onComplete();
        }
    }
}