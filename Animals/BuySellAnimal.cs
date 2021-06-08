using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySellAnimal : CustomMonoBehaviour {
    public Animals curAnimals;
    public GameObject animalList;
    public Transform target;
    public Inventory playerInventory;
    public SceneInfos playerBuildings;
    public Animals charAnimals;
    public FloatValue playerMoney;
    public Player player;
    public Text playerMoneyText;
    public Text topbarMoney;
    public Inventory shop;
    public GameObject buildingController;
    // public SceneInfos buildings;
    public Animals shopAnimals;
    public GameObject itemAlert;
    public SceneInfos allBuildings;
    public PlacedBuildings placedBuildings;
    public CanvasController canvasController;

    [SerializeField]
    private Signal currItemSoldSignal;
    private int totalAnimals;
    private int totalAnimalCapacity;

    void Start() {
        playerMoneyText.text = "$" + playerMoney.initialValue.ToString();
        canvasController = centralController.centralDictionary["CanvasController"].GetComponent<CanvasController>();
    }
    public bool Heal(Animal animal) {
        if (playerMoney.initialValue >= 100 - animal.health) {
            playerMoney.initialValue -= (100 - animal.health);
            animal.health = 100;
            playerMoneyText.text = "$" + playerMoney.initialValue.ToString();
            return true;
        }
        return false;
    }
    public bool Checkout(Inventory inventory) {
        if (playerMoney.initialValue >= inventory.TotalCost(buy: true)) {
            playerInventory.AddInventory(inventory);
            playerMoney.initialValue -= (float) inventory.TotalCost(buy: true);
            playerMoneyText.text = "$" + playerMoney.initialValue.ToString();
            return true;
        }
        return false;
    }
    public void redeemQuest(Quest quest) {
        playerMoney.initialValue += quest.reward;
        player.reputation += quest.reputationPoints;
        player.earnedMoney += quest.reward;
        playerMoneyText.text = "$" + playerMoney.initialValue.ToString();
    }
    public int TotalAnimalCapacity() {
        int totalAnimalCapacity = 0;
        foreach (PlacedBuilding building in placedBuildings.buildings) {
            if (!building.completed) {
                continue;
            }
            SceneInfo sceneInfo = allBuildings.sceneDict[building.sceneInfoId];
            totalAnimalCapacity += sceneInfo.animalMaxSize;
        }
        return totalAnimalCapacity;
    }
    public bool CanBuyAnimal() {
        totalAnimals = curAnimals.TotalAnimals();
        totalAnimalCapacity = TotalAnimalCapacity();
        Debug.Log("totalAnimals: " + totalAnimals + ", totalAnimalCapacity: " + totalAnimalCapacity);
        return totalAnimals < totalAnimalCapacity;
    }
    public bool buyAnimal(Animal newAnimal, Animals newShop) {
        Debug.Log("buy");
        if (!CanBuyAnimal()) {
            canvasController.initiateNotification("Sorry! Looks like your barns have a capacity of " + totalAnimalCapacity + ", and you already have " + totalAnimals + " animals . Try upgrading your barns or purchasing more!", true);
            return false;
        }
        if (playerMoney.initialValue >= newAnimal.shopCost) {
            curAnimals.addExistingAnimal(newAnimal);
            newShop.removeExistingAnimal(newAnimal.id);
            playerMoney.initialValue -= newAnimal.shopCost;
            playerMoneyText.text = "$" + playerMoney.initialValue.ToString();
            return true;
        }
        canvasController.initiateNotification("Sorry! Looks like you don't have enough.", true);
        return false;
    }
    public void SellItems(Inventory shoppingCart) {
        foreach (KeyValuePair<Item, double> kvp in shoppingCart.items) {
            for (int i = 0; i < kvp.Value; i++) {
                sellItem(kvp.Key);
            }
        }
    }
    public bool payForService(float price) {
        if (playerMoney.initialValue >= price) {
            playerMoney.initialValue -= price;
            playerMoneyText.text = "$" + playerMoney.initialValue.ToString();
            return true;
        }
        return false;
    }
    public void sellAnimal(Animal newAnimal) {
        curAnimals.removeExistingAnimal(newAnimal.id);
        playerMoney.initialValue += newAnimal.cost;
        player.earnedMoney += newAnimal.cost;
        animalList.GetComponent<AnimalList>().removeAnimal(newAnimal.id);
        playerMoneyText.text = "$" + playerMoney.initialValue.ToString();
    }
    public void adoptOutAnimal(Animal newAnimal, Character character) {
        curAnimals.removeExistingAnimal(newAnimal.id);
        playerMoney.initialValue += newAnimal.cost * character.multiplier;
        player.earnedMoney += newAnimal.cost * character.multiplier;
        animalList.GetComponent<AnimalList>().removeAnimal(newAnimal.id);
        playerMoneyText.text = "$" + playerMoney.initialValue.ToString();
        newAnimal.characterOwned = true;
        newAnimal.charId = character.id;
        charAnimals.addExistingAnimal(newAnimal);
        player.dailyAdoption += 1;
    }
    public bool buyItem(Item item) {
        if (playerMoney.initialValue >= item.cost) {
            playerMoney.initialValue -= item.cost;
            playerInventory.Additem(item);
            topbarMoney.text = "$" + playerMoney.initialValue.ToString();
            // itemAlert.GetComponent<ItemAlert>().startAlert(item);
            return true;
        }
        return false;
    }
    public bool buyBuilding(BuildingItem building, ItemList buildings) {
        if (playerMoney.initialValue >= building.cost) {
            playerMoney.initialValue -= building.cost;
            playerBuildings.AddBuilding(building.sceneInfo.id);
            // TODO: should the person be selling unlimited barns?
            // buildings.Remove(building.id);
            playerInventory.Additem(building);
            topbarMoney.text = "$" + playerMoney.initialValue.ToString();
            // itemAlert.GetComponent<ItemAlert>().startAlert(item);
            return true;
        }
        return false;
    }
    public void pickUpItem(Item item) {
        playerInventory.Additem(item);
        itemAlert.GetComponent<ItemAlert>().startAlert(item);
        if (!item.pickedUpAtLeastOnce) {
            player.dailyCollected.Add(item.id);
            item.pickedUpAtLeastOnce = true;
        }
    }
    public void sellItem(Item item) {
        playerMoney.initialValue += item.sellCost;
        player.earnedMoney += item.sellCost;
        playerInventory.Removeitem(item);
        if (playerInventory.currentItem == null) {
            target.Find("InventoryHold").GetComponent<PlayerInventory>().removeSprite();
            currItemSoldSignal.Raise();
        }
        topbarMoney.text = "$" + playerMoney.initialValue.ToString();
    }
}
