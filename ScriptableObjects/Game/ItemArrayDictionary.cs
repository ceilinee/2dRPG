using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class ItemArrayDictionary : ScriptableObject {
    public DictionaryOfIntAnditemArrayArray personalityItemArray = new DictionaryOfIntAnditemArrayArray();
    public itemArrayArray test = new itemArrayArray();
}

[System.Serializable]
public class DictionaryOfIntAnditemArrayArray : SerializableDictionary<int, itemArrayArray> { }

[System.Serializable]
public class itemArrayArray {
    public ItemArray[] array;
}