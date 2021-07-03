using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Sign : Interactable {
    public CanvasController canvasController;
    public DialogueManager dialogueManager;
    public string dialog;

    protected abstract void OnClose();

    protected abstract void OnOpen();

    private void Update() {
        if (Input.GetKeyUp(KeyCode.Space) && playerInRange) {
            if (!dialogueManager.IsDialogueBoxActive()) {
                if (canvasController.openCanvas()) {
                    canvasController.background.SetActive(false);
                    dialogueManager.StartSimpleDialog(dialog, OnOpen, OnClose);
                }
            }
        }
    }
}
