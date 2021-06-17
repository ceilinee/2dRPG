using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class BodyPart : ScriptableObject {
    [System.Serializable]
    public class BodyAnimation {
        public AnimationClip attackDown;
        public AnimationClip attackLeft;
        public AnimationClip attackRight;
        public AnimationClip attackUp;
        public AnimationClip holdDown;
        public AnimationClip holdDownIdle;
        public AnimationClip holdLeft;
        public AnimationClip holdLeftIdle;
        public AnimationClip holdRight;
        public AnimationClip holdRightIdle;
        public AnimationClip holdUp;
        public AnimationClip holdUpIdle;
        public AnimationClip idleDown;
        public AnimationClip idleLeft;
        public AnimationClip idleRight;
        public AnimationClip idleUp;
        public AnimationClip walkDown;
        public AnimationClip walkLeft;
        public AnimationClip walkRight;
        public AnimationClip walkUp;
    }
    public int id;
    public new string name;
    public Sprite bodyPartSprite;

    // The equivalent of bodyPartSprite when the player is holding an item
    // TODO: set this value on all relevant BodyPart instances
    public Sprite holdingSprite;
    public BodyAnimation bodyAnimation;
}
