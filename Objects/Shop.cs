using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {
    public Inventory playerInventory;
    public Inventory vetShop;
    public ItemDictionary itemDictionary;
    public Inventory shop;
    public Animals shopBreedAnimals;
    public Animal[] animals;
    public Animals shopAnimals;

    public ItemList shopBuildings;
    public GameObject breedAnimal;


    private void Awake() {
        itemDictionary.updateItemDict();
    }

    void Start() {
        UpdateAllInventory();
        updateSpecialShopWithAllBreeds();
        UpdateShopBuildings(itemDictionary);
        // addToAnimalShop(animals);
        if (shopAnimals.animalDict.Count < 50) {
            int count = shopAnimals.animalDict.Count;
            for (int i = 0; i < 50 - count; i++) {
                breedAnimal.GetComponent<BreedScript>().RandomShopAnimal(paramAnimals: shopAnimals, Type.FISH);
            }
        }
    }

    public void UpdateAllInventory() {
        playerInventory.UpdateInventory(itemDictionary);
        shop.UpdateInventory(itemDictionary);
        if (vetShop) {
            vetShop.UpdateInventory(itemDictionary);
        }
    }
    public void UpdateShopBuildings(ItemDictionary itemDictionary) {
        shopBuildings.itemList.Clear();
        foreach (string itemId in shopBuildings.list) {
            shopBuildings.itemList.Add(itemDictionary.Get(itemId));
        }
    }

    public void updateShop() {
        if (shopAnimals.animalDict.Count < 10) {
            int count = shopAnimals.animalDict.Count;
            for (int i = 0; i < 10 - count; i++) {
                breedAnimal.GetComponent<BreedScript>().RandomShopAnimal(paramAnimals: shopAnimals);
            }
        }
    }

    public void updateSpecialShop() {
        breedAnimal.GetComponent<BreedScript>().clearSpecialShop();
        AnimalBreed.Breed curBreed = breedAnimal.GetComponent<BreedScript>().animalBreed.breedArray[Random.Range(0, breedAnimal.GetComponent<BreedScript>().animalBreed.breedArray.Length)];
        shopBreedAnimals.breedName = curBreed.breedName;
        for (int i = 0; i < 4; i++) {
            breedAnimal.GetComponent<BreedScript>().RandomBreedAnimal(curBreed: curBreed, paramAnimals: shopBreedAnimals);
        }
    }

    //to help verify breeds are properly entered
    public void updateSpecialShopWithAllBreeds() {
        breedAnimal.GetComponent<BreedScript>().clearSpecialShop();
        AnimalBreed animalBreed = breedAnimal.GetComponent<BreedScript>().animalBreed;
        for (int i = 0; i < animalBreed.breedArray.Length; i++) {
            AnimalBreed.Breed curBreed = animalBreed.breedArray[i];
            shopBreedAnimals.breedName = curBreed.breedName;
            for (int j = 0; j < 4; j++) {
                breedAnimal.GetComponent<BreedScript>().RandomBreedAnimal(curBreed: curBreed, paramAnimals: shopBreedAnimals);
            }
        }
    }

    public void addToAnimalShop(Animal[] newAnimals) {
        for (int i = 0; i < newAnimals.Length; i++) {
            shopAnimals.addExistingAnimal(newAnimals[i]);
        }
    }
}
