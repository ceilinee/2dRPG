using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


[CreateAssetMenu]
[System.Serializable]
public class ItemDictionary : ScriptableObject {
    public List<Item> itemArray;
    [System.Serializable] public class DictionaryOfItem : SerializableDictionary<int, Item> { }
    public DictionaryOfItem itemDict = new DictionaryOfItem();

    public void updateItemDict() {
        foreach (Item item in itemArray) {
            itemDict[item.id] = item;
        }
    }

    public Item Get(int itemId) {
        Assert.IsTrue(itemDict.ContainsKey(itemId), "Missing Item: " + itemId);
        return itemDict[itemId];
    }
}
