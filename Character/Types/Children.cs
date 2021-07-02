using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class Children : GenericCharacter {
    private bool subscribed;
    public GameObject selection;
    private Button talk;
    private Button menu;
    private AIDestinationSetter aidest;
    public CharacterInformation characterMenu;

    public override void SetWakeUpTrue() {
        if (!anim) {
            anim = GetComponent<Animator>();
        }
        stop = false;
        anim.SetBool("wakeUp", true);
        aipath.enabled = true;
        if (characterTrait.follow) {
            if (aidest) {
                aidest.enabled = true;
            }
            aipath.maxSpeed = 7;
        } else {
            if (aidest) {
                aidest.enabled = false;
            }
            aipath.maxSpeed = 3;
            aipath.destination = characterTrait.selectedPath.dest.value;
        }
    }
    public void SetFollow() {
        aidest.target = target;
        SetWakeUpTrue();
    }
    public override void SetWakeUpFalse() {
        stop = true;
        anim.SetBool("wakeUp", false);
        aipath.enabled = false;
        if (aidest) {
            aidest.enabled = false;
        }
    }

    protected override void Start() {
        clickRange = 1f;
        aidest = GetComponent<AIDestinationSetter>();
        base.Start();
        characterMenu = centralController.Get("CharacterMenu").GetComponent<CharacterInformation>();
        selection = centralController.Get("Selection");
        if (!characterTrait.follow) {
            SetWakeUpFalse();
        } else {
            aidest.target = target;
            SetWakeUpTrue();
        }
        talk = selection.transform.Find("Talk").gameObject.GetComponent<Button>();
        menu = selection.transform.Find("Shop").gameObject.GetComponent<Button>();
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
                } else if (!conversation && !DialogueManager.activeInHierarchy) {
                    if (CanvasController.GetComponent<CanvasController>().openCanvas()) {
                        if (!subscribed) {
                            subscribe();
                        }
                        selection.SetActive(true);
                    }
                }
            }
        }
    }
    public void subscribe() {
        selection.transform.Find("Talk").Find("ConfirmText").gameObject.GetComponent<Text>().text = "Talk";
        selection.transform.Find("Shop").Find("ConfirmText").gameObject.GetComponent<Text>().text = "About";
        talk.onClick.AddListener(SelectDialogue);
        menu.onClick.AddListener(OpenMenu);
        subscribed = true;
    }
    public void closeSelection() {
        CanvasController.GetComponent<CanvasController>().closeAllCanvas();
        if (talk) { talk.onClick.RemoveListener(SelectDialogue); };
        if (menu) { menu.onClick.RemoveListener(OpenMenu); };
        subscribed = false;
    }
    public void SelectDialogue() {
        closeSelection();
        charDialogue();
    }
    public void OpenMenu() {
        closeSelection();
        characterMenu.SetUp(gameObject);
    }
    public override void CheckDistance() {
        if (!stop) {
            changeAnim(aipath.desiredVelocity);
            if (characterTrait.follow && Vector3.Distance(transform.position, target.position) < 1.5) {
                SetWakeUpFalse();
            } else if (!characterTrait.follow && Vector3.Distance(transform.position, characterTrait.selectedPath.dest.value) < 1) {
                SetWakeUpFalse();
            }
        }
        if (stop && !playerInRange && characterTrait.follow && Vector3.Distance(transform.position, target.position) > 1.5) {
            SetWakeUpTrue();
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
