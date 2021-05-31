using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
[System.Serializable]
public class Forest : ScriptableObject {
    public int level;
    public int width;
    public int height;
    public Vector2 entrance;
    public Vector2 exit;
    public bool maze = true;
}
