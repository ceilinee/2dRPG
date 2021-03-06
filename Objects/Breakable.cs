﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

/// <script>
/// Breakable should be attached to any game object to support breakable behavior.
/// Often times, we want more specific behavior depending on the object being broken. It is
/// recommended that you subclass this class in those situations. See Tree.cs for an example.
/// </script>
public class Breakable : CustomMonoBehaviour, IHittable {
    private Animator anim;

    // TODO: I'm not following the FloatValue SO pattern (not sure if necessary for this case)
    [Header(Annotation.HeaderMsgSetInInspector)]
    public float health;

    public bool isBroken;

    private Astar astar;

    protected virtual void Start() {
        anim = GetComponent<Animator>();
        Assert.IsNotNull(anim);

        Assert.IsNotNull(GetComponent<Collider2D>());

        astar = centralController.Get("A*").GetComponent<Astar>();
        Assert.IsNotNull(astar);
    }

    // Can choose to override in subclasses
    // to perform custom behaviour after the broken animation
    protected virtual void OnBroken() { }

    IEnumerator BreakCo() {
        yield return new WaitForSeconds(0.3f);
        OnBroken();
        // TODO: send a signal (have a signal for a complex placed item being destroyed / removed)
        // then in prefab placement controller, remove the placed item from placeditemscomplex
        var b = GetComponent<Collider2D>().bounds;
        gameObject.SetActive(false);
        astar.RescanAstarGraph(b);
    }

    public void BeHit(float damage) {
        if (isBroken) {
            return;
        }
        health = Math.Max(health - damage, 0);
        if (health == 0) {
            isBroken = true;
            // anim.SetBool("broken", true);
            StartCoroutine(BreakCo());
        }
    }
}
