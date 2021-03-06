using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu]
[System.Serializable]
public class AnimalMood : ScriptableObject {
    [System.Serializable]
    public class MoodArray {
        public string[] array;
        public Sprite[] giftReactions;
    }
    [System.Serializable] public class DictionaryOfStringAndSprite : SerializableDictionary<string, Sprite> { }
    public DictionaryOfStringAndSprite reactions;
    public Sprite[] giftReactions;
    public string[] reactionIds;
    [System.Serializable] public class DictionaryOfMoodArray : SerializableDictionary<Type, MoodArray> { }
    [System.Serializable] public class DictionaryOfPersonalityAndAnimal : SerializableDictionary<int, DictionaryOfMoodArray> { }
    public DictionaryOfPersonalityAndAnimal personalityMoodDict = new DictionaryOfPersonalityAndAnimal();

    public string GetSadId() {
        Assert.IsTrue(reactions.ContainsKey("sad"));
        return "sad";
    }
}
