using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Barn : CustomMonoBehaviour {
    // The building id of the barn, set from PlacedBuilding.buildingId
    [Header("Do not set in the inspector! Must be set by the context that creates this barn")]
    [SerializeField]
    private int buildingId;

    // The corresponding BuildingItem for this barn
    [Header("Do not set in the inspector! Must be set by the context that creates this barn")]
    [SerializeField]
    private BuildingItem buildingItem;

    [SerializeField]
    private SceneTransition sceneTransition;

    [SerializeField]
    private BarnSwitch barnSwitch;

    [SerializeField]
    private Animals curAnimals;

    private GameObject player;

    [SerializeField]
    private DescriptionSign sign;

    private PlacedBuilding placedBuilding;

    public void Initialize(int buildingId, BuildingItem buildingItem, PlacedBuilding placedBuilding) {
        this.buildingId = buildingId;
        this.buildingItem = buildingItem;
        this.placedBuilding = placedBuilding;
    }

    private void OnPetWillEnterSceneTransition(object sender,
        SceneTransition.OnEntityWillEnterSceneTransitionEventArgs args) {
        Collider2D petCollider = args.entity;
        var animal = curAnimals.animalDict[petCollider.gameObject.GetComponent<GenericAnimal>().animalTrait.id];
        // The animal is entering a virtual barn, so reassign the value of their scene to be <barn scene name>#<building id of barn>
        animal.scene = BuildingController.BuildBuildingSceneName(buildingItem.sceneInfo, buildingId);
    }

    private void Start() {
        Assert.IsNotNull(buildingItem, "Need to call Initialize() on this class with the correct arguments");
        player = centralController.Get("Player");

        sceneTransition.SetSceneInfo(buildingItem.sceneInfo);
        sceneTransition.AddPetWillEnterSceneTransitionListener(
            OnPetWillEnterSceneTransition);
        sceneTransition.animalList = centralController.Get("AnimalList");
        sceneTransition.gameSaveManager = centralController.Get("GameSaveManager");

        barnSwitch.SetBarnId(buildingId);
        barnSwitch.target = player.transform;
        barnSwitch.animalList = centralController.Get("AnimalList");
        barnSwitch.spawnAnimal = centralController.Get("SpawnAnimal");
        barnSwitch.confirmationModal = centralController.Get("Confirmation");

        sign.dialogueManager = centralController.Get("DialogueManager").GetComponent<DialogueManager>();
        sign.canvasController = centralController.Get("CanvasController").GetComponent<CanvasController>();
        sign.dialog = $"Barn: {placedBuilding.buildingName}";
    }
}
