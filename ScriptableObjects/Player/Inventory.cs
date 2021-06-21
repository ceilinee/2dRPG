using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu]
[System.Serializable]
public class Inventory : ScriptableObject {
    public Item currentItem = null;
    [System.Serializable] public class DictionaryOfItems : SerializableDictionary<Item, double> { }
    public DictionaryOfItems items = new DictionaryOfItems();
    public string currentItemId;
    [System.Serializable] public class DictionaryOfString : SerializableDictionary<string, double> { }
    public DictionaryOfString itemString = new DictionaryOfString();
    //for adding items easily
    public List<string> itemStringList;

    public double GetItemCount(string id = "", string itemName = "") {
        if (id != "" && itemString.ContainsKey(id)) {
            return itemString[id];
        }
        foreach (KeyValuePair<Item, double> kvp in items) {
            if (kvp.Key.itemName == itemName) {
                return kvp.Value;
            }
        }
        return 0;
    }
    public Item GetItem(string id = "", string itemName = "") {
        foreach (KeyValuePair<Item, double> kvp in items) {
            if (kvp.Key.itemName == itemName || kvp.Key.Id == id) {
                return kvp.Key;
            }
        }
        return null;
    }
    public void UpdateInventory(ItemDictionary itemDictionary) {
        foreach (KeyValuePair<string, double> kvp in itemString) {
            if (itemDictionary.itemDict.ContainsKey(kvp.Key)) {
                items[itemDictionary.itemDict[kvp.Key]] = kvp.Value;
            }
        }
        foreach (string id in itemStringList) {
            Assert.IsTrue(
                itemDictionary.itemDict.ContainsKey(id),
                $"Item {id} cannot be added to inventory properly");
            items[itemDictionary.itemDict[id]] = 40;
            itemString[id] = 40;
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
            itemString[itemToAdd.Id] = 1;
        } else {
            items[itemToAdd] += 1;
            itemString[itemToAdd.Id] += 1;
        }
    }
    public void AddInventory(Inventory inventory) {
        foreach (KeyValuePair<Item, double> kvp in inventory.items) {
            Item itemToAdd = kvp.Key;
            if (!items.ContainsKey(itemToAdd)) {
                items[itemToAdd] = kvp.Value;
                itemString[itemToAdd.Id] = kvp.Value;
            } else {
                items[itemToAdd] += kvp.Value;
                itemString[itemToAdd.Id] += kvp.Value;
            }
        }
    }
    public void Clear() {
        items = new DictionaryOfItems();
        itemString = new DictionaryOfString();
        currentItem = null;
        currentItemId = "";
    }
    public void Removeitem(Item itemToAdd) {
        //is the item a key?
        if (items[itemToAdd] == 1) {
            items.Remove(itemToAdd);
            itemString.Remove(itemToAdd.Id);
            if (currentItem == itemToAdd) {
                UnsetCurrentItem();
            }
        } else if (items[itemToAdd] > 1) {
            items[itemToAdd] -= 1;
            itemString[itemToAdd.Id] -= 1;
        }
    }
    public void UnsetCurrentItem() {
        //is the item a key?
        currentItem = null;
        currentItemId = "";
    }
    public void RemoveAllItem(Item item) {
        items.Remove(item);
        itemString.Remove(item.Id);
        if (currentItem == item) {
            UnsetCurrentItem();
        }
    }
    public double CountOf(Item item) {
        return item != null && items.ContainsKey(item) ? items[item] : 0;
    }
}
