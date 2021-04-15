using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public Item[] items;
    public Inventory playerInventory;
    public ItemDictionary itemDictionary;
    public Inventory shop;
    public Animals shopBreedAnimals;
    public Animal[] animals;
    public Animals shopAnimals;
    public GameObject breedAnimal;

    // Start is called before the first frame update
    void Start()
    {
      itemDictionary.updateItemDict();
      playerInventory.UpdateInventory(itemDictionary);
      shop.UpdateInventory(itemDictionary);
      addToShop(items);
      // addToAnimalShop(animals);
      if(shopAnimals.animalDict.Count < 10){
        int count = shopAnimals.animalDict.Count;
        for(int i = 0; i< 10-count; i++){
          breedAnimal.GetComponent<BreedScript>().RandomShopAnimal();
        }
      }
    }

    public void updateShop(){
      if(shopAnimals.animalDict.Count < 10){
        int count = shopAnimals.animalDict.Count;
        for(int i = 0; i< 10-count; i++){
          breedAnimal.GetComponent<BreedScript>().RandomShopAnimal();
        }
      }
    }

    public void updateSpecialShop(){
      breedAnimal.GetComponent<BreedScript>().clearSpecialShop();
      AnimalBreed.Breed curBreed = breedAnimal.GetComponent<BreedScript>().animalBreed.breedArray[Random.Range(0, breedAnimal.GetComponent<BreedScript>().animalBreed.breedArray.Length)];
      shopBreedAnimals.breedName = curBreed.breedName;
      for(int i =0; i<4; i++){
        breedAnimal.GetComponent<BreedScript>().RandomBreedAnimal(curBreed);
      }
    }

    public void addToShop(Item[] newItems){
      for(int i = 0; i< newItems.Length; i++){
        shop.Additem(newItems[i]);
      }
    }

    public void addToAnimalShop(Animal[] newAnimals){
      for(int i = 0; i< newAnimals.Length; i++){
        shopAnimals.addExistingAnimal(newAnimals[i]);
      }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
