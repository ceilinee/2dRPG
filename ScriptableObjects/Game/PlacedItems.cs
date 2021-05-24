using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlacedItem {
    public PlacedItem(int itemId, Vector2 itemPosition) {
        this.itemId = itemId;
        this.itemPosition = itemPosition;
    }
    public int itemId;
    public Vector2 itemPosition;
}

[CreateAssetMenu]
[System.Serializable]
public class PlacedItems : ScriptableObject {
    public List<PlacedItem> items;

    public void Add(int itemId, Vector2 itemPosition) {
        items.Add(new PlacedItem(itemId, itemPosition));
    }

    public void RemoveIfExists(int itemId) {
        items.RemoveAll(x => x.itemId == itemId);
    }
}
