using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

/// <summary>
/// This script controls the game object VetMenu
/// </summary>
public class VetInformation : CustomMonoBehaviour {
    public Text average;
    public Text sick;
    public Text friendship;
    public Text diversity;
    public Player player;
    public Animals curAnimals;
    public AnimalBreed animalBreed;
    public Text evaluation;
    public ListCreator breedList;
    public ListCreator curAnimalList;
    public ListCreator pharmacyList;
    private CanvasController canvasController;
    private Confirmation confirmation;
    private BuySellAnimal buySellAnimal;
    private Animal considerHealAnimal;
    public GameObject character;
    public BreedRegistration breedRegistration;

    void Start() {
        canvasController = centralController.centralDictionary["CanvasController"].GetComponent<CanvasController>();
        confirmation = centralController.centralDictionary["Confirmation"].GetComponent<Confirmation>();
        buySellAnimal = centralController.centralDictionary["AnimalBuySell"].GetComponent<BuySellAnimal>();
        breedRegistration = centralController.centralDictionary["BreedRegistration"].GetComponent<BreedRegistration>();
    }
    public void HealAnimal(Animal animal) {
        if (animal.health == 100) {
            canvasController.initiateNotification(animal.animalName + " is healthy as can be! You're doing a fantastic job.", true);
        } else {
            considerHealAnimal = animal;
            confirmation.initiateConfirmation(
                "Do you want me to heal " + animal.animalName + "? The treatments will be " + (100 - animal.health) + "$",
                (() => ConfirmHeal()),
                () => { },
                () => { },
                true
            );
        }
    }
    public void ConfirmHeal() {
        if (buySellAnimal.Heal(considerHealAnimal)) {
            if (considerHealAnimal.health > 10) {
                canvasController.initiateNotification("Great! I'll get to work.. Okay, " + considerHealAnimal.animalName + " is healthy as can be! Be sure to take good care of them in the future.", true);
            } else {
                canvasController.initiateNotification("Oh my, " + considerHealAnimal.animalName + " is really sick! This is an emergency... Okay, whew, I've healed them but please take better care of them in the future..", true);
            }
        }
    }
    public void MakeNewBreed(Animal animal) {
        breedRegistration.StartRegistration(animal.coloring);

    }
    void Update() {
        if (Input.GetButtonDown("Cancel") && !breedRegistration.gameObject.activeInHierarchy) {
            Time.timeScale = 1;
            gameObject.SetActive(false);
            canvasController.closeCanvas();
            character.GetComponent<GenericCharacter>().conversation = false;
        }
        if (gameObject.activeInHierarchy && Time.timeScale != 0) {
            Time.timeScale = 0;
        }
    }

    // Entrypoint of the script; does some setup and then makes the game object active
    public void Open() {
        Assert.IsFalse(gameObject.activeInHierarchy);
        gameObject.SetActive(true);
        updateAbout();
    }

    public void Clear() {
        breedList.GetComponent<ListCreator>().Clear();
        curAnimalList.GetComponent<ListCreator>().Clear();
        pharmacyList.GetComponent<ListCreator>().Clear();
    }
    public void updateAbout() {
        average.text = curAnimals.GetAverageHealth().ToString().Substring(0, 6) + "%";
        sick.text = curAnimals.GetSickAnimals().ToString() + " Animals";
        friendship.text = curAnimals.GetAverageFriendship().ToString().Substring(0, 6);
        diversity.text = curAnimals.GetBreeds() + " breeds across " + curAnimals.GetTypes() + " types ";
        evaluation.text = curAnimals.GetEvaluation();
        updateList();
    }
    public void updateList() {
        Clear();
        breedList.GetComponent<ListCreator>().type = "all";
        breedList.GetComponent<ListCreator>().GetNewBreedAnimals();
        breedList.GetComponent<ListCreator>().vetInformation = gameObject.GetComponent<VetInformation>();
        curAnimalList.GetComponent<ListCreator>().type = "all";
        curAnimalList.GetComponent<ListCreator>().GetAnimals();
        curAnimalList.GetComponent<ListCreator>().vetInformation = gameObject.GetComponent<VetInformation>();
        pharmacyList.GetComponent<ListCreator>().isShop = true;
        pharmacyList.GetComponent<ListCreator>().shopInformation = gameObject;
        pharmacyList.GetComponent<ListCreator>().GetShopItems();
    }
}
