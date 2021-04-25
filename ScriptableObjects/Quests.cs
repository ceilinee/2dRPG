using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType {
  adoption,
  talk,
  walk,
  collectItem,
  talkCharacter,
  giftCharacter,
  giftAnimal,
  scene
}

[System.Serializable]
public class Quest {
  public int id;
  public string name;
  public int posterCharId;
  public QuestType type;
  public int[] talkQuestCharId;
  public int giftQuestCharId;
  public int giftQuestItemId;
  public int[] collectQuestItemId;
  public int adoptionCount;
  public int giftAnimalCount;
  public int[] animalId;
  public int walk;
  public int talk;
  public int expirationDate;
  public int sceneId;
  public string message;
  public float reward;
  public int multiplier;
  public float experiencePoints;
  public bool accepted = false;
}

[CreateAssetMenu]
[System.Serializable]
public class Quests : ScriptableObject
{
    public Quest[] curQuests;

    public void addQuest(Quest quest){
      List<Quest> temp = new List<Quest>(curQuests);
      temp.Insert(0, quest);
      curQuests = temp.ToArray();
    }
    public void deleteQuest(Quest quest){
      List<Quest> temp = new List<Quest>();
      for(int i =0; i<curQuests.Length; i++){
        if(curQuests[i].id != quest.id){
          temp.Add(curQuests[i]);
        }
      }
      curQuests = temp.ToArray();
    }
    public void clearQuests(){
      curQuests = new Quest[0];
    }
}
