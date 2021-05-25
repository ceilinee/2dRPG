using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// This controller helps the player move and place items.
/// Other modules should interact with it via
/// BeginPlacement -> Pause/Unpause Placement (optional) -> EndPlacement
/// <summary>
public class PlacementController : MonoBehaviour {
    // The prefab of the object we want to place
    [SerializeField]
    private GameObject placeableObjectPrefab;

    // Set to the object we end up instantiating after the placement is complete
    private GameObject placeableObject;

    // A skeleton of the prefab; used to display where the placed object will be
    private GameObject placeableObjectBlueprint;

    // The SpriteRenderer component of the placeableObjectBlueprint
    private SpriteRenderer placeableObjectBlueprintSpriteRenderer;

    [SerializeField]
    private GameObject player;
    private PlayerMovement playerMovement;

    [SerializeField]
    private SpawnObject spawnObject;

    [SerializeField]
    private PlayerInformation playerInformation;

    [System.Serializable]
    private struct IdPrefabPair {
        // Currently, this id refers to Item.id
        public int id;
        public GameObject prefab;
    }

    [SerializeField]
    private IdPrefabPair[] idPrefabPairs;

    // Constructed from idPrefabPairs; Maps from an item id to the corresponding prefab game
    // object of the item
    private Dictionary<int, GameObject> idToGameObject;

    private Item itemBeingPlaced;
    public GameObject buySellAnimal;

    // Stores and saves the metadata regarding placed items
    [SerializeField]
    private PlacedItems placedItems;

    [SerializeField]
    private Inventory inventory;

    private Color faintRed;
    private Color faintBlue;

    private void ConvertArraystoDictionaries() {
        idToGameObject = new Dictionary<int, GameObject>();
        foreach (IdPrefabPair pair in idPrefabPairs) {
            Assert.IsFalse(idToGameObject.ContainsKey(pair.id),
                "Duplicate id for same game object prefab");
            idToGameObject[pair.id] = pair.prefab;
        }
    }

    // Load all items that were placed in the previous save
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
            placedGameObject.GetComponent<SpriteRenderer>()
                .sprite = item.DirectionToSprite(placedItem.direction);
        }
    }

    void Awake() {
        // By default, this script is disabled
        // To activate the placement controller, a module must invoke BeginPlacement
        enabled = false;
        faintRed = new Color(1f, 0f, 0f, 0.25f);
        faintBlue = new Color(0f, 1f, 1f, 0.25f);
        ConvertArraystoDictionaries();
        playerMovement = player.GetComponent<PlayerMovement>();

        // The player might have previously saved the game, while holding an item
        if (inventory.currentItem != null) {
            BeginPlacement(inventory.currentItemId);
        }
        LoadSavedPlacedItems();
    }

    // Call when the controller is no longer being used to place an item
    private void ResetState() {
        placeableObjectPrefab = null;
        placeableObject = null;
        Assert.IsNotNull(placeableObjectBlueprint);
        Destroy(placeableObjectBlueprint);
        placeableObjectBlueprint = null;
        placeableObjectBlueprintSpriteRenderer = null;
        itemBeingPlaced = null;
    }

    // Runs when the player physically places down the item
    private void PlaceObject() {
        placeableObjectBlueprint.SetActive(false);
        playerInformation.RemoveCurrentItemFromInventory();
        placeableObject = Instantiate(placeableObjectPrefab,
            placeableObjectBlueprint.transform.position,
            placeableObjectBlueprint.transform.rotation);
        placeableObject.GetComponent<Object>().item = itemBeingPlaced;
        placeableObject.GetComponent<Object>().buySellAnimal = buySellAnimal;
        var sprite = placeableObjectBlueprintSpriteRenderer.sprite;
        placeableObject.GetComponent<SpriteRenderer>().sprite = sprite;
        placedItems.Add(itemBeingPlaced.id, placeableObject.transform.position,
            itemBeingPlaced.SpriteToDirection(sprite));
        EndPlacement();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) &&
            !placeableObjectBlueprint.GetComponent<PlaceableObjectBlueprint>().triggerEntered) {
            PlaceObject();
            return;
        }
        // TODO: might need to adjust the `new Vector2(x,x)` values depending on the size of the item
        // TODO: can optimize by storing the `new Vector2` as constants, assuming we don't need to do the TODO above
        switch (playerMovement.direction) {
            case Direction.Left:
                placeableObjectBlueprint.transform.position = (Vector2) player.transform.position + new Vector2(-2, 0);
                placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.sprites.right;
                break;
            case Direction.Up:
                placeableObjectBlueprint.transform.position = (Vector2) player.transform.position + new Vector2(0, 3);
                placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.sprites.down;
                break;
            case Direction.Right:
                placeableObjectBlueprint.transform.position = (Vector2) player.transform.position + new Vector2(2, 0);
                placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.sprites.left;
                break;
            case Direction.Down:
                placeableObjectBlueprint.transform.position = (Vector2) player.transform.position + new Vector2(0, -2);
                placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.sprites.up;
                break;
            default:
                Assert.IsTrue(false, $"{playerMovement.direction} was not handled");
                break;
        }
    }

    public void BeginPlacement(int prefabId) {
        if (!enabled) {
            PrepareBlueprint(prefabId);
            enabled = true;
        }
    }

    public void PausePlacement() {
        if (enabled && placeableObjectBlueprint.activeInHierarchy) {
            placeableObjectBlueprint.SetActive(false);
            // Disabling means this script's update function won't be called
            enabled = false;
        } // Else cannot pause controller as placement has never started or is already paused
    }

    public void UnpausePlacement() {
        if (!enabled && placeableObjectBlueprint != null) {
            placeableObjectBlueprint.SetActive(true);
            enabled = true;
        } // Else trying to unpause controller, but controller was never paused in the first place
    }

    // Invoked when the player stops the placement process, either by placing the object
    // or by deselecting the object when it hasn't been placed yet, or by gifting away or selling
    // the currently held item
    public void EndPlacement() {
        // If placement is happening or the controller is paused
        if (enabled || placeableObjectBlueprint != null) {
            ResetState();
            enabled = false;
        }
    }

    private void PrepareBlueprint(int prefabId) {
        Assert.IsTrue(idToGameObject.ContainsKey(prefabId));
        placeableObjectPrefab = idToGameObject[prefabId];
        itemBeingPlaced = spawnObject.items.Find(x => x.id == prefabId);

        // TODO: a small edge case is that the prefab has a script attached that has side effects
        // If we instantiate the blueprint game object, unexpected changes might happen
        placeableObjectBlueprint = Instantiate(placeableObjectPrefab);
        placeableObjectBlueprintSpriteRenderer = placeableObjectBlueprint.GetComponent<SpriteRenderer>();
        placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.ItemSprite;
        placeableObjectBlueprintSpriteRenderer.color = faintBlue;
        var components = placeableObjectBlueprint.GetComponents<Component>();
        // Strip all components of the blueprint, except for the transform and sprite renderer
        foreach (Component c in components) {
            if (!(c is Transform) && !(c is SpriteRenderer)) {
                Destroy(c);
            }
        }
        placeableObjectBlueprint.AddComponent<PolygonCollider2D>();
        placeableObjectBlueprint.GetComponent<PolygonCollider2D>().isTrigger = true;
        placeableObjectBlueprint.AddComponent<Rigidbody2D>();
        placeableObjectBlueprint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        placeableObjectBlueprint.AddComponent<PlaceableObjectBlueprint>().SetTriggers(
            () => { placeableObjectBlueprintSpriteRenderer.color = faintRed; },
            () => { placeableObjectBlueprintSpriteRenderer.color = faintBlue; }
        );
    }
}
