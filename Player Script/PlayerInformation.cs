using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInformation : MonoBehaviour
{
    public Text playerName;
    public Text money;
    public Text topbarMoney;
    public Text married;
    public GameObject friendListView;
    public GameObject inventoryListView;
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

    void Update(){
      if(Input.GetButtonDown("Cancel")){
        if(animalModal.activeInHierarchy){
          animalModal.SetActive(false);
          animalModal.GetComponent<AnimalInformation>().Clear();
        }
        else{
          Time.timeScale = 1;
          gameObject.SetActive(false);
          CanvasController.GetComponent<CanvasController>().closeCanvas();
        }
      }
      if(gameObject.activeInHierarchy && Time.timeScale != 0){
        Time.timeScale = 0;
      }
    }
    public void openAnimal(Animal animalTrait){
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
    public GameObject getAnimal(Animal animalTrait){
      List<GameObject> list = animalList.GetComponent<AnimalList>().list;
      foreach (GameObject animal in list) {
          if(animal.GetComponent<GenericAnimal>().animalTrait.id == animalTrait.id){
            return animal;
          }
      }
      return null;
    }
    public void Clear(){
        friendListView.GetComponent<CharacterListCreator>().Clear();
        animalListView.GetComponent<ListCreator>().Clear();
        inventoryListView.GetComponent<ListCreator>().Clear();
    }
    public void selectItem(Item item){
        if(playerInventory.currentItem == item){
          playerInventory.currentItem = null;
          playerInventory.currentItemId = 0;
          inventoryHold.GetComponent<PlayerInventory>().removeSprite();
        }
        else{
          playerInventory.currentItem = item;
          playerInventory.currentItemId = item.id;
          inventoryHold.GetComponent<PlayerInventory>().updateSprite();
        }
    }
    public void updateAbout(){
        playerName.text = player.playerName;
        money.text = topbarMoney.text;
        married.text = player.married.ToString();
        updateList();
    }
    public void updateList(){
        Clear();
        friendListView.GetComponent<CharacterListCreator>().GetCharacters();
        animalListView.GetComponent<ListCreator>().type = "all";
        animalListView.GetComponent<ListCreator>().GetAnimals();
        inventoryListView.GetComponent<ListCreator>().GetItems();
    }
}
