using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

public class BuildingController : CustomMonoBehaviour {
    private class BuildingUnderConstruction {
        public int buildingId;
        public Timestamp completionTime;
        public BuildingItem item;
        public GameObject buildingBlueprintInstance;
        public GameObject buildingPrefab;

        public BuildingUnderConstruction(int buildingId, Timestamp ct, BuildingItem i, GameObject bb, GameObject bp) {
            this.buildingId = buildingId;
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

    // `sceneInfo` contains the scene info for building `building`
    // e.g.: if you have placed 2 barns, they will share the same sceneInfo
    public static string BuildBuildingSceneName(SceneInfo sceneInfo, PlacedBuilding building) {
        return BuildBuildingSceneName(sceneInfo, building.buildingId);
    }

    public static string BuildBuildingSceneName(SceneInfo sceneInfo, int buildingId) {
        return sceneInfo.sceneName + "#" + buildingId;
    }


    public static int BuildingIdFromBuildingSceneName(string buildingSceneName) {
        // Debug.Log(buildingSceneName);
        return Int32.Parse(buildingSceneName.Split('#')[1]);
    }

    // Assuming we are currently inside of a building, returns a virtual scene name
    // of the form <actual scene name>#<building id>
    public string BuildThisBuildingSceneName() {
        Assert.IsTrue(ActiveSceneType() == Loader.Scene.Barn);
        return ActiveScene().name + "#" + placedBuildings.buildingEnteredIdx;
    }

    void Start() {
        placementController = centralController.Get("PlacementController").GetComponent<PlacementController>();
        buildingsUnderConstruction = new List<BuildingUnderConstruction>();

        LoadSavedPlacedBuildings();
    }

    // Initialize a barn game object and its children with necessary arguments
    private void SetupCompletedBuilding(GameObject barn, BuildingItem item, int buildingId) {
        barn.GetComponent<Barn>().Initialize(buildingId, item);
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
                SetupCompletedBuilding(buildingObject, item, savedBuilding.buildingId);
            } else {
                buildingObject = Instantiate(buildingInfo.blueprintPrefab);
                placementController.SetSpritesToColor(
                    buildingObject.GetComponentsInChildren<SpriteRenderer>(), placementController.faintBlue);
                buildingsUnderConstruction.Add(
                    new BuildingUnderConstruction(
                        savedBuilding.buildingId,
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
        var newPlacedBuilding = placedBuildings.Add(
            item.id, buildingBlueprintInstance.transform.position, completionTime, item.sceneInfo.id);
        buildingsUnderConstruction.Add(
            new BuildingUnderConstruction(
                newPlacedBuilding.buildingId,
                completionTime,
                item,
                buildingBlueprintInstance,
                buildingPrefab
            )
        );
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
                SetupCompletedBuilding(completedBuilding, building.item, building.buildingId);
                completedBuilding.SetActive(true);
                Destroy(building.buildingBlueprintInstance);
                placedBuildings.SetBuildingCompleted(building.buildingId);
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

    // Signal listener invoked right before a player enters the scene with name `sceneName`
    public void OnPlayerWillSceneTransition(string sceneName) {
        Debug.Log("scene transition to: " + sceneName);
        var scene = (Loader.Scene) Enum.Parse(typeof(Loader.Scene), sceneName);
        if (scene == Loader.Scene.Barn) {
            var player = centralController.Get("Player");
            PlacedBuilding closestBuilding = null;
            float minDistance = float.MaxValue;
            foreach (PlacedBuilding building in placedBuildings.buildings) {
                float distancePlayerToBuilding = Vector2.Distance(player.transform.position, building.itemPosition);
                if (distancePlayerToBuilding < minDistance) {
                    minDistance = distancePlayerToBuilding;
                    closestBuilding = building;
                }
            }
            Assert.IsNotNull(closestBuilding,
                "There must be at least one barn, since this code is run before a player enters a barn");
            placedBuildings.SetBuildingEntered(closestBuilding);
        } else {
            // If we are leaving a building
            if (placedBuildings.buildingEnteredIdx != -1) {
                placedBuildings.buildingEnteredIdx = -1;
            }
        }
    }
}
