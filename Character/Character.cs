using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.XR;

[System.Serializable]
public class ItemArray {
    public Item[] array;
}
[System.Serializable]
public class dialogueArray {
    public ChoiceDialogue[] array;
    public ChoiceDialogue getDialogue() {
        List<ChoiceDialogue> selection = new List<ChoiceDialogue>();
        for (int i = 0; i < array.Length; i++) {
            if (array[i].oneTime && !array[i].displayed) {
                array[i].displayed = true;
                selection.Add(array[i]);
            } else {
                selection.Add(array[i]);
            }
        }
        ChoiceDialogue[] results = selection.ToArray();
        return results[UnityEngine.Random.Range(0, results.Length)];
    }
}
public enum characterMood {
    THRILLED,
    HAPPY,
    EXCITED,
    SAD,
    BLUE
}
[System.Serializable]
public class Character {
    public string name;
    public float moveSpeed;
    public Vector2 location;
    public string scene;
    public int id;
    public string occupation;
    public bool adoptAnimal;
    public Sprite image;
    public Sprite[] portrait;
    public Type[] preferredAnimals;
    public string gender;
    public float multiplier = 1f;
    public string[] travelTimes;
    public CharacterPath[] path;
    public CharacterPath selectedPath;
    public int friendshipScore = 200;
    public int currentAnimalId = 0;
    public int presentsDaily;
    public bool married;
    public bool date;
    public bool dateable;
    public bool talked;

    // Populated at runtime; maps from travelTimes to path
    [System.Serializable] public class DictionaryOfTimeAndLocation : SerializableDictionary<string, CharacterPath> { }
    public DictionaryOfTimeAndLocation characterMovement = new DictionaryOfTimeAndLocation();

    public dialogueArray[] characterSpeechArray;
    public dialogueArray[] characterGiftReceiveSpeechArray;
    public dialogueArray[] characterExSpeechArray;
    public dialogueArray[] characterExGiftReceiveSpeechArray;
    public ChoiceDialogue characterDivorceSpeech;
    public ChoiceDialogue characterBreakUpSpeech;
    public ChoiceDialogue characterMarriageSpeech;
    public ChoiceDialogue characterDatingSpeech;
    public ChoiceDialogue characterConfusionSpeech;
    public ChoiceDialogue characterRejectionSpeech;
    public ChoiceDialogue[] characterEnoughGiftSpeech;
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
    public List<int> friends;
    public characterMood mood = characterMood.HAPPY;
    public string GetMood() {
        return StringExtension.ToCamelCase(mood.ToString());
    }
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
    public string determineFriendship() {
        if (friendshipScore < 100) {
            return "Nemesis";
        } else if (friendshipScore < 200) {
            return "Enemy";
        } else if (friendshipScore == 200) {
            return "Stranger";
        } else if (friendshipScore < 300) {
            return "❤️";

        } else if (friendshipScore < 400) {
            return "❤️❤️";
        } else if (friendshipScore < 500) {
            return "❤️❤️❤️";
        } else if (friendshipScore < 600) {
            return "❤️❤️❤️❤️";
        } else if (friendshipScore < 700) {
            return "❤️❤️❤️❤️❤️";
        } else if (friendshipScore < 800) {
            return "❤️❤️❤️❤️❤️❤️";
        } else if (friendshipScore < 900) {
            return "❤️❤️❤️❤️❤️❤️❤️";
        } else if (friendshipScore < 1000) {
            return "❤️❤️❤️❤️❤️❤️❤️❤️";
        } else if (friendshipScore < 1100) {
            return "❤️❤️❤️❤️❤️❤❤️❤️❤️";
        } else {
            return "❤️❤️❤️❤️❤️❤️❤❤️❤️❤️";
        }
    }
    //determine color of friendship hearts in Player Menu
    public Color determineFriendshipColor() {
        if (date && !married) {
            return new Color(255 / 255f, 124 / 255f, 191 / 255f); ;
        }
        if (friendshipScore < 1200 && married) {
            return new Color(63 / 255f, 63 / 255f, 113 / 255f); ;
        }
        if (friendshipScore < 1300 && married) {
            return new Color(135 / 255f, 195 / 255f, 135 / 255f); ;
        }
        if (friendshipScore < 1400 && married) {
            return new Color(255 / 255f, 189 / 255f, 95 / 255f); ;
        }
        if (friendshipScore < 1500 && married) {
            return new Color(241 / 255f, 119 / 255f, 31 / 255f); ;
        }
        if (friendshipScore >= 1500 && married) {
            return new Color(195 / 255f, 67 / 255f, 67 / 255f); ;
        }
        return new Color(160 / 255f, 91 / 255f, 83 / 255f);
    }
    public List<String> GetFriendNames(Characters curCharacters) {
        List<String> result = new List<String>();
        foreach (int id in friends) {
            result.Add(curCharacters.characterDict[id].name);
        }
        return result;
    }
}
