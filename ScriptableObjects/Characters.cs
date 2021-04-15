using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
  public void Clear(){
    characterDict = new DictionaryOfCharacters();
  }
}
