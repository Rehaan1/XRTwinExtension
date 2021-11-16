using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRTwin.DataManager
{
    public class SignOutManager : MonoBehaviour
    {
        public void SignOut()
        {
            FindObjectOfType<PluginActivity>().SignOut();
        }
    }
}
