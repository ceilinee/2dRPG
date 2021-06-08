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
    //for adding items easily
    public List<int> itemIntList;

    public double GetItemCount(int id = -1, string itemName = "") {
        if (id != -1 && itemInt.ContainsKey(id)) {
            return itemInt[id];
        }
        foreach (KeyValuePair<Item, double> kvp in items) {
            if (kvp.Key.itemName == itemName) {
                return kvp.Value;
            }
        }
        return 0;
    }
    public Item GetItem(int id = -1, string itemName = "") {
        foreach (KeyValuePair<Item, double> kvp in items) {
            if (kvp.Key.itemName == itemName || kvp.Key.id == id) {
                return kvp.Key;
            }
        }
        return null;
    }
    public void UpdateInventory(ItemDictionary itemDictionary) {
        foreach (KeyValuePair<int, double> kvp in itemInt) {
            if (itemDictionary.itemDict.ContainsKey(kvp.Key)) {
                items[itemDictionary.itemDict[kvp.Key]] = kvp.Value;
            }
        }
        foreach (int id in itemIntList) {
            if (itemDictionary.itemDict.ContainsKey(id) && !itemInt.ContainsKey(id)) {
                items[itemDictionary.itemDict[id]] = 40;
                itemInt[id] = 40;
            }
        }
        if (itemDictionary.itemDict.ContainsKey(currentItemId)) {
            currentItem = itemDictionary.itemDict[currentItemId];
        }
    }
    public double TotalCost(bool buy = true) {
        double totalCost = 0;
        foreach (KeyValuePair<Item, double> kvp in items) {
            totalCost += buy ? kvp.Key.cost * kvp.Value : kvp.Key.sellCost * kvp.Value;
        }
        return totalCost;
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
    public void AddInventory(Inventory inventory) {
        foreach (KeyValuePair<Item, double> kvp in inventory.items) {
            Item itemToAdd = kvp.Key;
            if (!items.ContainsKey(itemToAdd)) {
                items[itemToAdd] = kvp.Value;
                itemInt[itemToAdd.id] = kvp.Value;
            } else {
                items[itemToAdd] += kvp.Value;
                itemInt[itemToAdd.id] += kvp.Value;
            }
        }
    }
    public void Clear() {
        items = new DictionaryOfItems();
        itemInt = new DictionaryOfInt();
        currentItem = null;
        currentItemId = 0;
    }
    public void Removeitem(Item itemToAdd) {
        //is the item a key?
        if (items[itemToAdd] == 1) {
            items.Remove(itemToAdd);
            itemInt.Remove(itemToAdd.id);
            if (currentItem == itemToAdd) {
                UnsetCurrentItem();
            }
        } else if (items[itemToAdd] > 1) {
            items[itemToAdd] -= 1;
            itemInt[itemToAdd.id] -= 1;
        }
    }
    public void UnsetCurrentItem() {
        //is the item a key?
        currentItem = null;
        currentItemId = 0;
    }
    public void RemoveAllItem(Item item) {
        items.Remove(item);
        itemInt.Remove(item.id);
        if (currentItem == item) {
            UnsetCurrentItem();
        }
    }
    public double CountOf(Item item) {
        return item != null && items.ContainsKey(item) ? items[item] : 0;
    }
}
