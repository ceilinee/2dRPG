using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Personalities : ScriptableObject
{
  public List<Animal.Personality> personalityList = new List<Animal.Personality>();
  
  public Animal.Personality getRandomPersonality(){
    List<Animal.Personality> tempList = new List<Animal.Personality>();
    foreach(Animal.Personality curPersonality in personalityList){
      for(int i =0; i<curPersonality.probability; i++){
        tempList.Add(curPersonality);
      }
    }
    Animal.Personality[] tempArray = tempList.ToArray();
    return tempArray[Random.Range(0, tempArray.Length)];
  }
}
