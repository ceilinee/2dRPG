using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

/// <summary>
/// PlacementController is an ABC that should be extended to implement
/// custom behavior for placing certain objects. Other modules should interact with PlacementController via:
/// (Any sort of custom initialization) -> BeginPlacement -> Pause/Unpause Placement (optional) -> EndPlacement
/// <summary>
public abstract class PlacementController : CustomMonoBehaviour {
    protected enum State {
        Disabled,
        Enabled,
        Paused,
    }

    private State _currState;
    protected State CurrState {
        get => _currState;
        set {
            _currState = value;
            switch (value) {
                case State.Disabled:
                    enabled = false;
                    break;
                case State.Enabled:
                    enabled = true;
                    placeableObjectBlueprint.SetActive(true);
                    break;
                case State.Paused:
                    enabled = false;
                    placeableObjectBlueprint.SetActive(false);
                    break;
            }
        }
    }

    // The prefab of the object we want to place
    protected GameObject placeableObjectPrefab;

    // Set to the object we end up instantiating after the placement is complete
    protected GameObject placeableObject;

    // A skeleton of the prefab; used to display where the placed object will be
    protected GameObject placeableObjectBlueprint;

    // The SpriteRenderer component of placeableObjectBlueprint
    protected SpriteRenderer placeableObjectBlueprintSpriteRenderer;

    [SerializeField]
    private GameObject player;
    private PlayerMovement playerMovement;

    public Color faintRed;
    public Color faintBlue;

    [SerializeField]
    protected BuildingController buildingController;

    protected string GetVirtualSceneName() {
        if (ActiveSceneType() == Loader.Scene.Barn) {
            return buildingController.BuildThisBuildingSceneName();
        }
        return ActiveScene().name;
    }

    protected abstract void LoadSaved();

    protected virtual void Awake() {
        Assert.IsNotNull(player);
        Assert.IsNotNull(buildingController);

        // By default, this script is disabled
        // To activate the placement controller, a module must invoke BeginPlacement
        CurrState = State.Disabled;
        faintRed = new Color(1f, 0f, 0f, 0.25f);
        faintBlue = new Color(0f, 1f, 1f, 0.25f);
        playerMovement = player.GetComponent<PlayerMovement>();

        LoadSaved();
    }

    // Call when the controller is no longer being used to place an item
    protected virtual void ResetState() {
        placeableObjectPrefab = null;
        placeableObject = null;
        Assert.IsNotNull(placeableObjectBlueprint);
        Destroy(placeableObjectBlueprint);
        placeableObjectBlueprint = null;
        placeableObjectBlueprintSpriteRenderer = null;
    }

    // Runs when the player physically places down the item
    protected abstract void PlaceObject();

    protected Vector2 RoundedPlayerPosition() {
        return new Vector2(
            (float) Math.Truncate(player.transform.position.x),
            (float) Math.Truncate(player.transform.position.y)
        );
    }

    /// The blueprint's position should be changed accordingly
    /// depending on the player's current direction and position
    protected abstract void PlayerFaceLeft();
    protected abstract void PlayerFaceUp();
    protected abstract void PlayerFaceRight();
    protected abstract void PlayerFaceDown();

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) &&
            !placeableObjectBlueprint.GetComponent<PlaceableObjectBlueprint>().triggerEntered) {
            PlaceObject();
            return;
        }
        switch (playerMovement.direction) {
            case Direction.Left:
                PlayerFaceLeft();
                break;
            case Direction.Up:
                PlayerFaceUp();
                break;
            case Direction.Right:
                PlayerFaceRight();
                break;
            case Direction.Down:
                PlayerFaceDown();
                break;
            default:
                Assert.IsTrue(false, $"{playerMovement.direction} was not handled");
                break;
        }
    }

    // By the time this function exits:
    // placeableObjectPrefab, placeableObjectBlueprint, placeableObjectBlueprintSpriteRenderer must be set
    protected abstract void PrepareBlueprint();

    /// Returns true if a new placement was started, or false otherwise.
    /// This logic starts a new placement iff none is currently in progress.
    /// Please override (but remember to call this base method) if you need
    /// additional logic, or write a wrapper around this method in the child class.
    protected bool BeginPlacement() {
        if (CurrState == State.Disabled) {
            PrepareBlueprint();
            CurrState = State.Enabled;
            return true;
        }
        return false;
    }

    public void PausePlacement() {
        if (CurrState == State.Enabled) {
            CurrState = State.Paused;
        }
    }

    public void UnpausePlacement() {
        if (CurrState == State.Paused) {
            CurrState = State.Enabled;
        }
    }

    // Invoked when the player wants to stop the placement process
    public void EndPlacement() {
        // If placement is happening or the controller is paused
        if (CurrState == State.Enabled || CurrState == State.Paused) {
            CurrState = State.Disabled;
            ResetState();
        }
    }

    // EndPlacement() if the number of objects we want to place is zero.
    public abstract void EndPlacementIfNoneLeft();

    public void SetSpritesToColor(SpriteRenderer[] spriteRenderers, Color color) {
        foreach (var renderer in spriteRenderers) {
            renderer.color = color;
        }
    }

    protected void AttachPlaceableObjectBlueprintScript(GameObject go) {
        SpriteRenderer[] sprites = go.GetComponentsInChildren<SpriteRenderer>();
        go.AddComponent<PlaceableObjectBlueprint>().SetTriggers(
            () => { SetSpritesToColor(sprites, faintRed); },
            () => { SetSpritesToColor(sprites, faintBlue); }
        );
    }

    protected void ConvertToBlueprint(GameObject go) {
        var components = go.GetComponentsInChildren<Component>();
        var hasCollider = false;
        // Strip all components, except for the transform, sprite renderer, and collider
        foreach (Component c in components) {
            if (!(c is SpriteRenderer) && !(c is Transform) && !(c is Collider2D)) {
                Destroy(c);
            } else if (c is Collider2D) {
                hasCollider = true;
                (c as Collider2D).isTrigger = true;
            }
        }
        if (!hasCollider) {
            go.AddComponent<PolygonCollider2D>();
            go.GetComponent<PolygonCollider2D>().isTrigger = true;
        }
        go.AddComponent<Rigidbody2D>();
        go.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        SpriteRenderer[] sprites = go.GetComponentsInChildren<SpriteRenderer>();
        SetSpritesToColor(sprites, faintBlue);
        AttachPlaceableObjectBlueprintScript(go);
    }
}
