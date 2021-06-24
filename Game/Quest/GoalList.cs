using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GoalList : MonoBehaviour {
    // [SerializeField]
    public Transform SpawnPoint = null;
    //
    // [SerializeField]
    public GameObject item = null;
    public RectTransform content = null;
    private int numberOfItems = 3;
    public Quest[] quests;
    public GameObject SettingsMenu;

    void Start() {

    }
    public void Clear() {
        foreach (Transform child in SpawnPoint.transform) {
            // GameObject.Destroy(child.gameObject);
            child.gameObject.SetActive(false);
        }
    }
    public void PopulateList() {
        //setContent Holder Height;
        int numberOfItemsInRow = 1;
        numberOfItems = quests.Length;
        content.sizeDelta = new Vector2(0, System.Math.Max(numberOfItems / numberOfItemsInRow, 3) * 45);
        for (int i = 0; i < numberOfItems; i++) {
            float spawnY = (int) System.Math.Floor((double) i / numberOfItemsInRow) * 41;
            //newSpawn Position
            Vector3 pos = new Vector3(107.9f, 18 + -spawnY, 0);
            //instantiate item
            GameObject SpawnedItem = Instantiate(item, pos, SpawnPoint.rotation);
            //setParent
            SpawnedItem.transform.SetParent(SpawnPoint, false);
            GoalItem goalItem = SpawnedItem.GetComponent<GoalItem>();
            goalItem.updateDetails(quests[i]);
            goalItem.settingsMenu = SettingsMenu;
            //goal item's calculation
        }
    }
}
