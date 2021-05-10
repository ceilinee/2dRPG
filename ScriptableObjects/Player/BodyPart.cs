using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class BodyPart : ScriptableObject {
    [System.Serializable]
    public class BodyAnimation {
        public Animation idleDown;
        public Animation idleLeft;
        public Animation idleRight;
        public Animation idleUp;
    }
    public int id;
    public new string name;
    public Sprite bodyPartSprite;
    public BodyAnimation bodyAnimation;
}
