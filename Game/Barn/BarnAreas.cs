using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BarnAreas : CustomMonoBehaviour {
    [SerializeField]
    private PlacedBuildings placedBuildings;

    [SerializeField]
    private List<GameObject> areas;

    private void Awake() {
        // Pick the correct tilemap based on the upgrade of this barn
        foreach (GameObject area in areas) {
            area.SetActive(false);
        }
        var barnInfo = placedBuildings.GetBuildingEntered();
        Debug.Log(barnInfo.buildingId);
        areas[barnInfo.upgrade].SetActive(true);
    }
}
