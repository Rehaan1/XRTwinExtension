using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

namespace XRTwin.Weather
{
    [System.Serializable]
    public class WeatherDataRecievedEvent : UnityEvent<string, string, string, int>
    {
    }

    public class WeatherDeck : MonoBehaviour
    {
        [SerializeField] TextMeshPro tempText;
        [SerializeField] TextMeshPro weatherText;
        [SerializeField] TextMeshPro humidityText;
        [SerializeField] GameObject sun;
        [SerializeField] GameObject rain;
        [SerializeField] GameObject snow;
        [SerializeField] GameObject clouds;
        [SerializeField] GameObject haze;

        public WeatherDataRecievedEvent onWeatherDataRecieved;

        void Start()
        {
            tempText.text = "....";
            weatherText.text = "....";
            humidityText.text = "....";

            onWeatherDataRecieved.AddListener(UpdateWeatherDeck);

            sun.SetActive(false);
            rain.SetActive(false);
            snow.SetActive(false);
            clouds.SetActive(false);
            haze.SetActive(false);
        }

        
        void Update()
        {

        }

        void UpdateWeatherDeck(string weather, string temp, string humidity, int cloud)
        {
            float tempC = float.Parse(temp) - 273.15f;
            string tempC1 = String.Format("{0:0.00}", tempC);
            string weatherC= weather.Substring(1, weather.Length - 2); 
            tempText.text = tempC1 + " 'C";
            weatherText.text = weatherC;
            humidityText.text = humidity;

            if(cloud > 50)
            {
                clouds.SetActive(true);
            }

            if (weatherC.Equals("Haze") || weatherC.Equals("Mist"))
            {
                sun.SetActive(true);
                haze.SetActive(true);
            }

            if (weatherC.Equals("Rain") || weatherC.Equals("Thunderstorm"))
            {
                rain.SetActive(true);
            }

            if (weatherC.Equals("Snow"))
            {
                snow.SetActive(true);
            }

            if (weatherC.Equals("Sunny"))
            {
                sun.SetActive(true);
            }
        }
    }
}