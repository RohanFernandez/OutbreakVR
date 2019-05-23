using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class JobAudio : JobBase
    {
        /// <summary>
        /// Audio Id of the clip to play
        /// </summary>
        [SerializeField]
        public string m_strAudioId = string.Empty;

        public JobAudio(Hashtable a_Hashtable)
            : base(a_Hashtable)
        {
            m_strAudioId = a_Hashtable["ClipID"].ToString();
        }

        public override void onExecute()
        {
            Debug.LogError("AudioID : "+ m_strAudioId);
            onComplete();
        }
    }
}