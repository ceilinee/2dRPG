using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdoptionSign : Interactable {
    public AdoptionRequests adoptionRequests;
    public GameObject alert;
    public GameObject adoptionInformation;
    public GameObject CanvasController;
    // Start is called before the first frame update
    void Start() {
        if (adoptionRequests.requests.Length > 0) {
            alert.SetActive(true);
        } else {
            alert.SetActive(false);
        }
    }
    public void checkAlert() {
        if (adoptionRequests.requests.Length > 0) {
            alert.SetActive(true);
        } else {
            alert.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && playerInRange) {
            if (!CanvasController.activeInHierarchy) {
                CanvasController.SetActive(true);
            }
            if (CanvasController.GetComponent<CanvasController>().openCanvas()) {
                adoptionInformation.SetActive(true);
            }
        }
    }
}
