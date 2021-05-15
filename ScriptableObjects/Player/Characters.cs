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
    public void Clear() {
        characterDict = new DictionaryOfCharacters();
    }
}
