using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskEnvironmentInteraction : TaskBase
    {
        #region ATTRIBUTE_KEY
        private const string ATTRIBUTE_GAMEOBJECT_ID = "GameObject_ID";
        private const string ATTRIBUTE_CODE = "Code";
        #endregion ATTRIBUTE_KEY

        #region ITEM_SPECIFIC

        //Door
        private const string ATTRIBUTE_IS_DOOR_LOCKED = "IsLocked";
        private const string ATTRIBUTE_VALUE_CODE_IS_DOOR = "Door";

        //Smashable
        private const string ATTRIBUTE_IS_SMASHABLE_SMASHED = "IsSmashed";
        private const string ATTRIBUTE_VALUE_CODE_IS_SMASHABLE = "Smashable";

        #endregion ITEM_SPECIFIC

        private string m_strGameObjectID = string.Empty;
        /// <summary>
        /// code of instructions
        /// </summary>
        private string m_strCode = string.Empty;

        public override void onInitialize()
        {
            base.onInitialize();
            m_strGameObjectID = getString(ATTRIBUTE_GAMEOBJECT_ID);
            m_strCode = getString(ATTRIBUTE_CODE);
        }

        public override void onExecute()
        {
            base.onExecute();

            switch (m_strCode)
            {
                case ATTRIBUTE_VALUE_CODE_IS_DOOR:
                    {
                        // Set door as locked/ unlocked
                        bool l_bIsDoorLocked = getBool(ATTRIBUTE_IS_DOOR_LOCKED);
                        GameObject l_GameObject = GameObjectManager.GetGameObjectById(m_strGameObjectID);
                        if (l_GameObject != null)
                        {
                            InteractiveDoor l_InteractiveDoor = l_GameObject.GetComponent<InteractiveDoor>();
                            if (l_InteractiveDoor != null)
                            {
                                l_InteractiveDoor.lockDoor(l_bIsDoorLocked);
                            }
                            else
                            {
                                EnvironmentInteractableObjectGroup l_EnvInteractiveObjGroup = l_GameObject.GetComponent<EnvironmentInteractableObjectGroup>();
                                if (l_EnvInteractiveObjGroup != null)
                                {
                                    List<AbsEnvironmentInteractableObject> lstEnvInteractableObjects = l_EnvInteractiveObjGroup.LstInteractableObjects;
                                    int l_iEnvInteractableObjCount = lstEnvInteractableObjects.Count;
                                    for (int l_iEnvInteractableObjIndex = 0; l_iEnvInteractableObjIndex < l_iEnvInteractableObjCount; l_iEnvInteractableObjIndex++)
                                    {
                                        AbsEnvironmentInteractableObject l_CurrentEnvInteractObj = lstEnvInteractableObjects[l_iEnvInteractableObjIndex];
                                        InteractiveDoor l_CurrentInteractiveDoor = l_CurrentEnvInteractObj.GetComponent<InteractiveDoor>();
                                        if (l_CurrentInteractiveDoor != null)
                                        {
                                            l_CurrentInteractiveDoor.lockDoor(l_bIsDoorLocked);
                                        }
                                    }
                                }
                            }
                        }

                        break;
                    }
                case ATTRIBUTE_VALUE_CODE_IS_SMASHABLE:
                    {
                        // Set smashable as smashed
                        bool l_bIsSmashed = getBool(ATTRIBUTE_IS_SMASHABLE_SMASHED);
                        GameObject l_GameObject = GameObjectManager.GetGameObjectById(m_strGameObjectID);
                        if (l_GameObject != null && l_bIsSmashed)
                        {
                            ISmashable l_InteractiveSmashable = l_GameObject.GetComponent<ISmashable>();
                            if (l_InteractiveSmashable != null)
                            {
                                l_InteractiveSmashable.smash();
                            }
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }

            }


            onComplete();
        }
    }
}