using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu]
[System.Serializable]
public class Characters : ScriptableObject
{
  [System.Serializable] public class DictionaryOfCharacters : SerializableDictionary<int, Character> {}
  public DictionaryOfCharacters characterDict = new DictionaryOfCharacters();

  public void updateCharacter(Character character){
    if(characterDict.ContainsKey(character.id)){
      characterDict[character.id] = character;
    }
  }
  public Character getRandomCharacter(){
    System.Random random = new System.Random();
    int index = random.Next(characterDict.Count);
    return characterDict.Values.ElementAt(index);
  }
  public void Clear(){
    characterDict = new DictionaryOfCharacters();
  }
}
