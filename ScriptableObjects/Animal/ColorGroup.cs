using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu]
[System.Serializable]
public class ColorGroup : CustomScriptableObject {
    [Header("A list of keys that refer to the entries in SO AnimalColors.colorDictionary.")]
    public List<int> colorDictKeys;
}
