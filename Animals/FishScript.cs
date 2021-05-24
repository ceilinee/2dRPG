using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : GenericAnimal {
    // Start is called before the first frame update
    private PolygonCollider2D waterArea;
    public GameObject water;
    private int range = 5;
    private int catchRange = 1;
    private int curSuspect = 0;
    private int suspectMax = 50;
    public GameObject suspect;
    public bool playerInCatchRange = false;
    public GameObject buySellAnimal;
    public Player player;
    public Item item;
    protected override void Awake() { }
    protected override void Start() {
        // update all variables
        target = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        waterArea = water.GetComponent<Water>().getValidSwimLocation(transform.position);
        setRandomPosition();
    }

    // Update is called once per frame
    protected override void Update() {
        // set player in range
        if (!animalTrait.wild) {
            updatePlayerInRange();
            ownAnimalUpdate();
        } else {
            if (Vector3.Distance(target.position, transform.position) <= range) {
                playerInRange = true;
            } else if (playerInRange == true) {
                playerInRange = false;
                if (curSuspect >= 0) {
                    curSuspect--;
                }
            }
            // raise suspect 
            if (playerInRange && !Input.GetKey(KeyCode.Space)) {
                curSuspect++;
                suspect.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, curSuspect * 255 / 2550f);
            }
            if (playerInRange && Random.Range(0, 6) < 1) {
                if (Random.Range(0, player.reputation + 20) >= 10) {
                    curSuspect++;
                }
            }
            // if too suspeect, disappear
            if (curSuspect >= suspectMax) {
                RemoveGameObject();
            }
            // if player in catch range, set catch range 
            if (Vector3.Distance(target.position, transform.position) <= catchRange) {
                playerInCatchRange = true;
            } else if (playerInCatchRange == true) {
                playerInCatchRange = false;
            }
            // if player in catch range and escape button, add imp
            if (playerInCatchRange && Input.GetKeyDown(KeyCode.Escape)) {
                RemoveGameObject();
                // buySellAnimal.GetComponent<BuySellAnimal>().pickUpItem(item);
                animalModal.GetComponent<AnimalInformation>().CanvasController.GetComponent<CanvasController>().initiateNotification("You've found a new fish friend! This little guy will go live in your pond now.");
                curAnimals.addExistingAnimal(animalTrait);
                spawnAnimal.GetComponent<SpawnAnimal>().wildAnimals.removeExistingAnimal(animalTrait.id);
                animalTrait.wild = false;
                animalTrait.location = new Vector2(-34.4f, -11.5f);
                spawnAnimal.GetComponent<SpawnAnimal>().Spawn(animalTrait);
            }
        }
    }
    public void RemoveGameObject() {
        gameObject.SetActive(false);
        spawnAnimal.GetComponent<SpawnAnimal>().animalGameObject.GetComponent<AnimalList>().removeAnimal(animalTrait.id);
    }
    protected override void FixedUpdate() {
        CheckDistanceFish();
    }
    public virtual void CheckDistanceFish() {
        if (!stop && Vector3.Distance(randomTarget, transform.position) > 0.5) {
            Vector3 temp = Vector3.MoveTowards(transform.position, randomTarget, moveSpeed * Time.deltaTime);
            changeAnim(temp - transform.position);
            myRigidbody.MovePosition(temp);
            anim.SetBool("Follow", true);
            SetGearSocketFollow(true);
            // Runs for animals randomly moving
        } else if (!stop && Vector3.Distance(randomTarget, transform.position) <= 0.5) {
            StartCoroutine(StopCo(5));
            setRandomPositionFish();
        } else {
            anim.SetBool("Follow", false);
            SetGearSocketFollow(false);
        }
    }
    private IEnumerator StopCo(float knockTime) {
        stop = true;
        yield return new WaitForSeconds(knockTime);
        stop = false;
    }
    public void setRandomPositionFish() {
        if (waterArea) {
            var bounds = waterArea.bounds;
            //generate random points
            for (int i = 0; i < 10; i++) {
                Vector2 position = new Vector2(
                bounds.center.x + Random.Range(-bounds.extents.x, bounds.extents.x),
                bounds.center.y + Random.Range(-bounds.extents.y, bounds.extents.y));
                if (waterArea.bounds.Contains(position)) {
                    randomTarget = position;
                    break;
                }
            }
        }
    }
}
