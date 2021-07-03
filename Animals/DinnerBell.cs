using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinnerBell : MonoBehaviour {
    public bool playerInRange;
    public SceneInfo sceneInfo;
    public GameObject spawnAnimal;
    public Transform target;
    // public SpriteRenderer barnSwitch;
    public GameObject doorLight;
    // public Sprite openSwitchImg;
    // public Sprite closedSwitchImg;
    public GameObject animalCollision;
    public Animals curAnimals;
    public GameObject animalList;
    public Animals newAnimals;
    public GameObject confirmationModal;

    [SerializeField]
    private PlacedBuildings placedBuildings;

    void Start() {
        if (sceneInfo.open) {
            openBarn();
        } else {
            closeBarn();
        }
    }
    // Update is called once per frame
    public void closeBarn() {
        sceneInfo.open = false;
        doorLight.SetActive(false);
        // barnSwitch.sprite = closedSwitchImg;
        animalCollision.SetActive(true);
    }
    public void openBarn() {
        sceneInfo.open = true;
        doorLight.SetActive(true);
        // barnSwitch.sprite = openSwitchImg;
        animalCollision.SetActive(false);
    }
    void Update() {
        if (Vector3.Distance(target.position, transform.position) <= 1) {
            playerInRange = true;
        } else if (Vector3.Distance(target.position, transform.position) > 1 && playerInRange == true) {
            playerInRange = false;
        }
        if (Input.GetKeyUp(KeyCode.Space) && playerInRange) {
            //call animals home
            newAnimals = Instantiate(curAnimals);
            Time.timeScale = 0;
            confirmationModal.GetComponent<Confirmation>().initiateConfirmation(
            "Ring the night bell and call the animals home for the night?",
            (() => confirm()),
            (() => cancel()));
        }
    }
    public void confirm() {
        Time.timeScale = 1;
        StartCoroutine(waitSpawn());
    }
    public void cancel() {
        Time.timeScale = 1;
    }
    IEnumerator waitSpawn() {
        openBarn();
        foreach (GameObject animal in animalList.GetComponent<AnimalList>().list) {
            newAnimals.removeExistingAnimal(animal.GetComponent<GenericAnimal>().animalTrait.id);
        }
        foreach (KeyValuePair<int, Animal> kvp in newAnimals.animalDict) {
            var home = kvp.Value.home;
            bool doesBelongToHome = home == placedBuildings.GetBuildingEntered().buildingName;
            if (doesBelongToHome && !kvp.Value.characterOwned) {
                kvp.Value.location = sceneInfo.entrance;
                kvp.Value.scene = BuildingController.BuildBuildingSceneName(
                    sceneInfo, placedBuildings.GetBuildingEntered());
                kvp.Value.target = sceneInfo.entrance + new Vector2(0, Random.Range(2, 6));
                curAnimals.animalDict[kvp.Value.id] = kvp.Value;
                yield return new WaitForSeconds(Random.Range(0, 5));
                spawnAnimal.GetComponent<SpawnAnimal>().Spawn(kvp.Value);
            }
        }
        closeBarn();
    }

}
