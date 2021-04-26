using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarInformation : MonoBehaviour
{
    public Calendar calendar;
    public Events events;
    public CurTime curTime;
    public Text season;
    public Characters curCharacters;
    public GameObject dates;
    public Sprite birthdayIcon;
    public Color birthdayColor;
    public GameObject CanvasController;

    // Start is called before the first frame update
    // void Start()
    // {
    //   updateCalendar();
    // }
    public void Open(){
      gameObject.SetActive(true);
      updateCalendar();
    }
    // Update is called once per frame
    void Update()
    {
      if(Input.GetButtonDown("Cancel")){
        gameObject.SetActive(false);
        CanvasController.GetComponent<CanvasController>().closeCanvas();
      }
      if(gameObject.activeInHierarchy && Time.timeScale != 0){
        Time.timeScale = 0;
      }
    }
    public void updateCalendar(){
      season.text = curTime.getSeasonInWords();
      Transform trans = dates.transform;
      for(int i = 1; i< 21; i++){
        Transform child = trans.Find(i.ToString());
        CalendarItem curItem = child.gameObject.GetComponent<CalendarItem>();
        Date curDate = calendar.seasonDict[curTime.season].dates[i-1];
        if(curDate.eventId != -1){
          Event curEvent = events.eventDict[curDate.eventId];
          child.gameObject.GetComponent<Image>().color = curEvent.color;
          curItem.events.text = curEvent.eventName;
          curItem.icon.sprite = curEvent.eventIcon;
        }
        else{
          curItem.icon.gameObject.SetActive(false);
        }
        if(curDate.birthdayCharId != -1){
          if(curDate.eventId != -1){
            curItem.events.text += ", " + curCharacters.characterDict[curDate.birthdayCharId].name + "'s birthday!";
            curItem.birthdayIcon.gameObject.SetActive(true);
          }
          else{
            curItem.events.text = curCharacters.characterDict[curDate.birthdayCharId].name + "'s birthday!";
            child.gameObject.GetComponent<Image>().color = birthdayColor;
            curItem.icon.gameObject.SetActive(true);
            curItem.icon.sprite = birthdayIcon;
            curItem.birthdayIcon.gameObject.SetActive(false);
          }
        }
        else{
          curItem.birthdayIcon.gameObject.SetActive(false);
        }
      }
    }
}
