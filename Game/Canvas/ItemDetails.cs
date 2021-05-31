using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetails : CustomMonoBehaviour {

    public Text itemName;
    public GameObject frame;
    public Text quantity;
    public GameObject question;
    public Text price;
    public AnimalBreed.Breed selectedBreed;
    public Image itemImage;
    public GameObject LlamaImage;
    public Item item;
    public Animal selectedAnimal;
    public GameObject animalInformation;
    public GameObject playerInformation;
    public GameObject adoptionInformation;
    public bool vet;
    private VetInformation vetInformation;
    public GameObject shop;
    public ItemList buildingItems;
    public Animals shopAnimals;
    public BuildingItem buildingItem;
    public GameObject buySellAnimal;
    public CanvasController CanvasController;
    void Start() {
        if (vet) {
            vetInformation = centralController.centralDictionary["VetMenu"].GetComponent<VetInformation>();
        }
        buySellAnimal = centralController.centralDictionary["AnimalBuySell"];
        CanvasController = centralController.centralDictionary["CanvasController"].GetComponent<CanvasController>();
    }
    public void HealAnimal() {
        vetInformation.HealAnimal(selectedAnimal);
    }
    public void MakeNewBreed() {
        vetInformation.MakeNewBreed(selectedAnimal);
    }
    public void BreedInfo() {
        if (selectedBreed.unlocked) {
            playerInformation.GetComponent<PlayerInformation>().CanvasController.GetComponent<CanvasController>().initiateNotification(selectedBreed.breedDescription, true);
        } else {
            playerInformation.GetComponent<PlayerInformation>().CanvasController.GetComponent<CanvasController>().initiateNotification("Sorry, you haven't unlocked this breed yet!", true);
        }
    }
    public void BuyBuilding() {
        bool buy = buySellAnimal.GetComponent<BuySellAnimal>().buyBuilding(buildingItem, buildingItems);
        if (buy) {
            shop.GetComponent<ShopInformation>().updateAbout();
            // TODO: refactor this! Call a function in ShopInformation
            shop.GetComponent<ShopInformation>().thanks.text = "Thanks for purchasing a building! You can find the building in"
                + " your inventory. Feel free to place the building foundation anywhere, and I'll help you build it within the next 2 days!";
        } else {
            shop.GetComponent<ShopInformation>().noMoney();
        }
    }
    public void SelectItem() {
        playerInformation.GetComponent<PlayerInformation>().selectItem(item, gameObject);
    }
    public void SetSelected() {
        if (frame) {
            frame.SetActive(true);
        }
    }
    public void SetUnselected() {
        if (frame) {
            frame.SetActive(false);
        }
    }
    public void AdoptOut() {
        adoptionInformation.GetComponent<AdoptionInformation>().adoptAnimal(selectedAnimal);
    }
    public void Breed() {
        animalInformation.GetComponent<AnimalInformation>().Breed(selectedAnimal);
    }
    public void OpenAnimal() {
        playerInformation.GetComponent<PlayerInformation>().openAnimal(selectedAnimal);
    }
    public void BuyItem() {
        bool buy = buySellAnimal.GetComponent<BuySellAnimal>().buyItem(item);
        if (buy) {
            if (vet) {
                vetInformation.updateAbout();
            } else {
                shop.GetComponent<ShopInformation>().updateAbout();
                shop.GetComponent<ShopInformation>().showThanks();
            }
        } else {
            if (vet) {
                vetInformation.updateAbout();
            } else {
                shop.GetComponent<ShopInformation>().noMoney();
            }
        }
    }
    public void BuyAnimal() {
        bool buy = buySellAnimal.GetComponent<BuySellAnimal>().buyAnimal(selectedAnimal, shopAnimals);
        if (buy) {
            shop.GetComponent<ShopInformation>().updateAbout();
            shop.GetComponent<ShopInformation>().showThanksAnimal();
        } else {
            shop.GetComponent<ShopInformation>().noMoney();
        }
    }
    public void SellItem() {
        buySellAnimal.GetComponent<BuySellAnimal>().sellItem(item);
        shop.GetComponent<ShopInformation>().updateAbout();
        shop.GetComponent<ShopInformation>().showThanks();
    }

    public void setButtonColorSelected() {
        /*
        ColorBlock cb = GetComponent<Button>().colors;
        cb.normalColor = cb.selectedColor;
        GetComponent<Button>().colors = cb;
        */
        GetComponent<Button>().Select();
    }
}
