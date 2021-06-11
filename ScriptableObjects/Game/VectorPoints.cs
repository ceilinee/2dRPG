using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]

public class VectorPoints : ScriptableObject {
    [Header("An optional name describing the location of the vector point")]
    public string locationName;
    public Vector2 value;
}
