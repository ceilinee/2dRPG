using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu]
[System.Serializable]
public class Characters : ScriptableObject {
    [System.Serializable] public class DictionaryOfCharacters : SerializableDictionary<int, Character> { }
    public DictionaryOfCharacters characterDict = new DictionaryOfCharacters();

    public void updateCharacter(Character character) {
        if (characterDict.ContainsKey(character.id)) {
            characterDict[character.id] = character;
        }
    }
    public void ClearDailies() {
        foreach (KeyValuePair<int, Character> kvp in characterDict) {
            kvp.Value.talked = false;
            kvp.Value.presentsDaily = 0;
        }
    }
    public Character getRandomCharacter() {
        System.Random random = new System.Random();
        int index = random.Next(characterDict.Count);
        return characterDict.Values.ElementAt(index);
    }
    public Character childBirthEvent(int date) {
        foreach (KeyValuePair<int, Character> kvp in characterDict) {
            if (kvp.Value.birthday == date && kvp.Value.unborn) {
                return kvp.Value;
            }
        }
        return null;
    }
    public bool ContainsUnbornChild() {
        foreach (KeyValuePair<int, Character> kvp in characterDict) {
            if (kvp.Value.unborn) {
                return true;
            }
        }
        return false;
    }
    public void Clear() {
        characterDict = new DictionaryOfCharacters();
    }
}
