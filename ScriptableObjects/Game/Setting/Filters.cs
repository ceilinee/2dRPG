using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Filter {
    public string name;
    public Color nightTimeColour;
    public Color dayTimeColour;
    public Color dawnTimeColour;
    public Color canvasColour;
}
[CreateAssetMenu]
[System.Serializable]
public class Filters : ScriptableObject {
    public List<Filter> filters;
}

