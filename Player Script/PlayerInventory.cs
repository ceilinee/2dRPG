using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
    public Inventory playerInventory;
    public GameObject player;
    // Start is called before the first frame update
    void Start() {
        if (playerInventory.currentItem != null) {
            gameObject.GetComponent<SpriteRenderer>().sprite = playerInventory.currentItem.ItemHeldSprite;
            player.GetComponent<PlayerMovement>().setHold();
        }
    }
    public void updateSprite() {
        gameObject.GetComponent<SpriteRenderer>().sprite = playerInventory.currentItem.ItemHeldSprite;
        player.GetComponent<PlayerMovement>().setHold();
    }
    public void removeSprite() {
        gameObject.GetComponent<SpriteRenderer>().sprite = null;
        player.GetComponent<PlayerMovement>().setUnhold();
    }
}
