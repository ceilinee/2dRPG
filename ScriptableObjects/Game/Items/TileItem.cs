using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// TileItem represents an item that can be placed as a tile on a tilemap
[CreateAssetMenu]
[System.Serializable]
public class TileItem : Item {
    public RuleTile tile;

    public bool isCollidable;
}
