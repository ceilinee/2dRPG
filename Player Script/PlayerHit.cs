using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <script>
/// This script is only meant to be used by the player's hitboxes (for now)
/// It handles behavior for the player dealing damage to other colliders
/// </script>
public class PlayerHit : MonoBehaviour {
    [Header(Annotation.HeaderMsgSetInInspector)]
    [Header("The amount of damage this GO should deal")]
    public float damage;

    private void Awake() {
        Assert.IsTrue(damage > 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        var hittable = other.GetComponent<IHittable>();
        hittable?.BeHit(damage);
    }
}
