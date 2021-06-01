using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class PlacedBuilding {
    public PlacedBuilding(int buildingItemId, Vector2 itemPosition, Timestamp completionTime) {
        this.buildingItemId = buildingItemId;
        this.itemPosition = itemPosition;
        this.completionTime = completionTime;
        this.completed = false;
    }
    public int buildingItemId;
    public Vector2 itemPosition;

    public Timestamp completionTime;

    // Whether or not the building has already been built
    // depending on what completionTime is
    public bool completed;
}

/// <summary>
/// This scriptable object stores metadata of all the buildings on the map
/// On start up, the building controller uses this data to instantiate the correct buildings
/// <summary>
[CreateAssetMenu]
[System.Serializable]
public class PlacedBuildings : ScriptableObject {
    public List<PlacedBuilding> buildings;

    public void Add(int buildingItemId, Vector2 itemPosition, Timestamp completionTime) {
        buildings.Add(
            new PlacedBuilding(buildingItemId, itemPosition, completionTime)
        );
    }

    // Mark that the building has finished being built
    public void SetBuildingCompleted(int buildingItemId) {
        var building = buildings.Find(x => x.buildingItemId == buildingItemId);
        Assert.IsNotNull(building);
        building.completed = true;
    }

    public void Clear() {
        buildings.Clear();
    }
}
