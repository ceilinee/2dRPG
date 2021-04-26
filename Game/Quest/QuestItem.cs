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
    public GameObject isNew;
    public GameObject complete;
    public GameObject questInformation;
    public void updateDetails(Quest newQuest){
      quest = newQuest;
      message.text = newQuest.message;
      if(!newQuest.accepted){
        isNew.SetActive(true);
        complete.SetActive(false);
      }
      else{
        isNew.SetActive(false);
        complete.SetActive(false);
      }
      if(newQuest.completed){
        complete.SetActive(true);
      }
    }
    public void selectQuest(){
      questInformation.GetComponent<QuestInformation>().displayQuest(quest);
    }
}
