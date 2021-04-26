using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    public string name;
    public float moveSpeed;
    public Vector2 location;
    public string scene;
    public int id;
    public bool adoptAnimal;
    public int currentPoint;
    public Sprite image;
    public Sprite[] portrait;
    public string[] preferredAnimals;
    public string gender;
    public float multiplier = 1f;
    public string[] travelTimes;
    public CharacterPath[] path;
    public CharacterPath selectedPath;
    public int friendship;
    public int friendshipScore;
    public int currentAnimalId = 0;
    public int presentsDaily;
    public bool married;
    public bool date;
    public bool talked;
    [System.Serializable] public class DictionaryOfTimeAndLocation : SerializableDictionary<string, CharacterPath> {}
    public DictionaryOfTimeAndLocation characterMovement = new DictionaryOfTimeAndLocation();
    [System.Serializable]
    public class dialogueArray
    {
        public Dialogue[] array;
    }
    public dialogueArray[] characterSpeechArray;
    public dialogueArray[] characterGiftReceiveSpeechArray;
    [System.Serializable]
    public class ItemArray
    {
        public Item[] array;
    }
    public ItemArray[] giftArray;
    [System.Serializable] public class DictionaryOfItemAndInt : SerializableDictionary<Item, int> {}
    public DictionaryOfItemAndInt giftDictionary = new DictionaryOfItemAndInt();
}
