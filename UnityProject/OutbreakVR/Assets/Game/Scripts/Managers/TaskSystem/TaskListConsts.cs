using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskListConsts
    {
        public const string XML_NODE_TASK_LIST = "TaskList";
        public const string XML_KEYWORD_ID = "ID";
        public const string XML_KEYWORD_TYPE = "TYPE";

        /// <summary>
        /// Logs an error if while searching for a keyword in the XML returned null
        /// </summary>
        /// <param name="a_strXMLPath"></param>
        /// <param name="a_strKeyword"></param>
        /// <param name="a_bIsXMLNodeMissing"></param>
        public static void LogXMLSearchError(string a_strXMLPath, string a_strKeyword, bool a_bIsXMLNodeMissing)
        {
            Debug.LogError("XML search failed for keyword " + a_strKeyword + " when attempting to find " + (a_bIsXMLNodeMissing ? "NODE" : "ELEMENT") + " at path " + a_strXMLPath);
        }
    }
}