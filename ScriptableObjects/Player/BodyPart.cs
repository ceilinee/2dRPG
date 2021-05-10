﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class BodyPart : ScriptableObject {
    [System.Serializable]
    public class BodyAnimation {
        public AnimationClip attackDown;
        public AnimationClip attackDownIdle;
        public AnimationClip attackLeft;
        public AnimationClip attackLeftIdle;
        public AnimationClip attackRight;
        public AnimationClip attackRightIdle;
        public AnimationClip attackUp;
        public AnimationClip attackUpIdle;
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
    public BodyAnimation bodyAnimation;
}
