using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Butterfly : CustomMonoBehaviour {
    private int range = 5;
    public Vector2 randomTarget;
    public Transform target;
    private int catchRange = 1;
    private int curSuspect = 0;
    public int suspectMax = 20;
    public int disappearMax = 200;
    public GameObject suspect;
    public Rigidbody2D myRigidbody;
    public bool playerInRange = false;
    public bool playerInCatchRange = false;
    public GameObject buySellAnimal;
    public GameObject animalModal;
    public Player player;
    private Animator anim;
    private AnimatorOverrideController animatorOverrideController;
    public Bug item;
    public bool stop;
    private int overrideSpeed = 0;
    public bool tamed = false;

    void Start() {
        target = centralController.Get("Player").transform;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (item.animation != null) {
            animatorOverrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
            animatorOverrideController["RedButterfly"] = item.animation;
            anim.runtimeAnimatorController = animatorOverrideController;
        }
        setRandomPosition();
    }

    // Update is called once per frame
    void Update() {
        if (Vector3.Distance(target.position, transform.position) <= range) {
            playerInRange = true;
        } else if (playerInRange == true) {
            playerInRange = false;
        }
        if (!tamed) {
            // raise suspect 
            if (playerInRange && !Input.GetKey(KeyCode.Space)) {
                curSuspect++;
                suspect.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, curSuspect * 255 / 2550f);
            }
            if (playerInRange && Random.Range(0, 6) < 1) {
                if (Random.Range(0, System.Math.Max(20, player.reputation / 100)) >= 10) {
                    curSuspect++;
                    suspect.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, curSuspect * 255 / 2550f);
                }
            }
            // if too suspeect, disappear
            if (curSuspect >= suspectMax && overrideSpeed == 0) {
                overrideSpeed = 4 * item.moveSpeed;
                setRandomPosition();
            }
            if (curSuspect < suspectMax && overrideSpeed != 0) {
                overrideSpeed = item.moveSpeed;
                setRandomPosition();
            }
            if (curSuspect >= disappearMax) {
                RemoveGameObject();
            }
            // if player in catch range, set catch range 
            if (Vector3.Distance(target.position, transform.position) <= catchRange) {
                playerInCatchRange = true;
            } else if (playerInCatchRange == true) {
                playerInCatchRange = false;
            }
            // if player in catch range and escape button, add imp
            if (playerInCatchRange && Input.GetKeyDown("f")) {
                RemoveGameObject();
                buySellAnimal.GetComponent<BuySellAnimal>().pickUpItem(item);
            }
            if (!playerInRange && Random.Range(0, 20) < 1) {
                if (curSuspect >= 0) {
                    curSuspect--;
                    suspect.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, curSuspect * 255 / 2550f);
                }
            }
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
            Vector3 temp = Vector3.MoveTowards(transform.position, randomTarget, (overrideSpeed != 0 ? overrideSpeed : item.moveSpeed) * Time.deltaTime);
            myRigidbody.MovePosition(temp);
            // Runs for animals randomly moving
        } else if (!stop && Vector3.Distance(randomTarget, transform.position) <= 0.5) {
            if (curSuspect <= suspectMax) {
                StartCoroutine(StopCo(5));
            }
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
