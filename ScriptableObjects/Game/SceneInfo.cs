using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
[System.Serializable]
public class SceneInfo : ScriptableObject
{
  public string sceneName;
  public int id;
  public int cost;
  public Sprite image;
  public int animalMaxSize;
  public int animalCurrentSize;
  public Vector2 entrance;
  public bool open;
}
