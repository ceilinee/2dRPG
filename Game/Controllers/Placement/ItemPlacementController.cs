using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

/// <summary>
/// ItemPlacementController helps the player place basic Item objects.
/// <summary>
public class ItemPlacementController : PlacementController {
    [SerializeField]
    private ItemDictionary itemDictionary;

    [SerializeField]
    private PlayerInformation playerInformation;

    // All Items share the same game object prefab
    [SerializeField]
    private GameObject basicItemPrefab;

    private Item itemBeingPlaced;

    [SerializeField]
    private GameObject buySellAnimal;

    // Stores and saves the metadata regarding placed items
    [SerializeField]
    private PlacedItems placedItems;

    [SerializeField]
    private Inventory playerInventory;

    // Load all items that were placed in the previous save
    protected override void LoadSaved() {
        var sceneName = GetVirtualSceneName();
        var itemList = placedItems.GetPlacedItems(sceneName);
        foreach (PlacedItem placedItem in itemList) {
            var item = itemDictionary.Get(placedItem.itemId);
            var placedGameObject = Instantiate(basicItemPrefab);
            placedGameObject.transform.position = placedItem.itemPosition;
            placedGameObject.GetComponent<Object>().item = item;
            placedGameObject.GetComponent<Object>().buySellAnimal = buySellAnimal;
            placedGameObject.GetComponent<SpriteRenderer>()
                .sprite = item.DirectionToSprite(placedItem.direction);
            placedGameObject.SetActive(true);
        }
    }

    protected override void Awake() {
        Assert.IsNotNull(itemDictionary);
        Assert.IsNotNull(playerInformation);
        Assert.IsNotNull(basicItemPrefab);
        Assert.IsNotNull(buySellAnimal);
        Assert.IsNotNull(placedItems);
        Assert.IsNotNull(playerInventory);
        base.Awake();
    }

    protected override void ResetState() {
        base.ResetState();
        itemBeingPlaced = null;
    }

    public PlacedItem AddToPlacedItems(Item item, Vector2 position, Direction direction) {
        var sceneName = GetVirtualSceneName();
        return placedItems.Add(sceneName, item.Id, position, direction);
    }

    // Runs when the player physically places down the item
    protected override void PlaceObject() {
        placeableObject = Instantiate(placeableObjectPrefab,
            placeableObjectBlueprint.transform.position,
            placeableObjectBlueprint.transform.rotation);

        placeableObject.GetComponent<Object>().item = itemBeingPlaced;
        placeableObject.GetComponent<Object>().buySellAnimal = buySellAnimal;

        var sprite = placeableObjectBlueprintSpriteRenderer.sprite;
        placeableObject.GetComponent<SpriteRenderer>().sprite = sprite;

        var placedItem = AddToPlacedItems(itemBeingPlaced, placeableObject.transform.position,
            itemBeingPlaced.SpriteToDirection(sprite));

        placeableObject.GetComponent<Object>().placedItem = placedItem;

        playerInformation.RemoveCurrentItemFromInventory();
        EndPlacementIfNoneLeft();
    }

    protected override void PlayerFaceLeft() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(-3, 0);
        placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.sprites.right;
    }
    protected override void PlayerFaceUp() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(0, 3);
        placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.sprites.down;
    }
    protected override void PlayerFaceRight() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(3, 0);
        placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.sprites.left;
    }
    protected override void PlayerFaceDown() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(0, -3);
        placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.sprites.up;
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
        placeableObjectPrefab = basicItemPrefab;

        // An edge case is that the prefab has a script attached that has side effects in awake
        // If we instantiate the blueprint game object, unexpected changes might happen
        // So we set it to be inactive first just to be safe
        placeableObjectPrefab.SetActive(false);
        placeableObjectBlueprint = Instantiate(placeableObjectPrefab);
        placeableObjectBlueprintSpriteRenderer = placeableObjectBlueprint.GetComponent<SpriteRenderer>();
        placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.ItemSprite;
        ConvertToBlueprint(placeableObjectBlueprint);
        placeableObjectPrefab.SetActive(true);
    }
}
