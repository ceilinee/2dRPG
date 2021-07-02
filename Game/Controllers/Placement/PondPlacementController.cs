using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

/// <summary>
/// <summary>
public class PondPlacementController : PlacementController {
    [SerializeField]
    private ItemDictionary itemDictionary;

    [SerializeField]
    private PlayerInformation playerInformation;

    [SerializeField]
    private GameObject pondPrefab;

    private Item itemBeingPlaced;

    [SerializeField]
    private GameObject buySellAnimal;

    [SerializeField]
    private Inventory playerInventory;

    [SerializeField]
    private CanvasController canvasController;

    [SerializeField]
    private GameObject birthAlert;

    [SerializeField]
    private PlacedPonds placedPonds;

    // Set at runtime; placedPondObjects[i] is the GO corresponding to
    // placedPonds[i]
    public List<GameObject> placedPondObjects;

    protected override void LoadSaved() {
        placedPondObjects = new List<GameObject>();
        foreach (PlacedPond pond in placedPonds.ponds) {
            var pondObject = Instantiate(pondPrefab);
            pondObject.transform.position = pond.position;
            placedPondObjects.Add(pondObject);
        }
    }

    protected override void Awake() {
        Assert.IsNotNull(itemDictionary);
        Assert.IsNotNull(playerInformation);
        Assert.IsNotNull(buySellAnimal);
        Assert.IsNotNull(playerInventory);
        base.Awake();
    }

    protected override void ResetState() {
        base.ResetState();
        itemBeingPlaced = null;
    }

    // Runs when the player physically places down the item
    protected override void PlaceObject() {
        canvasController.openCanvas();
        // TODO: add support for setting the image on the alert
        birthAlert.GetComponent<Alert>().initiateNameAlert(
            "What should be the name of this pond?",
            pondName => {
                canvasController.closeCanvas();

                placeableObject = Instantiate(placeableObjectPrefab);
                placeableObject.transform.position = placeableObjectBlueprint.transform.position;

                placedPonds.Add(pondName, placeableObject.transform.position);
                placedPondObjects.Add(placeableObject);

                playerInformation.RemoveCurrentItemFromInventory();
                EndPlacementIfNoneLeft();
            },
            true
        );
    }

    protected override void PlayerFaceLeft() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(-6, 0);
    }
    protected override void PlayerFaceUp() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(0, 6);
    }
    protected override void PlayerFaceRight() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(6, 0);
    }
    protected override void PlayerFaceDown() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(0, -6);
    }

    // Return value indicates whether a new placement process was started
    // prefabId is the id of the Item being placed (Item.id)
    public bool BeginPlacement(string prefabId) {
        // Begin placement only if either we are not placing an item,
        // or we are placing some item that is a different item
        if (BeginPlacement()) {
            return true;
        }
        if (prefabId != itemBeingPlaced.Id) {
            ResetState();
            PrepareBlueprint();
            return true;
        }
        return false;
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
        placeableObjectPrefab = pondPrefab;

        // An edge case is that the prefab has a script attached that has side effects in awake
        // If we instantiate the blueprint game object, unexpected changes might happen
        // So we set it to be inactive first just to be safe
        placeableObjectPrefab.SetActive(false);
        placeableObjectBlueprint = Instantiate(placeableObjectPrefab);
        placeableObjectBlueprintSpriteRenderer = placeableObjectBlueprint.GetComponent<SpriteRenderer>();
        ConvertToBlueprint(placeableObjectBlueprint);
        placeableObjectPrefab.SetActive(true);
    }
}
