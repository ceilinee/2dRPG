using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu]
[System.Serializable]

public class Item : ScriptableObject {
    [System.Serializable]
    public struct Sprites {
        // down stores the sprite for this item when the item is facing down
        public Sprite down;
        // TODO: We still need to create the assets for the sprites below
        public Sprite right;
        public Sprite left;
        public Sprite up;
    }

    public string itemName;
    public int id;

    // Readonly property that gets the sprite in its default orientation
    public Sprite ItemSprite {
        get => sprites.up;
    }
    // Every item has 4 different sprites, corresponding to its 4 different orientations
    public Sprites sprites;
    public int healthBonus;
    public int antiAge;
    public string itemDescription;
    public float cost;
    public bool date;
    public bool marriage;
    public bool divorce;
    public float sellCost;
    public int moveSpeed;
    public Square[] spawnLocations;
    public int[] spawnProbability;

    // Once a user picks up this item for the first time, this will be set to true
    // and will never change afterwords
    public bool pickedUpAtLeastOnce;

    // Given sprite, return the direction the sprite is associated with
    // Every item has 4 different sprites, representing the 4 different orientations
    public Direction SpriteToDirection(Sprite sprite) {
        if (sprite == sprites.down) {
            return Direction.Down;
        }
        if (sprite == sprites.right) {
            return Direction.Right;
        }
        if (sprite == sprites.left) {
            return Direction.Left;
        }
        if (sprite == sprites.up) {
            return Direction.Up;
        }
        Assert.IsTrue(false, $"Provided sprite is not associated with this item ({itemName})");
        return Direction.Up; // Return Up to pass the static analyzer, but this loc will never be executed
    }

    // The inverse of SpriteToDirection
    public Sprite DirectionToSprite(Direction direction) {
        switch (direction) {
            case Direction.Down:
                return sprites.down;
            case Direction.Right:
                return sprites.right;
            case Direction.Left:
                return sprites.left;
            case Direction.Up:
                return sprites.down;
            default:
                Assert.IsTrue(false,
                    $"Provided direction is not associated with a sprite of item {itemName}");
                return sprites.up;
        }
    }
}
