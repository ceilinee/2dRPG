using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <script>
/// By adding this script to a gameobject, the player can press space to open up
/// the player customization modal
/// <script>
public class PlayerCustomizer : Interactable_NonCollidable {
    private void Update() {
        if (Input.GetKeyUp(KeyCode.Space)) {
            UpdatePlayerInRange();
            if (playerInRange) {
                // TODO: open modal
            }
        }
    }
}
