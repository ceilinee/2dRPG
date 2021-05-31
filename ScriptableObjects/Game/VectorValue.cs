using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VectorValue : ScriptableObject {
    public Vector2 initialValue;
    public Vector2 defaultValue;
    public void updateInitialValue(Vector2 location) {
        initialValue = location;
    }
    public void Clear() {
        initialValue = defaultValue;
    }
}
