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
        triggerEntered = true;
        onTriggerEnter();
    }
    private void OnTriggerExit2D(Collider2D other) {
        triggerEntered = false;
        onTriggerExit();
    }
}
