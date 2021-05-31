using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Event {
    public int eventId;
    public string eventName;
    public Sprite eventIcon;
    public Color color;
}

[CreateAssetMenu]
[System.Serializable]
public class Events : ScriptableObject {
    public Event[] events;
    [System.Serializable] public class DictionaryOfEvents : SerializableDictionary<int, Event> { }
    public DictionaryOfEvents eventDict = new DictionaryOfEvents();
}
