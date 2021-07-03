using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Speed {
    SLOW,
    CLASSIC,
    FAST
}
[CreateAssetMenu]
[System.Serializable]
public class Settings : CustomScriptableObject, IClearable {
    public Speed speed = Speed.CLASSIC;
    public Speed geese = Speed.CLASSIC;
    public Filter filter = null;
    public string musicId;
    public void Clear() {
        speed = Speed.CLASSIC;
        geese = Speed.CLASSIC;
        filter = null;
    }

    public bool HasFilterSet() {
        return filter != null && filter.name != null && filter.name != "";
    }
}
