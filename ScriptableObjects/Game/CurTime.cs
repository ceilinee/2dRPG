using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Timestamp {
    public float time;
    public int days;
}

[CreateAssetMenu]
public class CurTime : ScriptableObject {
    // Amount of time that has passed in seconds, in the current day
    public float time;
    // Amount of days that have passed since start of game
    // TODO: overflow?
    public int days;
    public int season = 0;
    public int date = 0;
    public Event curEvent;
    public Date curDate;
    public int birthdayCharId;
    public int years = 0;
    public WeatherState weather;
    public void UpdateTime(float curTime) {
        time = curTime;
    }
    public float Hours {
        get { return time / 3600f; }
    }
    public void getYear() {
        years = (int) System.Math.Floor((double) days / 60f);
    }
    public void getSeason() {
        season = (int) System.Math.Floor((double) (days % 60) / 20f);
    }
    public int getSeasonVar(int curDateInt) {
        return (int) System.Math.Floor((double) (curDateInt % 60) / 20f);
    }
    public void getDate() {
        date = ((days % 60) % 20);
    }
    public int getDateVar(int curDateInt) {
        return ((curDateInt % 60) % 20);
    }
    public string daysToDateSeason(int curDateInt) {
        return getSeasonInWordsVar(getSeasonVar(curDateInt)) + ", " + getDateVar(curDateInt);
    }
    public string getSeasonInWordsVar(int curSeason) {
        if (curSeason == 0) {
            return "Spring";
        }
        if (curSeason == 1) {
            return "Summer";
        }
        if (curSeason == 2) {
            return "Fall";
        } else {
            return "Winter";
        }
    }
    public string getSeasonInWords() {
        if (season == 0) {
            return "Spring";
        }
        if (season == 1) {
            return "Summer";
        }
        if (season == 2) {
            return "Fall";
        } else {
            return "Winter";
        }
    }
    public float Minutes {
        get { return (time % 3600f / 60f); }
    }
    public void Clear() {
        time = 0f;
        days = 0;
        years = 0;
        birthdayCharId = -1;
        getSeason();
        getDate();
        curEvent = new Event();
        curDate = new Date();

    }
    public bool isCurrentTimeBigger(string time) {
        string[] split = time.Split(':');
        int hour = Convert.ToInt32(split[0]);
        int min = Convert.ToInt32(split[1]);
        if (hour < (int) Hours) {
            return true;
        } else if (hour == (int) Hours) {
            if (min < (int) Minutes) {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
        //is hh and mm bigger than time
    }
    public bool isCurrentTimeSmaller(string time) {
        string[] split = time.Split(':');
        int hour = Convert.ToInt32(split[0]);
        int min = Convert.ToInt32(split[1]);
        if (hour > (int) Hours) {
            return true;
        } else if (hour == (int) Hours) {
            if (min >= (int) Minutes) {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
        //is hh and mm smaller than time
    }
    public void UpdateDays(int curDays) {
        days = curDays;
        getYear();
        getSeason();
        getDate();
    }
    public float getTime {
        get { return time; }
    }

    // Get a timestamp equal to the current time + `days` days later
    public Timestamp DaysFromNow(int days) {
        var t = new Timestamp();
        t.time = time;
        t.days = this.days + days;
        return t;
    }

    // Determine if the current time is strictly later than t
    public bool isCurrentTimeBigger(Timestamp t) {
        return days >= t.days && time > t.time;
    }
}
