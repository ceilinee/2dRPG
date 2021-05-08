﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public enum CharacterStates {
    idle,
    walk
}

public class GenericCharacter : MonoBehaviour {
    public CharacterStates currentState;
    public GameObject spawnAnimal;
    public GameObject DialogueManager;
    public GameObject CanvasController;
    public Characters curCharacters;
    public Rigidbody2D myRigidbody;
    public bool shopKeeper;
    public GameObject shop;
    private Transform target;
    public Player player;
    public GameObject speechBubble;
    private Vector3 randomTarget;
    // private Vector3 previousLocation;
    private float clickRange = 1;
    private Animator anim;
    public bool playerInRange;
    // public GameObject animalModal;
    public Character characterTrait;
    public bool stop;
    public bool conversation;
    public Inventory playerInventory;
    public CurTime currentTime;
    public Animals curAnimals;

    [SerializeField]
    public GearSocket[] gearSockets;
    public SpriteRenderer sprite;

    // Start is called before the first frame update
    public void createCharacter(Character trait) {
        characterTrait = trait;
    }
    protected virtual void Update() {
        if (Input.GetKeyUp(KeyCode.Space) && playerInRange) {
            if (!shopKeeper && !conversation && !DialogueManager.activeInHierarchy) {
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
            if (shopKeeper && !conversation && !DialogueManager.activeInHierarchy && shop) {
                if (!CanvasController.activeInHierarchy) {
                    CanvasController.SetActive(true);
                }
                if (CanvasController.GetComponent<CanvasController>().openCanvas()) {
                    conversation = true;
                    shop.GetComponent<ShopInformation>().updateAbout();
                    shop.GetComponent<ShopInformation>().character = gameObject;
                    shop.SetActive(true);
                }
            }
        }
    }
    //starts generic dialogue with characters
    public void charDialogue() {
        if (!characterTrait.talked) {
            spawnAnimal.GetComponent<SpawnAnimal>().playerTalkCharacter(characterTrait.id);
            characterTrait.talked = true;
        }
        if (!CanvasController.activeInHierarchy) {
            CanvasController.SetActive(true);
        }
        if (CanvasController.GetComponent<CanvasController>().openCanvas()) {
            CanvasController.GetComponent<CanvasController>().background.SetActive(false);
            conversation = true;
            StartDialogue(characterTrait.characterSpeechArray[characterTrait.friendshipScore / 100]
            .array[UnityEngine.Random.Range(0, characterTrait.characterSpeechArray[characterTrait.friendshipScore / 100]
            .array
            .Length)]);
            increaseFriendship(3);
        }
    }
    //generates dialogue from string
    public void makeDialogue(string newDialogue) {
        Dialogue dialogue = new Dialogue();
        dialogue.sentence[0] = newDialogue;
        StartDialogue(dialogue);
    }
    //starts dialogue
    public void StartDialogue(Dialogue dialogue) {
        DialogueManager.SetActive(true);
        DialogueManager.GetComponent<DialogueManager>().startDialog(gameObject, dialogue);
    }
    public void ClearDailies() {
        characterTrait.talked = false;
        characterTrait.presentsDaily = 0;
    }
    public void giveGift() {
        conversation = true;
        characterTrait.presentsDaily += 1;
        Item curItem = playerInventory.currentItem;
        int like = getLike(curItem);
        increaseFriendship(like * 2);
        if (!curItem.date && !curItem.marriage && !curItem.divorce) {
            if (player.exCharId.Contains(characterTrait.id)) {
                StartDialogue(characterTrait.characterExGiftReceiveSpeechArray[like].array[Math.Min(characterTrait.friendshipScore / 100, Math.Max(0, characterTrait.characterExGiftReceiveSpeechArray[like].array.Length - 1))]);
            } else {
                DialogueManager.SetActive(true);
                StartDialogue(characterTrait.characterGiftReceiveSpeechArray[like].array[Math.Min(characterTrait.friendshipScore / 100, Math.Max(0, characterTrait.characterGiftReceiveSpeechArray[like].array.Length - 1))]);
            }
        } else {
            updateCharacterStatus(curItem);
        }
        playerInventory.Removeitem(curItem);
        if (playerInventory.currentItem == null) {
            target.Find("InventoryHold").GetComponent<PlayerInventory>().removeSprite();
        }
    }
    public void updateCharacterStatus(Item curItem) {
        if (curItem.date) {
            Debug.Log("date");
            if (characterTrait.friendshipScore >= 600) {
                player.date(characterTrait.id);
                characterTrait.date = true;
                StartDialogue(characterTrait.characterDatingSpeech);
            } else {
                StartDialogue(characterTrait.characterRejectionSpeech);
            }
        } else if (curItem.marriage) {
            if (characterTrait.friendshipScore >= 1000 && player.datingCharId.Contains(characterTrait.id)) {
                characterTrait.married = true;
                player.marry(characterTrait.id);
                StartDialogue(characterTrait.characterMarriageSpeech);
                // make all other dating characters hate player
            } else {
                StartDialogue(characterTrait.characterRejectionSpeech);
            }
            //add marriage event
        } else if (curItem.divorce) {
            if (player.marriedCharId == characterTrait.id) {
                player.divorce();
                characterTrait.married = false;
                characterTrait.date = false;
                characterTrait.friendshipScore = 0;
                StartDialogue(characterTrait.characterBreakUpSpeech);
            } else if (player.datingCharId.Contains(characterTrait.id)) {
                player.breakUp(characterTrait.id);
                characterTrait.date = false;
                characterTrait.friendshipScore = 0;
                StartDialogue(characterTrait.characterBreakUpSpeech);
            } else {
                StartDialogue(characterTrait.characterConfusionSpeech);
            }
        }
        curCharacters.updateCharacter(characterTrait);
    }
    public void increaseFriendship(int points) {
        characterTrait.friendshipScore += points;
        curCharacters.characterDict[characterTrait.id].friendshipScore = characterTrait.friendshipScore;
    }
    public int getLike(Item item) {
        int like = 0;
        if (characterTrait.giftDictionary.ContainsKey(item)) {
            like = characterTrait.giftDictionary[item];
        }
        return like;
    }
    public void updateCharacterMovement() {
        for (int i = 0; i < characterTrait.travelTimes.Length; i++) {
            characterTrait.characterMovement[characterTrait.travelTimes[i]] = characterTrait.path[i];
        }
    }
    void Start() {
        // updateCharacterMovement();
        setControllers();
        characterTrait.image = GetComponent<SpriteRenderer>().sprite;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        anim.SetBool("wakeUp", true);
        sprite = GetComponent<SpriteRenderer>();
        // Debug.Log(characterTrait.location);
        // transform.position = characterTrait.location;
        // characterTrait.selectedPath.pathArray = FindTime();
        for (int i = 0; i < characterTrait.giftArray.Length; i++) {
            foreach (Item item in characterTrait.giftArray[i].array) {
                characterTrait.giftDictionary[item] = i;
            }
        }
        if (curAnimals) {
            getAnimal();
        }
        // characterManager.GetComponent<CharacterManager>().UpdateManager();
    }
    public void setControllers() {
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("mainController");
        if (gameObjects.Length > 0) {
            Transform controller = gameObjects[0].transform;
            DialogueManager = controller.Find("DialogueManager").gameObject;
            CanvasController = controller.Find("CanvasController").gameObject;
            spawnAnimal = controller.Find("SpawnAnimal").gameObject;
        }
    }
    public void getAnimal() {
        if (characterTrait.currentAnimalId != 0) {
            spawnAnimal.GetComponent<SpawnAnimal>().SpawnCharAnimal(curAnimals.animalDict[characterTrait.currentAnimalId], transform);
        } else {
            getRandomAnimal();
        }
    }
    public void getRandomAnimal() {
        List<int> keyList = new List<int>(curAnimals.animalDict.Keys);
        List<int> keyListFiltered = keyList.FindAll(key => curAnimals.animalDict[key].characterOwned && curAnimals.animalDict[key].charId == characterTrait.id);
        if (keyListFiltered.Count > 0) {
            int randomKey = keyListFiltered[UnityEngine.Random.Range(0, keyListFiltered.Count)];
            spawnAnimal.GetComponent<SpawnAnimal>().SpawnCharAnimal(curAnimals.animalDict[randomKey], transform);
            characterTrait.currentAnimalId = randomKey;
        }
    }
    void FixedUpdate() {
        CheckDistance();
    }
    public void PauseGame() {
        Time.timeScale = 0;
    }
    public void ResumeGame() {
        Time.timeScale = 1;
    }
    public void SetWakeUpTrue() {
        if (!anim) {
            anim = GetComponent<Animator>();
        }
        stop = false;
        anim.SetBool("wakeUp", true);
    }
    public void SetWakeUpFalse() {
        stop = true;
        anim.SetBool("wakeUp", false);
    }
    public void CheckDistance() {
        // Debug.Log(characterTrait.currentPoint);
        // Debug.Log(characterTrait.selectedPath.pathArray);
        // if don't stop and path exists
        if (!stop && characterTrait.selectedPath.scene != null && characterTrait.selectedPath.pathArray.Length > 0) {
            //if theres still distance between goal and position
            if (characterTrait.currentPoint < characterTrait.selectedPath.pathArray.Length
            && Vector3.Distance(transform.position, characterTrait.selectedPath.pathArray[characterTrait.currentPoint].value) > 1) {
                Vector3 temp = Vector3.MoveTowards(
                    transform.position,
                    characterTrait.selectedPath.pathArray[characterTrait.currentPoint].value,
                    characterTrait.moveSpeed * Time.deltaTime
                );
                // Debug.Log(characterTrait.name + temp.ToString());
                changeAnim(temp - transform.position);
                myRigidbody.MovePosition(temp);
            } else {
                ChangeGoal();
            }
            //if reached the end of selectedpath
            // Debug.Log(Vector3.Distance(transform.position, characterTrait.selectedPath.pathArray[Mathf.Max(characterTrait.selectedPath.pathArray.Length-1, 0)].position));
            if (Vector3.Distance(transform.position, characterTrait.selectedPath.pathArray[Mathf.Max(characterTrait.selectedPath.pathArray.Length - 1, 0)].value) < 1) {
                // Debug.Log(characterTrait.id);
                SetWakeUpFalse();
            }
        }
        if (!stop && (characterTrait.selectedPath.pathArray.Length == 0 || characterTrait.currentPoint >= characterTrait.selectedPath.pathArray.Length)) {
            SetWakeUpFalse();
        }
        // if(!stop && characterTrait.selectedPath.pathArray.Length > 0){
        //   if(Vector3.Distance(transform.position, characterTrait.selectedPath.pathArray[Mathf.Max(characterTrait.selectedPath.pathArray.Length-1, 0)].position) < 1){
        //     SetWakeUpFalse();
        //   }
        // }
        if (Vector3.Distance(target.position, transform.position) <= clickRange) {
            playerInRange = true;
            speechBubble.SetActive(true);
            SetWakeUpFalse();
        } else if (playerInRange && Vector3.Distance(target.position, transform.position) > clickRange) {
            playerInRange = false;
            speechBubble.SetActive(false);
            SetWakeUpTrue();
        }
    }
    private void ChangeGoal() {
        if (characterTrait.currentPoint >= characterTrait.selectedPath.pathArray.Length - 1) {
            // characterTrait.currentPoint = 0;
        } else {
            characterTrait.currentPoint++;
        }
    }
    public void SetAnimFloat(Vector2 setVector) {
        anim.SetFloat("X", setVector.x);
        anim.SetFloat("Y", setVector.y);
        foreach (GearSocket g in gearSockets) {
            g.SetXAndY(setVector.x, setVector.y);
        }
    }
    public void changeAnim(Vector2 direction) {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
            if (direction.x > 0) {
                SetAnimFloat(Vector2.right);
            } else {
                SetAnimFloat(Vector2.left);
            }
        } else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y)) {
            if (direction.y > 0) {
                SetAnimFloat(Vector2.up);
            } else {
                SetAnimFloat(Vector2.down);
            }
        }
    }
    public void ChangeState(CharacterStates newState) {
        if (currentState != newState) {
            currentState = newState;
        }
    }
}
