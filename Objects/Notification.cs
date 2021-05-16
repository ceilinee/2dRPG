using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class Notification : MonoBehaviour {
    public Text question;
    public GameObject CanvasController;

    public bool onlyCloseSelf;
    public void initiateNotification(string newQuestion, bool _onlyCloseSelf = false) {
        question.text = newQuestion;
        onlyCloseSelf = _onlyCloseSelf;
        gameObject.SetActive(true);
    }
    public void submit() {
        if (!onlyCloseSelf) {
            CanvasController.GetComponent<CanvasController>().closeCanvas();
        }
        gameObject.SetActive(false);
    }
}
