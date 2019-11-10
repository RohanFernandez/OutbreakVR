using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskSetTransform : TaskBase
    {
        #region ATTRIBUTE_KEY
        private const string ATTRIBUTE_POSITION = "Position";
        private const string ATTRIBUTE_ROTATION = "Rotation";
        private const string ATTRIBUTE_SCALE    = "Scale";
        private const string ATTRIBUTE_GAMEOBJECT_ID = "GameObject_ID";
        private const string ATTRIBUTE_PARENT_GAMEOBJECT_ID = "ParentGameObject_ID";
        private const string ATTRIBUTE_IS_LOCAL = "IsLocal";
        #endregion ATTRIBUTE_KEY

        private Vector3 m_v3Position = Vector3.zero;
        private Vector3 m_v3Rotation = Vector3.zero;
        private Vector3 m_v3Scale = Vector3.zero;
        private string m_strGameObjectID = string.Empty;
        private GameObject m_GameObject = null;
        private string m_strParentGameObjectID = string.Empty;
        private GameObject m_ParentGameObject = null;
        private bool m_bIsLocal = false;

        public override void onInitialize()
        {
            base.onInitialize();

            m_strGameObjectID = getString(ATTRIBUTE_GAMEOBJECT_ID);
            m_GameObject = GameObjectManager.GetGameObjectById(m_strGameObjectID);

            m_strParentGameObjectID = getString(ATTRIBUTE_PARENT_GAMEOBJECT_ID);
            m_ParentGameObject = GameObjectManager.GetGameObjectById(m_strParentGameObjectID);

            m_bIsLocal = getBool(ATTRIBUTE_IS_LOCAL);

            if (m_GameObject != null)
            {
                m_v3Position = getVec3(ATTRIBUTE_POSITION, m_GameObject.transform.position);
                m_v3Rotation = getVec3(ATTRIBUTE_ROTATION, m_GameObject.transform.rotation.eulerAngles);
                m_v3Scale = getVec3(ATTRIBUTE_SCALE, m_GameObject.transform.localScale);
            }
        }

        public override void onExecute()
        {
            base.onExecute();
            if (m_GameObject != null)
            {
                if (m_ParentGameObject != null)
                {
                    m_GameObject.transform.SetParent(m_ParentGameObject.transform);
                }

                CharacterController l_CharController = m_GameObject.GetComponent<CharacterController>();
                if (l_CharController != null)
                {
                    l_CharController.enabled = false;
                    setGameObjTransform();
                    l_CharController.enabled = true;
                }
                else
                {
                    setGameObjTransform();
                }
            }
            
            onComplete();
        }

        private void setGameObjTransform()
        {
            if (m_bIsLocal)
            {
                m_GameObject.transform.localPosition = m_v3Position;
                m_GameObject.transform.localRotation = Quaternion.Euler(m_v3Rotation);
            }
            else
            {
                m_GameObject.transform.SetPositionAndRotation(m_v3Position, Quaternion.Euler(m_v3Rotation));
            }
            m_GameObject.transform.localScale = m_v3Scale;
        }
    }
}