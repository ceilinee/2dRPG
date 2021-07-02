using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UpgradeBuildingItemDetails : ItemDetails {
    public Text upgradeStatus;

    // The placed building SO corresponding to the building we are upgrading
    public PlacedBuilding placedBuilding;

    public Action<PlacedBuilding> OnUpgradeBuilding;

    // Called by button listener on the same GO
    public void UpgradeBuilding() {
        OnUpgradeBuilding.Invoke(placedBuilding);
    }
}
