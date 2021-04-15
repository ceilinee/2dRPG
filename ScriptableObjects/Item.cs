using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]

public class Item : ScriptableObject
{
    public string itemName;
    public int id;
    public Sprite itemSprite;
    public int healthBonus;
    public int antiAge;
    public string itemDescription;
    public float cost;
    public float sellCost;
    public Square[] spawnLocations;
    public int[] spawnProbability;
}
