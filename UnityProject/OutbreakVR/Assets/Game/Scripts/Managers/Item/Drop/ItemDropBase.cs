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
        /// The gameobject that holds the model
        /// </summary>
        [SerializeField]
        private GameObject m_goItemModel = null;

        /// <summary>
        /// the unique type of item
        /// </summary>
        [SerializeField]
        private ITEM_TYPE m_ItemType;
        public ITEM_TYPE getItemType()
        {
            return m_ItemType;
        }

        /// <summary>
        /// The outline hightlighter
        /// </summary>
        [SerializeField]
        private OutlineHighlighterBase m_OutlineGroupHighlighterBase = null;

        /// <summary>
        /// The normal outline color
        /// </summary>
        [SerializeField]
        private Color m_colorOutlineNormal;

        /// <summary>
        /// The highlighted outline color
        /// </summary>
        [SerializeField]
        private Color m_colorOutlineHighlighted;

        /// <summary>
        /// Objective id to be triggered on item pickup
        /// </summary>
        [SerializeField]
        private string m_strObjectiveTriggerOnPickup = string.Empty;
        public string ObjectiveTriggerOnPickup
        {
            get { return m_strObjectiveTriggerOnPickup; }
            set { m_strObjectiveTriggerOnPickup = value; }
        }

        public abstract ITEM_CATEGORY getItemCategoryType();

        /// <summary>
        /// The rotation speed in the Y axis
        /// </summary>
        protected const float ROTATION_SPEED = 20.0f;

        public virtual void Update()
        {
            //m_goItemModel.transform.Rotate(m_goItemModel.transform.up, ROTATION_SPEED * Time.deltaTime);
            m_goItemTitle.transform.LookAt(ControllerManager.GetHeadsetAnchor().transform);
        }

        public virtual void onReturnedToPool()
        {
            if (m_OutlineGroupHighlighterBase != null)
            {
                m_OutlineGroupHighlighterBase.toggleHighlighter(false, m_colorOutlineNormal);
            }
        }

        public virtual void onRetrievedFromPool()
        {
            if (m_OutlineGroupHighlighterBase != null)
            {
                m_OutlineGroupHighlighterBase.toggleHighlighter(true, m_colorOutlineNormal);
            }
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

        public virtual void onPointerEnter()
        {
            if (m_OutlineGroupHighlighterBase != null)
            {
                m_OutlineGroupHighlighterBase.toggleHighlighter(true, m_colorOutlineHighlighted);
            }
        }

        public virtual void onPointerExit()
        {
            if (m_OutlineGroupHighlighterBase != null)
            {
                m_OutlineGroupHighlighterBase.toggleHighlighter(true, m_colorOutlineNormal);
            }
        }

        public virtual void onPointerInteract()
        {
            EventHash l_EventHash = EventManager.GetEventHashtable();
            l_EventHash.Add(GameEventTypeConst.ID_ITEM_DROP_TYPE, m_ItemType);
            l_EventHash.Add(GameEventTypeConst.ID_ITEM_BASE, this);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_ITEM_PICK_UP_ATTEMPTED, l_EventHash);
        }
        #endregion IPointerOver Interface Implemetation
    }
}