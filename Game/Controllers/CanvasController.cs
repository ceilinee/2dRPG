﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : CustomMonoBehaviour {
    public GameObject topBar;
    public GameObject playerMenu;
    public GameObject dayController;
    public GameObject animalMenu;
    public GameObject dialogBox;
    public GameObject birthAlert;
    public GameObject confirmation;
    public GameObject itemShop;
    public GameObject buildingShop;
    public GameObject notification;
    public GameObject shop;
    public GameObject mailInformation;
    public GameObject selection;
    public GameObject calendarInformation;
    public GameObject adoptionInformation;
    public GameObject background;
    public bool open;
    // public GameObject currentlyOpen;
    public Queue openedCanvas;
    public GameObject vetMenu;

    // Start is called before the first frame update
    void Start() {
        if (centralController.centralDictionary.ContainsKey("VetMenu")) {
            vetMenu = centralController.centralDictionary["VetMenu"];
        }
    }

    // Update is called once per frame
    void Update() {
        if (open && Time.timeScale != 0) {
            Time.timeScale = 0;
        } else if (!open && Time.timeScale == 0) {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }
    public bool openCanvas() {
        if (!open) {
            gameObject.SetActive(true);
            closeAllCanvas();
            if (background) {
                background.SetActive(true);
            }
            open = true;
            return true;
        } else {
            return false;
        }
    }
    public void openCanvasAgain() {
        if (background) {
            background.SetActive(true);
        }
        open = true;
    }
    public void initiateNotification(string text, bool _onlyCloseSelf = false) {
        if (openCanvas()) {
            notification.GetComponent<Notification>().initiateNotification(text, _onlyCloseSelf);
        } else {
            openCanvasAgain();
            notification.GetComponent<Notification>().initiateNotification(text, _onlyCloseSelf);
        }
    }
    public void closeCanvas() {
        Debug.Log("Close Canvas");
        if (background) {
            background.SetActive(false);
        }
        open = false;
        Time.timeScale = 1;
    }
    public void closeCanvasIfAllElseClosed() {
        if (dayController && dayController.activeInHierarchy) {
            return;
        }
        if (playerMenu && playerMenu.activeInHierarchy) {
            return;
        }
        if (mailInformation && mailInformation.activeInHierarchy) {
            return;
        }
        if (animalMenu && animalMenu.activeInHierarchy) {
            return;
        }
        if (dialogBox && dialogBox.activeInHierarchy) {
            return;
        }
        if (itemShop && itemShop.activeInHierarchy) {
            return;
        }
        if (buildingShop && buildingShop.activeInHierarchy) {
            return;
        }
        if (birthAlert && birthAlert.activeInHierarchy) {
            return;
        }
        if (confirmation && confirmation.activeInHierarchy) {
            return;
        }
        if (background && background.activeInHierarchy) {
            return;
        }
        if (calendarInformation && calendarInformation.activeInHierarchy) {
            return;
        }
        if (adoptionInformation && adoptionInformation.activeInHierarchy) {
            return;
        }
        if (selection && selection.activeInHierarchy) {
            return;
        }
        if (notification && notification.activeInHierarchy) {
            return;
        }
        if (shop && shop.activeInHierarchy) {
            return;
        }
        Debug.Log("Vet Menu: " + vetMenu.activeInHierarchy);
        if (vetMenu && vetMenu.activeInHierarchy) {
            return;
        }
        closeCanvas();
    }
    // public bool openNext(){
    //   // if(!open){
    //   //   closeAllCanvas();
    //   //   open = true;
    //   //   return true;
    //   // }
    //   // else{
    //   //   return false;
    //   // }
    // }
    public bool closeAllCanvas() {
        if (playerMenu) {
            playerMenu.SetActive(false);
        }
        if (animalMenu) {
            animalMenu.SetActive(false);
        }
        if (dialogBox) {
            dialogBox.SetActive(false);
        }
        if (dayController) {
            dayController.SetActive(false);
        }
        if (itemShop) {
            itemShop.SetActive(false);
        }
        if (adoptionInformation) {
            adoptionInformation.SetActive(false);
        }
        if (calendarInformation) {
            calendarInformation.SetActive(false);
        }
        if (buildingShop) {
            buildingShop.SetActive(false);
        }
        if (birthAlert) {
            birthAlert.SetActive(false);
        }
        if (mailInformation) {
            mailInformation.SetActive(false);
        }
        if (confirmation) {
            confirmation.SetActive(false);
        }
        if (background) {
            background.SetActive(false);
        }
        if (selection) {
            selection.SetActive(false);
        }
        if (notification) {
            notification.SetActive(false);
        }
        if (shop) {
            shop.SetActive(false);
        }
        if (vetMenu) {
            vetMenu.SetActive(false);
        }
        open = false;
        return true;
    }
}
