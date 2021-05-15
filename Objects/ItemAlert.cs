using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemAlert : MonoBehaviour {
    public Image image;
    // Start is called before the first frame update
    void Start() {

    }
    public void startAlert(Item item) {
        image.sprite = item.itemSprite;
        gameObject.SetActive(true);
        StartCoroutine(waitSpawn());
    }
    IEnumerator waitSpawn() {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update() {

    }
}
