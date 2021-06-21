using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class AnimalInformation : CustomMonoBehaviour {
    public InputField animalName;
    public Text age;
    public Text friendship;
    public Text type;
    public Text gender;
    public Text sell;
    public Text mood;
    public Text follow;
    public Text body;
    public Text breed;
    public Text face;
    public Text eyes;
    public Text personality;
    public Image healthBar;
    public Text healthPercentage;
    public Text ears;
    public Dropdown homeDropdown;
    public List<string> dropdownOptions;
    public PlacedBuildings placedBuildings;
    public SceneInfos allBuildings;
    public Text spots;
    public Text back;
    public Text legs;
    public Text tail;
    public Text star;
    public Text pregnant;
    public AnimalColors animalColors;
    public GameObject offspringButton;
    public GameObject CanvasController;
    public GameObject animal;
    public Animal animalTraitInformation;
    public GameObject playerInformation;
    public GameObject list;
    public Animals curAnimals;
    public GameObject buySellObject;
    public GameObject breedAnimal;
    public GameObject SpawnAnimal;
    public CurTime curTime;

    public AnimalList animalList;
    public GameObject followButton;
    public Transform player;
    public Confirmation confirmation;

    [SerializeField] private VectorPointsList pondList;

    void Start() {
        confirmation = centralController.Get("Confirmation").GetComponent<Confirmation>();
        animalName.onEndEdit.AddListener(saveName);
        homeDropdown.onValueChanged.AddListener(delegate {
            updateHome(homeDropdown);
        });
        player = centralController.Get("Player").transform;
        SpawnAnimal = centralController.Get("SpawnAnimal");
    }
    private void saveName(string textInField) {
        if (textInField.Length == 0) {
            animalName.text = animalTraitInformation.animalName;
            CanvasController.GetComponent<CanvasController>().initiateNotification("Your pet's name must be atleast one character!", true);
        } else {
            curAnimals.animalDict[animalTraitInformation.id].animalName = textInField;
            CanvasController.GetComponent<CanvasController>().initiateNotification("Your pet's name has been changed to " + textInField + "!", true);
        }
    }
    public void Breed(Animal selectedAnimal) {
        breedAnimal.GetComponent<BreedScript>().BreedAnimals(animalTraitInformation, selectedAnimal);
    }
    public void sellAnimal() {
        confirmation.initiateConfirmation("Are you sure you want to sell " + animalTraitInformation.animalName + " for $" + animalTraitInformation.cost + "? You won't be able to get them back.",
            ConfirmAnimal,
            () => { },
            () => { },
            true
        );
    }
    public void ConfirmAnimal() {
        buySellObject.GetComponent<BuySellAnimal>().sellAnimal(animalTraitInformation);
        CloseIfPlayerMenuNotOpen();
        animal.SetActive(false);
        var animalScript = animal.GetComponent<GenericAnimal>();
        animalScript.contextOff.Raise();
        if (playerInformation.activeInHierarchy) {
            playerInformation.GetComponent<PlayerInformation>().updateAbout();
        }
        foreach (int babyId in animalScript.animalTrait.babyId) {
            var baby = animalList.findAnimalGameObject(babyId);
            Assert.IsNotNull(baby);
            baby.GetComponent<GenericAnimal>().ParentSold();
        }
    }
    void Update() {
        if (Input.GetButtonDown("Cancel") && gameObject.activeInHierarchy) {
            CloseIfPlayerMenuNotOpen();
        }
    }
    public string getFriendship(Animal animalTrait) {
        if (animalTrait.love <= 50) {
            return animalTrait.animalName + " is scared of you";
        }
        if (animalTrait.love <= 100) {
            return animalTrait.animalName + " is a bit nervous around you, ♥";
        }
        if (animalTrait.love <= 200) {
            return animalTrait.animalName + " is relaxed around you, ♥♥";
        }
        if (animalTrait.love <= 300) {
            return animalTrait.animalName + " is excited to see you, ♥♥♥";
        }
        if (animalTrait.love <= 400) {
            return animalTrait.animalName + " adores you, ♥♥♥♥";
        }
        return animalTrait.animalName + " loves you, ♥♥♥♥♥";
    }
    public void updateHome(Dropdown target) {
        if (animalTraitInformation.home != dropdownOptions[target.value]) {
            animalTraitInformation.home = dropdownOptions[target.value];
            var msg = "";
            if (animalTraitInformation.type == Animal.TypeFish) {
                VectorPoints pond = pondList.vectorPoints.Find(x => x.locationName == animalTraitInformation.home);
                Assert.IsNotNull(pond);
                animalTraitInformation.location = pond.value;
                animal.transform.position = pond.value;
                animal.GetComponent<FishScript>().UpdateWater();
                msg = animalTraitInformation.animalName + "'s home has been changed to " +
                    animalTraitInformation.home +
                    "! They have been transported over there and are super excited to make new friends!";
            } else {
                msg = animalTraitInformation.animalName + "'s home has been changed to " +
                    animalTraitInformation.home + "! They're super excited to make new friends there! When you ring " +
                    curAnimals.animalDict[animalTraitInformation.id].home +
                    "'s dinner bell, this pet will be sure to go in.";
            }
            CanvasController.GetComponent<CanvasController>().initiateNotification(msg, true);
        }
    }
    public void SetupHomeDropdown(Animal animalTrait) {
        dropdownOptions = new List<string>();
        int position = 0;
        if (animalTrait.type == Animal.TypeFish) {
            for (int i = 0; i < pondList.vectorPoints.Count; ++i) {
                VectorPoints vector = pondList.vectorPoints[i];
                dropdownOptions.Add(vector.locationName);
                if (vector.locationName == animalTrait.home) {
                    position = i;
                }
            }
        } else {
            for (int i = 0; i < placedBuildings.buildings.Count; ++i) {
                PlacedBuilding building = placedBuildings.buildings[i];
                if (!building.completed) {
                    continue;
                }
                SceneInfo sceneInfo = allBuildings.sceneDict[building.sceneInfoId];
                var buildingSceneName = BuildingController.BuildBuildingSceneName(sceneInfo, building);
                dropdownOptions.Add(buildingSceneName);
                if (buildingSceneName == animalTrait.home) {
                    position = i;
                }
            }
        }
        homeDropdown.ClearOptions();
        homeDropdown.AddOptions(dropdownOptions);
        homeDropdown.value = position;
    }

    public void updateAbout(Animal animalTrait, GameObject newAnimal) {
        animalTraitInformation = animalTrait;
        animal = newAnimal;

        animalName.text = animalTrait.animalName;
        age.text = animalTrait.age.ToString() + " days old";
        friendship.text = getFriendship(animalTrait);
        type.text = animalTrait.type;
        personality.text = animalTrait.personality.personality;
        healthBar.fillAmount = animalTrait.health / 100f;
        healthPercentage.text = animalTrait.health.ToString() + "%";
        // healthBar.color = getColor(animalTrait.health);
        body.text = animalColors.colorDictionary[animalTrait.coloring.body].ColorName;
        breed.text = animalTrait.breed;
        eyes.text = animalColors.colorDictionary[animalTrait.coloring.eyes].ColorName;
        face.text = animalColors.colorDictionary[animalTrait.coloring.face].ColorName;
        ears.text = animalColors.colorDictionary[animalTrait.coloring.ears].ColorName;

        SetupHomeDropdown(animalTrait);

        if (animalColors.colorDictionary[animalTrait.coloring.ears].ColorName == "Invisible") {
            ears.text = "N/A";
        }
        spots.text = animalColors.colorDictionary[animalTrait.coloring.dots].ColorName;
        if (animalColors.colorDictionary[animalTrait.coloring.dots].ColorName == "Invisible") {
            spots.text = "N/A";
        }
        back.text = animalColors.colorDictionary[animalTrait.coloring.back].ColorName;
        if (animalColors.colorDictionary[animalTrait.coloring.back].ColorName == "Invisible") {
            back.text = "N/A";
        }
        legs.text = animalColors.colorDictionary[animalTrait.coloring.legs].ColorName;
        if (animalColors.colorDictionary[animalTrait.coloring.legs].ColorName == "Invisible") {
            legs.text = "N/A";
        }
        tail.text = animalColors.colorDictionary[animalTrait.coloring.tail].ColorName;
        if (animalColors.colorDictionary[animalTrait.coloring.tail].ColorName == "Invisible") {
            tail.text = "N/A";
        }
        star.text = animalColors.colorDictionary[animalTrait.coloring.star].ColorName;
        if (animalColors.colorDictionary[animalTrait.coloring.star].ColorName == "Invisible") {
            star.text = "N/A";
        }
        if (animalTrait.pregnant) {
            offspringButton.GetComponent<Button>().interactable = false;
            pregnant.text = "Yes! The little one(s) are coming on " + curTime.getSeasonInWordsVar(curTime.getSeasonVar(animalTrait.deliveryDate)) + " " + curTime.getDateVar(animalTrait.deliveryDate).ToString();
        } else if (!animalTrait.pregnant && animalTrait.gender == "Female") {
            if (animalTrait.age >= 5 && animalTrait.age < 60) {
                pregnant.text = "Not yet!";
            } else if (animalTrait.age >= 60) {
                offspringButton.GetComponent<Button>().interactable = false;
                pregnant.text = "This gal is retired now and enjoying the slow life!";
            } else {
                offspringButton.GetComponent<Button>().interactable = false;
                pregnant.text = "No, this little one is still young";
            }
        } else if (animalTrait.gender == "Male") {
            if (animalTrait.age >= 5 && animalTrait.age < 60) {
                pregnant.text = "No (This one's a dude)";
            } else if (animalTrait.age >= 60) {
                offspringButton.GetComponent<Button>().interactable = false;
                pregnant.text = "This dude is retired now and enjoying the slow life!";
            } else {
                offspringButton.GetComponent<Button>().interactable = false;
                pregnant.text = "No (This one's still young and a dude)";
            }
        }
        gender.text = animalTrait.gender;
        sell.text = "Sell for $" + animalTrait.cost.ToString();
        mood.text = animalTrait.mood;
        updateList(animalTrait.type, animalTrait.gender);
    }

    public void SetFollow() {
        if (animalTraitInformation.follow) {
            animal.GetComponent<GenericAnimal>().setAnimalUnfollow();
        } else {
            // if the animal is currently not in the scene, move it to the scene 
            if (animalTraitInformation.scene != SceneManager.GetActiveScene().name
            && !(animalTraitInformation.scene.StartsWith(SceneManager.GetActiveScene().name) &&
            BuildingController.BuildingIdFromBuildingSceneName(animalTraitInformation.scene) ==
            placedBuildings.buildingEnteredIdx)) {
                Debug.Log("Spawn Animal to follow player");
                animalTraitInformation.scene = SceneManager.GetActiveScene().name;
                animalTraitInformation.location = player.position;
                SpawnAnimal.GetComponent<SpawnAnimal>().Spawn(animalTraitInformation);
                animal = SpawnAnimal.GetComponent<SpawnAnimal>().animalGameObject.GetComponent<AnimalList>().findAnimal(animalTraitInformation.id).gameObject;
            }
            animal.SetActive(true);
            animal.GetComponent<GenericAnimal>().setAnimalFollow();
        }
        curAnimals.updateAnimal(animalTraitInformation);
        SetFollowText();
        CloseIfPlayerMenuNotOpen();
    }
    public void SetFollowText() {
        if (animalTraitInformation.follow) {
            follow.text = "Send back home";
        } else {
            follow.text = "Take for a Walk";
        }
    }
    public void CloseIfPlayerMenuNotOpen() {
        Debug.Log("player menu: " + playerInformation.activeInHierarchy);
        if (!playerInformation.activeInHierarchy) {
            Close();
        } else {
            gameObject.SetActive(false);
            Clear();
        }
    }
    public void NotifyDeliveryDate(Animal femaleAnimal) {
        CanvasController.GetComponent<CanvasController>().initiateNotification(
            femaleAnimal.animalName + " will be giving birth on " + curTime.getSeasonInWordsVar(curTime.getSeasonVar(femaleAnimal.deliveryDate)) + " " + curTime.getDateVar(femaleAnimal.deliveryDate).ToString(),
            true
        );
        CloseIfPlayerMenuNotOpen();
    }
    public void Close() {
        gameObject.SetActive(false);
        CanvasController.GetComponent<CanvasController>().closeCanvas();
        Clear();
        Time.timeScale = 1;
    }
    public void Clear() {
        list.GetComponent<ListCreator>().Clear();
        offspringButton.GetComponent<Button>().interactable = true;
        offspringButton.GetComponent<ButtonScript>().toggleAboutButton();
        if (playerInformation.activeInHierarchy) {
            playerInformation.GetComponent<PlayerInformation>().updateAbout();
        }
    }
    public void updateList(string newType, string newGender) {
        if (animalTraitInformation.type == "Fish") {
            followButton.SetActive(false);
        }
        if (animalTraitInformation.type != "Fish" && !followButton.activeInHierarchy) {
            followButton.SetActive(true);
        }
        SetFollowText();
        list.GetComponent<ListCreator>().type = newType;
        list.GetComponent<ListCreator>().gender = newGender;
        list.GetComponent<ListCreator>().GetAnimals();
    }
    public Color getColor(int health) {
        Color color = new Color(26f / 255f, 28f / 255f, 44f / 255f);
        if (health >= 90) {
            color = new Color(56f / 255f, 183f / 255f, 100f / 255f);
        } else if (health >= 80) {
            color = new Color(167f / 255f, 240f / 255f, 112f / 255f);
        } else if (health >= 60) {
            color = new Color(207f / 255f, 255f / 255f, 171f / 255f);
        } else if (health >= 50) {
            color = new Color(255f / 255f, 228f / 255f, 120f / 255f);
        } else if (health >= 40) {
            color = new Color(255f / 255f, 181f / 255f, 112f / 255f);
        } else if (health >= 30) {
            color = new Color(255f / 255f, 145f / 255f, 102f / 255f);
        } else if (health >= 20) {
            color = new Color(235f / 255f, 86f / 255f, 75f / 255f);
        } else if (health >= 10) {
            color = new Color(177f / 255f, 62f / 255f, 83f / 255f);
        }
        return color;
    }
}
