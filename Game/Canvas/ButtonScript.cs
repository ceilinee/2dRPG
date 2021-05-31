using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {
    public GameObject about;
    public GameObject offspring;
    public GameObject aboutTab;
    public GameObject breeds;
    public GameObject traitTab;
    public GameObject inventoryTab;
    public GameObject animalTab;
    public GameObject sellTab;
    public GameObject buyTab;
    public GameObject adoptionTab;
    public GameObject adoptionAnimalsTab;
    public GameObject buyAnimalsTab;
    public GameObject buySpecialAnimalsTab;
    public GameObject accountSelection;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    public void toggleAdoption() {
        adoptionTab.SetActive(true);
        adoptionAnimalsTab.SetActive(false);
    }
    public void toggleAdoptionAnimal() {
        adoptionAnimalsTab.SetActive(true);
        adoptionTab.SetActive(false);
    }
    public void toggleMainMenu() {
        Loader.Load(Loader.Scene.MainScene);
    }
    public void toggleAccountMenu() {
        accountSelection.SetActive(true);
        accountSelection.GetComponent<AccountListCreator>().updateList();
    }
    public void toggleBuyButton() {
        buyTab.SetActive(true);
        if (sellTab) {
            sellTab.SetActive(false);
        }
        if (buyAnimalsTab) {
            buyAnimalsTab.SetActive(false);

        }
        if (buySpecialAnimalsTab) {
            buySpecialAnimalsTab.SetActive(false);
        }
    }
    public void toggleBuyAnimalsButton() {
        buyAnimalsTab.SetActive(true);
        if (sellTab) {
            sellTab.SetActive(false);
        }
        if (buyTab) {
            buyTab.SetActive(false);

        }
        if (buySpecialAnimalsTab) {
            buySpecialAnimalsTab.SetActive(false);
        }
    }
    public void toggleBuySpecialAnimalsButton() {
        buySpecialAnimalsTab.SetActive(true);
        if (sellTab) {
            sellTab.SetActive(false);
        }
        if (buyTab) {
            buyTab.SetActive(false);

        }
        if (buyAnimalsTab) {
            buyAnimalsTab.SetActive(false);
        }
    }
    public void toggleSellButton() {
        sellTab.SetActive(true);
        if (buyTab) {
            buyTab.SetActive(false);

        }
        if (buyAnimalsTab) {
            buyAnimalsTab.SetActive(false);
        }
        if (buySpecialAnimalsTab) {
            buySpecialAnimalsTab.SetActive(false);
        }
    }
    public void toggleAboutButton() {
        about.SetActive(true);
        traitTab.SetActive(false);
        offspring.SetActive(false);
    }
    public void toggleOffspringButton() {
        offspring.SetActive(true);
        traitTab.SetActive(false);
        about.SetActive(false);
    }
    public void toggleTraitButton() {
        offspring.SetActive(false);
        traitTab.SetActive(true);
        about.SetActive(false);
    }
    public void toggleAboutTabButton() {
        aboutTab.SetActive(true);
        inventoryTab.SetActive(false);
        animalTab.SetActive(false);
        breeds.SetActive(false);
    }
    public void toggleInventoryTabButton() {
        inventoryTab.SetActive(true);
        aboutTab.SetActive(false);
        animalTab.SetActive(false);
        breeds.SetActive(false);
    }
    public void toggleAnimalTabButton() {
        animalTab.SetActive(true);
        inventoryTab.SetActive(false);
        aboutTab.SetActive(false);
        breeds.SetActive(false);
    }
    public void toggleBreedsTabButton() {
        animalTab.SetActive(false);
        inventoryTab.SetActive(false);
        aboutTab.SetActive(false);
        breeds.SetActive(true);
    }
}
