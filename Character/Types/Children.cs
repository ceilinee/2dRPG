using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Children : GenericCharacter {

    [SerializeField]
    private bool shouldFollowPlayer;

    private AIDestinationSetter aidest;

    public override void SetWakeUpTrue() {
        if (!anim) {
            anim = GetComponent<Animator>();
        }
        stop = false;
        anim.SetBool("wakeUp", true);
        aipath.enabled = true;
        aidest.enabled = true;
    }

    public override void SetWakeUpFalse() {
        aidest.target = target;
        stop = true;
        anim.SetBool("wakeUp", false);
        aipath.enabled = false;
        aidest.enabled = false;
    }

    protected override void Start() {
        base.Start();
        aidest = GetComponent<AIDestinationSetter>();
        if (!shouldFollowPlayer) {
            SetWakeUpFalse();
        } else {
            aidest.target = target;
            SetWakeUpTrue();
        }
    }

    protected override void Update() {
        if (Input.GetKeyUp(KeyCode.Space) && playerInRange) {
            if (!conversation && !DialogueManager.activeInHierarchy) {
                if (playerInventory.currentItem != null) {
                    if (!CanvasController.activeInHierarchy) {
                        CanvasController.SetActive(true);
                    }
                    if (CanvasController.GetComponent<CanvasController>().openCanvas()) {
                        if (characterTrait.presentsDaily <= 2) {
                            if (characterTrait.presentsDaily == 0) {
                                spawnAnimal.GetComponent<SpawnAnimal>().playerGiftCharacter(characterTrait.id);
                            }
                            giveGift();
                        } else {
                            StartDialogue(characterTrait.characterEnoughGiftSpeech[characterTrait.friendshipScore / 100]);
                        }
                    }
                } else {
                    charDialogue();
                }
            }
        }
    }

    public override void CheckDistance() {
        if (!stop && shouldFollowPlayer) {
            changeAnim(aipath.desiredVelocity);
            if (Vector3.Distance(transform.position, target.position) < 1) {
                SetWakeUpFalse();
            }
        }
        if (!playerInRange && Vector3.Distance(target.position, transform.position) <= clickRange) {
            playerInRange = true;
            speechBubble.SetActive(true);
            SetWakeUpFalse();
            playerInRangeSignal.Raise();
        } else if (playerInRange && Vector3.Distance(target.position, transform.position) > clickRange) {
            playerInRange = false;
            speechBubble.SetActive(false);
            SetWakeUpTrue();
            playerOutsideRangeSignal.Raise();
        }
    }
}
