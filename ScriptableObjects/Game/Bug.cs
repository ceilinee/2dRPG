using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
[System.Serializable]
public class Bug : Item {
    public AnimationClip animation = null;
    public int moveSpeed;
    public int suspectMax = 20;
}
