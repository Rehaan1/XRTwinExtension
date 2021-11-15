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


    public void GoogleOAuth()
    {
        if (_pluginActivity != null)
        {
            _pluginActivity.Call("signIn");
            Debug.Log("Called Sign In");
        }
    }

    public void SignOut()
    {
        if (_pluginActivity != null)
        {
            _pluginActivity.Call("signOut");
            Debug.Log("Called Sign Out");
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

    public void IDToken(string result)
    {
        Debug.Log("ID Token GOT >>>>>>>>>>>> : " + result);
    }
}
