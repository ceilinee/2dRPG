using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum PlayerState {
    walk,
    attack,
    interact,
    stagger,
    idle
}

public enum Direction {
    Left,
    Up,
    Right,
    Down
}

public class PlayerMovement : MonoBehaviour {
    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidbody;
    public GameObject playerMenu;
    private Vector3 change;
    private Animator animator;
    public RuntimeAnimatorController maleAnimator;
    public RuntimeAnimatorController femaleAnimator;
    public Player player;
    public Inventory inventory;
    public GameObject CanvasController;
    public FloatValue currentHealth;
    public Signal playerHealthSignal;
    public VectorValue startingPosition;
    public PlayerGearSocket[] gearSockets;

    [Header("All the supported colors")]
    public AnimalColors colors;

    // The direction the player is currently facing
    public Direction direction;

    // The default value of the virtual axis at the start of the game
    private const int initialChangeX = 0;
    private const int initialChangeY = -1;

    [SerializeField]
    private PlacementController placementController;

    void Awake() {
        UpdateDirection(initialChangeX, initialChangeY);
        Assert.IsNotNull(placementController);
    }

    // Start is called before the first frame update
    void Start() {
        currentState = PlayerState.walk;
        setAnimator();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", initialChangeX);
        animator.SetFloat("moveY", initialChangeY);
        transform.position = startingPosition.initialValue;
        GetComponent<SpriteRenderer>().color = colors.colorDictionary[player.appearance.skinColorId].color;
    }
    public void setAnimator() {
        animator = GetComponent<Animator>();
        if (player.female) {
            animator.runtimeAnimatorController = femaleAnimator as RuntimeAnimatorController;
        } else {
            animator.runtimeAnimatorController = maleAnimator as RuntimeAnimatorController;
        }
    }
    // Update is called once per frame
    void FixedUpdate() {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (currentState == PlayerState.walk || currentState == PlayerState.idle) {
            UpdateDirection(change.x, change.y);
            UpdateAnimationAndMove();
        }
    }
    void Update() {
        if (Input.GetButtonDown("menu") && !playerMenu.activeInHierarchy) {
            if (!CanvasController.activeInHierarchy) {
                CanvasController.SetActive(true);
            }
            if (CanvasController.GetComponent<CanvasController>().openCanvas()) {
                playerMenu.GetComponent<PlayerInformation>().Open();
                Time.timeScale = 0;
            }
        }
    }
    private IEnumerator AttackCo() {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        currentState = PlayerState.walk;
    }
    public void setHold() {
        if (!animator) {
            setAnimator();
        }
        animator.SetBool("attacking", true);
    }
    public void setUnhold() {
        animator.SetBool("attacking", false);
    }
    void UpdateAnimationAndMove() {
        if (change != Vector3.zero) {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
            foreach (PlayerGearSocket socket in gearSockets) {
                socket.SetXAndY(change.x, change.y);
                socket.SetMoving(true);
            }
        } else {
            animator.SetBool("moving", false);
            foreach (PlayerGearSocket socket in gearSockets) {
                socket.SetMoving(false);
            }
        }
    }
    void MoveCharacter() {
        change.Normalize();
        myRigidbody.MovePosition(
            transform.position + change * speed * Time.deltaTime
        );
    }
    public void Knock(Rigidbody2D myRigidbody, float knockTime, float damage) {
        currentHealth.RunTimeValue -= damage;
        if (currentHealth.RunTimeValue > 0) {
            playerHealthSignal.Raise();
            StartCoroutine(KnockCo(myRigidbody, knockTime, damage));
        } else {
            playerHealthSignal.Raise();
            StartCoroutine(KnockCo(myRigidbody, knockTime, damage));
            this.gameObject.SetActive(false);
        }
    }
    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime, float damage) {
        if (myRigidbody != null) {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }

    // Set the direction the player is facing
    private void UpdateDirection(float x, float y) {
        if (x == -1 && y == 0) {
            direction = Direction.Left;
        } else if (x == 0 && y == 1) {
            direction = Direction.Up;
        } else if (x == 1 && y == 0) {
            direction = Direction.Right;
        } else if (x == 0 && y == -1) {
            direction = Direction.Down;
        }
    }

    public void BeWithinRangeOfInteractable() {
        placementController.PausePlacement();
    }

    public void BeOutsideRangeOfInteractable() {
        placementController.UnpausePlacement();
    }
}
