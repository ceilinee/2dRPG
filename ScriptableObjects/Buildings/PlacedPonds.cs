using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

[System.Serializable]
public class PlacedPond {
    public string name;

    public Vector2 position;
}

/// <summary>
/// This scriptable object stores metadata of all the placed ponds (ie not including
/// the natural bodies of water that are inherently part of the map)
/// <summary>
[CreateAssetMenu]
[System.Serializable]
public class PlacedPonds : CustomScriptableObject, IClearable {
    public List<PlacedPond> ponds;

    public void Clear() {
        ponds = new List<PlacedPond>();
    }

    public PlacedPond Get(string pondName) {
        return ponds.Find(x => x.name == pondName);
    }

    public void Add(string pondName, Vector2 position) {
        Assert.IsTrue(Get(pondName) == null);
        ponds.Add(new PlacedPond() { name = pondName, position = position });
    }
}
