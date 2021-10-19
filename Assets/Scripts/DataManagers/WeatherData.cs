using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using XRTwin.Weather;
using System;

namespace XRTwin.DataManager
{

    public class WeatherData : MonoBehaviour
    {
        WeatherDeck weatherdeck;

        readonly string locationUrl = "http://ip-api.com/json/";
        readonly string weatherUrl = "https://xrtwindata.herokuapp.com/weatherInfo/getInfo";

        void Start()
        {
           weatherdeck = FindObjectOfType<WeatherDeck>();
           StartCoroutine(GetLocation());  
        }

    
        void Update()
        {
        
        }

        IEnumerator GetLocation()
        {
            UnityWebRequest locationRequest = UnityWebRequest.Get(locationUrl);

            yield return locationRequest.SendWebRequest();

            if(locationRequest.isNetworkError || locationRequest.isHttpError)
            {
                Debug.LogError(locationRequest.error);
                yield break;
            }

            JSONNode locationInfo = JSON.Parse(locationRequest.downloadHandler.text);
        
            StartCoroutine(GetWeatherData(locationInfo));
        }

        IEnumerator GetWeatherData(JSONNode locationInfo)
        {
            WWWForm weatherForm = new WWWForm();
            string city = locationInfo["city"];
            string region = locationInfo["region"];
            string countryCode = locationInfo["countryCode"];

            weatherForm.AddField("city", city);
            weatherForm.AddField("region", region);
            weatherForm.AddField("countryCode", countryCode);
        
            UnityWebRequest weatherRequest = UnityWebRequest.Post(weatherUrl, weatherForm);
            weatherRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            yield return weatherRequest.SendWebRequest();

            if(weatherRequest.isNetworkError || weatherRequest.isHttpError)
            {
                Debug.LogError(weatherRequest.error);
                yield break;
            }

            JSONNode weatherInfo = JSON.Parse(weatherRequest.downloadHandler.text);

            string weather = weatherInfo["data"]["weather"][0]["main"].ToString();
            string tempC = weatherInfo["data"]["main"]["temp"].ToString();
            string temp = String.Format("{0:0.00}", tempC);
            string humidity = weatherInfo["data"]["main"]["humidity"].ToString();
            int clouds = (int) weatherInfo["data"]["clouds"]["all"];

            weatherdeck.onWeatherDataRecieved.Invoke(weather, temp, humidity, clouds);
        }
    }
}
