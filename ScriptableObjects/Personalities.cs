using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Personalities : ScriptableObject
{
  public List<Animal.Personality> personalityList = new List<Animal.Personality>();
}
