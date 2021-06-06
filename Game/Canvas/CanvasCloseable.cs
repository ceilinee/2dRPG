using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

// Attach this script to any canvas game object to have it close
// the game object and the canvas when the user presses the cancel key
public class CanvasCloseable : CustomMonoBehaviour {
    public CanvasController canvasController;

    private void Awake() {
        Assert.IsNotNull(canvasController);
    }

    private void Update() {
        if (Input.GetButtonDown("Cancel")) {
            Time.timeScale = 1;
            gameObject.SetActive(false);
            canvasController.closeCanvas();
        }
    }
}
