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

    public void Initialize(int buildingId, BuildingItem buildingItem) {
        this.buildingId = buildingId;
        this.buildingItem = buildingItem;
    }

    private void OnPetWillEnterSceneTransition(object sender, SceneTransition.OnEntityWillEnterSceneTransitionEventArgs args) {
        Collider2D petCollider = args.entity;
        var animal = curAnimals.animalDict[petCollider.gameObject.GetComponent<GenericAnimal>().animalTrait.id];
        // The animal is entering a virtual barn, so reassign the value of their scene to be <barn scene name>#<building id of barn>
        animal.scene = BuildingController.BuildBuildingSceneName(buildingItem.sceneInfo, buildingId);
    }

    private void Start() {
        Assert.IsNotNull(buildingItem, "Need to call Initialize() on this class with the correct arguments");

        sceneTransition.SetSceneInfo(buildingItem.sceneInfo);
        sceneTransition.AddPetWillEnterSceneTransitionListener(
            OnPetWillEnterSceneTransition);

        barnSwitch.SetBarnId(buildingId);
    }
}
