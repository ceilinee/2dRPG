using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class BuySellAnimal : CustomMonoBehaviour {
    public Animals curAnimals;
    public ItemDictionary itemDictionary;
    public GameObject animalList;
    public Transform target;
    public Inventory playerInventory;
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

    [SerializeField] private VectorPointsList pondList;

    [SerializeField]
    private Signal currItemSoldSignal;
    public int totalAnimals;
    public int totalAnimalCapacity;

    private void Awake() {
        Assert.IsNotNull(pondList);
    }

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
        player.AddEarnedMoney(quest.reward);
        playerMoneyText.text = "$" + playerMoney.initialValue.ToString();
    }
    public int TotalAnimalCapacity() {
        int totalAnimalCapacity = 0;
        foreach (PlacedBuilding building in placedBuildings.GetBuildings(PlacedBuilding.Status.Done)) {
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
        // TODO: we also want to enforce a threshold on the number of fish you can purchase
        if (newAnimal.type != Type.FISH && !CanBuyAnimal()) {
            canvasController.initiateNotification("Sorry! Looks like your barns have a capacity of " + totalAnimalCapacity + ", and you already have " + totalAnimals + " animals . Try upgrading your barns or purchasing more!", true);
            return false;
        }
        if (playerMoney.initialValue >= newAnimal.shopCost) {
            if (newAnimal.type == Type.FISH) {
                newAnimal.home = pondList.GetDefaultPond().locationName;
                newAnimal.location = pondList.GetDefaultPond().value;
            }
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
    public float payForServicePercentage(float percentage) {
        float money = (float) System.Math.Floor(playerMoney.initialValue * percentage);
        playerMoney.initialValue -= money;
        playerMoneyText.text = "$" + playerMoney.initialValue.ToString();
        return money;
    }
    public void sellAnimal(Animal newAnimal) {
        curAnimals.removeExistingAnimal(newAnimal.id);
        playerMoney.initialValue += newAnimal.cost;
        player.AddEarnedMoney(newAnimal.cost);
        animalList.GetComponent<AnimalList>().removeAnimal(newAnimal.id);
        playerMoneyText.text = "$" + playerMoney.initialValue.ToString();
    }
    public void adoptOutAnimal(Animal newAnimal, Character character) {
        curAnimals.removeExistingAnimal(newAnimal.id);
        playerMoney.initialValue += newAnimal.cost * character.multiplier;
        player.AddEarnedMoney(newAnimal.cost * character.multiplier);
        animalList.GetComponent<AnimalList>().removeAnimal(newAnimal.id);
        playerMoneyText.text = "$" + playerMoney.initialValue.ToString();
        newAnimal.characterOwned = true;
        newAnimal.charId = character.id;
        charAnimals.addExistingAnimal(newAnimal);
        player.AddAdoption();
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
            // TODO: should the person be selling unlimited barns?
            // buildings.Remove(building.id);
            playerInventory.Additem(building);
            topbarMoney.text = "$" + playerMoney.initialValue.ToString();
            // itemAlert.GetComponent<ItemAlert>().startAlert(item);
            return true;
        }
        return false;
    }
    public void pickUpItem(string itemId) {
        Item item = itemDictionary.Get(itemId);
        pickUpItem(item);
    }
    public void pickUpItem(Item item) {
        playerInventory.Additem(item);
        itemAlert.GetComponent<ItemAlert>().startAlert(item);
        if (!item.pickedUpAtLeastOnce) {
            player.AddCollected(item.Id);
            item.pickedUpAtLeastOnce = true;
        }
    }
    public void sellItem(Item item) {
        playerMoney.initialValue += item.sellCost;
        player.AddEarnedMoney(item.sellCost);
        playerInventory.Removeitem(item);
        if (playerInventory.currentItem == null) {
            target.Find("InventoryHold").GetComponent<PlayerInventory>().removeSprite();
            currItemSoldSignal.Raise();
        }
        topbarMoney.text = "$" + playerMoney.initialValue.ToString();
    }
}
