﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateController : CustomMonoBehaviour {
    public CurTime curtime;
    public Calendar calendar;
    public Events events;
    public GameObject CharacterManager;
    public GameObject WeatherController;
    public List<GameObject> spring;
    public List<GameObject> summer;
    public List<GameObject> fall;
    public List<GameObject> winter;

    private Astar astar;

    // Start is called before the first frame update
    void Start() {
        //called to initialize calendar
        // for(int i =0; i<calendar.season.Length; i++){
        //   calendar.dateArray.dates = calendar.dates;
        //   calendar.seasonDict[calendar.season[i]] = calendar.dateArray;
        //   Debug.Log(calendar.seasonDict[calendar.season[i]]);
        // }
        //called to initialize Events
        // for(int i =0; i<events.events.Length; i++){
        //   events.eventDict[events.events[i].eventId] = events.events[i];
        // }
        astar = centralController.Get("A*").GetComponent<Astar>();
        UpdateSeason();
    }

    // Update is called once per frame
    public void UpdateDays() {
        curtime.curDate = calendar.seasonDict[curtime.season].dates[curtime.date];
        if (events.eventDict.ContainsKey(calendar.seasonDict[curtime.season].dates[curtime.date].eventId)) {
            curtime.curEvent = events.eventDict[calendar.seasonDict[curtime.season].dates[curtime.date].eventId];
        } else {
            curtime.curEvent = new Event();
        }
        curtime.birthdayCharId = calendar.seasonDict[curtime.season].dates[curtime.date].birthdayCharId;
        if (curtime.date == 0) {
            UpdateSeason();
            CharacterManager.GetComponent<CharacterManager>().ageChildren();
        }
        if (WeatherController) {
            updateWeather();
        }
    }
    public void updateWeather() {
        WeatherState[] weatherArray = new WeatherState[]{
        WeatherState.rain, WeatherState.snow, WeatherState.cloudy, WeatherState.sun
      };
        if (curtime.season == 1 || curtime.season == 2) {
            weatherArray = new WeatherState[]{
          WeatherState.rain, WeatherState.cloudy, WeatherState.sun
        };
        }
        if (curtime.season == 3) {
            weatherArray = new WeatherState[]{
          WeatherState.snow, WeatherState.cloudy, WeatherState.sun
        };
        }
        curtime.weather = weatherArray[Random.Range(0, weatherArray.Length)];
        WeatherController.GetComponent<WeatherController>().updateWeather();
    }
    public void UpdateSeason() {
        if (curtime.season == 0) {
            foreach (GameObject ob in spring) {
                ob.SetActive(true);
            }
            foreach (GameObject ob in summer) {
                ob.SetActive(false);
            }
            foreach (GameObject ob in fall) {
                ob.SetActive(false);
            }
            foreach (GameObject ob in winter) {
                ob.SetActive(false);
            }
        } else if (curtime.season == 1) {
            foreach (GameObject ob in spring) {
                ob.SetActive(false);
            }
            foreach (GameObject ob in summer) {
                ob.SetActive(true);
            }
            foreach (GameObject ob in fall) {
                ob.SetActive(false);
            }
            foreach (GameObject ob in winter) {
                ob.SetActive(false);
            }
        } else if (curtime.season == 2) {
            foreach (GameObject ob in spring) {
                ob.SetActive(false);
            }
            foreach (GameObject ob in summer) {
                ob.SetActive(false);
            }
            foreach (GameObject ob in fall) {
                ob.SetActive(true);
            }
            foreach (GameObject ob in winter) {
                ob.SetActive(false);
            }
        } else {
            foreach (GameObject ob in spring) {
                ob.SetActive(false);
            }
            foreach (GameObject ob in summer) {
                ob.SetActive(false);
            }
            foreach (GameObject ob in fall) {
                ob.SetActive(false);
            }
            foreach (GameObject ob in winter) {
                ob.SetActive(true);
            }
        }
        astar.RescanAstarGraph();
    }
}
