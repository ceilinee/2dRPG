using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterPath
{
  public VectorPoints[] pathArray = new VectorPoints[]{};
  public string action;
  public string scene = null;
  public bool spawn;
  public int charId;
}
