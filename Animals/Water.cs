using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : CustomMonoBehaviour {
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "pet" || other.gameObject.tag == "Player" || other.gameObject.tag == "character" || other.gameObject.tag == "fish") {
            other.gameObject.transform.Find("Water").gameObject.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "pet" || other.gameObject.tag == "Player" || other.gameObject.tag == "character" || other.gameObject.tag == "fish") {
            other.gameObject.transform.Find("Water").gameObject.SetActive(false);
        }
    }

    public Collider2D getValidSwimLocation(Vector2 curSpot) {
        //find collider that curspot is in
        Collider2D[] colliders = gameObject.GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders) {
            if (collider.OverlapPoint(curSpot)) {
                return collider;
            }
        }
        return null;
    }
}
