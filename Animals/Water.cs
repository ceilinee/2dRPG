using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "pet" || other.gameObject.tag == "Player") {
            other.gameObject.transform.Find("Water").gameObject.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "pet" || other.gameObject.tag == "Player") {
            other.gameObject.transform.Find("Water").gameObject.SetActive(false);
        }
    }
}
