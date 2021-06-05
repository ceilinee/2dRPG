using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class PlacedBuilding {
    public PlacedBuilding(int buildingId, int buildingItemId, Vector2 itemPosition, Timestamp completionTime, int sceneInfoId) {
        this.buildingId = buildingId;
        this.buildingItemId = buildingItemId;
        this.itemPosition = itemPosition;
        this.completionTime = completionTime;
        this.completed = false;
        this.sceneInfoId = sceneInfoId;
        this.upgrade = 0;
    }
    // Unique id for a placed building
    public int buildingId;
    // Multiple placed buildings might share teh same BuildingItem instance
    public int buildingItemId;
    public Vector2 itemPosition;

    public Timestamp completionTime;

    // Whether or not the building has already been built
    // depending on what completionTime is
    public bool completed;

    // Id of the corresponding SceneInfo SO
    public int sceneInfoId;

    // Barns have different tiers, with 0 being the lowest
    // They can be upgraded from 0 to 1 to 2 etc.
    public int upgrade;
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

    public PlacedBuilding Add(int buildingItemId, Vector2 itemPosition, Timestamp completionTime, int sceneInfoId) {
        // Each placed building is assigned an id, which is equal to the idx of the building in `buildings`
        var newPlacedBuilding = new PlacedBuilding(buildings.Count, buildingItemId, itemPosition, completionTime, sceneInfoId);
        buildings.Add(newPlacedBuilding);
        return newPlacedBuilding;
    }

    // Mark that the building has finished being built
    public void SetBuildingCompleted(int buildingId) {
        var building = buildings.Find(x => x.buildingId == buildingId);
        Assert.IsNotNull(building);
        building.completed = true;
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

    public void Clear() {
        buildings.Clear();
        buildingEnteredIdx = -1;
    }
}
