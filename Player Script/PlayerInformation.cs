using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

/// <summary>
/// This script controls the game object PlayerMenu
/// </summary>
public class PlayerInformation : MonoBehaviour {
    public Text playerName;
    public Text money;
    public Text topbarMoney;
    public Text married;
    public GameObject friendListView;
    public GameObject inventoryListView;
    public GameObject breedListView;
    public GameObject animalList;
    public GameObject CanvasController;
    public GameObject inventoryHold;
    public Player player;
    public FloatValue playerMoney;
    public Inventory playerInventory;
    public GameObject characters;
    public GameObject animalListView;
    public GameObject animalModal;
    public GameObject spawnAnimal;
    public GameObject currentlyHeldObject;
    public PlayerDesignComplex playerDesign;

    [SerializeField]
    private PlacementManager placementManager;

    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            if (animalModal.activeInHierarchy) {
                animalModal.GetComponent<AnimalInformation>().CloseIfPlayerMenuNotOpen();
            } else {
                Time.timeScale = 1;
                gameObject.SetActive(false);
                CanvasController.GetComponent<CanvasController>().closeCanvas();
            }
            if (playerInventory.currentItem != null) {
                placementManager.BeginPlacement(playerInventory.currentItem);
            }
        }
        if (gameObject.activeInHierarchy && Time.timeScale != 0) {
            Time.timeScale = 0;
        }
    }

    // Entrypoint of the script; does some setup and then makes the game object active
    public void Open() {
        Assert.IsFalse(gameObject.activeInHierarchy);
        if (playerInventory.currentItem != null) {
            playerDesign.SetHold(playerInventory.currentItem);
        } else if (playerDesign.CurrentlyHolding()) {
            // Player might have been holding something, which was gifted away or sold so
            // we don't want them holding it anymore
            playerDesign.UnsetHold();
        }
        gameObject.SetActive(true);
        updateAbout();
    }

    public void openAnimal(Animal animalTrait) {
        GameObject curGameObject = getAnimal(animalTrait);
        // Debug.Log(curGameObject.GetComponent<GenericAnimal>().animalTrait.animalName);
        RectTransform trans = animalModal.GetComponent<RectTransform>();
        GameObject profile = trans.Find("Profile").gameObject;
        RectTransform profileTrans = profile.GetComponent<RectTransform>();
        spawnAnimal.GetComponent<SpawnAnimal>().setAnimalImage(profileTrans.Find("ProfileImage").gameObject, animalTrait);
        animalTrait.colorAnimal(profileTrans.Find("ProfileImage").gameObject);
        animalModal.GetComponent<AnimalInformation>().updateAbout(animalTrait, curGameObject);
        animalModal.SetActive(true);
    }
    public GameObject getAnimal(Animal animalTrait) {
        List<GameObject> list = animalList.GetComponent<AnimalList>().list;
        foreach (GameObject animal in list) {
            if (animal.GetComponent<GenericAnimal>().animalTrait.id == animalTrait.id) {
                return animal;
            }
        }
        return null;
    }
    public void Clear() {
        friendListView.GetComponent<CharacterListCreator>().Clear();
        animalListView.GetComponent<ListCreator>().Clear();
        inventoryListView.GetComponent<ListCreator>().Clear();
        breedListView.GetComponent<ListCreator>().Clear();
    }

    public void DeselectCurrentItem() {
        playerInventory.UnsetCurrentItem();
        inventoryHold.GetComponent<PlayerInventory>().removeSprite();
        playerDesign.UnsetHold();
        if (currentlyHeldObject != null) {
            currentlyHeldObject.GetComponent<ItemDetails>().SetUnselected();
            currentlyHeldObject = null;
        }
    }

    public void selectItem(Item item, GameObject itemObject) {
        if (playerInventory.currentItem == item) {
            DeselectCurrentItem();
            placementManager.EndPlacement();
        } else {
            playerInventory.currentItem = item;
            playerInventory.currentItemId = item.Id;
            if (currentlyHeldObject != null) {
                currentlyHeldObject.GetComponent<ItemDetails>().SetUnselected();
            }
            currentlyHeldObject = itemObject;
            itemObject.GetComponent<ItemDetails>().SetSelected();
            inventoryHold.GetComponent<PlayerInventory>().updateSprite();
            playerDesign.SetHold(item);
        }
        updateList();
    }
    public void updateAbout() {
        playerName.text = player.playerName;
        money.text = topbarMoney.text;
        married.text = player.married.ToString();
        updateList();
    }
    public void updateList() {
        Clear();
        friendListView.GetComponent<CharacterListCreator>().GetCharacters();
        animalListView.GetComponent<ListCreator>().type = "all";
        animalListView.GetComponent<ListCreator>().GetAnimals();
        breedListView.GetComponent<ListCreator>().isBreed = true;
        breedListView.GetComponent<ListCreator>().GetBreedItems();
        inventoryListView.GetComponent<ListCreator>().GetItems();
    }

    public void RemoveCurrentItemFromInventory() {
        var item = playerInventory.currentItem;
        playerInventory.Removeitem(item);
        if (playerInventory.CountOf(item) == 0) {
            DeselectCurrentItem();
        }
    }

    public double CountOfItemInInventory(Item item) {
        return playerInventory.CountOf(item);
    }
}
