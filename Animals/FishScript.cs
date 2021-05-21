using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour {
    // Start is called before the first frame update
    private Vector2 randomTarget;
    private int moveSpeed = 3;
    public Animator anim;
    public Rigidbody2D myRigidbody;
    private bool stop;
    private Transform target;
    private PolygonCollider2D waterArea;
    public GameObject water;
    private int range = 5;
    private int catchRange = 1;
    private int curSuspect = 0;
    private int suspectMax = 50;
    public GameObject suspect;
    private bool playerInRange = false;
    public bool playerInCatchRange = false;
    public GameObject buySellAnimal;
    public Player player;
    public Item item;

    void Start() {
        // update all variables
        target = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        waterArea = water.GetComponent<Water>().getValidSwimLocation(transform.position);
        setRandomPosition();
    }

    // Update is called once per frame
    void Update() {
        // set player in range
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
            gameObject.SetActive(false);
        }
        // if player in catch range, set catch range 
        if (Vector3.Distance(target.position, transform.position) <= catchRange) {
            playerInCatchRange = true;
        } else if (playerInCatchRange == true) {
            playerInCatchRange = false;
        }
        // if player in catch range and escape button, add imp
        if (playerInCatchRange && Input.GetKeyDown(KeyCode.Escape)) {
            gameObject.SetActive(false);
            buySellAnimal.GetComponent<BuySellAnimal>().pickUpItem(item);
        }
    }

    void FixedUpdate() {
        CheckDistance();
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
    public void SetAnimFloat(Vector2 setVector) {
        anim.SetFloat("X", setVector.x);
        anim.SetFloat("Y", setVector.y);
    }
    public virtual void CheckDistance() {
        if (!stop && Vector3.Distance(randomTarget, transform.position) > 0.5) {
            Vector3 temp = Vector3.MoveTowards(transform.position, randomTarget, moveSpeed * Time.deltaTime);
            changeAnim(temp - transform.position);
            myRigidbody.MovePosition(temp);
            anim.SetBool("Follow", true);
            // Runs for animals randomly moving
        } else if (!stop && Vector3.Distance(randomTarget, transform.position) <= 0.5) {
            StartCoroutine(StopCo(5));
            setRandomPosition();
        } else {
            anim.SetBool("Follow", false);
        }
    }
    private IEnumerator StopCo(float knockTime) {
        stop = true;
        yield return new WaitForSeconds(knockTime);
        stop = false;
    }
    public void setRandomPosition() {
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
