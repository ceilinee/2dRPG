﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolLog : Log {
    // Update is called once per frame
    public Transform[] path;
    public int currentPoint;
    public Transform currentGoal;
    public float roundingDistance;
    public bool menu = false;
    public override void Awake() {
        if (!menu) {
            base.Awake();
        }
    }
    public override void Start() {
        if (!menu) {
            base.Start();
        } else {
            myRigidbody = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            anim.SetBool("wakeUp", true);
        }
    }
    public override void CheckDistance() {
        // TODO: make this also use A* (see parent class log)
        anim.SetBool("wakeUp", true);
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius) {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk
            && currentState != EnemyState.stagger) {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                myRigidbody.MovePosition(temp);
                ChangeState(EnemyState.walk);
            }
        } else if (Vector3.Distance(target.position, transform.position) > chaseRadius) {
            if (Vector3.Distance(transform.position, path[currentPoint].position) > roundingDistance) {
                Vector3 temp = Vector3.MoveTowards(transform.position, path[currentPoint].position, moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                myRigidbody.MovePosition(temp);
            } else {
                ChangeGoal();
            }
        }
    }
    private void ChangeGoal() {
        if (currentPoint == path.Length - 1) {
            currentPoint = 0;
            currentGoal = path[0];
        } else {
            currentPoint++;
            currentGoal = path[currentPoint];
        }
    }
}
