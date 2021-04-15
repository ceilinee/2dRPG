using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shopkeeper : GenericCharacter
{
    public GameObject selection;
    private Button talk;
    private Button give;
    private Button shopButton;
    public string[] thanksArray;
    public SceneInfos buildings;
    public Inventory items;
    public string startTime;
    public bool building;
    public string endTime;
    private bool subscribed;
    // Update is called once per frame
    protected override void Update()
    {
      if(Input.GetKeyUp(KeyCode.Space) && playerInRange){
        if(currentTime.isCurrentTimeBigger(startTime) && currentTime.isCurrentTimeSmaller(endTime)){
          shopKeeper = true;
        }
        else{
          shopKeeper = false;
        }
        if(playerInventory.currentItem != null){
          if(!CanvasController.activeInHierarchy){
            CanvasController.SetActive(true);
          }
          if(CanvasController.GetComponent<CanvasController>().openCanvas()){
            giveGift();
          }
        }
        else if(shopKeeper && !conversation && !DialogueManager.activeInHierarchy && shop){
          if(CanvasController.GetComponent<CanvasController>().openCanvas()){
            if(!subscribed){
              subscribe();
            }
            selection.SetActive(true);
          }
        }
        else if(!shopKeeper && !conversation && !DialogueManager.activeInHierarchy){
          dialogue();
        }
      }
      if(Input.GetButtonDown("Cancel") && selection.activeInHierarchy){
        closeSelection();
      }
    }
    public void subscribe(){
      talk = selection.transform.Find("Talk").gameObject.GetComponent<Button>();
      shopButton = selection.transform.Find("Shop").gameObject.GetComponent<Button>();
      talk.onClick.AddListener(dialogue);
      shopButton.onClick.AddListener(openShop);
      subscribed = true;
    }
    public void closeSelection(){
      CanvasController.GetComponent<CanvasController>().closeAllCanvas();
      talk.onClick.RemoveListener(dialogue);
      shopButton.onClick.RemoveListener(openShop);
      subscribed = false;
    }
    public void openShop(){
      closeSelection();
      if(!CanvasController.activeInHierarchy){
        CanvasController.SetActive(true);
      }
      if(CanvasController.GetComponent<CanvasController>().openCanvas()){
        conversation = true;
        if(characterTrait.portrait.Length > 0){
          shop.GetComponent<ShopInformation>().profileSprite = characterTrait.portrait[0];
        }
        shop.GetComponent<ShopInformation>().building = building;
        if(building && buildings){
          shop.GetComponent<ShopInformation>().buildings = buildings;
        }
        else if(items){
          shop.GetComponent<ShopInformation>().shopInventory = items;
        }
        shop.GetComponent<ShopInformation>().thanks.text = thanksArray[0];
        shop.GetComponent<ShopInformation>().thanksArray = thanksArray;
        shop.GetComponent<ShopInformation>().character = gameObject;
        shop.GetComponent<ShopInformation>().updateAbout();
        shop.SetActive(true);
      }
    }
    public void dialogue(){
      if(shopKeeper){
        closeSelection();
      }
      charDialogue();
    }
}
