using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : CustomMonoBehaviour {
    public bool playerInRange;
    public Signal contextOn;
    public Signal contextOff;
    public bool triggerEntered;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerInRange = true;
            contextOn.Raise();
        }
        triggerEntered = true;
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerInRange = false;
            contextOff.Raise();
        }
        triggerEntered = false;
    }
}
