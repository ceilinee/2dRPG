using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class PlacedItem {
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
/// This scriptable object stores metadata of all the items for each scene,
/// including their current orientation, location, and the associated item.
/// On start up, the placement controller uses this data to instantiate the correct
/// game objects
/// <summary>
[CreateAssetMenu]
[System.Serializable]
public class PlacedItems : ScriptableObject {
    // Need to subclass List<PlacedItem> and use that as the type of the values in sceneToPlacedItems
    // in order to have sceneToPlacedItems serialize properly by the GSM (see https://answers.unity.com/questions/460727/how-to-serialize-dictionary-with-unity-serializati.html)
    [System.Serializable] public class ItemList : List<PlacedItem> { }
    [System.Serializable] public class SceneToPlacedItems : SerializableDictionary<string, ItemList> { }
    public SceneToPlacedItems sceneToPlacedItems = new SceneToPlacedItems();

    public List<PlacedItem> GetPlacedItems(string sceneName) {
        if (!sceneToPlacedItems.ContainsKey(sceneName)) {
            return new List<PlacedItem>();
        }
        return sceneToPlacedItems[sceneName];
    }

    public void Add(string sceneName, int itemId, Vector2 itemPosition, Direction direction) {
        if (!sceneToPlacedItems.ContainsKey(sceneName)) {
            sceneToPlacedItems[sceneName] = new ItemList();
        }
        sceneToPlacedItems[sceneName].Add(new PlacedItem(itemId, itemPosition, direction));
    }

    public void RemoveIfExists(string sceneName, int itemId) {
        if (sceneToPlacedItems.ContainsKey(sceneName)) {
            sceneToPlacedItems[sceneName].RemoveAll(x => x.itemId == itemId);
        }
    }

    public void Clear() {
        sceneToPlacedItems = new SceneToPlacedItems();
    }
}
