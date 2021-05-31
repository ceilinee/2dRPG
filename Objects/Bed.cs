using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable {
    public GameObject CanvasController;
    public GameObject DayUpdater;
    public GameObject TimeController;
    public bool sleptRecently = false;
    // Start is called before the first frame update
    void Start() {

    }

    // If user interacts, speed up time and open DayUpdater
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && playerInRange && !sleptRecently && !CanvasController.GetComponent<CanvasController>().open) {
            sleptRecently = true;
            TimeController.GetComponent<TimeController>().fastForward();
            sleptRecently = false;
        } else if (Input.GetKeyDown(KeyCode.Space) && playerInRange && sleptRecently) {
            CanvasController.GetComponent<CanvasController>().initiateNotification("You're not feeling tired yet!");
        }
    }
}
