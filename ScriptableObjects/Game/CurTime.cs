using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class CurTime : ScriptableObject
{
  public float time;
  public int days;
  public int season = 0;
  public int date = 0;
  public Event curEvent;
  public Date curDate;
  public int birthdayCharId;
  public int years = 0;
  public WeatherState weather;
  public void UpdateTime(float curTime){
     time = curTime;
  }
  public float Hours {
      get {return time / 3600f;}
  }
  public void getYear () {
      years = (int)System.Math.Floor((double)days / 60f);
  }
  public void getSeason () {
      season = (int)System.Math.Floor((double)(days % 60)/20f);
  }
  public void getDate () {
      date = ((days % 60) % 20) + 1;
  }
  public string getSeasonInWords(){
    if(season == 0){
      return "Spring";
    }
    if(season == 1){
      return "Summer";
    }
    if(season == 2){
      return "Fall";
    }
    else{
      return "Winter";
    }
  }
  public float Minutes {
      get {return (time % 3600f / 60f);}
  }
  public void Clear(){
    time = 0f;
    days = 0;
    years = 0;
  }
  public bool isCurrentTimeBigger(string time){
    string[] split = time.Split(':');
    int hour = Convert.ToInt32(split[0]);
    int min = Convert.ToInt32(split[1]);
    if(hour < (int)Hours){
      return true;
    }
    else if(hour == (int)Hours){
      if(min < (int)Minutes){
        return true;
      }
      else{
        return false;
      }
    }
    else{
      return false;
    }
    //is hh and mm bigger than time
  }
  public bool isCurrentTimeSmaller(string time){
    string[] split = time.Split(':');
    int hour = Convert.ToInt32(split[0]);
    int min = Convert.ToInt32(split[1]);
    if(hour > (int)Hours){
      return true;
    }
    else if(hour == (int)Hours){
      if(min >= (int)Minutes){
        return true;
      }
      else{
        return false;
      }
    }
    else{
      return false;
    }
    //is hh and mm smaller than time
  }
  public void UpdateDays(int curDays){
    days = curDays;
    getYear();
    getSeason();
    getDate();
  }
  public float getTime{
    get {return time;}
  }
}
