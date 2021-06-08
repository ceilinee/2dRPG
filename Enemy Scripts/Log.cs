using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Log : Enemy, IFollower {
    public Rigidbody2D myRigidbody;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    public Animator anim;

    private AIPath aiPath;
    private AIDestinationSetter aiDest;

    public virtual void Awake() {
        aiPath = GetComponent<AIPath>();
        aiDest = GetComponent<AIDestinationSetter>();

        aiPath.maxSpeed = moveSpeed;
    }

    // Start is called before the first frame update
    public virtual void Start() {
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        SetTarget(target ?? centralController.Get("Player").transform);
        anim.SetBool("wakeUp", true);
    }

    // Update is called once per frame
    void FixedUpdate() {
        CheckDistance();
    }

    public virtual void CheckDistance() {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius) {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk
            && currentState != EnemyState.stagger) {
                if (!IsFollowing()) {
                    FollowStart();
                }
                changeAnim(aiPath.desiredVelocity);
                ChangeState(EnemyState.walk);
                anim.SetBool("wakeUp", true);
            }
        } else if (Vector3.Distance(target.position, transform.position) > chaseRadius) {
            if (IsFollowing()) {
                FollowStop();
            }
            anim.SetBool("wakeUp", false);
        }
    }
    public void SetAnimFloat(Vector2 setVector) {
        anim.SetFloat("moveX", setVector.x);
        anim.SetFloat("moveY", setVector.y);
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
    public void ChangeState(EnemyState newState) {
        if (currentState != newState) {
            currentState = newState;
        }
    }

    public void SetTarget(Transform target) {
        this.target = target;
        aiDest.target = target;
    }

    public void FollowStart() {
        aiDest.enabled = true;
        aiPath.enabled = true;
    }

    public void FollowStop() {
        aiDest.enabled = false;
        aiPath.enabled = false;
    }

    public bool IsFollowing() {
        return aiPath.enabled;
    }
}
