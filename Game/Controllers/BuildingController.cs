using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour {
    public SceneInfos playerBuildings;
    public GameObject[] buildings;
    public SceneInfos allBuildings;
    public int[] ids;
    public CharacterManager.DictionaryOfIntAndGameObject buildingGameObjectDictionary = new CharacterManager.DictionaryOfIntAndGameObject();

    void Start() {
        allBuildings.updateSceneDict();
        for (int i = 0; i < buildings.Length; i++) {
            buildingGameObjectDictionary[ids[i]] = buildings[i];
            buildings[i].SetActive(false);
        }
        updateBuildings();
    }
    // Start is called before the first frame update
    public void updateBuildings() {
        foreach (int id in playerBuildings.sceneArray) {
            buildingGameObjectDictionary[id].SetActive(true);
        }
    }
}
