using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class ListCreator : CustomMonoBehaviour {

    [SerializeField]
    private Transform SpawnPoint = null;

    [SerializeField]
    private GameObject item = null;

    [SerializeField]
    private RectTransform content = null;

    [SerializeField]
    private int numberOfItems = 3;

    public string[] itemNames = null;
    public Sprite[] itemImages = null;
    public GameObject animalInformation;
    public GameObject playerInformation;
    public GameObject adoptionInformation;
    public GameObject shopInformation;
    public VetInformation vetInformation;
    public GameObject spawnAnimal;
    public AnimalColors animalColors;
    public Animals curAnimals;
    public SceneInfos allBuildings;
    public ItemList buildingItems;
    public string type;
    public bool building = false;
    public bool isItem;
    public bool isShop;
    public bool isBreed;
    public bool isSell;
    public string gender;
    public AnimalBreed animalBreeds;
    public Inventory inventory;
    public Inventory shop;
    public bool adopt;
    public Item[] selectedItems;
    public Animal[] selectedAnimals;
    public BuildingItem[] selectedBuildingItems;
    public AnimalBreed.Breed[] selectedBreeds;
    public GameObject buySellAnimal;

    // Use this for initialization
    void Start() {
    }
    public void Clear() {
        foreach (Transform child in SpawnPoint.transform) {
            // GameObject.Destroy(child.gameObject);
            child.gameObject.SetActive(false);
        }
    }
    public void GetNewBreedAnimals() {
        List<Animal> selectedAnimalsList = curAnimals.GetNewBreedAnimals();
        selectedAnimals = selectedAnimalsList.ToArray();
        numberOfItems = selectedAnimals.Length;
        PopulateList();
    }
    public void GetAnimals() {
        List<Animal> selectedAnimalsList = new List<Animal>();
        foreach (KeyValuePair<int, Animal> kvp in curAnimals.animalDict) {
            if (type == "all") {
                if (kvp.Value.age >= 0 || isShop) {
                    selectedAnimalsList.Add(kvp.Value);
                }
            } else if (kvp.Value.type == type && kvp.Value.gender != gender && kvp.Value.pregnant == false && kvp.Value.age >= 5) {
                selectedAnimalsList.Add(kvp.Value);
            }
        }
        selectedAnimals = selectedAnimalsList.ToArray();
        numberOfItems = selectedAnimals.Length;
        PopulateList();
    }
    public void MatchAnimals(AdoptionRequest request) {
        List<Animal> selectedAnimalsList = new List<Animal>();
        foreach (KeyValuePair<int, Animal> kvp in curAnimals.animalDict) {
            if (request.type == "" || kvp.Value.type == request.type) {
                if (request.personality.personality == "" || request.personality.personality == kvp.Value.personality.personality) {
                    if (animalBreeds.matchRequest(kvp.Value.coloring, request.coloring)) {
                        selectedAnimalsList.Add(kvp.Value);
                    }
                }
            }
        }
        selectedAnimals = selectedAnimalsList.ToArray();
        numberOfItems = selectedAnimals.Length;
        PopulateList();
    }
    public void GetItems() {
        List<Item> selectedItemList = new List<Item>();
        foreach (KeyValuePair<Item, double> kvp in inventory.items) {
            selectedItemList.Add(kvp.Key);
        }
        selectedItems = selectedItemList.ToArray();
        numberOfItems = selectedItems.Length;
        PopulateList();
    }
    public void GetShopItems() {
        List<Item> selectedItemList = new List<Item>();
        foreach (KeyValuePair<Item, double> kvp in shop.items) {
            selectedItemList.Add(kvp.Key);
        }
        selectedItems = selectedItemList.ToArray();
        numberOfItems = selectedItems.Length;
        PopulateList();
    }
    public void GetBreedItems() {
        List<AnimalBreed.Breed> selectedAnimalsList = new List<AnimalBreed.Breed>();
        foreach (KeyValuePair<string, AnimalBreed.Breed> kvp in animalBreeds.breedDictionary) {
            selectedAnimalsList.Add(kvp.Value);
        }
        selectedBreeds = selectedAnimalsList.ToArray();
        numberOfItems = selectedBreeds.Length;
        PopulateList();
    }
    public void GetBuildingItems() {
        List<BuildingItem> buildingList = new List<BuildingItem>();
        foreach (Item item in buildingItems.list) {
            Assert.IsTrue(item is BuildingItem);
            BuildingItem buildingItem = (BuildingItem) item;
            Assert.IsTrue(allBuildings.sceneDict.ContainsKey(buildingItem.sceneInfo.id));
            buildingList.Add(buildingItem);
        }
        selectedBuildingItems = buildingList.ToArray();
        numberOfItems = selectedBuildingItems.Length;
        PopulateList();
    }
    public void PopulateList() {
        //setContent Holder Height;
        int numberOfItemsInRow = 5;

        content.sizeDelta = new Vector2(0, System.Math.Max(numberOfItems / numberOfItemsInRow, 3) * 80 + 90);
        for (int i = 0; i < numberOfItems; i++) {
            for (int j = 0; j < numberOfItemsInRow && i + j < numberOfItems; j++) {
                // 60 width of item
                float spawnY = (int) System.Math.Floor((double) i / numberOfItemsInRow) * 80;
                //newSpawn Position
                Vector3 pos = new Vector3(80 * j, -spawnY, 0);
                //instantiate item
                GameObject SpawnedItem = Instantiate(item, pos, SpawnPoint.rotation);
                //setParent
                SpawnedItem.transform.SetParent(SpawnPoint, false);
                //get ItemDetails Component
                ItemDetails itemDetails = SpawnedItem.GetComponent<ItemDetails>();
                if (isBreed) {
                    AnimalBreed.Breed breed = selectedBreeds[i + j];
                    itemDetails.itemName.text = breed.breedName;
                    itemDetails.selectedBreed = selectedBreeds[i + j];
                    itemDetails.playerInformation = playerInformation;
                    if (breed.unlocked) {
                        itemDetails.price.text = "Unlocked";
                        itemDetails.question.SetActive(false);
                        Animal tempAnimal = new Animal();
                        tempAnimal.coloring = breed.exampleColoring;
                        tempAnimal.animalColors = animalColors;
                        tempAnimal.colorAnimal(itemDetails.LlamaImage);
                    } else {
                        itemDetails.price.text = "Locked";
                        itemDetails.LlamaImage.SetActive(false);
                    }
                }
                //set name
                else if (building) {
                    var sceneInfo = selectedBuildingItems[i + j].sceneInfo;
                    itemDetails.itemName.text = sceneInfo.sceneName;
                    itemDetails.itemImage.sprite = sceneInfo.image;
                    itemDetails.price.text = "$" + sceneInfo.cost.ToString();
                    itemDetails.buySellAnimal = buySellAnimal;
                    itemDetails.shop = shopInformation;
                    itemDetails.buildingItems = buildingItems;
                    itemDetails.buildingItem = selectedBuildingItems[i + j];
                } else if (!isItem) {
                    selectedAnimals[i + j].animalColors = animalColors;
                    if (isShop) {
                        itemDetails.itemName.text = selectedAnimals[i + j].animalName;
                        itemDetails.price.text = "$" + selectedAnimals[i + j].shopCost.ToString();
                        itemDetails.buySellAnimal = buySellAnimal;
                        itemDetails.shop = shopInformation;
                        itemDetails.shopAnimals = curAnimals;
                    } else {
                        itemDetails.itemName.text = selectedAnimals[i + j].animalName;
                        itemDetails.price.text = "$" + selectedAnimals[i + j].cost.ToString();
                    }
                    if (adopt) {
                        itemDetails.adoptionInformation = adoptionInformation;
                        itemDetails.price.text = "";
                    }
                    if (selectedAnimals[i + j].gender == "Female") {
                        itemDetails.itemName.color = new Color(255f / 255f, 129f / 255f, 130f / 255f, 1f);
                    } else {
                        itemDetails.itemName.color = new Color(0f / 255f, 59f / 255f, 138f / 255f, 1f);
                    }
                    if (selectedAnimals[i + j].pregnant == true) {
                        itemDetails.itemName.text = itemDetails.itemName.text + "❤";
                    }
                    itemDetails.selectedAnimal = selectedAnimals[i + j];
                    itemDetails.animalInformation = animalInformation;
                    //set image
                    spawnAnimal.GetComponent<SpawnAnimal>().setAnimalImage(itemDetails.LlamaImage, selectedAnimals[i + j]);
                    selectedAnimals[i + j].animalColors = animalColors;
                    selectedAnimals[i + j].colorAnimal(itemDetails.LlamaImage);
                    if (playerInformation.activeInHierarchy) {
                        itemDetails.playerInformation = playerInformation;
                    }
                } else if (isItem && isShop) {
                    if (isSell) {
                        itemDetails.itemName.text = selectedItems[i + j].itemName;
                        itemDetails.price.text = "$" + selectedItems[i + j].sellCost.ToString();
                        itemDetails.quantity.text = 'x' + inventory.items[selectedItems[i + j]].ToString();
                    } else {
                        itemDetails.itemName.text = selectedItems[i + j].itemName;
                        itemDetails.price.text = "$" + selectedItems[i + j].cost.ToString();
                    }
                    itemDetails.itemImage.sprite = selectedItems[i + j].ItemSprite;
                    itemDetails.animalInformation = animalInformation;
                    itemDetails.buySellAnimal = buySellAnimal;
                    itemDetails.shop = shopInformation;
                    itemDetails.shopAnimals = curAnimals;
                    itemDetails.item = selectedItems[i + j];
                    if (playerInformation.activeInHierarchy) {
                        itemDetails.playerInformation = playerInformation;
                    }
                } else if (isItem) {
                    if (selectedItems[i + j] == inventory.currentItem) {
                        itemDetails.SetSelected();
                        if (playerInformation) {
                            playerInformation.GetComponent<PlayerInformation>().currentlyHeldObject = itemDetails.gameObject;
                        }
                    }
                    itemDetails.itemName.text = selectedItems[i + j].itemName;
                    itemDetails.price.gameObject.SetActive(false);
                    itemDetails.quantity.text = 'x' + inventory.items[selectedItems[i + j]].ToString();
                    itemDetails.itemImage.sprite = selectedItems[i + j].ItemSprite;
                    itemDetails.animalInformation = animalInformation;
                    itemDetails.item = selectedItems[i + j];
                    if (playerInformation.activeInHierarchy) {
                        itemDetails.playerInformation = playerInformation;
                    }
                }
            }
            i = i + numberOfItemsInRow - 1;
        }
    }
}
