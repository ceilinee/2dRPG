using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <script>
/// This script controls the PlayerDesign Variant prefab
/// </script>
public class PlayerDesignComplex : PlayerDesign {
    [Header("The item being held by the player")]
    public GameObject heldItem;

    void Start() {
        RenderUsingPlayerSO();
    }

    public void SetHold(Item item) {
        heldItem.GetComponent<Image>().sprite = item.ItemSprite;
        heldItem.SetActive(true);
        body.sprite = bodyPartManager.bodyHoldingSprite;
        // TODO: change other body part sprites like outfit to holding too once we have the assets for them
    }

    public void UnsetHold() {
        heldItem.GetComponent<Image>().sprite = null;
        heldItem.SetActive(false);
        body.sprite = bodyPartManager.bodySprite;
        // TODO: change other body part sprites like outfit to unholding too once we have the assets for them
    }

    public bool CurrentlyHolding() {
        return heldItem.GetComponent<Image>().sprite != null;
    }
}
