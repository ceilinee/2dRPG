using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class FishScript : GenericAnimal {
    [Header(Annotation.HeaderMsgDoNotSetInInspector)]
    [SerializeField]
    private Collider2D waterArea;

    // This gameobject stores colliders for all the naturally occuring
    // water bodies / ponds in the farm area
    // Set externally in SpawnAnimal.cs
    public GameObject naturalWaterFarm;

    // Used by this script because it stores a list of all the pond GOs
    // placed by the player
    // Set externally in SpawnAnimal.cs
    public PondPlacementController pondPlacementController;
    private int range = 5;
    private int catchRange = 1;
    private int curSuspect = 0;
    private int suspectMax = 50;
    public GameObject suspect;
    public bool playerInCatchRange = false;
    public GameObject buySellAnimal;
    public Player player;
    public Item item;

    [SerializeField] private VectorPointsList pondList;

    protected override void Awake() {
        Assert.IsNotNull(pondList);
    }

    // UpdateWater finds the collider for the body of water the fish is currently in,
    // with the help of the fish's current transform.position
    // The fish is either in a natural body of water or a placed pond
    public void UpdateWater() {
        waterArea = naturalWaterFarm.GetComponent<Water>().getValidSwimLocation(transform.position);
        // If the fish is not in a natural body of water, it must be in a placed pond
        if (waterArea == null) {
            foreach (GameObject placedPond in pondPlacementController.placedPondObjects) {
                waterArea = placedPond.GetComponent<Water>().getValidSwimLocation(transform.position);
                // TODO: if you assign a fish to a placed pond, waterArea will not be null
                // however, if they are in a placed pond and you save and restart the game, waterArea is null
                // for some reason
                if (waterArea != null) {
                    break;
                }
            }
        }
        Assert.IsNotNull(waterArea, "Fish's location must be within a body of water!");
        SetRandomPositionFish();
    }

    protected override void Start() {
        // update all variables
        target = centralController.Get("Player").transform;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        UpdateWater();
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
                var defaultPondVec = pondList.GetDefaultPond();
                animalTrait.location = defaultPondVec.value;
                animalTrait.home = defaultPondVec.locationName;
                spawnAnimal.GetComponent<SpawnAnimal>().Spawn(animalTrait);
            }
        }
    }
    public void RemoveGameObject() {
        spawnAnimal.GetComponent<SpawnAnimal>().animalGameObject.GetComponent<AnimalList>().removeAnimal(animalTrait.id);
        Destroy(gameObject);
    }
    protected override void FixedUpdate() {
        CheckDistanceFish();
    }

    private void OnDisable() {
        // This fish might be set inactive by AnimalList while it's executing StopCo, in which, stop
        // would never be set back to false, so we need to add this cleanup logic here
        if (stop) {
            stop = false;
        }
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
            StartCoroutine(StopCo(Random.Range(1, 6)));
            SetRandomPositionFish();
        } else {
            anim.SetBool("Follow", false);
            SetGearSocketFollow(false);
        }
    }

    bool ColliderContainsPoint(Transform ColliderTransform, Vector3 Point, bool Enabled) {
        Vector3 localPos = ColliderTransform.InverseTransformPoint(Point);
        if (Enabled && Mathf.Abs(localPos.x) < 0.5f && Mathf.Abs(localPos.y) < 0.5f && Mathf.Abs(localPos.z) < 0.5f)
            return true;
        else
            return false;
    }

    public void SetRandomPositionFish() {
        float delta = 4f;
        for (int i = 0; i < 10; i++) {
            Vector2 position = new Vector2(
                Random.Range(transform.position.x - delta, transform.position.x + delta),
                Random.Range(transform.position.y - delta, transform.position.y + delta));
            if (waterArea.OverlapPoint(position)) {
                randomTarget = position;
                return;
            }
        }
        randomTarget = transform.position;
    }
}
