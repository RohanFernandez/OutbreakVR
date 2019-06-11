using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class ItemDropBase : MonoBehaviour, IItemDrop
    {
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
        /// The rotation speed in the Y axis
        /// </summary>
        [SerializeField]
        protected float m_fRotationSpeed = 20.0f;

        public virtual void Update()
        {
            transform.Rotate(Vector3.up, m_fRotationSpeed * Time.deltaTime);
        }

        public virtual void onReturnedToPool()
        {

        }

        public virtual void onRetrievedFromPool()
        {

        }
    }
}