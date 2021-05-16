﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Pathfinding;

public class GenericAnimal : AnimalState {
    public Rigidbody2D myRigidbody;
    public Transform target;
    public Transform parent;
    private bool followParent;
    public Vector3 randomTarget;
    public float clickRange;
    public float chaseRadius;
    public float attackRadius;
    public Animator anim;
    public TextMesh animalName;
    private Vector3 previousLocation;
    public bool playerInRange;
    public Signal contextOn;
    public Inventory playerInventory;
    public Animals curAnimals;
    public GameObject contextClue;
    public GameObject animalModal;
    // public GameObject cameraObject;
    public GameObject spawnAnimal;
    public Signal contextOff;
    public Animal animalTrait;
    public string[] giftMoods;
    public AnimalMood.DictionaryOfStringAndSprite reactions;
    // public bool follow;
    public bool stop;
    public ItemArray[] giftArray;
    public Character.DictionaryOfItemAndInt giftDictionary;

    [SerializeField]
    public GearSocket[] gearSockets;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    public void createAnimal(Animal trait) {
        animalTrait = trait;
    }
    void Update() {
        if (!animalTrait.characterOwned && !animalTrait.wild) {
            if (Input.GetKeyUp(KeyCode.Space) && currentState == AnimalStates.rest) {
                updatePlayerInRange();
                if (playerInRange) {
                    unsetRest();
                }
            } else if (Input.GetKeyUp(KeyCode.Space) && playerInRange && !animalModal.activeInHierarchy) {
                if (!animalModal.GetComponent<AnimalInformation>().CanvasController.GetComponent<CanvasController>().open && playerInventory.currentItem != null) {
                    if (animalTrait.presentsDaily <= 2) {
                        if (animalTrait.presentsDaily == 0) {
                            spawnAnimal.GetComponent<SpawnAnimal>().playerGiftAnimal();
                        }
                        giveGift();
                    } else {
                        animalModal.GetComponent<AnimalInformation>().CanvasController.GetComponent<CanvasController>().initiateNotification(animalTrait.animalName + " says they got enough presents already today!");
                    }
                } else {
                    if (!animalModal.GetComponent<AnimalInformation>().CanvasController.activeInHierarchy) {
                        animalModal.GetComponent<AnimalInformation>().CanvasController.SetActive(true);
                    }
                    if (animalModal.GetComponent<AnimalInformation>().CanvasController.GetComponent<CanvasController>().openCanvas()) {
                        openAnimalInformation();
                    }
                }
            }
            if (Input.GetButtonDown("Cancel") && animalModal.activeInHierarchy) {
                animalModal.GetComponent<AnimalInformation>().CloseIfPlayerMenuNotOpen();
            }
        }
        // if the animal is wild do this
        if (animalTrait.wild) {
            if (Input.GetKeyUp(KeyCode.Space) && currentState == AnimalStates.rest) {
                updatePlayerInRange();
                if (playerInRange) {
                    unsetRest();
                }
            } else if (Input.GetKeyUp(KeyCode.Space) && playerInRange && !animalModal.activeInHierarchy) {
                if (!animalModal.GetComponent<AnimalInformation>().CanvasController.GetComponent<CanvasController>().open && playerInventory.currentItem != null) {
                    if (animalTrait.presentsDaily <= 2) {
                        if (animalTrait.presentsDaily == 0) {
                            spawnAnimal.GetComponent<SpawnAnimal>().playerGiftAnimal();
                        }
                        giveGift();
                    } else {
                        animalModal.GetComponent<AnimalInformation>().CanvasController.GetComponent<CanvasController>().initiateNotification("This wild " + animalTrait.type + " got enough presents today and is backing away!");
                    }
                } else {
                    animalModal.GetComponent<AnimalInformation>().CanvasController.GetComponent<CanvasController>().initiateNotification("This " + animalTrait.type + " is still wild! Maybe you can earn it's trust by offering some presents.. Your friendship score is currently: " + animalTrait.love);
                }
            }
        }
    }
    public void giveGift() {
        float like = getLike(playerInventory.currentItem);
        animalTrait.presentsDaily += 1;
        animalTrait.age = System.Math.Max(0, animalTrait.age - playerInventory.currentItem.antiAge);
        animalTrait.health = System.Math.Min(100, animalTrait.health + playerInventory.currentItem.healthBonus);
        playerInventory.Removeitem(playerInventory.currentItem);
        if (playerInventory.currentItem == null) {
            target.Find("InventoryHold").GetComponent<PlayerInventory>().removeSprite();
        }
        increaseFriendship(like * 2);
        setContextClue(reactions[System.Math.Floor(like).ToString()]);
        animalTrait.mood = giftMoods[(int) System.Math.Floor(like)];
        animalTrait.moodId = System.Math.Floor(like).ToString();
        setContextClue(reactions[System.Math.Floor(like).ToString()]);
    }
    public void increaseFriendship(float points) {
        animalTrait.love += points;
        if (animalTrait.wild && animalTrait.love >= 0) {
            setAnimalTamed();
        }
    }
    public void setAnimalTamed() {
        Debug.Log("tamed");
        animalTrait.wild = false;
        animalModal.GetComponent<AnimalInformation>().CanvasController.GetComponent<CanvasController>().initiateNotification("This " + animalTrait.type + " trusts you now! Why don't you get them to follow you and bring them home?");
        curAnimals.addExistingAnimal(animalTrait);
        spawnAnimal.GetComponent<SpawnAnimal>().wildAnimals.removeExistingAnimal(animalTrait.id);
    }
    public void setContextClue(Sprite react) {
        if (!contextClue.activeInHierarchy) {
            contextClue.SetActive(true);
        }
        contextClue.GetComponent<SpriteRenderer>().sprite = react;
    }
    public void setContextClueOff() {
        contextClue.SetActive(false);
    }
    public float getLike(Item item) {
        int like = 0;
        if (giftDictionary.ContainsKey(item)) {
            like = giftDictionary[item];
        }
        return like;
    }
    public void openAnimalInformation() {
        RectTransform trans = animalModal.GetComponent<RectTransform>();
        PauseGame();
        GameObject profile = trans.Find("Profile").gameObject;
        RectTransform profileTrans = profile.GetComponent<RectTransform>();
        spawnAnimal.GetComponent<SpawnAnimal>().setAnimalImage(profileTrans.Find("ProfileImage").gameObject, animalTrait);
        animalTrait.colorAnimal(profileTrans.Find("ProfileImage").gameObject);
        animalModal.GetComponent<AnimalInformation>().updateAbout(animalTrait, gameObject);
        animalModal.SetActive(true);
    }
    void Start() {
        if (!animalTrait.characterOwned) {
            target = GameObject.FindWithTag("Player").transform;
        }
        if (animalTrait.age < 5 && parent) {
            followParent = true;
            startAStar(parent);
        } else if (animalTrait.follow && !animalTrait.characterOwned) {
            // The animal must be currently following the player
            moveSpeed *= 3;
            startAStar(target);
        }
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (animalName) {
            animalName.text = animalTrait.animalName;
        }
        anim.SetBool("Follow", true);
        SetGearSocketFollow(true);
        sprite = GetComponent<SpriteRenderer>();
        // transform.position = animalTrait.location;
        // Debug.Log(transform.position);
    }
    public void updateAnimal() {
        transform.position = animalTrait.location;
        if (animalTrait.target == new Vector2(0, 0)) {
            setRandomPosition();
        } else {
            randomTarget = animalTrait.target;
        }
        for (int i = 0; i < giftArray.Length; i++) {
            foreach (Item item in giftArray[i].array) {
                giftDictionary[item] = i;
            }
        }
    }
    public void flashEmotion() {

    }
    public void setRandomPosition() {
        if (animalTrait.target == new Vector2(0, 0)) {
            randomTarget = new Vector3(transform.position.x + Random.Range(-5.0F, 5.0F), transform.position.y + Random.Range(5.0F, -5.0F), 0);
        }
    }

    public void setAnimalFollow() {
        animalTrait.follow = true;
        moveSpeed *= 3;
        if (!animalTrait.walked) {
            spawnAnimal.GetComponent<SpawnAnimal>().playerWalkAnimal();
            animalTrait.walked = true;
            curAnimals.animalDict[animalTrait.id].walked = true;
        }
        unsetRest();
        startAStar(target);
    }

    public void startAStar(Transform target) {
        // The A* grid graph needs to be recalculated, as the map may have gone
        // through structural changes, such as a building being constructed
        AstarPath.active.Scan();

        AIPath aipath = GetComponent<AIPath>();
        aipath.maxSpeed = moveSpeed;

        AIDestinationSetter astar = GetComponent<AIDestinationSetter>();
        // Necessary because target is not set in the inspector
        astar.target = target;

        enableAStar();
    }

    public void enableAStar() {
        GetComponent<AIPath>().enabled = true;
        GetComponent<AIDestinationSetter>().enabled = true;
    }

    public void disableAStar() {
        GetComponent<AIDestinationSetter>().enabled = false;
        GetComponent<AIPath>().enabled = false;
    }

    public void setAnimalUnfollow() {
        animalTrait.follow = false;
        moveSpeed /= 3;

        AIPath aipath = GetComponent<AIPath>();
        aipath.enabled = false;

        AIDestinationSetter astar = GetComponent<AIDestinationSetter>();
        astar.enabled = false;
    }

    // private void OnTriggerEnter2D(Collider2D other){
    //   Debug.Log("Enter");
    //   if(other.CompareTag("Player")){
    //     playerInRange = true;
    //     contextOn.Raise();
    //     Physics.IgnoreLayerCollision(10, 9, true);
    //   }
    // }
    // private void OnTriggerExit2D(Collider2D other){
    //   Debug.Log("Exit");
    //   if(other.CompareTag("Player")){
    //     playerInRange = false;
    //     contextOff.Raise();
    //     // Physics.IgnoreLayerCollision(10, 9, false);
    //   }
    // }
    // Update is called once per frame
    void FixedUpdate() {
        if (currentState == AnimalStates.rest) {
        } else if (currentState == AnimalStates.eat) {
        } else {
            CheckDistance();
        }
    }
    public void PauseGame() {
        Time.timeScale = 0;
    }
    public void ResumeGame() {
        Time.timeScale = 1;
    }
    public virtual void CheckDistance() {
        if (animalTrait.follow && !animalTrait.characterOwned) {
            increaseFriendship(0.005f);
        }
        // Runs for animals following you or NPCs
        if (!stop && animalTrait.follow && Vector3.Distance(target.position, transform.position) > attackRadius) {
            if (currentState == AnimalStates.idle || currentState == AnimalStates.walk) {
                /*
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                myRigidbody.MovePosition(temp);
                ChangeState(AnimalStates.walk);
                anim.SetBool("Follow", true);
                SetGearSocketFollow(true);
                */
                AIPath aipath = GetComponent<AIPath>();
                changeAnim(aipath.desiredVelocity);
                ChangeState(AnimalStates.walk);
                anim.SetBool("Follow", true);
                SetGearSocketFollow(true);
            }
            // Runs for baby animals following a parent
        } else if (!stop && followParent && !animalTrait.follow && Vector3.Distance(parent.position, transform.position) > attackRadius) {
            if (currentState == AnimalStates.idle || currentState == AnimalStates.walk) {
                AIPath aipath = GetComponent<AIPath>();
                changeAnim(aipath.desiredVelocity);
                ChangeState(AnimalStates.walk);
                anim.SetBool("Follow", true);
                SetGearSocketFollow(true);
            }
            // Runs for animals randomly moving
        } else if (!stop && !followParent && !animalTrait.follow && Vector3.Distance(randomTarget, transform.position) > 0.5) {
            if (currentState == AnimalStates.idle || currentState == AnimalStates.walk) {
                Vector3 temp = Vector3.MoveTowards(transform.position, randomTarget, moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                myRigidbody.MovePosition(temp);
                ChangeState(AnimalStates.walk);
                anim.SetBool("Follow", true);
                SetGearSocketFollow(true);
                if (transform.position == previousLocation || Vector3.Distance(transform.position, previousLocation) <= 0.001) {
                    setRandomPosition();
                }
                previousLocation = transform.position;
            }
            // Runs for animals randomly moving
        } else if (!stop && !followParent && !animalTrait.follow && Vector3.Distance(randomTarget, transform.position) <= 0.5) {
            if (animalTrait.target != new Vector2(0, 0)) {
                animalTrait.target = new Vector2(0, 0);
            }
            StartCoroutine(StopCo(5));
            setRandomPosition();
        } else {
            anim.SetBool("Follow", false);
            SetGearSocketFollow(false);
        }
        updatePlayerInRange();
    }

    public void setRest() {
        // If the animal is following a person, it should not go to sleep
        if (animalTrait.follow) {
            return;
        }
        currentState = AnimalStates.rest;
        anim = GetComponent<Animator>();
        anim.SetBool("Follow", false);
        SetGearSocketFollow(false);
        if (followParent) {
            disableAStar();
        }
    }

    public void unsetRest() {
        currentState = AnimalStates.walk;
        if (followParent) {
            enableAStar();
        }
    }

    private void updatePlayerInRange() {
        //if owned by a character
        if (animalTrait.characterOwned) {
            if (Vector3.Distance(target.position, transform.position) <= clickRange) {
                playerInRange = true;
                stop = true;
            } else if (playerInRange == true) {
                playerInRange = false;
                stop = false;
            }
        }
        //if owned by player
        else {
            if (Vector3.Distance(target.position, transform.position) <= clickRange) {
                playerInRange = true;
                stop = true;
                setContextClue(reactions[animalTrait.moodId]);
                contextOn.Raise();
            } else if (playerInRange == true) {
                playerInRange = false;
                stop = false;
                setContextClueOff();
                contextOff.Raise();
            }
        }
    }
    private IEnumerator StopCo(float knockTime) {
        stop = true;
        yield return new WaitForSeconds(knockTime);
        stop = false;
    }
    public void SetGearSocketFollow(bool follow) {
        foreach (GearSocket g in gearSockets) {
            g.SetFollow(follow);
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
    public void ChangeState(AnimalStates newState) {
        if (currentState != newState) {
            currentState = newState;
        }
    }
}
