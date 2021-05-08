using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Personalities : ScriptableObject
{
  public List<Personality> personalityList = new List<Personality>();
  
  public Personality getRandomPersonality(){
    List<Personality> tempList = new List<Personality>();
    foreach(Personality curPersonality in personalityList){
      for(int i =0; i<curPersonality.probability; i++){
        tempList.Add(curPersonality);
      }
    }
    Personality[] tempArray = tempList.ToArray();
    return tempArray[Random.Range(0, tempArray.Length)];
  }
}
