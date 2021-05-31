using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class ScheduleDictionary : ScriptableObject {
    public DictionaryOfIntAndStringArray travelTimesDict = new DictionaryOfIntAndStringArray();
    public DictionaryOfIntAndCharacterPathArray characterPathDict = new DictionaryOfIntAndCharacterPathArray();
}
[System.Serializable]
public class DictionaryOfIntAndStringArray : SerializableDictionary<int, StringArray> { }

[System.Serializable]
public class DictionaryOfIntAndCharacterPathArray : SerializableDictionary<int, CharacterPathArray> { }

[System.Serializable]
public class StringArray {
    public string[] array;
}
[System.Serializable]
public class CharacterPathArray {
    public CharacterPath[] array;
}