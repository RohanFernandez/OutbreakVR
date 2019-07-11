using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class ItemDropBase : MonoBehaviour, IItemDrop
    {
        [SerializeField]
        private string m_strID = string.Empty;

        /// <summary>
        /// The gameobject that parents the title of the item
        /// </summary>
        [SerializeField]
        private GameObject m_goItemTitle = null;

        /// <summary>
        /// the unique type of item
        /// </summary>
        [SerializeField]
        private ITEM_TYPE m_ItemType;
        public ITEM_TYPE getItemType()
        {
            return m_ItemType;
        }

        public abstract ITEM_CATEGORY getItemCategoryType();

        /// <summary>
        /// The rotation speed in the Y axis
        /// </summary>
        protected const float ROTATION_SPEED = 20.0f;

        public virtual void Update()
        {
            transform.Rotate(Vector3.up, ROTATION_SPEED * Time.deltaTime);
            m_goItemTitle.transform.LookAt(ControllerManager.GetHeadsetAnchor().transform);
        }

        public virtual void onReturnedToPool()
        {

        }

        public virtual void onRetrievedFromPool()
        {

        }

        /// <summary>
        /// The unique ID to refer to
        /// </summary>
        /// <param name="a_strID"></param>
        public void setID(string a_strID)
        {
            m_strID = a_strID;
        }

        /// <summary>
        /// returns ID
        /// </summary>
        /// <returns></returns>
        public string getID()
        {
            return m_strID;
        }

        #region IPointerOver Interface Implemetation

        [SerializeField]
        MeshRenderer m_MeshRenderer = null;

        public virtual void onPointerEnter()
        {
            Debug.LogError("onPointerEnter");
            m_MeshRenderer.material.color = new Color(1.0f, 0.0f, 0.0f);
        }

        public virtual void onPointerExit()
        {
            Debug.LogError("onPointerExit");
            m_MeshRenderer.material.color = new Color(0.0f, 1.0f, 0.0f);
        }

        public virtual void onPointerInteract()
        {
            Debug.LogError("onPointerInteract");
            m_MeshRenderer.material.color = new Color(0.0f, 0.0f, 1.0f);

            EventHash l_EventHash = EventManager.GetEventHashtable();
            l_EventHash.Add(GameEventTypeConst.ID_ITEM_DROP_TYPE, m_ItemType);
            l_EventHash.Add(GameEventTypeConst.ID_ITEM_BASE, this);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_ITEM_PICKED_UP, l_EventHash);
        }
        #endregion IPointerOver Interface Implemetation
    }
}