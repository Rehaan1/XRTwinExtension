using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRTwin.DataManager
{
    public class PluginInit : MonoBehaviour
    {
        [SerializeField] string pluginName;

        AndroidJavaClass unityClass;
        AndroidJavaObject unityActivity;
        AndroidJavaObject _pluginInstance;

        void Start()
        {
            InitializePlugin("com.xrdsc.xrtwin.PluginInstance");
        }


        void Update()
        {

        }

        void InitializePlugin(string pluginName)
        {
            unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            _pluginInstance = new AndroidJavaObject(pluginName);
            if(_pluginInstance == null)
            {
                Debug.Log("Plugin Instance Error");
            }
            _pluginInstance.CallStatic("receiveUnityActivity", unityActivity);
        }

        public void Add()
        {
            Debug.Log("Entered Add");
            if (_pluginInstance != null)
            {
                var result = _pluginInstance.Call<int>("Add", 5, 6);
                Debug.Log("Add: "+result);
            }
        }

        public void Toast()
        {
            Debug.Log("Entered Toast");
            if(_pluginInstance != null)
            {
                _pluginInstance.Call("Toast", "Hi! From Unity");
            }
        }
    }
}
