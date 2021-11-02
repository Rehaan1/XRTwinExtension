using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PluginActivity : MonoBehaviour
{
    AndroidJavaObject _pluginActivity;

    void Start()
    {
        _pluginActivity = new AndroidJavaObject("com.xrdsc.xrtwinactivity.PluginActivity");
    }

    
    void Update()
    {
        
    }

    public void Add()
    {
        if(_pluginActivity != null)
        {
            var result = _pluginActivity.Call<int>("Add", 7, 8);
            Debug.Log("result: " + result);
        }
    }

    public void ShowToast()
    {
        if(_pluginActivity != null)
        {
            _pluginActivity.Call("ShowToast", "Called From Unity!");
        }
    }

    public void GetInput(InputField inputField)
    {
        if(_pluginActivity != null)
        {
            if(inputField.text.Length > 0)
            {
                _pluginActivity.Call("ReceiveInput", int.Parse(inputField.text));
            }
        }
    }

    public void ResultTrue(string result)
    {
        Debug.Log("Called from Unity Plugin: " + result);
    }
}
