using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BuildingController : CustomMonoBehaviour {
    private class BuildingUnderConstruction {
        public Timestamp completionTime;
        public BuildingItem item;
        public GameObject buildingBlueprintInstance;
        public GameObject buildingPrefab;

        public BuildingUnderConstruction(Timestamp ct, BuildingItem i, GameObject bb, GameObject bp) {
            this.completionTime = ct;
            this.item = i;
            this.buildingBlueprintInstance = bb;
            this.buildingPrefab = bp;
        }
    }

    private List<BuildingUnderConstruction> buildingsUnderConstruction;

    [SerializeField]
    private CurTime curTime;

    // Used for saving buildings
    [SerializeField]
    private PlacedBuildings placedBuildings;

    [SerializeField]
    private ItemDictionary itemDictionary;

    private PlacementController placementController;

    void Start() {
        placementController = centralController.Get("PlacementController").GetComponent<PlacementController>();
        buildingsUnderConstruction = new List<BuildingUnderConstruction>();

        LoadSavedPlacedBuildings();
    }

    // Load buildings that have been placed.
    // In addition, populate buildingsUnderConstruction with buildings that have yet
    // to been built
    public void LoadSavedPlacedBuildings() {
        foreach (PlacedBuilding savedBuilding in placedBuildings.buildings) {
            Assert.IsTrue(itemDictionary.itemDict.ContainsKey(savedBuilding.buildingItemId));
            BuildingItem item = (BuildingItem) itemDictionary.itemDict[savedBuilding.buildingItemId];
            var buildingInfo = placementController.GetBuildingInfo(savedBuilding.buildingItemId);
            GameObject buildingObject = null;
            if (savedBuilding.completed) {
                buildingObject = Instantiate(buildingInfo.prefab);
                var sceneTransition = buildingObject.GetComponentInChildren<SceneTransition>();
                sceneTransition.SetSceneInfo(item.sceneInfo);
            } else {
                buildingObject = Instantiate(buildingInfo.blueprintPrefab);
                placementController.SetSpritesToColor(
                    buildingObject.GetComponentsInChildren<SpriteRenderer>(), placementController.faintBlue);
                buildingsUnderConstruction.Add(
                    new BuildingUnderConstruction(
                        savedBuilding.completionTime,
                        item,
                        buildingObject,
                        buildingInfo.prefab
                    )
                );
            }
            buildingObject.transform.position = savedBuilding.itemPosition;
            buildingObject.SetActive(true);
        }
    }

    // RegisterBuildingCreation is invoked after a building blueprint has been placed.
    // `buildingBlueprint` represents the foundation of the building
    // After 2 days, the real house (an instance of buildingPrefab) should be built by this controller
    public void RegisterBuildingCreation(
        BuildingItem item, GameObject buildingBlueprintInstance, GameObject buildingPrefab) {
        Timestamp completionTime = curTime.DaysFromNow(2);
        buildingsUnderConstruction.Add(
            new BuildingUnderConstruction(
                completionTime,
                item,
                buildingBlueprintInstance,
                buildingPrefab
            )
        );
        placedBuildings.Add(item.id, buildingBlueprintInstance.transform.position, completionTime);
    }

    // Called by SignalListener
    public void OnTimeUpdate() {
        HashSet<int> removedIdxs = new HashSet<int>();
        for (int i = 0; i < buildingsUnderConstruction.Count; ++i) {
            BuildingUnderConstruction building = buildingsUnderConstruction[i];
            if (curTime.isCurrentTimeBigger(building.completionTime)) {
                removedIdxs.Add(i);
                var completedBuilding = Instantiate(building.buildingPrefab,
                    building.buildingBlueprintInstance.transform.position, building.buildingBlueprintInstance.transform.rotation);
                var sceneTransition = completedBuilding.GetComponentInChildren<SceneTransition>();
                sceneTransition.SetSceneInfo(building.item.sceneInfo);
                completedBuilding.SetActive(true);
                Destroy(building.buildingBlueprintInstance);
                placedBuildings.SetBuildingCompleted(building.item.id);
            }
        }
        if (removedIdxs.Count > 0) {
            var temp = buildingsUnderConstruction;
            buildingsUnderConstruction = new List<BuildingUnderConstruction>();
            for (int i = 0; i < temp.Count; ++i) {
                if (!removedIdxs.Contains(i)) {
                    buildingsUnderConstruction.Add(temp[i]);
                }
            }
        }
    }
}
