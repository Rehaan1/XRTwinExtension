using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PluginActivity : MonoBehaviour
{
    [SerializeField] int mainScene = 1;
    [SerializeField] int loginScene = 0;
    AndroidJavaObject _pluginActivity;
    string idToken;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        idToken = PlayerPrefs.GetString("idToken", "Null");
        if(!String.Equals(idToken, "Null"))
        {
            SceneManager.LoadScene(mainScene);
        }
    }

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

        PlayerPrefs.SetString("idToken", "Null");
        SceneManager.LoadScene(loginScene);
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
        PlayerPrefs.SetString("idToken", result);
        SceneManager.LoadScene(mainScene);
    }
}
