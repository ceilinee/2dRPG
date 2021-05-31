using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerGearSocket : MonoBehaviour {
    public string bodyPart;
    public Animator MyAnimator { get; set; }
    private Animator parentAnimator;
    private SpriteRenderer spriteRenderer;
    private AnimatorOverrideController animatorOverrideController;

    public Player player;
    public BodyPartManager bodyPartManager;

    [Header("All the supported colors")]
    public AnimalColors colors;


    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentAnimator = GetComponentInParent<Animator>();

        MyAnimator = GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(MyAnimator.runtimeAnimatorController);
        MyAnimator.runtimeAnimatorController = animatorOverrideController;
    }
    // Start is called before the first frame update
    void Start() {
        string[] animationNames = { "attackDown", "attackDownIdle", "attackLeft", "attackLeftIdle",
            "attackRight", "attackRightIdle", "attackUp", "attackUpIdle", "idleDown", "idleLeft", "idleRight", "idleUp",
            "walkDown", "walkLeft", "walkRight", "walkUp" };

        var appearance = player.appearance;

        BodyPart.BodyAnimation a = null;
        if (bodyPart == "hair") {
            spriteRenderer.color = colors.colorDictionary[appearance.hairColorId].color;
            BodyPart hairStyle = bodyPartManager.hairStyles[appearance.hairId];
            a = hairStyle.bodyAnimation;
        } else if (bodyPart == "outfit") {
            BodyPart outfit = bodyPartManager.outfits[appearance.outfitId];
            a = outfit.bodyAnimation;
        } else if (bodyPart == "eyes") {
            spriteRenderer.color = colors.colorDictionary[appearance.eyeColorId].color;
            BodyPart eyes = bodyPartManager.eyes[appearance.eyesId];
            a = eyes.bodyAnimation;
        } else Assert.IsTrue(false);

        foreach (string animationName in animationNames) {
            animatorOverrideController[animationName] = (AnimationClip) a.GetType().GetField(animationName).GetValue(a);
        }
    }

    public void SetXAndY(float x, float y) {
        MyAnimator.SetFloat("moveX", x);
        MyAnimator.SetFloat("moveY", y);
    }

    public void SetMoving(bool follow) {
        MyAnimator.SetBool("moving", follow);
    }
}
