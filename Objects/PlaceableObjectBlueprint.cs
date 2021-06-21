using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlaceableObjectBlueprint : MonoBehaviour {
    private Action onTriggerEnter;
    private Action onTriggerExit;

    public bool triggerEntered;

    public void SetTriggers(Action onTriggerEnter, Action onTriggerExit) {
        this.onTriggerEnter = onTriggerEnter;
        this.onTriggerExit = onTriggerExit;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!triggerEntered && !other.CompareTag("pet") && !other.CompareTag("placedObjectNoCollider")) {
            triggerEntered = true;
            onTriggerEnter();
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (!triggerEntered && !other.CompareTag("pet") && !other.CompareTag("placedObjectNoCollider")) {
            triggerEntered = true;
            onTriggerEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (triggerEntered) {
            triggerEntered = false;
            onTriggerExit();
        }
    }
}
