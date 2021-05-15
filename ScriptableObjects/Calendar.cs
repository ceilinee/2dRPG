using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Date {
    public int date;
    public int eventId;
    public int birthdayCharId;
}
[System.Serializable]
public class DateArray {
    public Date[] dates;
}
[CreateAssetMenu]
[System.Serializable]
public class Calendar : ScriptableObject {
    public int[] season;
    public Date[] dates;
    public DateArray dateArray;
    [System.Serializable] public class DictionaryOfSeasons : SerializableDictionary<int, DateArray> { }
    public DictionaryOfSeasons seasonDict = new DictionaryOfSeasons();
}
