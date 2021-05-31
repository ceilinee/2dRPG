using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu]
[System.Serializable]
public class ItemList : ScriptableObject {
    public List<Item> list;

    public void Remove(int itemId) {
        Assert.IsTrue(list.Exists(item => item.id == itemId));
        list.RemoveAll(item => item.id == itemId);
    }
}
