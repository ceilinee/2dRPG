using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <script>
/// CustomTree is a script that should be attached to all tree GOs
/// The script is not called "Tree" because Tree is the name of a built-in Unity component
/// </script>
public class CustomTree : Breakable {
    // TODO: update this in the inspector after we get real wood assets
    [SerializeField] private GameObject woodPrefab;

    private PlacementController placementController;

    private void Awake() {
        Assert.IsNotNull(woodPrefab);
    }

    private void Start() {
        placementController = centralController.Get("PlacementController").GetComponent<PlacementController>();
        Assert.IsNotNull(placementController);
    }

    protected override void OnBroken() {
        var wood = Instantiate(woodPrefab, transform.position, transform.rotation);
        var placedItem = placementController.AddToPlacedItems(
            wood.GetComponent<Object>().item, wood.transform.position, Direction.Down);
        wood.GetComponent<Object>().placedItem = placedItem;
    }
}
