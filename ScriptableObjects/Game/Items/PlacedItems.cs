using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class PlacedItem {
    public PlacedItem(string itemId, Vector2 itemPosition, Direction direction) {
        this.itemId = itemId;
        this.itemPosition = itemPosition;
        this.direction = direction;
    }
    public string itemId;
    public Vector2 itemPosition;

    public Direction direction;
}

/// <summary>
/// This scriptable object stores metadata of all the items for each scene,
/// including their current orientation, location, and the associated item.
/// On start up, the placement controller uses this data to instantiate the correct
/// game objects
/// <summary>
[CreateAssetMenu]
[System.Serializable]
public class PlacedItems : CustomScriptableObject, IClearable {
    // Need to compose List<PlacedItem> and use that as the type of the values in sceneToPlacedItems
    // in order to have sceneToPlacedItems serialize properly by the GSM (see https://answers.unity.com/questions/460727/how-to-serialize-dictionary-with-unity-serializati.html)
    [System.Serializable]
    public class ListPlacedItem {
        public List<PlacedItem> placedItems = new List<PlacedItem>();
        public List<PlacedItem> Get() { return placedItems; }
    }
    [System.Serializable] public class SceneToPlacedItems : SerializableDictionary<string, ListPlacedItem> { }
    public SceneToPlacedItems sceneToPlacedItems;

    public List<PlacedItem> GetPlacedItems(string sceneName) {
        if (!sceneToPlacedItems.ContainsKey(sceneName)) {
            return new List<PlacedItem>();
        }
        return sceneToPlacedItems[sceneName].Get();
    }

    /// Returns the newly created PlacedItem instance
    public PlacedItem Add(string sceneName, string itemId, Vector2 itemPosition, Direction direction) {
        if (!sceneToPlacedItems.ContainsKey(sceneName)) {
            sceneToPlacedItems[sceneName] = new ListPlacedItem();
        }
        var placedItem = new PlacedItem(itemId, itemPosition, direction);
        sceneToPlacedItems[sceneName].Get().Add(placedItem);
        return placedItem;
    }

    public void RemoveIfExists(string sceneName, PlacedItem placedItem) {
        if (sceneToPlacedItems.ContainsKey(sceneName)) {
            sceneToPlacedItems[sceneName].Get().Remove(placedItem);
            if (sceneToPlacedItems[sceneName].Get().Count == 0) {
                sceneToPlacedItems.Remove(sceneName);
            }
        }
    }

    public List<PlacedItem> RemoveIfExists(string sceneName, Vector2 position) {
        List<PlacedItem> removed = new List<PlacedItem>();
        if (sceneToPlacedItems.ContainsKey(sceneName)) {
            foreach (PlacedItem placedItem in sceneToPlacedItems[sceneName].Get()) {
                if (placedItem.itemPosition == position) {
                    removed.Add(placedItem);
                }
            }
            sceneToPlacedItems[sceneName].Get().RemoveAll(x => x.itemPosition == position);
            if (sceneToPlacedItems[sceneName].Get().Count == 0) {
                sceneToPlacedItems.Remove(sceneName);
            }
        }
        return removed;
    }

    public void Clear() {
        sceneToPlacedItems = new SceneToPlacedItems();
    }
}
