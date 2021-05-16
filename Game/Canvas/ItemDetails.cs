using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetails : MonoBehaviour {

    public Text itemName;
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
    public GameObject shop;
    public SceneInfos buildings;
    public Animals shopAnimals;
    public SceneInfo building;
    public GameObject buySellAnimal;

    public void BreedInfo() {
        if (selectedBreed.unlocked) {
            playerInformation.GetComponent<PlayerInformation>().CanvasController.GetComponent<CanvasController>().initiateNotification(selectedBreed.breedDescription, true);
        } else {
            playerInformation.GetComponent<PlayerInformation>().CanvasController.GetComponent<CanvasController>().initiateNotification("Sorry, you haven't unlocked this breed yet!", true);
        }
    }
    public void BuyBuilding() {
        bool buy = buySellAnimal.GetComponent<BuySellAnimal>().buyBuilding(building, buildings);
        if (buy) {
            shop.GetComponent<ShopInformation>().updateAbout();
            shop.GetComponent<ShopInformation>().showThanks();
        } else {
            shop.GetComponent<ShopInformation>().noMoney();
        }
    }
    public void SelectItem() {
        playerInformation.GetComponent<PlayerInformation>().selectItem(item);
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
            shop.GetComponent<ShopInformation>().updateAbout();
            shop.GetComponent<ShopInformation>().showThanks();
        } else {
            shop.GetComponent<ShopInformation>().noMoney();
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
}
