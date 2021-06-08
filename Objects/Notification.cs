using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class Notification : MonoBehaviour {
    public Text question;
    public GameObject CanvasController;
    public GameObject confirmationBackground;
    public bool onlyCloseSelf;
    public void initiateNotification(string newQuestion, bool _onlyCloseSelf = false) {
        if (transform.parent.gameObject.name == "Notification Background") {
            confirmationBackground = transform.parent.gameObject;
        }
        question.text = newQuestion;
        onlyCloseSelf = _onlyCloseSelf;
        gameObject.SetActive(true);
        if (confirmationBackground) {
            confirmationBackground.SetActive(true);
        }
    }
    public void submit() {
        if (!onlyCloseSelf) {
            CanvasController.GetComponent<CanvasController>().closeCanvas();
        }
        gameObject.SetActive(false);
        if (confirmationBackground) {
            confirmationBackground.SetActive(false);
        }
    }
    public void update() {
        if (Input.GetButtonDown("Cancel")) {
            if (!onlyCloseSelf) {
                CanvasController.GetComponent<CanvasController>().closeCanvas();
            }
            gameObject.SetActive(false);
            if (confirmationBackground) {
                confirmationBackground.SetActive(false);
            }
        }
        if (gameObject.activeInHierarchy && Time.timeScale != 0) {
            Time.timeScale = 0;
        }
    }
}
