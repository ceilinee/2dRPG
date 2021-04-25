using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    // Start is called before the first frame update

    public Image charImage;
    public Text message;
    public Text charName;
    public Quest quest;
    public GameObject unread;
    public GameObject calendarInformation;
    public void updateDetails(Quest newQuest){
      quest = newQuest;
      message.text = newQuest.message;
      if(!newQuest.accepted){
        unread.SetActive(true);
      }
      else{
        unread.SetActive(false);
      }
    }
    public void selectMail(){
    }
}
