using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour {
    // This controller manages events such as children birth, marriage etc. 
    public GameObject confirmation;
    public GameObject birthModal;
    public GameObject childrenManager;
    public GameObject CanvasController;
    public Player player;
    private Character child;
    private int date;
    public Characters curChar;
    public void updateDay(int _date) {
        date = _date;
        marriageEvents();
    }
    IEnumerator kidRequest() {
        CanvasController.GetComponent<CanvasController>().openCanvas();
        confirmation.GetComponent<Confirmation>().initiateConfirmation("Do you want a kid?", confirmKid, () => {
            CanvasController.GetComponent<CanvasController>().closeCanvas();
        });
        while (confirmation.activeInHierarchy) {
            yield return null;
        }
    }
    public void childBorn() {
        CanvasController.GetComponent<CanvasController>().openCanvas();
        birthModal.GetComponent<Alert>().initiateNameAlert("Your child arrived! What should the baby be called?", saveChildNameFunction, true);
        void saveChildNameFunction(string name) {
            child.name = name;
            child.unborn = false;
            CanvasController.GetComponent<CanvasController>().closeCanvas();
        }
    }

    public void confirmKid() {
        StartCoroutine(confirmKidModel());
    }
    IEnumerator confirmKidModel() {
        childrenManager.GetComponent<ChildrenManager>().generateChild(date);
        CanvasController.GetComponent<CanvasController>().openCanvas();
        CanvasController.GetComponent<CanvasController>().notification.GetComponent<Notification>().initiateNotification("Great! The child will be arriving soon.. You've got a lot of shopping to do!");
        while (CanvasController.GetComponent<CanvasController>().notification.activeInHierarchy) {
            yield return null;
        }
        CanvasController.GetComponent<CanvasController>().closeCanvas();
    }
    void marriageEvents() {
        if (Random.Range(0, 10) <= 1 && player.childrenCharId.Count < 4 && !curChar.ContainsUnbornChild()) {
            StartCoroutine(kidRequest());
        }
        if (curChar.childBirthEvent(date) != null) {
            child = curChar.childBirthEvent(date);
            childBorn();
        }
        if (date % 5 == 0) {
            Debug.Log(date);
            curChar.AgeChildren();
        }
    }

}
