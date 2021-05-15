using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInformation : MonoBehaviour {
    public GameObject sellListView;
    public GameObject buyListView;
    public Player player;
    public Text thanks;
    public bool building;
    public Image profileImage;
    public Sprite profileSprite;
    public SceneInfos buildings;
    public Inventory shopInventory;
    public string[] thanksArray;
    public FloatValue playerMoney;
    public Inventory playerInventory;
    public GameObject CanvasController;
    public GameObject character;
    public GameObject shopController;
    public Text specialBreedText;
    public GameObject animalListView;
    public GameObject specialAnimalListView;
    public GameObject spawnAnimal;
    void Start() {
        thanks.text = "Welcome to my shop! I hope you're having a wonderful day, " + player.playerName + "!";
    }
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            Time.timeScale = 1;
            gameObject.SetActive(false);
            CanvasController.GetComponent<CanvasController>().closeCanvas();
            Debug.Log("close");
            character.GetComponent<GenericCharacter>().conversation = false;
        }
        if (gameObject.activeInHierarchy && Time.timeScale != 0) {
            Time.timeScale = 0;
        }
    }
    public void Clear() {
        if (animalListView) {
            animalListView.GetComponent<ListCreator>().Clear();
        }
        if (buyListView) {
            buyListView.GetComponent<ListCreator>().Clear();
        }
        if (sellListView) {
            sellListView.GetComponent<ListCreator>().Clear();
        }
        // thanks.text = "Nice to see you again, " + player.playerName + "!";
        if (specialAnimalListView) {
            specialAnimalListView.GetComponent<ListCreator>().Clear();
        }
    }
    public void updateAbout() {
        updateList();
    }
    public void showThanks() {
        thanks.text = thanksArray[1];
        // "Wow, thanks for the business!";
    }
    public void showThanksAnimal() {
        thanks.text = thanksArray[2];
        // thanks.text = "I'm so glad this baby found a great home with you! I'll prepare their stuff and bring them over tonight. Please take care of them!";
    }
    public void noMoney() {
        thanks.text = thanksArray[3];
        // thanks.text = "Sorry, " + player.playerName +  " but it looks like you don't have enough on you right now..";
    }
    public void updateList() {
        Clear();
        if (profileSprite) {
            profileImage.sprite = profileSprite;
        }
        if (sellListView) {
            sellListView.GetComponent<ListCreator>().isShop = true;
            sellListView.GetComponent<ListCreator>().shopInformation = gameObject;
            sellListView.GetComponent<ListCreator>().GetItems();
            sellListView.GetComponent<ListCreator>().isSell = true;
        }
        if (buyListView) {
            buyListView.GetComponent<ListCreator>().isShop = true;
            buyListView.GetComponent<ListCreator>().shopInformation = gameObject;
            if (shopInventory) {
                buyListView.GetComponent<ListCreator>().shop = shopInventory;
            }
            if (building && buildings) {
                buyListView.GetComponent<ListCreator>().building = true;
                buyListView.GetComponent<ListCreator>().buildings = buildings;
                buyListView.GetComponent<ListCreator>().GetBuildingItems();
            }
            if (!building) {
                buyListView.GetComponent<ListCreator>().GetShopItems();
            }
        }
        if (animalListView) {
            animalListView.GetComponent<ListCreator>().isShop = true;
            animalListView.GetComponent<ListCreator>().type = "all";
            animalListView.GetComponent<ListCreator>().shopInformation = gameObject;
            animalListView.GetComponent<ListCreator>().GetAnimals();
        }
        if (specialAnimalListView) {
            if (specialAnimalListView.GetComponent<ListCreator>().curAnimals.animalDict.Count != 0) {
                specialBreedText.text = "I have a litter of " + shopController.GetComponent<Shop>().shopBreedAnimals.breedName + " babies available!";
            } else {
                specialBreedText.text = "I sometimes have litters of certain breeds available, check back!";
            }
            specialAnimalListView.GetComponent<ListCreator>().isShop = true;
            specialAnimalListView.GetComponent<ListCreator>().type = "all";
            specialAnimalListView.GetComponent<ListCreator>().shopInformation = gameObject;
            specialAnimalListView.GetComponent<ListCreator>().GetAnimals();
        }
    }
}
