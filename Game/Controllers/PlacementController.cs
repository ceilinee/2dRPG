using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlacementController : MonoBehaviour {
    [SerializeField]
    private GameObject placeableObjectPrefab;

    private GameObject placeableObject;

    private GameObject placeableObjectBlueprint;

    [SerializeField]
    private GameObject player;

    private PlayerMovement playerMovement;

    [SerializeField]
    private SpawnObject spawnObject;

    [SerializeField]
    private PlayerInformation playerInformation;

    [System.Serializable]
    private struct IdPrefabPair {
        // TODO: right now, id refers to Item.id
        public int id;
        public GameObject prefab;
    }

    [SerializeField]
    private IdPrefabPair[] idPrefabPairs;

    private Dictionary<int, GameObject> idToGameObject;

    // TODO: want to change the variables below; ideally placed objects should know how to initialize themselves from some data structure like an Item object
    private Item item;
    public GameObject buySellAnimal;

    [SerializeField]
    private PlacedItems placedItems;

    private void ConvertArraystoDictionaries() {
        idToGameObject = new Dictionary<int, GameObject>();
        foreach (IdPrefabPair pair in idPrefabPairs) {
            Assert.IsFalse(idToGameObject.ContainsKey(pair.id),
                "Duplicate id for same game object prefab");
            idToGameObject[pair.id] = pair.prefab;
        }
    }

    private void LoadSavedPlacedItems() {
        foreach (PlacedItem placedItem in placedItems.items) {
            var id = placedItem.itemId;
            Assert.IsTrue(idToGameObject.ContainsKey(id));
            var prefab = idToGameObject[id];
            var item = spawnObject.items.Find(x => x.id == id);
            var placedGameObject = Instantiate(prefab);
            placedGameObject.transform.position = placedItem.itemPosition;
            placedGameObject.GetComponent<Object>().item = item;
            placedGameObject.GetComponent<Object>().buySellAnimal = buySellAnimal;
            placedGameObject.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
        }
    }

    void Awake() {
        // By default, this script is disabled, until BeginPlacement is called
        enabled = false;
        ConvertArraystoDictionaries();
        playerMovement = player.GetComponent<PlayerMovement>();

        LoadSavedPlacedItems();
    }

    // Call when the controller is no longer being used to place an item
    private void ResetState() {
        placeableObjectPrefab = null;
        placeableObject = null;
        Assert.IsNotNull(placeableObjectBlueprint);
        Destroy(placeableObjectBlueprint);
        placeableObjectBlueprint = null;
    }

    // Runs when the player physically places down the item
    private void PlaceObject() {
        placeableObjectBlueprint.SetActive(false);
        playerInformation.RemoveCurrentItemFromInventory();
        placeableObject = Instantiate(placeableObjectPrefab, placeableObjectBlueprint.transform.position,
            placeableObjectBlueprint.transform.rotation);
        placeableObject.GetComponent<Object>().item = item;
        placeableObject.GetComponent<Object>().buySellAnimal = buySellAnimal;
        placeableObject.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
        placedItems.Add(item.id, placeableObject.transform.position);
        EndPlacement();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && !placeableObjectBlueprint.GetComponent<Interactable>().triggerEntered) {
            PlaceObject();
            return;
        }
        // TODO: save the current direction to optimize
        if (playerMovement.direction == Direction.Left) {
            placeableObjectBlueprint.transform.position = (Vector2) player.transform.position + new Vector2(-3, 0);
        } else if (playerMovement.direction == Direction.Up) {
            placeableObjectBlueprint.transform.position = (Vector2) player.transform.position + new Vector2(0, 4);
        } else if (playerMovement.direction == Direction.Right) {
            placeableObjectBlueprint.transform.position = (Vector2) player.transform.position + new Vector2(3, 0);
        } else if (playerMovement.direction == Direction.Down) {
            placeableObjectBlueprint.transform.position = (Vector2) player.transform.position + new Vector2(0, -3);
        } else Assert.IsTrue(false);

        // TODO: use callbacks to set color, shouldn't use getComponent in update
        if (placeableObjectBlueprint.GetComponent<Interactable>().triggerEntered) {
            placeableObjectBlueprint.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.25f);
        } else {
            placeableObjectBlueprint.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 1f, 0.25f);
        }
    }

    public void BeginPlacement(int prefabId) {
        if (!enabled) {
            PrepareBlueprint(prefabId);
            // Disabling means this script's update function won't be called
            enabled = true;
        }
    }

    // Invoked when the player stops the placement process, either by placing the object
    // or by deselecting the object when it hasn't been placed yet
    public void EndPlacement() {
        if (enabled) {
            ResetState();
            enabled = false;
        }
    }

    // Must call this function before making this script active
    private void PrepareBlueprint(int prefabId) {
        Assert.IsTrue(idToGameObject.ContainsKey(prefabId));
        placeableObjectPrefab = idToGameObject[prefabId];

        item = spawnObject.items.Find(x => x.id == prefabId);

        // Do we want to instantiate here? What if the script attached to the instantiated objects has side effects?
        placeableObjectBlueprint = Instantiate(placeableObjectPrefab);
        placeableObjectBlueprint.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
        // TODO: use new Color(1, 0, 0, 0.25) to give the sprite a reddish tint
        placeableObjectBlueprint.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 1f, 0.25f);
        var components = placeableObjectBlueprint.GetComponents<Component>();
        foreach (Component c in components) {
            if (!(c is Transform) && !(c is SpriteRenderer)) {
                Destroy(c);
            }
        }
        placeableObjectBlueprint.AddComponent<PolygonCollider2D>();
        placeableObjectBlueprint.GetComponent<PolygonCollider2D>().isTrigger = true;
        placeableObjectBlueprint.AddComponent<Rigidbody2D>();
        placeableObjectBlueprint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        placeableObjectBlueprint.AddComponent<Interactable>();
    }
}
