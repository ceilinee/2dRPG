using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


[CreateAssetMenu]
[System.Serializable]
public class ItemDictionary : ScriptableObject {
    public List<Item> itemArray;
    [System.Serializable] public class DictionaryOfItem : SerializableDictionary<string, Item> { }
    public DictionaryOfItem itemDict;

    public void updateItemDict() {
        itemDict = new DictionaryOfItem();
        foreach (Item item in itemArray) {
            Assert.IsFalse(itemDict.ContainsKey(item.Id), "No two items can have the same id!");
            itemDict[item.Id] = item;
        }
    }

    public Item Get(string itemId) {
        Assert.IsTrue(itemDict.ContainsKey(itemId), "Missing Item: " + itemId);
        return itemDict[itemId];
    }
}
