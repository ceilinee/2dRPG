using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemArray {
    public Item[] array;
}
[System.Serializable]
public class dialogueArray {
    public Dialogue[] array;
}

[System.Serializable]
public class Character {
    public string name;
    public float moveSpeed;
    public Vector2 location;
    public string scene;
    public int id;
    public bool adoptAnimal;
    public Sprite image;
    public Sprite[] portrait;
    public string[] preferredAnimals;
    public string gender;
    public float multiplier = 1f;
    public string[] travelTimes;
    public CharacterPath[] path;
    public CharacterPath selectedPath;
    public int friendshipScore;
    public int currentAnimalId = 0;
    public int presentsDaily;
    public bool married;
    public bool date;
    public bool talked;

    // Populated at runtime; maps from travelTimes to path
    [System.Serializable] public class DictionaryOfTimeAndLocation : SerializableDictionary<string, CharacterPath> { }
    public DictionaryOfTimeAndLocation characterMovement = new DictionaryOfTimeAndLocation();

    public dialogueArray[] characterSpeechArray;
    public dialogueArray[] characterGiftReceiveSpeechArray;
    public dialogueArray[] characterExSpeechArray;
    public dialogueArray[] characterExGiftReceiveSpeechArray;
    public Dialogue characterDivorceSpeech;
    public Dialogue characterBreakUpSpeech;
    public Dialogue characterMarriageSpeech;
    public Dialogue characterDatingSpeech;
    public Dialogue characterConfusionSpeech;
    public Dialogue characterRejectionSpeech;
    public Dialogue[] characterEnoughGiftSpeech;
    public ItemArray[] giftArray;
    [System.Serializable] public class DictionaryOfItemAndInt : SerializableDictionary<Item, int> { }
    public DictionaryOfItemAndInt giftDictionary = new DictionaryOfItemAndInt();
    //attributes for children
    public bool child;
    public bool unborn = true;
    public int birthday;
    public int age;
    public bool follow;
    public int personality;

    public void generateChildCharacter(int newPersonality,
    dialogueArray[] newCharacterSpeechArray,
    dialogueArray[] newCharacterGiftReceiveSpeechArray,
    ItemArray[] newGiftArray,
    string[] newTravelTimes,
    CharacterPath[] newPath,
    int newId, int _birthday) {
        personality = newPersonality;
        characterSpeechArray = newCharacterSpeechArray;
        characterGiftReceiveSpeechArray = newCharacterGiftReceiveSpeechArray;
        giftArray = newGiftArray;
        id = newId;
        child = true;
        age = 0;
        scene = "MainScene";
        travelTimes = newTravelTimes;
        path = newPath;
        birthday = _birthday;
        if (travelTimes != null) {
            for (int i = 0; i < travelTimes.Length; i++) {
                characterMovement[travelTimes[i]] = path[i];
            }
        }
    }
}
