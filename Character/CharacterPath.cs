using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterPath {
    public VectorPoints[] pathArray = new VectorPoints[] { };

    [Header("The starting location of the character")]
    public VectorPoints src;

    [Header("The final destination the character should move to")]
    public VectorPoints dest;
    public string action;
    public string scene = null;
    public bool spawn;
    public int charId;
}
