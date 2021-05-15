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

    // Optional
    // Always runs after confirmFunction and cancelFunction
    public Action finallyFunction;

    public void initiateConfirmation(string newQuestion, Action newConfirmFunction, Action newCancelFunction) {
        initiateConfirmation(newQuestion, newConfirmFunction, newCancelFunction, null);
    }

    public void initiateConfirmation(
      string newQuestion, Action newConfirmFunction, Action newCancelFunction, Action newFinallyFunction) {
        question.text = newQuestion;
        confirmFunction = newConfirmFunction;
        cancelFunction = newCancelFunction;
        finallyFunction = newFinallyFunction;
        gameObject.SetActive(true);
    }

    void Update() {
        if (gameObject.activeInHierarchy && Time.timeScale != 0) {
            Time.timeScale = 0;
        }
    }
    public void confirm() {
        CanvasController.GetComponent<CanvasController>().closeCanvas();
        gameObject.SetActive(false);
        confirmFunction();
        if (finallyFunction != null) {
            finallyFunction();
        }
    }
    public void cancel() {
        gameObject.SetActive(false);
        CanvasController.GetComponent<CanvasController>().closeCanvasIfAllElseClosed();
        cancelFunction();
        if (finallyFunction != null) {
            finallyFunction();
        }
    }
}
