using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

/// <summary>
/// This class manages all of the placement controllers. Modules should not use
/// placement controllers directly, but rather interact with them via PlacementManager's interface.
/// <summary>
public class PlacementManager : CustomMonoBehaviour {
    // Nullable: set to the currently active controller; either zero or one controller is allowed
    // to be active at any point in time.
    private PlacementController currActiveController;
    private ItemPlacementController itemPlacementController;
    private BuildingPlacementController buildingPlacementController;
    private PrefabPlacementController prefabPlacementController;

    private TilePlacementController tilePlacementController;

    private PondPlacementController pondPlacementController;

    private void Awake() {
        itemPlacementController = GetComponent<ItemPlacementController>();
        buildingPlacementController = GetComponent<BuildingPlacementController>();
        prefabPlacementController = GetComponent<PrefabPlacementController>();
        tilePlacementController = GetComponent<TilePlacementController>();
        pondPlacementController = GetComponent<PondPlacementController>();
    }

    private PlacementController StartController(Item item) {
        switch (item.prefabType) {
            case ItemPrefabType.Object:
                itemPlacementController.BeginPlacement(item.Id);
                return itemPlacementController;
            case ItemPrefabType.Barn:
                buildingPlacementController.BeginPlacement(item.Id);
                return buildingPlacementController;
            case ItemPrefabType.Tile:
                tilePlacementController.BeginPlacement(item.Id);
                return tilePlacementController;
            case ItemPrefabType.Pond:
                pondPlacementController.BeginPlacement(item.Id);
                return pondPlacementController;
            default:
                prefabPlacementController.BeginPlacement(item.Id);
                return prefabPlacementController;
        }
    }

    public void BeginPlacement(Item item) {
        if (currActiveController != null) {
            EndPlacement();
        }
        currActiveController = StartController(item);
    }

    public void PausePlacement() {
        currActiveController?.PausePlacement();
    }

    public void UnpausePlacement() {
        currActiveController?.UnpausePlacement();
    }

    public void EndPlacement() {
        currActiveController?.EndPlacement();
        currActiveController = null;
    }

    public void EndPlacementIfNoneLeft() {
        currActiveController?.EndPlacementIfNoneLeft();
    }

    public PlacedItem AddToPlacedItems(Item item, Vector2 position, Direction direction) {
        return itemPlacementController.AddToPlacedItems(item, position, direction);
    }
}