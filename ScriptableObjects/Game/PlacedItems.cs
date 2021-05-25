using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlacedItem {
    public PlacedItem(int itemId, Vector2 itemPosition, Direction direction) {
        this.itemId = itemId;
        this.itemPosition = itemPosition;
        this.direction = direction;
    }
    public int itemId;
    public Vector2 itemPosition;

    public Direction direction;
}

/// <summary>
/// This scriptable object stores metadata of all the items on the map
/// including their current orientation, location, and the associated item.
/// On start up, the placement controller uses this data to instantiate the correct
/// game objects
/// <summary>
[CreateAssetMenu]
[System.Serializable]
public class PlacedItems : ScriptableObject {
    public List<PlacedItem> items;

    public void Add(int itemId, Vector2 itemPosition, Direction direction) {
        items.Add(new PlacedItem(itemId, itemPosition, direction));
    }

    public void RemoveIfExists(int itemId) {
        items.RemoveAll(x => x.itemId == itemId);
    }
}
