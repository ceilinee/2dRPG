using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeatherState{
  rain,
  sun,
  snow,
  cloudy
}
public class WeatherController : MonoBehaviour
{
    public GameObject rain;
    public GameObject snow;
    public GameObject sun;
    public CurTime curTime;
    // Start is called before the first frame update
    void Start()
    {
      updateWeather();
    }
    public void updateWeather(){
      if(curTime.weather == WeatherState.rain){
        startRain();
      }
      else if(curTime.weather == WeatherState.sun){
        startSun();
      }
      else if(curTime.weather == WeatherState.snow){
        startSnow();
      }
      else{
        startCloud();
      }
    }
    public void startRain(){
      rain.SetActive(true);
      snow.SetActive(false);
      sun.SetActive(false);
    }
    public void startSun(){
      rain.SetActive(false);
      snow.SetActive(false);
      sun.SetActive(true);
    }
    public void startSnow(){
      rain.SetActive(false);
      snow.SetActive(true);
      sun.SetActive(false);
    }
    public void startCloud(){
      rain.SetActive(false);
      snow.SetActive(false);
      sun.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
