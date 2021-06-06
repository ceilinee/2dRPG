using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <script>
/// By adding this script to a gameobject, the player can press space to open up
/// the player customization modal
/// <script>
public class PlayerCustomizer : Interactable_NonCollidable {
    [SerializeField]
    private GameObject canvasController;

    [SerializeField]
    private GameObject playerCustomizationModal;

    [SerializeField]
    private Signal playerAppearanceChanged;

    private void Awake() {
        Assert.IsNotNull(canvasController);
        Assert.IsNotNull(playerCustomizationModal);
        Assert.IsNotNull(playerAppearanceChanged);
    }

    private void OnPlayerCustomizationSubmit() {
        canvasController.GetComponent<CanvasController>().closeCanvas();
        // TODO: Add dialogue?
        playerAppearanceChanged.Raise();
    }

    private void OpenPlayerCustomizationModal() {
        if (!canvasController.activeInHierarchy) {
            canvasController.SetActive(true);
        }
        if (canvasController.GetComponent<CanvasController>().openCanvas()) {
            playerCustomizationModal.GetComponent<CharacterCreation>().Open(
                OnPlayerCustomizationSubmit);
        }
    }

    private void Update() {
        if (Input.GetKeyUp(KeyCode.Space)) {
            UpdatePlayerInRange();
            if (playerInRange) {
                // TODO: Add dialogue?
                OpenPlayerCustomizationModal();
            }
        }
    }
}
