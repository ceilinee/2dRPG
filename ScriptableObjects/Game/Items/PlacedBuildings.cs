using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

[System.Serializable]
public class PlacedBuilding {
    public enum Status {
        // The building has just been placed (blueprint is placed down)
        WaitingBuilt,
        // The building is finished but is currently in the process of upgrading
        WaitingUpgrade,
        // The building is built and not being upgraded
        Done
    }

    public PlacedBuilding(
        int buildingId,
        string buildingItemId,
        Vector2 itemPosition,
        Timestamp completionTime,
        int sceneInfoId
    ) {
        this.buildingId = buildingId;
        this.buildingItemId = buildingItemId;
        this.itemPosition = itemPosition;
        this.completionTime = completionTime;
        this.sceneInfoId = sceneInfoId;
        this.upgrade = 0;
        this.status = Status.WaitingBuilt;
    }
    // Unique id for a placed building
    public int buildingId;
    // Multiple placed buildings might share teh same BuildingItem instance
    public string buildingItemId;
    public Vector2 itemPosition;

    // Only relevant when status is WaitingXXX
    public Timestamp completionTime;

    // Id of the corresponding SceneInfo SO
    public int sceneInfoId;

    // Barns have different tiers, with 0 being the lowest
    // They can be upgraded from 0 to 1 to 2 etc.
    // This integer corresponds to the corresponding enum BarnUpgrades.Upgrade
    public int upgrade;

    public Status status;

    public BarnUpgrades.Upgrade GetUpgrade() {
        return (BarnUpgrades.Upgrade) upgrade;
    }

    public BarnUpgrades.Upgrade GetNextUpgrade() {
        Assert.IsTrue(upgrade + 1 < BarnUpgrades.NumUpgrades());
        return (BarnUpgrades.Upgrade) (upgrade + 1);
    }

    public bool IsMaxUpgrade() {
        return upgrade == BarnUpgrades.NumUpgrades() - 1;
    }

    public void UpgradeToNext() {
        Assert.IsTrue(upgrade + 1 < BarnUpgrades.NumUpgrades());
        upgrade++;
    }
}

/// <summary>
/// This scriptable object stores metadata of all the buildings on the map
/// On start up, the building controller uses this data to instantiate the correct buildings
/// <summary>
[CreateAssetMenu]
[System.Serializable]
public class PlacedBuildings : ScriptableObject {
    public List<PlacedBuilding> buildings;

    // buildings[buildingEnteredIdx] is the building we are currently in
    // -1 means we are not in a building
    public int buildingEnteredIdx;
    public PlacedBuilding Add(string buildingItemId, Vector2 itemPosition, Timestamp completionTime, int sceneInfoId) {
        // Each placed building is assigned an id, which is equal to the idx of the building in `buildings`
        var newPlacedBuilding = new PlacedBuilding(buildings.Count, buildingItemId, itemPosition, completionTime, sceneInfoId);
        buildings.Add(newPlacedBuilding);
        return newPlacedBuilding;
    }
    // Mark that the building has finished being built
    public void SetBuildingCompleted(int buildingId) {
        var building = buildings.Find(x => x.buildingId == buildingId);
        Assert.IsNotNull(building);
        if (building.status == PlacedBuilding.Status.WaitingUpgrade) {
            // Complete the upgrade
            building.UpgradeToNext();
        }
        building.status = PlacedBuilding.Status.Done;
    }

    // Call this when player enters building `building`
    public void SetBuildingEntered(PlacedBuilding building) {
        buildingEnteredIdx = buildings.IndexOf(building);
    }

    // Gets the placedbuilding corresponding to the current building we are in
    public PlacedBuilding GetBuildingEntered() {
        return buildingEnteredIdx != -1 ? buildings[buildingEnteredIdx] : null;
    }

    public PlacedBuilding GetBuilding(int buildingId) {
        return buildings.Find(x => x.buildingId == buildingId);
    }

    public List<PlacedBuilding> GetBuildings(PlacedBuilding.Status status) {
        return buildings.FindAll(x => x.status == status);
    }

    public void Clear() {
        buildings = new List<PlacedBuilding>();
        buildingEnteredIdx = -1;
    }
}
