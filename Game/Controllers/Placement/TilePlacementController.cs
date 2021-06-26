using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;
using UnityEngine.Tilemaps;

/// <summary>
/// TilePlacementController helps the player place basic rule tiles on a tilemap.
/// <summary>
public class TilePlacementController : PlacementController {
    [SerializeField]
    private ItemDictionary itemDictionary;

    [SerializeField]
    private PlayerInformation playerInformation;

    private TileItem itemBeingPlaced;

    [SerializeField]
    private GameObject buySellAnimal;

    // Stores and saves the metadata regarding placed items
    [SerializeField]
    private PlacedItems placedItemsTiles;

    [SerializeField]
    private Inventory playerInventory;

    [Tooltip("The tile map the player can place non-collidable tiles on.")]
    [SerializeField]
    private Tilemap placementTileMap;

    [Tooltip("The tile map the player can place collidable tiles on.")]
    [SerializeField]
    private Tilemap placementTileMapCollision;

    private CompositeCollider2D placementTileMapCollisionCollider;

    private RuleTile tileBeingPlaced;

    [SerializeField]
    private GameObject tileBlueprintPrefab;

    protected override void LoadSaved() {
        var sceneName = GetVirtualSceneName();
        var itemList = placedItemsTiles.GetPlacedItems(sceneName);
        foreach (PlacedItem placedItem in itemList) {
            var item = (TileItem) itemDictionary.Get(placedItem.itemId);
            if (item.isCollidable) {
                placementTileMapCollision.SetTile(ToTilemapPos(placedItem.itemPosition), item.tile);
            } else {
                placementTileMap.SetTile(ToTilemapPos(placedItem.itemPosition), item.tile);
            }
        }
        placementTileMapCollisionCollider.GenerateGeometry();
        astar.RescanAstarGraph(placementTileMapCollisionCollider.bounds);
    }

    protected override void Awake() {
        Assert.IsNotNull(itemDictionary);
        Assert.IsNotNull(playerInformation);
        Assert.IsNotNull(buySellAnimal);
        Assert.IsNotNull(placedItemsTiles);
        Assert.IsNotNull(playerInventory);
        Assert.IsNotNull(placementTileMap);
        Assert.IsNotNull(placementTileMapCollision);
        Assert.IsNotNull(tileBlueprintPrefab);
        placementTileMapCollisionCollider = placementTileMapCollision.GetComponent<CompositeCollider2D>();

        base.Awake();
    }

    protected override void ResetState() {
        base.ResetState();
        itemBeingPlaced = null;
    }

    public PlacedItem AddToPlacedItems(Item item, Vector2 position, Direction direction) {
        var sceneName = GetVirtualSceneName();
        return placedItemsTiles.Add(sceneName, item.Id, position, direction);
    }

    private Vector3Int ToTilemapPos(Vector3 position) {
        // A position in the scene has to be offset by -1 in the x and y direction for some reason to
        // match the same location in a tilemap grid
        return new Vector3Int((int) Math.Truncate(position.x) - 1, (int) Math.Truncate(position.y) - 1, 0);
    }

    // Runs when the player physically places down the item
    protected override void PlaceObject() {
        var pos = placeableObjectBlueprint.transform.position;
        if (itemBeingPlaced.isCollidable) {
            placementTileMapCollision.SetTile(ToTilemapPos(pos), tileBeingPlaced);
            placementTileMapCollisionCollider.GenerateGeometry();
            astar.RescanAstarGraph(placementTileMapCollisionCollider.bounds);
        } else {
            placementTileMap.SetTile(ToTilemapPos(pos), tileBeingPlaced);
        }
        AddToPlacedItems(itemBeingPlaced, pos, Direction.Down);
        playerInformation.RemoveCurrentItemFromInventory();
        EndPlacementIfNoneLeft();
    }

    protected override void PlayerFaceLeft() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(-3, 0);
    }
    protected override void PlayerFaceUp() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(0, 3);
    }
    protected override void PlayerFaceRight() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(3, 0);
    }
    protected override void PlayerFaceDown() {
        placeableObjectBlueprint.transform.position = RoundedPlayerPosition() + new Vector2(0, -3);
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
        itemBeingPlaced = (TileItem) playerInventory.currentItem;
        tileBeingPlaced = itemBeingPlaced.tile;
        Assert.IsNotNull(tileBeingPlaced);

        // placeableObjectPrefab, placeableObjectBlueprintSpriteRenderer
        // are intentionally not set, as we will be placing tiles instead via `tileBeingPlaced`
        placeableObjectBlueprint = Instantiate(tileBlueprintPrefab);
        SpriteRenderer[] sprites = placeableObjectBlueprint.GetComponentsInChildren<SpriteRenderer>();
        SetSpritesToColor(sprites, faintBlue);
        AttachPlaceableObjectBlueprintScript(placeableObjectBlueprint);
    }

    // Invoked by Signal Listener for signal PlayerPickupSignal
    public void OnPlayerPickupSignalRaised() {
        var playerPosRounded = RoundedPlayerPosition();
        var sceneName = GetVirtualSceneName();
        var tilemapPos = ToTilemapPos(playerPosRounded);
        if (placementTileMap.HasTile(tilemapPos)) {
            placementTileMap.SetTile(tilemapPos, null);
            var removed = placedItemsTiles.RemoveIfExists(sceneName, playerPosRounded);
            foreach (PlacedItem placedItem in removed) {
                var item = (TileItem) itemDictionary.Get(placedItem.itemId);
                buySellAnimal.GetComponent<BuySellAnimal>().pickUpItem(item);
            }
        }
        if (placementTileMapCollision.HasTile(tilemapPos)) {
            placementTileMapCollision.SetTile(tilemapPos, null);
            var removed = placedItemsTiles.RemoveIfExists(sceneName, playerPosRounded);
            foreach (PlacedItem placedItem in removed) {
                var item = (TileItem) itemDictionary.Get(placedItem.itemId);
                buySellAnimal.GetComponent<BuySellAnimal>().pickUpItem(item);
            }
            placementTileMapCollisionCollider.GenerateGeometry();
            astar.RescanAstarGraph(placementTileMapCollisionCollider.bounds);
        }
    }
}
