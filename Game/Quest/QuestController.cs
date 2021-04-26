using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestController : MonoBehaviour
{
    public Characters curCharacters;
    public Player player;
    public AdoptionRequests adoption;
    public Animals curAnimals;
    public Quests availableQuests;
    public Quest newQuest;
    public Quests playerQuest;
    public SceneInfos sceneInfos;
    public GameObject SpawnObject;
    // Start is called before the first frame update
    void Start()
    {
      for(int i =0 ;i< 20; i++){
        generateQuest(10);
      }
    }
    public void increaseDate(int date, int repeat){
      Debug.Log("increase Days");
      clearQuest(date);
      for(int i =0; i<repeat; i++){
        generateQuest(date);
      }
    }
    public void clearQuest(int date){
      for(int i = 0; i<playerQuest.curQuests.Length; i++){
        if(playerQuest.curQuests[i].expirationDate < date){
          playerQuest.deleteQuest(playerQuest.curQuests[i]);
        }
      }
      for(int i = 0; i<availableQuests.curQuests.Length; i++){
        if(availableQuests.curQuests[i].expirationDate < date){
          availableQuests.deleteQuest(availableQuests.curQuests[i]);
        }
      }
    }
    public void generateQuest(int date){
      newQuest = new Quest();
      newQuest.expirationDate = date;
      newQuest.id = Random.Range(0, 100000000);
      newQuest.type = (QuestType)Random.Range(0, 8);
      System.Random random = new System.Random();
      switch(newQuest.type){
        case QuestType.adoption:
            Debug.Log("adoption");
            newQuest.adoptionCount = Random.Range(1,3);
            newQuest.reward = 100 * newQuest.adoptionCount;
            newQuest.reputationPoints = 10 * newQuest.adoptionCount;
            newQuest.posterCharId = 1;
            newQuest.message = "The adoption waitlist seems a bit long, can you try to do " + newQuest.adoptionCount + " adoptions today?";
            break;
        case QuestType.talk:
            Debug.Log("talk");
            newQuest.talk = Random.Range(5,10);
            newQuest.reward = 10 * newQuest.talk;
            newQuest.reputationPoints = 1 * newQuest.talk;
            newQuest.posterCharId = 0;
            newQuest.message = "Hey! Today seems like a perfect day for some mingling! Try to talk to " + newQuest.talk + " Town folks today!";
            break;
        case QuestType.walk:
            Debug.Log("walk");
            newQuest.walk = Random.Range(1, (int)System.Math.Min(10, curAnimals.animalDict.Count));
            newQuest.reward = 10 * newQuest.walk;
            newQuest.reputationPoints = 1 * newQuest.walk;
            newQuest.posterCharId = 1;
            newQuest.message = "Passed by your farm yesterday and some of your animals seemed a bit bored, want to walk " + newQuest.walk + " today?";
            break;
        case QuestType.collectItem:
            List<Item> items = SpawnObject.GetComponent<SpawnObject>().items;
            List<int> selectedItems = new List<int>();
            int count = Random.Range(1,4);
            newQuest.reward = 50 * count;
            newQuest.reputationPoints = 5 * count;
            int index = random.Next(items.Count);
            string itemString = "";
            for(int i = 0; i < count; i++){
              index = random.Next(items.Count);
              selectedItems.Add(items[index].id);
              itemString += items[index].itemName;
              if(i < count-1){
                itemString += ", ";
              }
            }
            newQuest.collectQuestItemId = selectedItems.Distinct().ToArray();
            newQuest.posterCharId = 1;
            newQuest.message = "Hey! I've seen some " + itemString + " laying on the ground, you should go find these today!";
            break;
        case QuestType.talkCharacter:
            List<int> selectedCharacters = new List<int>();
            count = Random.Range(1,6);
            string charString = "";
            for(int i = 0; i < count; i++){
              index = random.Next(curCharacters.characterDict.Count);
              Character selectedChar = curCharacters.characterDict.Values.ElementAt(index);
              selectedCharacters.Add(selectedChar.id);
              charString += selectedChar.name;
              if(i < count-1){
                charString += ", ";
              }
            }
            newQuest.reward = 10 * count;
            newQuest.reputationPoints = 1 * count;
            newQuest.talkQuestCharId = selectedCharacters.Distinct().ToArray();
            newQuest.posterCharId = 0;
            newQuest.message = "Hey! I saw " + charString + " lounging around, you should go say hi to them today!";
            break;
        case QuestType.giftCharacter:
            items = SpawnObject.GetComponent<SpawnObject>().items;
            index = random.Next(curCharacters.characterDict.Count);
            Character selected = curCharacters.characterDict.Values.ElementAt(index);
            newQuest.giftQuestCharId = selected.id;
            index = random.Next(items.Count);
            newQuest.reward = 10 + index;
            newQuest.reputationPoints = 1;
            newQuest.giftQuestItemId = items[index].id;
            newQuest.posterCharId = 0;
            newQuest.message = "Hey! I saw " + selected.name + " mopping around, you should give them a present!";
            Debug.Log("giftCharacter");
            break;
        case QuestType.giftAnimal:
            newQuest.giftAnimalCount = Random.Range(1, (int)System.Math.Min(10, curAnimals.animalDict.Count));
            newQuest.reward = 10 * newQuest.giftAnimalCount;
            newQuest.reputationPoints = 1 * newQuest.giftAnimalCount;
            newQuest.posterCharId = 1;
            newQuest.message = "Hey! You should give " + newQuest.giftAnimalCount + " animals presents today!";
            Debug.Log("giftAnimal");
            break;
        case QuestType.scene:
            Debug.Log("scene");
            index = random.Next(sceneInfos.sceneDict.Count);
            newQuest.reward = 10;
            newQuest.reputationPoints = 1;
            SceneInfo selectedSceneInfo = sceneInfos.sceneDict.Values.ElementAt(index);
            newQuest.sceneId = selectedSceneInfo.id;
            newQuest.message = "Hey! You should check out " + selectedSceneInfo.name + " today!";
            newQuest.posterCharId = 0;
            break;
        default:
            Debug.Log("scene");
            index = random.Next(sceneInfos.sceneDict.Count);
            selectedSceneInfo = sceneInfos.sceneDict.Values.ElementAt(index);
            newQuest.sceneId = selectedSceneInfo.id;
            newQuest.message = "Hey! You should check out " + selectedSceneInfo.name + " today!";
            newQuest.posterCharId = 0;
            break;
      }
      availableQuests.addQuest(newQuest);
    }
}
