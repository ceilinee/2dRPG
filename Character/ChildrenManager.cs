using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildrenManager : MonoBehaviour {
    public dialogueArrayArray exampleSpeechArrayDict;
    public itemArrayArray exampleItemArrayDict;
    public DialogueDictionary childrenPersonalitySpeechArrayDict;
    public DialogueDictionary childrenGiftReceiveSpeechArrayDict;
    public ItemArrayDictionary childrenItemArrayDict;
    public Personalities childrenPersonalities;
    public Player player;
    public ScheduleDictionary scheduleDictionary;
    public Characters curChar;

    // Start is called before the first frame update
    void Start() {
        // run code when personality is updated
        updateDict();

        // generateChild();
    }
    public void updateDict() {
        foreach (Personality pers in childrenPersonalities.personalityList) {
            childrenPersonalitySpeechArrayDict.personalityDialogueArrayArrayDict[pers.id] = exampleSpeechArrayDict;
            childrenGiftReceiveSpeechArrayDict.personalityDialogueArrayArrayDict[pers.id] = exampleSpeechArrayDict;
            childrenItemArrayDict.personalityItemArray[pers.id] = exampleItemArrayDict;
            scheduleDictionary.characterPathDict[pers.id] = new CharacterPathArray();
            scheduleDictionary.travelTimesDict[pers.id] = new StringArray();
        }
    }
    // Update is called once per frame
    void Update() {

    }

    public void generateChild(int date) {
        if (player.childrenCharId.Count > 4) {
            return;
        }
        // generate personality 
        Personality personality = childrenPersonalities.personalityList[Random.Range(0, childrenPersonalities.personalityList.Count)];

        // fetch speech, gift and item arrays
        dialogueArrayArray speechArray = childrenPersonalitySpeechArrayDict.personalityDialogueArrayArrayDict[personality.id];
        dialogueArrayArray giftReceiveSpeechArray = childrenGiftReceiveSpeechArrayDict.personalityDialogueArrayArrayDict[personality.id];
        itemArrayArray itemArrayArray = childrenItemArrayDict.personalityItemArray[personality.id];

        // generate new character
        Character newCharacter = new Character();

        // fetch travel times and travel paths
        CharacterPathArray charPath = scheduleDictionary.characterPathDict[personality.id];
        StringArray travel = scheduleDictionary.travelTimesDict[personality.id];
        // int birthday = date + 10 + Random.Range(0, 10);
        int birthday = date + 1;
        // save arrays to character, generate new id as 100 + # of children
        int id = 100 + player.childrenCharId.Count;
        newCharacter.generateChildCharacter(personality.id, speechArray.dialogue, giftReceiveSpeechArray.dialogue, itemArrayArray.array, travel.array, charPath.array, id, _birthday: birthday);

        //update curChar and player
        curChar.characterDict[id] = newCharacter;
        player.childrenCharId.Add(id);
    }
}
