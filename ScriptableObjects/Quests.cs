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
  public int[] giftQuestCharId;
  public int[] giftQuestItemId;
  public int[] collectQuestItemId;
  public int adoptionCount;
  public int giftAnimalCount;
  public int[] animalId;
  public int walk;
  public int talk;
  public string message;
  public float reward;
  public int multiplier;
  public float experiencePoints;
}

[CreateAssetMenu]
[System.Serializable]
public class Quests : ScriptableObject
{
    public Quest[] quests;

    public void addQuest(Quest quest){
      List<Quest> temp = new List<Quest>(quests);
      temp.Insert(0, quest);
      quests = temp.ToArray();
    }
    public void deleteQuest(Quest quest){
      List<Quest> temp = new List<Quest>();
      for(int i =0; i<quests.Length; i++){
        if(quests[i].id != quest.id){
          temp.Add(quests[i]);
        }
      }
      quests = temp.ToArray();
    }
    public void clearQuests(){
      quests = new Quest[0];
    }
}
