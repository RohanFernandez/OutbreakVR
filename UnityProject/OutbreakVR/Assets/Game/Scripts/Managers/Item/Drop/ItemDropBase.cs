﻿using System.Collections;
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
        /// The start size of the item pool. That many objects will be created on start.
        /// </summary>
        [SerializeField]
        private int m_iStartPoolCreatedSize = 2;
        public int StartPoolCreatedSize
        {
            get { return m_iStartPoolCreatedSize; }
        }

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

        private float m_fDefaultMinViewableDistance = 10.0f;

        private float m_fCurrentViewableDistance = 5.0f;

        public float MinViewableDistance
        {
            get { return m_fCurrentViewableDistance; }
            set
            {
                m_fCurrentViewableDistance = (value < 0.5f) ? m_fDefaultMinViewableDistance : value;
            }
        }

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

        [SerializeField]
        private Collider m_Collider = null;

        public virtual void Update()
        {
            if (Vector3.Distance(PlayerManager.GetPosition(), transform.position) < MinViewableDistance)
            {
                if ((m_goItemTitle != null))
                {
                    m_goItemTitle.SetActive(true);
                    m_goItemTitle.transform.LookAt(ControllerManager.GetHeadsetAnchor().transform);
                }
                m_goItemModel.SetActive(true);
            }
            else
            {
                if ((m_goItemTitle != null))
                {
                    m_goItemTitle.SetActive(false);
                }
                m_goItemModel.SetActive(false);
            }


            //m_goItemModel.transform.Rotate(m_goItemModel.transform.up, ROTATION_SPEED * Time.deltaTime);
            
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
                m_OutlineGroupHighlighterBase.toggleHighlighter(false, m_colorOutlineNormal);
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
                m_OutlineGroupHighlighterBase.toggleHighlighter(false, m_colorOutlineNormal);
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

        /// <summary>
        /// Activates deactivates the collider
        /// </summary>
        /// <param name="a_bIsActive"></param>
        public virtual void toggleInteractive(bool a_bIsActive)
        {
            if (a_bIsActive == IsColliderActive()){ return; }

            m_Collider.enabled = a_bIsActive;
            //if (m_OutlineGroupHighlighterBase != null)
            //{
            //    m_OutlineGroupHighlighterBase.toggleHighlighter(a_bIsActive, m_colorOutlineNormal);
            //}

            if (m_goItemTitle != null)
            {
                m_goItemTitle.SetActive(a_bIsActive);
            }
        }

        public bool IsColliderActive()
        {
            return (m_Collider != null) ? m_Collider.enabled : false;
        }
    }
}