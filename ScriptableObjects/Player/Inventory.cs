using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Inventory : ScriptableObject {
    public Item currentItem = null;
    [System.Serializable] public class DictionaryOfItems : SerializableDictionary<Item, double> { }
    public DictionaryOfItems items = new DictionaryOfItems();
    public int currentItemId;
    [System.Serializable] public class DictionaryOfInt : SerializableDictionary<int, double> { }
    public DictionaryOfInt itemInt = new DictionaryOfInt();

    public void UpdateInventory(ItemDictionary itemDictionary) {
        foreach (KeyValuePair<int, double> kvp in itemInt) {
            if (itemDictionary.itemDict.ContainsKey(kvp.Key)) {
                items[itemDictionary.itemDict[kvp.Key]] = kvp.Value;
            }
        }
        if (itemDictionary.itemDict.ContainsKey(currentItemId)) {
            currentItem = itemDictionary.itemDict[currentItemId];
        }
    }
    public void Additem(Item itemToAdd) {
        //is the item a key?
        if (!items.ContainsKey(itemToAdd)) {
            items[itemToAdd] = 1;
            itemInt[itemToAdd.id] = 1;
        } else {
            items[itemToAdd] += 1;
            itemInt[itemToAdd.id] += 1;

        }
    }
    public void Clear() {
        items = new DictionaryOfItems();
        currentItem = null;
        currentItemId = 0;
    }
    public void Removeitem(Item itemToAdd) {
        //is the item a key?
        if (items[itemToAdd] == 1) {
            items.Remove(itemToAdd);
            itemInt.Remove(itemToAdd.id);
            if (currentItem == itemToAdd) {
                currentItem = null;
                currentItemId = 0;
            }
        } else if (items[itemToAdd] > 1) {
            items[itemToAdd] -= 1;
            itemInt[itemToAdd.id] -= 1;
        }
    }
    public void RemoveCurrentItem() {
        //is the item a key?
        currentItem = null;
        currentItemId = 0;
    }
}
