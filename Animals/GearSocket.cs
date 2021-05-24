using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSocket : MonoBehaviour {
    public AnimationClip[] animations;
    public Animator MyAnimator { get; set; }
    private Animator parentAnimator;
    public string type;
    private SpriteRenderer spriteRenderer;
    private AnimatorOverrideController animatorOverrideController;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentAnimator = GetComponentInParent<Animator>();

        MyAnimator = GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(MyAnimator.runtimeAnimatorController);
        MyAnimator.runtimeAnimatorController = animatorOverrideController;
    }
    // Start is called before the first frame update
    void Start() {
        if (type == "rabbit") {
            animatorOverrideController["RabbitIdle"] = animations[0];
            animatorOverrideController["RabbitIdleLeft"] = animations[1];
            animatorOverrideController["RabbitIdleRight"] = animations[2];
            animatorOverrideController["RabbitIdleUp"] = animations[3];
            animatorOverrideController["RabbitWalk"] = animations[4];
            animatorOverrideController["RabbitWalkLeft"] = animations[5];
            animatorOverrideController["RabbitWalkRight"] = animations[6];
            animatorOverrideController["RabbitWalkUp"] = animations[7];
        }
        if (type == "goose") {
            animatorOverrideController["GooseIdle"] = animations[0];
            animatorOverrideController["GooseIdleLeft"] = animations[1];
            animatorOverrideController["GooseIdleRight"] = animations[2];
            animatorOverrideController["GooseIdleUp"] = animations[3];
            animatorOverrideController["GooseWalk"] = animations[4];
            animatorOverrideController["GooseWalkLeft"] = animations[5];
            animatorOverrideController["GooseWalkRight"] = animations[6];
            animatorOverrideController["GooseWalkUp"] = animations[7];
        }
        if (type == "fish") {
            animatorOverrideController["FishIdleDown"] = animations[0];
            animatorOverrideController["FishIdleLeft"] = animations[1];
            animatorOverrideController["FishIdleRight"] = animations[2];
            animatorOverrideController["FishIdleUp"] = animations[3];
            animatorOverrideController["FishSwimDown"] = animations[4];
            animatorOverrideController["FishSwimLeft"] = animations[5];
            animatorOverrideController["FishSwimRight"] = animations[6];
            animatorOverrideController["FishSwimUp"] = animations[7];
        }
        if (type == "pig") {
            animatorOverrideController["PigIdle"] = animations[0];
            animatorOverrideController["PigIdleLeft"] = animations[1];
            animatorOverrideController["PigIdleRight"] = animations[2];
            animatorOverrideController["PigIdleUp"] = animations[3];
            animatorOverrideController["PigWalk"] = animations[4];
            animatorOverrideController["PigWalkLeft"] = animations[5];
            animatorOverrideController["PigWalkRight"] = animations[6];
            animatorOverrideController["PigWalkUp"] = animations[7];
        } else if (type == "goat") {
            animatorOverrideController["GoatIdle"] = animations[0];
            animatorOverrideController["GoatIdleLeft"] = animations[1];
            animatorOverrideController["GoatIdleRight"] = animations[2];
            animatorOverrideController["GoatIdleUp"] = animations[3];
            animatorOverrideController["GoatWalk"] = animations[4];
            animatorOverrideController["GoatWalkLeft"] = animations[5];
            animatorOverrideController["GoatWalkRight"] = animations[6];
            animatorOverrideController["GoatWalkUp"] = animations[7];
        } else {
            animatorOverrideController["LlamaIdle"] = animations[0];
            animatorOverrideController["LlamaIdleLeft"] = animations[1];
            animatorOverrideController["LlamaIdleRight"] = animations[2];
            animatorOverrideController["LlamaIdleUp"] = animations[3];
            animatorOverrideController["LlamaWalk"] = animations[4];
            animatorOverrideController["LlamaWalkLeft"] = animations[5];
            animatorOverrideController["LlamaWalkRight"] = animations[6];
            animatorOverrideController["LlamaWalkUp"] = animations[7];
        }
    }

    public void SetXAndY(float x, float y) {
        MyAnimator.SetFloat("X", x);
        MyAnimator.SetFloat("Y", y);
    }
    public void SetFollow(bool follow) {
        MyAnimator.SetBool("Follow", follow);
    }
    // Update is called once per frame
    void Update() {

    }
    public void Equip() {
        animatorOverrideController["LlamaIdle"] = animations[0];
        animatorOverrideController["LlamaIdleLeft"] = animations[1];
        animatorOverrideController["LlamaIdleRight"] = animations[2];
        animatorOverrideController["LlamaIdleUp"] = animations[3];
        animatorOverrideController["LlamaWalk"] = animations[4];
        animatorOverrideController["LlamaWalkLeft"] = animations[5];
        animatorOverrideController["LlamaWalkRight"] = animations[6];
        animatorOverrideController["LlamaWalkUp"] = animations[7];
    }
    public void Dequip() {
        animatorOverrideController["LlamaIdle"] = null;
        animatorOverrideController["LlamaIdleLeft"] = null;
        animatorOverrideController["LlamaIdleRight"] = null;
        animatorOverrideController["LlamaIdleUp"] = null;
        animatorOverrideController["LlamaWalk"] = null;
        animatorOverrideController["LlamaWalkLeft"] = null;
        animatorOverrideController["LlamaWalkRight"] = null;
        animatorOverrideController["LlamaWalkUp"] = null;

        Color c = spriteRenderer.color;
        c.a = 0;
        spriteRenderer.color = c;
    }
}
