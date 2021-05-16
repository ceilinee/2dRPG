using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ListCreator : MonoBehaviour {

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
    public GameObject spawnAnimal;
    public AnimalColors animalColors;
    public Animals curAnimals;
    public SceneInfos allBuildings;
    public SceneInfos buildings;
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
    public SceneInfo[] selectedBuildings;
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
                // Debug.Log(kvp.Value.type);
                if (request.personality.personality == "" || request.personality.personality == kvp.Value.personality.personality) {
                    // Debug.Log(kvp.Value.personality.personality);
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
        Debug.Log("add");
        List<AnimalBreed.Breed> selectedAnimalsList = new List<AnimalBreed.Breed>();
        foreach (KeyValuePair<string, AnimalBreed.Breed> kvp in animalBreeds.breedDictionary) {
            selectedAnimalsList.Add(kvp.Value);
        }
        selectedBreeds = selectedAnimalsList.ToArray();
        numberOfItems = selectedBreeds.Length;
        PopulateList();
    }
    public void GetBuildingItems() {
        List<SceneInfo> buildingList = new List<SceneInfo>();
        foreach (int id in buildings.sceneArray) {
            if (allBuildings.sceneDict.ContainsKey(id)) {
                buildingList.Add(allBuildings.sceneDict[id]);
            }
        }
        selectedBuildings = buildingList.ToArray();
        numberOfItems = selectedBuildings.Length;
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
                    itemDetails.itemName.text = selectedBuildings[i + j].sceneName;
                    itemDetails.itemImage.sprite = selectedBuildings[i + j].image;
                    itemDetails.price.text = "$" + selectedBuildings[i + j].cost.ToString();
                    itemDetails.buySellAnimal = buySellAnimal;
                    itemDetails.shop = shopInformation;
                    itemDetails.buildings = buildings;
                    itemDetails.building = selectedBuildings[i + j];
                } else if (!isItem) {
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
                    if (selectedAnimals[i + j].animalColors == null) {
                        selectedAnimals[i + j].animalColors = animalColors;
                    }
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
                    itemDetails.itemImage.sprite = selectedItems[i + j].itemSprite;
                    itemDetails.animalInformation = animalInformation;
                    itemDetails.buySellAnimal = buySellAnimal;
                    itemDetails.shop = shopInformation;
                    itemDetails.shopAnimals = curAnimals;
                    itemDetails.item = selectedItems[i + j];
                    if (playerInformation.activeInHierarchy) {
                        itemDetails.playerInformation = playerInformation;
                    }
                } else if (isItem) {
                    itemDetails.itemName.text = selectedItems[i + j].itemName;
                    itemDetails.quantity.text = 'x' + inventory.items[selectedItems[i + j]].ToString();
                    itemDetails.itemImage.sprite = selectedItems[i + j].itemSprite;
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
