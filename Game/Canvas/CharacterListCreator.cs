using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CharacterListCreator : MonoBehaviour {
    // [SerializeField]
    public Transform SpawnPoint = null;
    //
    // [SerializeField]
    public GameObject item = null;
    //
    // [SerializeField]
    public RectTransform content = null;
    //
    // [SerializeField]
    private int numberOfItems = 3;
    //
    // public GameObject playerInformation;
    // public GameObject CharacterManager;
    public Characters curCharacters;
    public Character[] curCharactersSorted;
    // public string type;
    // public string gender;

    // Use this for initialization
    void Start() {

    }
    public void Clear() {
        foreach (Transform child in SpawnPoint.transform) {
            // GameObject.Destroy(child.gameObject);
            child.gameObject.SetActive(false);
        }
    }
    public void GetCharacters() {
        List<Character> templist = new List<Character>();
        foreach (KeyValuePair<int, Character> kvp in curCharacters.characterDict) {
            templist.Add(kvp.Value);
        }
        curCharactersSorted = templist.ToArray();
        PopulateList();
        // curCharactersSorted = Array.Sort(templist.ToArray(), delegate(Character char1, Character char2) {
        //     return char1.friendshipScore.CompareTo(char2.friendshipScore);
        // });
    }
    public void PopulateList() {
        //setContent Holder Height;
        int numberOfItemsInRow = 5;
        numberOfItems = curCharactersSorted.Length;
        content.sizeDelta = new Vector2(0, System.Math.Max(numberOfItems / numberOfItemsInRow, 3) * 80 + 90);
        for (int i = 0; i < numberOfItems; i++) {
            for (int j = 0; j < numberOfItemsInRow && i + j < numberOfItems; j++) {
                // 60 width of item
                float spawnY = (int) System.Math.Floor((double) i / numberOfItemsInRow) * 80;
                //newSpawn Position
                Vector3 pos = new Vector3(80 * j, -spawnY, 0);
                //instantiate item
                GameObject SpawnedItem = Instantiate(item, pos, SpawnPoint.rotation);
                //setParent
                SpawnedItem.transform.SetParent(SpawnPoint, false);
                //get ItemDetails Component
                CharacterItem characterItem = SpawnedItem.GetComponent<CharacterItem>();
                //set name
                characterItem.text.text = curCharactersSorted[i + j].name;
                //set image
                characterItem.image.sprite = curCharactersSorted[i + j].image;

                characterItem.friendship.text = curCharactersSorted[i + j].determineFriendship();
                characterItem.friendship.color = curCharactersSorted[i + j].determineFriendshipColor();
            }
            i = i + numberOfItemsInRow - 1;
        }
    }
}
