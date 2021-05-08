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

    public Characters curChar;

    // Start is called before the first frame update
    void Start() {
        // run code when personality is updated
        // updateDict();

        generateChild();
    }
    public void updateDict() {
        foreach (Personality pers in childrenPersonalities.personalityList) {
            childrenPersonalitySpeechArrayDict.personalityDialogueArrayArrayDict[pers.id] = exampleSpeechArrayDict;
            childrenGiftReceiveSpeechArrayDict.personalityDialogueArrayArrayDict[pers.id] = exampleSpeechArrayDict;
            childrenItemArrayDict.personalityItemArray[pers.id] = exampleItemArrayDict;
        }
    }
    // Update is called once per frame
    void Update() {

    }

    public void generateChild() {
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
        // save arrays to character, generate new id as 100 + # of children
        newCharacter.generateChildCharacter(personality.id, speechArray.dialogue, giftReceiveSpeechArray.dialogue, itemArrayArray.array, 100 + player.childrenCharId.Count);
        // curChar.Add();
    }
}
