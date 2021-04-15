using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdoptionRequest {
  public int charId;
  public string type;
  public Animal.StringAndAnimalColor coloring;
  public int price;
}
[CreateAssetMenu]
[System.Serializable]
public class AdoptionRequests : ScriptableObject
{
  public AdoptionRequest[] requests;
}
