using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject topBar;
    public GameObject playerMenu;
    public GameObject animalMenu;
    public GameObject dialogBox;
    public GameObject birthAlert;
    public GameObject confirmation;
    public GameObject itemShop;
    public GameObject buildingShop;
    public GameObject notification;
    public GameObject shop;
    public GameObject selection;
    public GameObject background;
    public bool open;
    // public GameObject currentlyOpen;
    public Queue openedCanvas;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      if(open && Time.timeScale != 0){
        Time.timeScale = 0;
      }
      else if(!open && Time.timeScale == 0){
        Time.timeScale = 1;
        gameObject.SetActive(false);
      }
    }
    public bool openCanvas(){
      if(!open){
        closeAllCanvas();
        background.SetActive(true);
        open = true;
        return true;
      }
      else{
        return false;
      }
    }
    public void initiateNotification(string text){
      if(openCanvas()){
        notification.GetComponent<Notification>().initiateNotification(text);
      };
    }
    public void closeCanvas(){
      background.SetActive(false);
      open = false;
    }
    public void closeCanvasIfAllElseClosed(){
      if(playerMenu && playerMenu.activeInHierarchy){
        return;
      }
      if(animalMenu && animalMenu.activeInHierarchy){
        return;
      }
      if(dialogBox && dialogBox.activeInHierarchy){
        return;
      }
      if(itemShop && itemShop.activeInHierarchy){
        return;
      }
      if(buildingShop && buildingShop.activeInHierarchy){
        return;
      }
      if(birthAlert && birthAlert.activeInHierarchy){
        return;
      }
      if(confirmation && confirmation.activeInHierarchy){
        return;
      }
      if(background && background.activeInHierarchy){
        return;
      }
      if(selection && selection.activeInHierarchy){
        return;
      }
      if(notification && notification.activeInHierarchy){
        return;
      }
      if(shop && shop.activeInHierarchy){
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
    public bool closeAllCanvas(){
      if(playerMenu){
        playerMenu.SetActive(false);
      }
      if(animalMenu){
        animalMenu.SetActive(false);
      }
      if(dialogBox){
        dialogBox.SetActive(false);
      }
      if(itemShop){
        itemShop.SetActive(false);
      }
      if(buildingShop){
        buildingShop.SetActive(false);
      }
      if(birthAlert){
        birthAlert.SetActive(false);
      }
      if(confirmation){
        confirmation.SetActive(false);
      }
      if(background){
        background.SetActive(false);
      }
      if(selection){
        selection.SetActive(false);
      }
      if(notification){
        notification.SetActive(false);
      }
      if(shop){
        shop.SetActive(false);
      }
      open = false;
      return true;
    }
}
