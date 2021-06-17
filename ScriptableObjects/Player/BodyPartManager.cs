using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class BodyPartManager : ScriptableObject {
    public List<BodyPart> hairStyles = new List<BodyPart>();
    public List<BodyPart> outfits = new List<BodyPart>();
    public List<BodyPart> eyes = new List<BodyPart>();

    public List<BodyPart> bottoms = new List<BodyPart>();

    // A constant, the image of the default naked player
    public Sprite bodySprite;

    // TODO: need to make the correct asset and replace the current placeholder asset
    public Sprite bodyHoldingSprite;
}
