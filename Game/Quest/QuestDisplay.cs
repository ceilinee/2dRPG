using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestDisplay : MonoBehaviour
{
    public Quest quest;
    public Player player;
    public Text questMessage;
    public Text questCharacter;
    public Text expirationDate;
    public Text reward;
    public Text firstCriteria;
    public Text firstCriteriaAnswer;
    public Image portrait;
    public GameObject acceptQuest;
    public GameObject submitQuest;
    public GameObject removeQuest;
    public Characters charList;
    public Quests availableQuests;
    public Quests playerQuests;
    public FloatValue playerMoney;
    public GameObject CanvasController;
    public GameObject QuestInformation;
    public CurTime curTime;
    public ItemDictionary itemDictionary;
    public SceneInfos sceneInfos;
    public GameObject completeObject;
    public Characters curCharacters;

    // Update is called once per frame
    public void clearCriteria(){
      firstCriteria.gameObject.SetActive(false);
      firstCriteriaAnswer.gameObject.SetActive(false);
      submitQuest.SetActive(false);
    }
    public void displayQuest(Quest newQuest){
      clearCriteria();
      acceptQuest.SetActive(true);
      quest = newQuest;
      questMessage.text = newQuest.message;
      questCharacter.text = charList.characterDict[newQuest.posterCharId].name;
      if(charList.characterDict[newQuest.posterCharId].portrait.Length > 0){
        portrait.sprite = charList.characterDict[newQuest.posterCharId].portrait[0];
      }
      expirationDate.text = curTime.daysToDateSeason(newQuest.expirationDate);
      reward.text = newQuest.reward.ToString() + "$, " + newQuest.reputationPoints.ToString() + " Reputation";
      displayCriteria(newQuest);
    }
    public void acceptQuestFunction(){
      quest.accepted = true;
      availableQuests.SetQuestAccepted(quest);
      QuestInformation.GetComponent<QuestInformation>().refresh();
    }
    public void setQuestCompletedFunction(){
      quest.completed = true;
      availableQuests.SetQuestCompleted(quest);
      // QuestInformation.GetComponent<QuestInformation>().refresh();
    }
    public void deleteQuestFunction(){
      availableQuests.deleteQuest(quest);
      QuestInformation.GetComponent<QuestInformation>().refreshDelete();
    }
    public void submitQuestFunction(){
      quest.redeemed = true;
      availableQuests.SetQuestRedeemed(quest);
      playerMoney.initialValue += quest.reward;
      player.reputation += quest.reputationPoints;
      availableQuests.deleteQuest(quest);
      QuestInformation.GetComponent<QuestInformation>().refresh();
    }
    void Update()
    {
      if(Input.GetButtonDown("Cancel")){
        gameObject.SetActive(false);
        CanvasController.GetComponent<CanvasController>().closeCanvas();
      }
    }
    public void displayCriteria(Quest newQuest){
      firstCriteria.gameObject.SetActive(true);
      firstCriteriaAnswer.gameObject.SetActive(true);
      switch(newQuest.type){
        case QuestType.adoption:
            firstCriteria.text = "# Adoption Complete:";
            firstCriteriaAnswer.text = player.dailyAdoption.ToString() + "/" + newQuest.adoptionCount;
            float percentage = player.dailyAdoption/newQuest.adoptionCount;
            if(percentage>=1.0 || newQuest.completed){
              if(!newQuest.completed){
                setQuestCompletedFunction();
              }
              firstCriteriaAnswer.color = Color.green;
            }
            else{
              firstCriteriaAnswer.color = Color.red;
            }
            break;
        case QuestType.talk:
            firstCriteria.text = "# People Talked To:";
            firstCriteriaAnswer.text = player.dailyTalk.ToString() + "/" + newQuest.talk;
            percentage = player.dailyTalk/newQuest.talk;
            if(percentage>=1.0 || newQuest.completed){
              if(!newQuest.completed){
                setQuestCompletedFunction();
              }
              firstCriteriaAnswer.color = Color.green;
            }
            else{
              firstCriteriaAnswer.color = Color.red;
            }
            break;
        case QuestType.walk:
            firstCriteria.text = "# Pets Walked:";
            firstCriteriaAnswer.text = player.dailyWalk.ToString() + "/" + newQuest.walk;
            percentage = player.dailyWalk/newQuest.walk;
            if(percentage>=1.0 || newQuest.completed){
              if(!newQuest.completed){
                setQuestCompletedFunction();
              }
              firstCriteriaAnswer.color = Color.green;
            }
            else{
              firstCriteriaAnswer.color = Color.red;
            }
            break;
        case QuestType.collectItem:
            firstCriteria.text = "Items left to collect:";
            if(newQuest.completed){
              firstCriteriaAnswer.color = Color.green;
              firstCriteriaAnswer.text = "None";
            }
            else{
              List<string> items = new List<string>();
              for(int i = 0 ; i< newQuest.collectQuestItemId.Length; i++){
                if(!player.dailyCollected.Contains(newQuest.collectQuestItemId[i])){
                  items.Add(itemDictionary.itemDict[newQuest.collectQuestItemId[i]].itemName);
                }
              }
              if(!newQuest.completed && items.Count > 0){
                firstCriteriaAnswer.text = string.Join(",", items.ToArray());
                firstCriteriaAnswer.color = Color.red;
              }
              else{
                if(!newQuest.completed){
                  setQuestCompletedFunction();
                }
                firstCriteriaAnswer.color = Color.green;
                firstCriteriaAnswer.text = "None";
              }
            }
            break;
        case QuestType.talkCharacter:
            firstCriteria.text = "Characters left to talk to:";
            if(newQuest.completed){
              firstCriteriaAnswer.color = Color.green;
              firstCriteriaAnswer.text = "None";
            }
            else{
              List<string> character = new List<string>();
              for(int i = 0 ; i< newQuest.talkQuestCharId.Length; i++){
                if(!player.dailyTalkedTo.Contains(newQuest.talkQuestCharId[i])){
                  character.Add(curCharacters.characterDict[newQuest.talkQuestCharId[i]].name);
                }
              }
              if(!newQuest.completed && character.Count > 0){
                firstCriteriaAnswer.text = string.Join(",", character.ToArray());
                firstCriteriaAnswer.color = Color.red;
              }
              else{
                if(!newQuest.completed){
                  setQuestCompletedFunction();
                }
                firstCriteriaAnswer.color = Color.green;
                firstCriteriaAnswer.text = "None";
              }
            }
            break;
        case QuestType.giftCharacter:
            firstCriteria.text = "Characters left to give gift to:";
            if(!newQuest.completed && !player.dailyGiftedTo.Contains(newQuest.giftQuestCharId)){
              firstCriteriaAnswer.text = curCharacters.characterDict[newQuest.giftQuestCharId].name;
              firstCriteriaAnswer.color = Color.red;
            }
            else{
              if(!newQuest.completed){
                setQuestCompletedFunction();
              }
              firstCriteriaAnswer.color = Color.green;
              firstCriteriaAnswer.text = "None";
            }
            break;
        case QuestType.giftAnimal:
            firstCriteria.text = "# Pets with Gifts:";
            firstCriteriaAnswer.text = player.dailyGiftAnimal.ToString() + "/" + newQuest.giftAnimalCount;
            percentage = player.dailyGiftAnimal/newQuest.giftAnimalCount;
            if(percentage>=1.0 || newQuest.completed){
              if(!newQuest.completed){
                setQuestCompletedFunction();
              }
              firstCriteriaAnswer.color = Color.green;
            }
            else{
              firstCriteriaAnswer.color = Color.red;
            }
            break;
        default:
            Debug.Log("scene");
            firstCriteria.text = "Scenes to Vist:";
            if(!newQuest.completed && !player.dailyScenesVisited.Contains(newQuest.sceneId)){
              firstCriteriaAnswer.text = sceneInfos.sceneDict[newQuest.sceneId].sceneName;
              firstCriteriaAnswer.color = Color.red;
            }
            else{
              if(!newQuest.completed){
                setQuestCompletedFunction();
              }
              firstCriteriaAnswer.text = "None";
            }
            break;
      }
      completeObject.SetActive(false);
      submitQuest.SetActive(false);
      removeQuest.SetActive(false);
      acceptQuest.SetActive(false);
      if(newQuest.redeemed){
        completeObject.SetActive(true);
        removeQuest.SetActive(true);
      }
      else if(newQuest.completed && newQuest.accepted && !newQuest.redeemed){
        submitQuest.SetActive(true);
        removeQuest.SetActive(true);
      }
      else if(!newQuest.accepted){
        acceptQuest.SetActive(true);
      }
      else{
        removeQuest.SetActive(true);
      }
    }
}
