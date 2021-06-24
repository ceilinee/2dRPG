using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

/// <summary>
/// PrefabPlacementController helps the player place instances of basic prefabs.
/// <summary>
public class PrefabPlacementController : PlacementController {
    [SerializeField]
    private ItemDictionary itemDictionary;

    [SerializeField]
    private PlayerInformation playerInformation;

    [SerializeField]
    private PrefabsByType itemPrefabsByType;

    private Item itemBeingPlaced;

    [SerializeField]
    private GameObject buySellAnimal;

    // Stores and saves the metadata regarding placed items
    [SerializeField]
    private PlacedItems placedItemsComplex;

    [SerializeField]
    private Inventory playerInventory;

    // Load all items that were placed in the previous save
    // TODO: potential race condition; place two trees, exit the game, then start the game
    // again (start mainscene directly) the two trees will appear and the placeditemscomplex SO
    // will be cleared by the GSM *after* this LoadSaved() function runs
    protected override void LoadSaved() {
        var sceneName = GetVirtualSceneName();
        var itemList = placedItemsComplex.GetPlacedItems(sceneName);
        foreach (PlacedItem placedItem in itemList) {
            var item = itemDictionary.Get(placedItem.itemId);
            CreatePlacedObject(
                item,
                itemPrefabsByType.FindPrefab(item.prefabType),
                placedItem.itemPosition,
                item.DirectionToSprite(placedItem.direction)
            );
        }
    }

    protected override void Awake() {
        Assert.IsNotNull(itemDictionary);
        Assert.IsNotNull(playerInformation);
        Assert.IsNotNull(buySellAnimal);
        Assert.IsNotNull(itemPrefabsByType);
        Assert.IsNotNull(placedItemsComplex);
        Assert.IsNotNull(playerInventory);
        base.Awake();
    }

    protected override void ResetState() {
        base.ResetState();
        itemBeingPlaced = null;
    }

    private GameObject CreatePlacedObject(Item item, GameObject prefab, Vector2 position, Sprite sprite) {
        var created = Instantiate(prefab);
        created.transform.position = position;
        created.GetComponent<SpriteRenderer>().sprite = sprite;
        // Placed items that cannot be hit / broken can be picked up.
        // Therefore, we attach the Object.cs script to them (unless they already have it).
        if (created.GetComponent<Breakable>() == null && created.GetComponent<Object>() == null) {
            Assert.IsTrue(created.GetComponent<Collider2D>() != null,
                "Talk to Ethan about this (Object script only works if GO has a collider2D trigger)");
            var objectScript = created.AddComponent<Object>();
            objectScript.item = item;
            objectScript.buySellAnimal = buySellAnimal;
            // TODO: there are some more fields to populate on Object
        }
        return created;
    }

    public PlacedItem AddToPlacedItems(Item item, Vector2 position, Direction direction) {
        var sceneName = GetVirtualSceneName();
        return placedItemsComplex.Add(sceneName, item.Id, position, direction);
    }

    // Runs when the player physically places down the item
    protected override void PlaceObject() {
        var sprite = placeableObjectBlueprintSpriteRenderer.sprite;
        placeableObject = CreatePlacedObject(
            itemBeingPlaced,
            placeableObjectPrefab,
            placeableObjectBlueprint.transform.position,
            sprite
        );
        var placedItem = AddToPlacedItems(itemBeingPlaced, placeableObject.transform.position,
            itemBeingPlaced.SpriteToDirection(sprite));

        playerInformation.RemoveCurrentItemFromInventory();
        EndPlacementIfNoneLeft();
    }

    protected override void PlayerFaceLeft() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(-5, 0);
        placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.sprites.right;
    }
    protected override void PlayerFaceUp() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(0, 5);
        placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.sprites.down;
    }
    protected override void PlayerFaceRight() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(5, 0);
        placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.sprites.left;
    }
    protected override void PlayerFaceDown() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(0, -5);
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
        // TODO: change this to use field from itemBeingPlaced
        placeableObjectPrefab = itemPrefabsByType.FindPrefab(playerInventory.currentItem.prefabType);

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
