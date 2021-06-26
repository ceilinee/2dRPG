using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is for controlling the PlayerDesign prefab or variants thereof
/// </summary>
public class PlayerDesign : MonoBehaviour {
    public Image body;

    [Header("The currently selected hairstyle")]
    public Image hair;

    [Header("The currently selected outfit")]
    public Image outfit;
    [Header("The currently selected bottom")]
    public Image bottom;

    [Header("The currently selected eyes")]
    public Image eyes;

    public Player player;

    public BodyPartManager bodyPartManager;

    [Header("The source of truth for supported colors in the game")]
    public AnimalColors colors;

    public void UpdateHair(Sprite sprite, Color color) {
        hair.sprite = sprite;
        hair.color = color;
    }

    public void UpdateOutfit(Sprite sprite) {
        outfit.sprite = sprite;
    }
    public void UpdateBottom(Sprite sprite) {
        Debug.Log(sprite);
        bottom.sprite = sprite;
    }

    public void UpdateEyes(Sprite sprite, Color color) {
        eyes.sprite = sprite;
        eyes.color = color;
    }

    public void UpdateBody(Color color) {
        body.color = color;
    }

    // Updates the body parts of the player using the saved appearance in Player SO
    public void RenderUsingPlayerSO() {
        var appearance = player.appearance;
        UpdateHair(
            bodyPartManager.hairStyles[appearance.hairId].bodyPartSprite,
            colors.colorDictionary[appearance.hairColorId].color
        );
        UpdateOutfit(bodyPartManager.outfits[appearance.outfitId].bodyPartSprite);
        UpdateEyes(
            bodyPartManager.eyes[appearance.eyesId].bodyPartSprite,
            colors.colorDictionary[appearance.eyeColorId].color
        );
        UpdateBody(colors.colorDictionary[appearance.skinColorId].color);
        UpdateBottom(bodyPartManager.bottoms[appearance.bottomId].bodyPartSprite);
    }
}
