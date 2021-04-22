using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Player : ScriptableObject
{
  public string playerName;
  public bool female;
  public bool married;
  public float experience;
  public int dailyAdoption = 0;
  public int dailyTalk = 0;
  public int dailyWalk = 0;
  public int dailyGiftCharacter = 0;
  public int dailyGiftAnimal = 0;
  public float earnedMoney;
  public float earnedExperience;
}
