using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]

public class Square : ScriptableObject {
    public VectorPoints start;
    public VectorPoints end;
    public Type[] types;
    public int[] probability;
}
