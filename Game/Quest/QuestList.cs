using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class QuestList : MonoBehaviour {
    // [SerializeField]
    public Transform SpawnPoint = null;
    //
    // [SerializeField]
    public GameObject item = null;
    public RectTransform content = null;
    private int numberOfItems = 3;
    public Quests quests;
    public GameObject questInformation;
    public Characters charList;

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
        numberOfItems = quests.curQuests.Length;
        content.sizeDelta = new Vector2(0, System.Math.Max(numberOfItems / numberOfItemsInRow, 3) * 45);
        for (int i = 0; i < numberOfItems; i++) {
            float spawnY = (int) System.Math.Floor((double) i / numberOfItemsInRow) * 45;
            //newSpawn Position
            Vector3 pos = new Vector3(35.49f, 15 + -spawnY, 0);
            //instantiate item
            GameObject SpawnedItem = Instantiate(item, pos, SpawnPoint.rotation);
            //setParent
            SpawnedItem.transform.SetParent(SpawnPoint, false);
            QuestItem questItem = SpawnedItem.GetComponent<QuestItem>();
            questItem.updateDetails(quests.curQuests[i]);
            questItem.questInformation = questInformation;
            questItem.charName.text = charList.characterDict[quests.curQuests[i].posterCharId].name;
            if (charList.characterDict[quests.curQuests[i].posterCharId].image) {
                questItem.charImage.sprite = charList.characterDict[quests.curQuests[i].posterCharId].image;
            }
        }
    }
}
