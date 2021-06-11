using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class VectorPointsList : ScriptableObject {
    public List<VectorPoints> vectorPoints;

    public VectorPoints GetDefaultPond() {
        return vectorPoints[0];
    }
}
