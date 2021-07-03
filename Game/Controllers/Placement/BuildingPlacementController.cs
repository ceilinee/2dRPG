using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

/// <summary>
/// BuildingPlacementController helps the player place buildings (barns for now).
/// This script is tightly coupled with BuildingController, which handles the behavior of
/// buildings *after* they've been placed.
/// <summary>
public class BuildingPlacementController : PlacementController {
    [SerializeField]
    private PlayerInformation playerInformation;

    [System.Serializable]
    public class BuildingInfo {
        public string buildingItemId;
        public GameObject blueprintPrefab;
        public GameObject prefab;
    }

    [Header(Annotation.HeaderMsgSetInInspector)]
    [SerializeField]
    private List<BuildingInfo> buildingInfoList;

    private Item itemBeingPlaced;

    [SerializeField]
    private Inventory playerInventory;

    [SerializeField]
    private Confirmation confirmation;

    [SerializeField]
    private GameObject birthAlert;

    [SerializeField]
    private CanvasController canvasController;

    protected override void LoadSaved() {
        // NOOP: the loading of saved buildings is done by BuildingController
    }

    protected override void Awake() {
        Assert.IsNotNull(playerInformation);
        Assert.IsTrue(buildingInfoList.Count > 0);
        Assert.IsNotNull(playerInventory);
        Assert.IsNotNull(confirmation);
        Assert.IsNotNull(birthAlert);
        base.Awake();
    }

    protected override void ResetState() {
        base.ResetState();
        itemBeingPlaced = null;
    }

    // Runs when the player physically places down the item
    protected override void PlaceObject() {
        confirmation.initiateConfirmation(
            "Are you sure you want to build your house here?",
            () => {
                placeableObject = Instantiate(placeableObjectBlueprint);
                Destroy(placeableObject.GetComponent<PlaceableObjectBlueprint>());
                var prefab = buildingInfoList.Find(x => x.buildingItemId == itemBeingPlaced.Id).prefab;

                canvasController.openCanvas();
                birthAlert.GetComponent<Alert>().initiateNameAlert(
                    "What should be the name of this barn?",
                    barnName => {
                        canvasController.closeCanvas();
                        buildingController.RegisterBuildingCreation(
                            (BuildingItem) itemBeingPlaced, placeableObject, prefab, barnName);
                        playerInformation.RemoveCurrentItemFromInventory();
                        EndPlacementIfNoneLeft();
                    },
                    true
                );
            },
            () => { },
            () => { Time.timeScale = 1; }
        );
    }

    protected override void PlayerFaceLeft() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(-5, 0);
        placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.sprites.right;
    }

    protected override void PlayerFaceUp() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(0, 4);
        placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.sprites.down;
    }

    protected override void PlayerFaceRight() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(5, 0);
        placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.sprites.left;
    }

    protected override void PlayerFaceDown() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(0, -7);
        placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.sprites.up;
    }

    public bool BeginPlacement(string itemId) {
        return BeginPlacement();
    }

    public override void EndPlacementIfNoneLeft() {
        // If there are more instances of the currently held item, we do not stop placement
        if (playerInformation.CountOfItemInInventory(itemBeingPlaced) == 0) {
            EndPlacement();
        }
    }

    protected override void PrepareBlueprint() {
        Assert.IsNotNull(playerInventory.currentItem);
        itemBeingPlaced = playerInventory.currentItem;
        placeableObjectPrefab = buildingInfoList.Find(
            x => x.buildingItemId == itemBeingPlaced.Id).blueprintPrefab;

        // An edge case is that the prefab has a script attached that has side effects in awake
        // If we instantiate the blueprint game object, unexpected changes might happen
        // So we set it to be inactive first just to be safe
        placeableObjectPrefab.SetActive(false);
        placeableObjectBlueprint = Instantiate(placeableObjectPrefab);
        placeableObjectBlueprintSpriteRenderer = placeableObjectBlueprint.GetComponent<SpriteRenderer>();
        SpriteRenderer[] sprites = placeableObjectBlueprint.GetComponentsInChildren<SpriteRenderer>();
        SetSpritesToColor(sprites, faintBlue);
        AttachPlaceableObjectBlueprintScript(placeableObjectBlueprint);
        placeableObjectBlueprint.SetActive(true);
    }

    public BuildingInfo GetBuildingInfo(string buildingItemId) {
        var res = buildingInfoList.Find(x => x.buildingItemId == buildingItemId);
        Assert.IsNotNull(res);
        return res;
    }
}
