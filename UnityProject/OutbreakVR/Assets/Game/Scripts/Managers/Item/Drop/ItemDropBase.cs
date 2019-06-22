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
        }
        #endregion IPointerOver Interface Implemetation
    }
}