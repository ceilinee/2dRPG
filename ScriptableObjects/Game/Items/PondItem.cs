using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class PondItem : Item {
    // Some sort of toolkit
    public Sprite itemHeldSprite;

    // Readonly property that gets the sprite of the item when it's held
    public override Sprite ItemHeldSprite {
        get => itemHeldSprite;
    }
}
