using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarController : Interactable
{
    public GameObject alert;
    public GameObject calendarInformation;
    public GameObject questInformation;
    public GameObject CanvasController;
    public GameObject selection;
    private Button calendar;
    private Button quest;
    private bool subscribed;
    // Start is called before the first frame update
    void Start()
    {
      // if(mailbox.unread > 0){
      //   alert.SetActive(true);
      // }
      // else{
      //   alert.SetActive(false);
      // }
    }
    public void checkAlert(){
      // if(mailbox.unread > 0){
      //   alert.SetActive(true);
      // }
      // else{
      //   alert.SetActive(false);
      // }
    }
    // Update is called once per frame
    public void subscribe(){
      calendar = selection.transform.Find("Talk").gameObject.GetComponent<Button>();
      selection.transform.Find("Talk").Find("ConfirmText").gameObject.GetComponent<Text>().text = "Calendar";
      calendar.onClick.AddListener(openCalendar);
      quest = selection.transform.Find("Shop").gameObject.GetComponent<Button>();
      selection.transform.Find("Shop").Find("ConfirmText").gameObject.GetComponent<Text>().text = "Quest";
      quest.onClick.AddListener(openQuest);
      subscribed = true;
    }
    public void closeSelection(){
      CanvasController.GetComponent<CanvasController>().closeAllCanvas();
      calendar.onClick.RemoveListener(openCalendar);
      quest.onClick.RemoveListener(closeSelection);
      subscribed = false;
    }
    public void openQuest(){
      closeSelection();
      if(!CanvasController.activeInHierarchy){
        CanvasController.SetActive(true);
      }
      if(CanvasController.GetComponent<CanvasController>().openCanvas()){
        questInformation.GetComponent<QuestInformation>().Open();
      }
    }
    public void openCalendar(){
      closeSelection();
      if(!CanvasController.activeInHierarchy){
        CanvasController.SetActive(true);
      }
      if(CanvasController.GetComponent<CanvasController>().openCanvas()){
        calendarInformation.GetComponent<CalendarInformation>().Open();
      }
    }
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.Space) && playerInRange){
        if(!CanvasController.activeInHierarchy){
          CanvasController.SetActive(true);
        }
        if(CanvasController.GetComponent<CanvasController>().openCanvas()){
          if(!subscribed){
            subscribe();
          }
          selection.SetActive(true);
        }
      }
    }
}
