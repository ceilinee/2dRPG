using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "pet" || other.gameObject.tag == "Player" || other.gameObject.tag == "character") {
            other.gameObject.transform.Find("Water").gameObject.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "pet" || other.gameObject.tag == "Player" || other.gameObject.tag == "character") {
            other.gameObject.transform.Find("Water").gameObject.SetActive(false);
        }
    }
    public PolygonCollider2D getValidSwimLocation(Vector2 curSpot) {
        //find collider that curspot is in
        PolygonCollider2D[] colliders = gameObject.GetComponents<PolygonCollider2D>();
        PolygonCollider2D selectedCollider = null;
        foreach (PolygonCollider2D collider in colliders) {
            if (collider.bounds.Contains(curSpot)) {
                selectedCollider = collider;
                break;
            }
        }
        return selectedCollider;
    }
}
