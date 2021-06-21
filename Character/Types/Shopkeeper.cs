using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shopkeeper : GenericCharacter {
    public GameObject selection;
    private Button talk;
    private Button give;
    private Button shopButton;
    public string[] thanksArray;
    public ItemList buildingsForSale;
    public Inventory items;
    public string startTime;
    public bool building;
    public string endTime;
    private VetInformation vetMenu;
    public bool vet;
    private bool subscribed;
    protected override void Start() {
        base.Start();
        if (vet) {
            vetMenu = centralController.centralDictionary["VetMenu"].GetComponent<VetInformation>();
        }
    }
    // Update is called once per frame
    protected override void Update() {
        if (Input.GetKeyUp(KeyCode.Space) && playerInRange) {
            if (currentTime.isCurrentTimeBigger(startTime) && currentTime.isCurrentTimeSmaller(endTime)) {
                shopKeeper = true;
            } else {
                shopKeeper = false;
            }
            if (playerInventory.currentItem != null) {
                if (!CanvasController.activeInHierarchy) {
                    CanvasController.SetActive(true);
                }
                if (CanvasController.GetComponent<CanvasController>().openCanvas()) {
                    if (characterTrait.presentsDaily <= 2) {
                        if (characterTrait.presentsDaily == 0) {
                            spawnAnimal.GetComponent<SpawnAnimal>().playerGiftAnimal();
                        }
                        giveGift();
                    } else {
                        CanvasController.GetComponent<CanvasController>().initiateNotification("You've given " + characterTrait.name + " enough presents today!");
                    }
                }
            } else if (shopKeeper && !conversation && !DialogueManager.activeInHierarchy && shop) {
                if (CanvasController.GetComponent<CanvasController>().openCanvas()) {
                    if (!subscribed) {
                        subscribe();
                    }
                    selection.SetActive(true);
                }
            } else if (!shopKeeper && !conversation && !DialogueManager.activeInHierarchy) {
                SelectDialogue();
            }
        }
        if (Input.GetButtonDown("Cancel") && selection.activeInHierarchy) {
            closeSelection();
        }
    }
    public void subscribe() {
        talk = selection.transform.Find("Talk").gameObject.GetComponent<Button>();
        selection.transform.Find("Talk").Find("ConfirmText").gameObject.GetComponent<Text>().text = "Talk";
        shopButton = selection.transform.Find("Shop").gameObject.GetComponent<Button>();
        selection.transform.Find("Shop").Find("ConfirmText").gameObject.GetComponent<Text>().text = "Shop";
        talk.onClick.AddListener(SelectDialogue);
        shopButton.onClick.AddListener(openShop);
        subscribed = true;
    }
    public void closeSelection() {
        CanvasController.GetComponent<CanvasController>().closeAllCanvas();
        if (talk) { talk.onClick.RemoveListener(SelectDialogue); };
        if (shopButton) { shopButton.onClick.RemoveListener(openShop); };
        subscribed = false;
    }
    public void openShop() {
        closeSelection();
        if (!CanvasController.activeInHierarchy) {
            CanvasController.SetActive(true);
        }
        if (CanvasController.GetComponent<CanvasController>().openCanvas()) {
            conversation = true;
            if (vetMenu) {
                openVetMenu();
                return;
            }
            if (characterTrait.portrait.Length > 0) {
                shop.GetComponent<ShopInformation>().profileSprite = characterTrait.portrait[0];
            }
            if (building) {
                shop.GetComponent<ShopInformation>().building = building;
            }
            if (building && buildingsForSale) {
                shop.GetComponent<ShopInformation>().buildingsForSale = buildingsForSale;
            } else if (items) {
                shop.GetComponent<ShopInformation>().shopInventory = items;
            }
            shop.GetComponent<ShopInformation>().thanks.text = thanksArray[0];
            shop.GetComponent<ShopInformation>().thanksArray = thanksArray;
            shop.GetComponent<ShopInformation>().character = gameObject;
            shop.GetComponent<ShopInformation>().updateAbout();
            shop.SetActive(true);
        }
    }
    public void openVetMenu() {
        vetMenu.character = gameObject;
        vetMenu.Open();
    }
    public void SelectDialogue() {
        if (shopKeeper) {
            closeSelection();
        }
        charDialogue();
    }
}
