using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BarnAreas : CustomMonoBehaviour {
    [SerializeField]
    private PlacedBuildings placedBuildings;

    [SerializeField]
    private List<GameObject> areas;

    void Start() {
        // Pick the correct tilemap based on the upgrade of this barn
        foreach (GameObject area in areas) {
            area.SetActive(false);
        }
        var placedBuilding = placedBuildings.GetBuildingEntered();
        areas[(int) placedBuilding.GetUpgrade()].SetActive(true);
    }
}
