using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Confirmation : MonoBehaviour {
    public Text question;
    public GameObject CanvasController;
    public Action confirmFunction;
    public Action cancelFunction;
    private bool onlyCloseSelf;
    public GameObject confirmationBackground;
    // Optional
    // Always runs after confirmFunction and cancelFunction
    public Action finallyFunction;

    public void initiateConfirmation(string newQuestion, Action newConfirmFunction, Action newCancelFunction) {
        initiateConfirmation(newQuestion, newConfirmFunction, newCancelFunction, null);
    }
    public void initiateConfirmation(
      string newQuestion, Action newConfirmFunction, Action newCancelFunction, Action newFinallyFunction, bool _onlyCloseSelf = false) {
        if (transform.parent.gameObject.name == "Confirmation Background") {
            confirmationBackground = transform.parent.gameObject;
        }
        question.text = newQuestion;
        confirmFunction = newConfirmFunction;
        cancelFunction = newCancelFunction;
        finallyFunction = newFinallyFunction;
        onlyCloseSelf = _onlyCloseSelf;
        gameObject.SetActive(true);
        if (confirmationBackground) {
            confirmationBackground.SetActive(true);
        }
    }

    void Update() {
        if (gameObject.activeInHierarchy && Time.timeScale != 0) {
            Time.timeScale = 0;
        }
    }
    public void confirm() {
        gameObject.SetActive(false);
        if (confirmationBackground) {
            confirmationBackground.SetActive(false);
        }
        if (!onlyCloseSelf) {
            CanvasController.GetComponent<CanvasController>().closeCanvas();
        } else {
            CanvasController.GetComponent<CanvasController>().closeCanvasIfAllElseClosed();
        }
        confirmFunction();
        if (finallyFunction != null) {
            finallyFunction();
        }
    }
    public void cancel() {
        gameObject.SetActive(false);
        if (confirmationBackground) {
            confirmationBackground.SetActive(false);
        }
        if (!onlyCloseSelf) {
            CanvasController.GetComponent<CanvasController>().closeCanvas();
        } else {
            CanvasController.GetComponent<CanvasController>().closeCanvasIfAllElseClosed();
        }
        cancelFunction();
        if (finallyFunction != null) {
            finallyFunction();
        }
    }
}
