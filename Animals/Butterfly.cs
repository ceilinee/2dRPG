using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour {
    // Start is called before the first frame update
    private int range = 5;
    public Vector2 randomTarget;
    public Transform target;
    private int catchRange = 1;
    private int curSuspect = 0;
    private int suspectMax = 50;
    public GameObject suspect;
    public Rigidbody2D myRigidbody;
    public bool playerInRange = false;
    public bool playerInCatchRange = false;
    public GameObject buySellAnimal;
    public GameObject animalModal;
    public Player player;
    public Animator anim;
    public Item item;
    public bool stop;
    void Awake() { }
    void Start() {
        // update all variables
        target = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        setRandomPosition();
    }

    // Update is called once per frame
    void Update() {
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
            buySellAnimal.GetComponent<BuySellAnimal>().pickUpItem(item);
        }
    }
    public void RemoveGameObject() {
        gameObject.SetActive(false);
    }
    void FixedUpdate() {
        CheckDistance();
    }
    void CheckDistance() {
        if (!stop && Vector3.Distance(randomTarget, transform.position) > 0.5) {
            Vector3 temp = Vector3.MoveTowards(transform.position, randomTarget, item.moveSpeed * Time.deltaTime);
            myRigidbody.MovePosition(temp);
            // Runs for animals randomly moving
        } else if (!stop && Vector3.Distance(randomTarget, transform.position) <= 0.5) {
            StartCoroutine(StopCo(5));
            setRandomPosition();
        }
    }
    private IEnumerator StopCo(float knockTime) {
        stop = true;
        yield return new WaitForSeconds(knockTime);
        stop = false;
    }
    public void setRandomPosition() {
        randomTarget = new Vector3(transform.position.x + Random.Range(-5.0F, 5.0F), transform.position.y + Random.Range(5.0F, -5.0F), 0);
    }
}
