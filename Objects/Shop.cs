using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {
    public Item[] items;
    public Inventory playerInventory;
    public Inventory vetShop;
    public ItemDictionary itemDictionary;
    public Inventory shop;
    public Animals shopBreedAnimals;
    public Animal[] animals;
    public Animals shopAnimals;

    public ItemList shopBuildings;
    public GameObject breedAnimal;

    // Start is called before the first frame update
    void Start() {
        itemDictionary.updateItemDict();
        playerInventory.UpdateInventory(itemDictionary);
        shop.UpdateInventory(itemDictionary);
        if (vetShop) {
            vetShop.UpdateInventory(itemDictionary);
        }
        addToShop(items);
        updateSpecialShopWithAllBreeds();
        UpdateShopBuildings(itemDictionary);
        // addToAnimalShop(animals);
        if (shopAnimals.animalDict.Count < 10) {
            int count = shopAnimals.animalDict.Count;
            for (int i = 0; i < 10 - count; i++) {
                breedAnimal.GetComponent<BreedScript>().RandomShopAnimal(paramAnimals: shopAnimals);
            }
        }
    }

    public void UpdateShopBuildings(ItemDictionary itemDictionary) {
        foreach (int itemId in shopBuildings.list) {
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

    public void addToShop(Item[] newItems) {
        for (int i = 0; i < newItems.Length; i++) {
            shop.Additem(newItems[i]);
        }
    }

    public void addToAnimalShop(Animal[] newAnimals) {
        for (int i = 0; i < newAnimals.Length; i++) {
            shopAnimals.addExistingAnimal(newAnimals[i]);
        }
    }
    // Update is called once per frame
    void Update() {

    }
}
