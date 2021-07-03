using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;

public class QuestController : MonoBehaviour {
    public Characters curCharacters;
    public Player player;
    public AdoptionRequests adoption;
    public Animals curAnimals;
    public Quests availableQuests;
    public Quest newQuest;
    public Quests playerQuest;
    public SceneInfos sceneInfos;
    public GameObject SpawnObject;

    // Character `posterCharId` says `message` for quest `questType`
    [System.Serializable]
    public struct CharMsg {
        public QuestType questType;
        public int posterCharId;
        public string message;
    }
    // Used for getting input from the inspector
    public CharMsg[] charMsgs;

    // Maps from a quest type to a list of relevant characters and messages
    public Dictionary<QuestType, List<CharMsg>> questToCharMsgs = new Dictionary<QuestType, List<CharMsg>>();

    // Start is called before the first frame update
    void Start() {
        // Convert user input to internal data structures
        foreach (CharMsg charMsg in charMsgs) {
            if (!questToCharMsgs.ContainsKey(charMsg.questType)) {
                questToCharMsgs[charMsg.questType] = new List<CharMsg>();
            }
            Assert.IsTrue(!questToCharMsgs[charMsg.questType].Exists(
                e => e.posterCharId == charMsg.posterCharId),
                "Duplicate character Id under the same quest type");
            questToCharMsgs[charMsg.questType].Add(charMsg);
        }
        for (int i = 0; i < 10; i++) {
            generateQuest(10);
        }
    }

    public void increaseDate(int date, int repeat) {
        clearQuest(date);
        for (int i = 0; i < repeat; i++) {
            generateQuest(date);
        }
    }

    // Randomly picks a (character / message) for a quest type
    private CharMsg GetQuestCharAndMsg(Quest quest) {
        Assert.IsTrue(
            questToCharMsgs.ContainsKey(quest.type),
            "No character/message info for quest type: " + quest.type);
        List<CharMsg> charMsgs = questToCharMsgs[quest.type];
        int idx = Random.Range(0, charMsgs.Count);
        return charMsgs[idx];
    }

    public void clearQuest(int date) {
        for (int i = 0; i < playerQuest.curQuests.Length; i++) {
            if (playerQuest.curQuests[i].expirationDate < date) {
                playerQuest.deleteQuest(playerQuest.curQuests[i]);
            }
        }
        for (int i = 0; i < availableQuests.curQuests.Length; i++) {
            if (availableQuests.curQuests[i].expirationDate < date) {
                availableQuests.deleteQuest(availableQuests.curQuests[i]);
            }
        }
    }

    public void generateQuest(int date) {
        newQuest = new Quest();
        newQuest.expirationDate = date;
        newQuest.id = Random.Range(0, 100000000);
        var questTypes = System.Enum.GetValues(typeof(QuestType));
        newQuest.type = (QuestType) Random.Range(0, questTypes.Length - 1);
        System.Random random = new System.Random();
        CharMsg charMsg = GetQuestCharAndMsg(newQuest);
        newQuest.posterCharId = charMsg.posterCharId;
        switch (newQuest.type) {
            case QuestType.adoption:
                newQuest.adoptionCount = Random.Range(1, 3);
                newQuest.reward = 50 * newQuest.adoptionCount;
                newQuest.reputationPoints = 5 * newQuest.adoptionCount;
                newQuest.message = string.Format(charMsg.message, newQuest.adoptionCount);
                break;
            case QuestType.talk:
                newQuest.talk = Random.Range(5, 10);
                newQuest.reward = 10 * newQuest.talk;
                newQuest.reputationPoints = 1 * newQuest.talk;
                newQuest.message = string.Format(charMsg.message, newQuest.talk);
                break;
            case QuestType.walk:
                newQuest.walk = Random.Range(1, (int) System.Math.Min(10, curAnimals.animalDict.Count));
                newQuest.reward = 10 * newQuest.walk;
                newQuest.reputationPoints = 1 * newQuest.walk;
                newQuest.message = string.Format(charMsg.message, newQuest.walk);
                break;
            case QuestType.collectItem:
                List<Item> items = SpawnObject.GetComponent<SpawnObject>().items;
                List<string> selectedItems = new List<string>();
                int count = Random.Range(1, 4);
                newQuest.reward = 20 * count;
                newQuest.reputationPoints = 2 * count;
                int index = random.Next(items.Count);
                string itemString = "";
                for (int i = 0; i < count; i++) {
                    index = random.Next(items.Count);
                    selectedItems.Add(items[index].Id);
                    itemString += items[index].itemName;
                    if (i < count - 1) {
                        itemString += ", ";
                    }
                }
                newQuest.collectQuestItemId = selectedItems.Distinct().ToArray();
                newQuest.message = string.Format(charMsg.message, itemString);
                break;
            case QuestType.talkCharacter:
                List<int> selectedCharacters = new List<int>();
                count = Random.Range(1, 6);
                string charString = "";
                for (int i = 0; i < count; i++) {
                    index = random.Next(curCharacters.characterDict.Count);
                    Character selectedChar = curCharacters.characterDict.Values.ElementAt(index);
                    selectedCharacters.Add(selectedChar.id);
                    charString += selectedChar.name;
                    if (i < count - 1) {
                        charString += ", ";
                    }
                }
                newQuest.reward = 10 * count;
                newQuest.reputationPoints = 1 * count;
                newQuest.talkQuestCharId = selectedCharacters.Distinct().ToArray();
                newQuest.message = string.Format(charMsg.message, charString);
                break;
            case QuestType.giftCharacter:
                items = SpawnObject.GetComponent<SpawnObject>().items;
                index = random.Next(curCharacters.characterDict.Count);
                Character selected = curCharacters.characterDict.Values.ElementAt(index);
                newQuest.giftQuestCharId = selected.id;
                index = random.Next(items.Count);
                newQuest.reward = 10 + index;
                newQuest.reputationPoints = 1;
                newQuest.giftQuestItemId = items[index].Id;
                newQuest.message = string.Format(charMsg.message, selected.name);
                break;
            case QuestType.giftAnimal:
                newQuest.giftAnimalCount = Random.Range(1, (int) System.Math.Min(10, curAnimals.animalDict.Count));
                newQuest.reward = 10 * newQuest.giftAnimalCount;
                newQuest.reputationPoints = 1 * newQuest.giftAnimalCount;
                newQuest.message = string.Format(charMsg.message, newQuest.giftAnimalCount);
                break;
            case QuestType.scene:
                index = random.Next(sceneInfos.sceneDict.Count);
                newQuest.reward = 10;
                newQuest.reputationPoints = 1;
                SceneInfo selectedSceneInfo = sceneInfos.sceneDict.Values.ElementAt(index);
                newQuest.sceneId = selectedSceneInfo.id;
                newQuest.message = string.Format(charMsg.message, selectedSceneInfo.name);
                break;
            default:
                Assert.IsTrue(false, "Code should never be executed");
                break;
        }
        availableQuests.addQuest(newQuest);
    }
}
