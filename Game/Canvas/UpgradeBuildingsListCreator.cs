using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class UpgradeBuildingsListCreator : ListCreator {

    [SerializeField]
    private PlacedBuildings placedBuildings;

    private BuildingController buildingController;

    [SerializeField]
    private Confirmation confirmation;

    [SerializeField]
    private FloatValue playerMoney;

    [SerializeField]
    private BarnUpgrades barnUpgrades;

    public Signal playerMoneyChangedSignal;

    private List<PlacedBuilding> donePlacedBuildings;

    protected void Awake() {
        Assert.IsNotNull(placedBuildings);
    }

    protected void Start() {
        buildingController = centralController.Get(
            "BuildingController").GetComponent<BuildingController>();
    }

    public void GetBuildingUpgrades() {
        donePlacedBuildings = new List<PlacedBuilding>();

        // Only allow upgrades for barns that are done and are not fully upgraded
        foreach (var doneBuilding in placedBuildings.GetBuildings(PlacedBuilding.Status.Done)) {
            if (!doneBuilding.IsMaxUpgrade()) {
                donePlacedBuildings.Add(doneBuilding);
            }
        }

        numberOfItems = donePlacedBuildings.Count;
        PopulateList();
    }

    public override void PopulateList() {
        //setContent Holder Height;
        int numberOfItemsInRow = 4;

        content.sizeDelta = new Vector2(0, System.Math.Max(numberOfItems / numberOfItemsInRow, 3) * 80 + 90);
        for (int i = 0; i < numberOfItems; i++) {
            for (int j = 0; j < numberOfItemsInRow && i + j < numberOfItems; j++) {
                // 60 width of item
                float spawnY = (int) System.Math.Floor((double) i / numberOfItemsInRow) * 100;
                //newSpawn Position
                Vector3 pos = new Vector3(100 * j, -spawnY, 0);
                //instantiate item
                GameObject SpawnedItem = Instantiate(item, pos, SpawnPoint.rotation);
                //setParent
                SpawnedItem.transform.SetParent(SpawnPoint, false);
                //get ItemDetails Component
                var itemDetails = SpawnedItem.GetComponent<UpgradeBuildingItemDetails>();
                var placedBuilding = donePlacedBuildings[i + j];
                var sceneInfo = allBuildings.sceneDict[placedBuilding.sceneInfoId];

                itemDetails.itemName.text = placedBuilding.buildingName;
                itemDetails.itemImage.sprite = sceneInfo.image;
                itemDetails.price.text = "Upgrade cost: $" + barnUpgrades.Get(placedBuilding.GetNextUpgrade()).costForThis;
                itemDetails.upgradeStatus.text = "Current upgrade: " + placedBuilding.GetUpgrade();
                itemDetails.placedBuilding = placedBuilding;
                itemDetails.OnUpgradeBuilding = OnUpgradeBuilding;
                SpawnedItem.SetActive(true);
            }
            i = i + numberOfItemsInRow - 1;
        }
    }

    private void OnUpgradeBuilding(PlacedBuilding placedBuilding) {
        Debug.Log(placedBuilding.buildingId);
        var costToUpgrade = barnUpgrades.CostToUpgrade(placedBuilding.GetUpgrade());

        if (playerMoney.initialValue >= costToUpgrade) {
            confirmation.initiateConfirmation(
                $"Are you sure you want to upgrade this barn to the next level: {placedBuilding.GetNextUpgrade()}?",
                () => {
                    playerMoney.initialValue -= costToUpgrade;
                    playerMoneyChangedSignal.Raise();
                    buildingController.RegisterBuildingUpgrade(placedBuilding);
                    shopInformation.GetComponent<ShopInformation>().updateAbout();
                    shopInformation.GetComponent<ShopInformation>().ShowUpgradeBuildingText(placedBuilding);
                    GetBuildingUpgrades();
                },
                () => { },
                () => { },
                true
            );
        } else {
            shopInformation.GetComponent<ShopInformation>().noMoney();
        }
    }
}
