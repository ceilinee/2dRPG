using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu]
[System.Serializable]
public class ItemList : ScriptableObject {
    // List of Item ids; use ItemDictionary to get the actual Item at runtime
    public List<string> list;

    // Populated at runtime using `list`
    [Header("Do not populate in inspector!")]
    public List<Item> itemList;

    // The default list of ids; you may want to use this to store initial values
    // and transfer to `list` When a new game is created
    public List<string> defaultList;

    public void Remove(string itemId) {
        Assert.IsTrue(list.Exists(id => id == itemId));
        list.RemoveAll(id => id == itemId);
        itemList.RemoveAll(item => item.Id == itemId);
    }

    // Called when a game save is deleted
    public void Clear() {
        list = new List<string>(defaultList);
        itemList = new List<Item>();
    }
}
