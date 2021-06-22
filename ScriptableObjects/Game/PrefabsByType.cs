using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyPrefabPair<T> {
    public T key;
    public GameObject gameObject;
}

[CreateAssetMenu]
[System.Serializable]
public class PrefabsByType : CustomScriptableObject {
    [System.Serializable]
    public class KeyPrefabPairType : KeyPrefabPair<ItemPrefabType> { }
    public List<KeyPrefabPairType> keyPrefabPairs;

    public GameObject FindPrefab(ItemPrefabType type) {
        return keyPrefabPairs.Find(x => x.key == type).gameObject;
    }
}
