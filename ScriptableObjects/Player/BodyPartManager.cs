using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class BodyPartManager : ScriptableObject {
    public List<BodyPart> hairStyles = new List<BodyPart>();
    public List<BodyPart> outfits = new List<BodyPart>();
    public List<BodyPart> eyes = new List<BodyPart>();
}
