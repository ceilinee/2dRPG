using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdoptionInformation : MonoBehaviour {
    public AdoptionRequest selectedAdoption;
    public AdoptionRequests adoptionRequests;
    public Text inbox;
    public Text message;
    public Text from;
    public Text dots;
    public Text back;
    public Text type;
    public Text legs;
    public Text tail;
    public Text star;
    public Text body;
    public Text breed;
    public Text eyes;
    public Text ears;
    public Text personality;
    public Image portrait;
    public Animals curAnimals;
    public Player player;
    public AnimalColors animalColors;
    public GameObject adoptionController;
    public GameObject adoptionView;
    public Characters charList;
    public GameObject aboutTab;
    public GameObject animalView;
    public GameObject buySellAnimal;
    public GameObject CanvasController;
    public GameObject confirmationModal;

    // Start is called before the first frame update
    void Start() {
        aboutTab.SetActive(false);
        updateList();
    }
    // public void deleteSelectedMail(){
    //   mailbox.deleteMessage(selectedMail);
    //   updateList();
    //   if(mailbox.mailbox.Length > 0){
    //     updateSelectedMail(mailbox.mailbox[0]);
    //   }
    //   else{
    //     aboutTab.SetActive(false);
    //   }
    // }
    public void adoptAnimal(Animal selectedAnimal) {
        confirmationModal.GetComponent<Confirmation>().initiateConfirmation(
  "Are you sure you want to adopt out " + selectedAnimal.animalName + " for " + selectedAnimal.cost * charList.characterDict[selectedAdoption.charId].multiplier + "$?",
  () => {
      buySellAnimal.GetComponent<BuySellAnimal>().adoptOutAnimal(selectedAnimal, charList.characterDict[selectedAdoption.charId]);
      deleteRequest();
      CanvasController.GetComponent<CanvasController>().openCanvasAgain();
  },
  () => { },
  () => { }
  );
    }
    public void deleteRequest() {
        adoptionRequests.deleteRequest(selectedAdoption);
        updateList();
        aboutTab.SetActive(false);
    }
    public void updateSelectedRequest(AdoptionRequest request) {
        aboutTab.SetActive(true);
        selectedAdoption = request;
        if (charList.characterDict[request.charId].portrait.Length > 0) {
            portrait.sprite = charList.characterDict[request.charId].portrait[0];
        }
        from.text = charList.characterDict[request.charId].name;
        dots.text = animalColors.colorDictionary[request.coloring.dots].ColorName;
        back.text = animalColors.colorDictionary[request.coloring.back].ColorName;
        legs.text = animalColors.colorDictionary[request.coloring.legs].ColorName;
        tail.text = animalColors.colorDictionary[request.coloring.tail].ColorName;
        star.text = animalColors.colorDictionary[request.coloring.star].ColorName;
        body.text = animalColors.colorDictionary[request.coloring.body].ColorName;
        breed.text = request.breed;
        type.text = request.type;
        if (type.text == "") {
            type.text = "Any";
        }
        if (breed.text == "") {
            breed.text = "Any";
        }
        eyes.text = animalColors.colorDictionary[request.coloring.eyes].ColorName;
        ears.text = animalColors.colorDictionary[request.coloring.ears].ColorName;
        personality.text = request.personality.personality;
        if (personality.text == "") {
            personality.text = "Any";
        }
        message.text = request.message;
        findAnimals(request);
    }
    public void findAnimals(AdoptionRequest request) {
        animalView.GetComponent<ListCreator>().Clear();
        animalView.GetComponent<ListCreator>().adopt = true;
        animalView.GetComponent<ListCreator>().adoptionInformation = gameObject;
        animalView.GetComponent<ListCreator>().MatchAnimals(request);
    }
    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            gameObject.SetActive(false);
            CanvasController.GetComponent<CanvasController>().closeCanvas();
        }
        if (gameObject.activeInHierarchy && Time.timeScale != 0) {
            Time.timeScale = 0;
        }
    }
    public void Clear() {
        adoptionView.GetComponent<AdoptionList>().Clear();
        animalView.GetComponent<ListCreator>().Clear();
    }
    public void updateList() {
        Clear();
        adoptionView.GetComponent<AdoptionList>().adoptionInformation = gameObject;
        adoptionView.GetComponent<AdoptionList>().PopulateList();
    }
}
