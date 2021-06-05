using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is somewhat similar to Interactable.cs, except that it is intended to be
/// used on a gameobject that cannot collide with the player, but still be able to
/// detect the player
/// <summary>
public class Interactable_NonCollidable : CustomMonoBehaviour {
    private Transform playerTransform;
    public bool playerInRange;
    private float clickRange = 1;

    private void Start() {
        playerTransform = centralController.Get("Player").transform;
    }

    private void Update() {
        UpdatePlayerInRange();
    }

    protected void UpdatePlayerInRange() {
        if (!playerInRange && Vector3.Distance(playerTransform.position, transform.position) <= clickRange) {
            playerInRange = true;
        } else if (playerInRange && Vector3.Distance(playerTransform.position, transform.position) > clickRange) {
            playerInRange = false;
        }
    }
}
