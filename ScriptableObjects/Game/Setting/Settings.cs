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
public class Settings : ScriptableObject {
    public Speed speed = Speed.CLASSIC;
    public Speed geese = Speed.CLASSIC;
    public Filter filter;
    public string musicId;
}
