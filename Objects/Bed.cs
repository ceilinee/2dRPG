using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable {
    public GameObject CanvasController;
    public GameObject DayUpdater;
    public GameObject TimeController;
    // Start is called before the first frame update
    void Start() {

    }

    // If user interacts, speed up time and open DayUpdater
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && playerInRange) {
            if (!CanvasController.activeInHierarchy) {
                CanvasController.SetActive(true);
            }
            if (CanvasController.GetComponent<CanvasController>().openCanvas()) {
                DayUpdater.SetActive(true);
            }
        }
    }
}
