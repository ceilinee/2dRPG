using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// This controller helps the player move and place items.
/// Other modules should interact with it via
/// BeginPlacement -> Pause/Unpause Placement (optional) -> EndPlacement
/// <summary>
public class PlacementController : CustomMonoBehaviour {
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
    private struct BasicItemInfo {
        // Currently, this id refers to Item.id
        public int id;
        public GameObject prefab;
    }

    [SerializeField]
    private BasicItemInfo[] basicItemInfoList;

    // Constructed from idPrefabPairs; Maps from an item id to the corresponding prefab game
    // object of the item
    private Dictionary<int, GameObject> basicItemInfoDict;

    // Stores the information necessary for placing & building buildings
    [System.Serializable]
    public class BuildingInfo {
        public int buildingItemId;
        public GameObject blueprintPrefab;
        public GameObject prefab;
    }

    [SerializeField]
    private List<BuildingInfo> buildingInfoList;

    private Item itemBeingPlaced;
    public GameObject buySellAnimal;

    // Stores and saves the metadata regarding placed items
    [SerializeField]
    private PlacedItems placedItems;

    [SerializeField]
    private Inventory inventory;

    private Color faintRed;
    private Color faintBlue;

    [SerializeField]
    private Confirmation confirmation;

    private void ConvertArraystoDictionaries() {
        basicItemInfoDict = new Dictionary<int, GameObject>();
        foreach (BasicItemInfo pair in basicItemInfoList) {
            Assert.IsFalse(basicItemInfoDict.ContainsKey(pair.id),
                "Duplicate id for same game object prefab");
            basicItemInfoDict[pair.id] = pair.prefab;
        }
    }

    // Load all items that were placed in the previous save
    private void LoadSavedPlacedItems() {
        foreach (PlacedItem placedItem in placedItems.items) {
            var id = placedItem.itemId;
            var prefab = basicItemInfoDict[-1];
            var item = spawnObject.items.Find(x => x.id == id);
            var placedGameObject = Instantiate(prefab);
            placedGameObject.transform.position = placedItem.itemPosition;
            placedGameObject.GetComponent<Object>().item = item;
            placedGameObject.GetComponent<Object>().buySellAnimal = buySellAnimal;
            placedGameObject.GetComponent<SpriteRenderer>()
                .sprite = item.DirectionToSprite(placedItem.direction);
            placedGameObject.SetActive(true);
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
        if (itemBeingPlaced is BuildingItem) {
            confirmation.initiateConfirmation("Are you sure you want to building your house here?", () => {
                placeableObject = Instantiate(placeableObjectBlueprint);
                Destroy(placeableObject.GetComponent<PlaceableObjectBlueprint>());
                var buildingController = centralController.Get("BuildingController").GetComponent<BuildingController>();
                var prefab = buildingInfoList.Find(x => x.buildingItemId == itemBeingPlaced.id).prefab;
                buildingController.RegisterBuildingCreation((BuildingItem) itemBeingPlaced, placeableObject, prefab);
                playerInformation.RemoveCurrentItemFromInventory();
                EndPlacementIfNoItemsLeft();
            }, () => { }, () => { Time.timeScale = 1; });
        } else {
            placeableObject = Instantiate(placeableObjectPrefab,
                placeableObjectBlueprint.transform.position,
                placeableObjectBlueprint.transform.rotation);

            placeableObject.GetComponent<Object>().item = itemBeingPlaced;
            placeableObject.GetComponent<Object>().buySellAnimal = buySellAnimal;

            var sprite = placeableObjectBlueprintSpriteRenderer.sprite;
            placeableObject.GetComponent<SpriteRenderer>().sprite = sprite;
            placeableObject.SetActive(true);
            placedItems.Add(itemBeingPlaced.id, placeableObject.transform.position,
                            itemBeingPlaced.SpriteToDirection(sprite));
            playerInformation.RemoveCurrentItemFromInventory();
            EndPlacementIfNoItemsLeft();
        }
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
                placeableObjectBlueprint.transform.position = (Vector2) player.transform.position + new Vector2(itemBeingPlaced is BuildingItem ? -5 : -2, 0);
                placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.sprites.right;
                break;
            case Direction.Up:
                placeableObjectBlueprint.transform.position = (Vector2) player.transform.position + new Vector2(0, itemBeingPlaced is BuildingItem ? 4 : 3);
                placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.sprites.down;
                break;
            case Direction.Right:
                placeableObjectBlueprint.transform.position = (Vector2) player.transform.position + new Vector2(itemBeingPlaced is BuildingItem ? 5 : 2, 0);
                placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.sprites.left;
                break;
            case Direction.Down:
                placeableObjectBlueprint.transform.position = (Vector2) player.transform.position + new Vector2(0, itemBeingPlaced is BuildingItem ? -6.5f : -2);
                placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.sprites.up;
                break;
            default:
                Assert.IsTrue(false, $"{playerMovement.direction} was not handled");
                break;
        }
    }

    public void BeginPlacement(int prefabId) {
        // Begin placement only if either we are not placing an item,
        // or we are placing some item that is a different item
        if (!enabled) {
            PrepareBlueprint(prefabId);
            enabled = true;
        } else if (prefabId != itemBeingPlaced.id) {
            ResetState();
            PrepareBlueprint(prefabId);
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

    // Invoked when the player wants to stop the placement process
    public void EndPlacement() {
        // If placement is happening or the controller is paused
        if (enabled || placeableObjectBlueprint != null) {
            ResetState();
            enabled = false;
        }
    }

    public void EndPlacementIfNoItemsLeft() {
        // If there are more instances of the currently held item, we do not stop placement
        if (playerInformation.CountOfItemInInventory(itemBeingPlaced) == 0) {
            EndPlacement();
        }
    }

    private void SetSpritesToColor(SpriteRenderer[] spriteRenderers, Color color) {
        foreach (var renderer in spriteRenderers) {
            renderer.color = color;
        }
    }

    // prefabId is the id of the Item being placed 
    private void PrepareBlueprint(int prefabId) {
        Assert.IsNotNull(inventory.currentItem);
        itemBeingPlaced = inventory.currentItem;

        if (itemBeingPlaced is BuildingItem) {
            placeableObjectPrefab = buildingInfoList.Find(
                x => x.buildingItemId == prefabId).blueprintPrefab;
        } else {
            // TODO: modify this logic to make it cleaner and more correct
            // Right now, all basic items use the Object prefab as their game object
            // But I'm not sure if this will change in the future
            placeableObjectPrefab = basicItemInfoDict[-1];
        }
        // An edge case is that the prefab has a script attached that has side effects in awake
        // If we instantiate the blueprint game object, unexpected changes might happen
        // So we set it to be inactive first just to be safe
        placeableObjectPrefab.SetActive(false);
        placeableObjectBlueprint = Instantiate(placeableObjectPrefab);
        Assert.IsFalse(placeableObjectBlueprint.activeInHierarchy);
        placeableObjectBlueprintSpriteRenderer = placeableObjectBlueprint.GetComponent<SpriteRenderer>();

        if (itemBeingPlaced is BuildingItem) {
            // TODO: for now, no need to do anything, as the blueprint is hardcoded
        } else {
            placeableObjectBlueprintSpriteRenderer.sprite = itemBeingPlaced.ItemSprite;
            var components = placeableObjectBlueprint.GetComponentsInChildren<Component>();
            // Strip all components of the blueprint, except for the transform and sprite renderer
            foreach (Component c in components) {
                if (!(c is SpriteRenderer) && !(c is Transform)) {
                    Destroy(c);
                }
            }
            placeableObjectBlueprint.AddComponent<PolygonCollider2D>();
            placeableObjectBlueprint.GetComponent<PolygonCollider2D>().isTrigger = true;
            placeableObjectBlueprint.AddComponent<Rigidbody2D>();
            placeableObjectBlueprint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
        SpriteRenderer[] sprites = placeableObjectBlueprint.GetComponentsInChildren<SpriteRenderer>();
        SetSpritesToColor(sprites, faintBlue);

        placeableObjectBlueprint.AddComponent<PlaceableObjectBlueprint>().SetTriggers(
            () => { SetSpritesToColor(sprites, faintRed); },
            () => { SetSpritesToColor(sprites, faintBlue); }
        );
        placeableObjectBlueprint.SetActive(true);
    }

    public BuildingInfo GetBuildingInfo(int buildingItemId) {
        var res = buildingInfoList.Find(x => x.buildingItemId == buildingItemId);
        Assert.IsNotNull(res);
        return res;
    }
}
