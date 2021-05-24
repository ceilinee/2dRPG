using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySellAnimal : MonoBehaviour {
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

    void Start() {
        playerMoneyText.text = "$" + playerMoney.initialValue.ToString();
    }
    public void redeemQuest(Quest quest) {
        playerMoney.initialValue += quest.reward;
        player.reputation += quest.reputationPoints;
        player.earnedMoney += quest.reward;
        playerMoneyText.text = "$" + playerMoney.initialValue.ToString();
    }
    public bool buyAnimal(Animal newAnimal, Animals newShop) {
        Debug.Log("buy");
        if (playerMoney.initialValue >= newAnimal.shopCost) {
            curAnimals.addExistingAnimal(newAnimal);
            newShop.removeExistingAnimal(newAnimal.id);
            playerMoney.initialValue -= newAnimal.shopCost;
            playerMoneyText.text = "$" + playerMoney.initialValue.ToString();
            return true;
        }
        return false;
    }

    public void sellAnimal(Animal newAnimal) {
        curAnimals.removeExistingAnimal(newAnimal.id);
        playerMoney.initialValue += newAnimal.cost;
        player.earnedMoney += newAnimal.cost;
        animalList.GetComponent<AnimalList>().removeAnimal(newAnimal);
        playerMoneyText.text = "$" + playerMoney.initialValue.ToString();
    }
    public void adoptOutAnimal(Animal newAnimal, Character character) {
        curAnimals.removeExistingAnimal(newAnimal.id);
        playerMoney.initialValue += newAnimal.cost * character.multiplier;
        player.earnedMoney += newAnimal.cost * character.multiplier;
        animalList.GetComponent<AnimalList>().removeAnimal(newAnimal);
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
    public bool buyBuilding(SceneInfo building, SceneInfos buildings) {
        if (playerMoney.initialValue >= building.cost) {
            playerMoney.initialValue -= building.cost;
            playerBuildings.AddBuilding(building.id);
            buildings.RemoveBuilding(building.id);
            topbarMoney.text = "$" + playerMoney.initialValue.ToString();
            buildingController.GetComponent<BuildingController>().updateBuildings();
            // itemAlert.GetComponent<ItemAlert>().startAlert(item);
            return true;
        }
        return false;
    }
    public void pickUpItem(Item item) {
        playerInventory.Additem(item);
        itemAlert.GetComponent<ItemAlert>().startAlert(item);
        // TODO: if this item spawned as a wild item, the player picked it up, and then placed it down, and then
        // picked it up, this function will run a 2nd time, but we don't want to add it to dailyCollected
        player.dailyCollected.Add(item.id);
    }
    public void sellItem(Item item) {
        playerMoney.initialValue += item.sellCost;
        player.earnedMoney += item.sellCost;
        playerInventory.Removeitem(item);
        if (playerInventory.currentItem == null) {
            target.Find("InventoryHold").GetComponent<PlayerInventory>().removeSprite();
        }
        topbarMoney.text = "$" + playerMoney.initialValue.ToString();
    }
}
