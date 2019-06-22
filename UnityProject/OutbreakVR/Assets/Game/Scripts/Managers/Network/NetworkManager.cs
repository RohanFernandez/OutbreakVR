using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class NetworkManager : AbsComponentHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static NetworkManager s_Instance = null;

        /// <summary>
        /// sets singleton instance
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
        /// destroys singleton instance
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
        /// Check for internet connection.
        /// If successful calls action with true or false as an arg
        /// </summary>
        /// <param name="a_actionIsInternetConnected"></param>
        public static void CheckForNetworkConnection(System.Action<bool> a_actionIsInternetConnected)
        {
            s_Instance.StartCoroutine(s_Instance.checkForServerConnection(a_actionIsInternetConnected));
        }

        /// <summary>
        /// Checks if system is connected to the internet.
        /// </summary>
        /// <param name="a_bIsContinuousUntilSuccess"></param>
        /// <param name="a_actionIsInternetConnected"></param>
        /// <returns></returns>
        private IEnumerator checkForServerConnection(System.Action<bool> a_actionIsInternetConnected)
        {
            yield break;

            //List<UnityEngine.Networking.IMultipartFormSection> l_formData = new List<UnityEngine.Networking.IMultipartFormSection>();
            //l_formData.Add(new UnityEngine.Networking.MultipartFormDataSection(DB_KEY_GAME_ID, VALUE_GAME_ID));

            //UnityEngine.Networking.UnityWebRequest l_WebRequest = UnityEngine.Networking.UnityWebRequest.Post(LINK_SERVER_CONNECTION_CHECK, l_formData);
            //yield return l_WebRequest.SendWebRequest();

            //string l_strRetrievedText = l_WebRequest.downloadHandler.text;
            //string[] l_arrEchoesResult = l_strRetrievedText.Split(DIVIDER_MSG);

            //bool l_bIsConnectionSuccessful = !(l_WebRequest.isNetworkError || l_WebRequest.isHttpError)
            //    && (l_arrEchoesResult.Length >= 2) && l_arrEchoesResult[0].Equals(ECHO_MSG_SERVER_CONNECTION_SUCCESSFUL);

            //string l_strConnectionResultDebugMessage = l_bIsConnectionSuccessful ? "ServerConnectionHandler::CheckForInternetConnection:: Connection successful."
            //    : "ServerConnectionHandler::CheckForInternetConnection:: Connection unsuccessful with error msg : '" + l_WebRequest.error + "'";
            //Debug.Log(l_strConnectionResultDebugMessage);

            //// stop server connection check if started elsewhere
            //if (s_Instance.serverConnectionCheck != null)
            //{
            //    s_Instance.StopCoroutine(s_Instance.serverConnectionCheck);
            //}

            //if (a_bIsContinuousUntilSuccess && !l_bIsConnectionSuccessful)
            //{
            //    s_Instance.serverConnectionCheck = s_Instance.StartCoroutine(s_Instance.scheduledServerConnectionCheck(a_bIsContinuousUntilSuccess, a_actionIsInternetConnected));
            //}

            //if (OnInternetConnectionCheck != null)
            //{
            //    OnInternetConnectionCheck(l_bIsConnectionSuccessful);
            //}

            //if (a_actionIsInternetConnected != null)
            //{
            //    a_actionIsInternetConnected(l_bIsConnectionSuccessful);
            //}
            //Debug.Log("CheckForServerConnection::CheckForServerConnection::Connection check detected :" + l_bIsConnectionSuccessful);
        }
    }
}