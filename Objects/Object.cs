using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour {
    public Item item;
    public bool playerInRange;
    public GameObject buySellAnimal;

    [SerializeField]
    private PlacedItems placedItems;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyUp(KeyCode.Space) && playerInRange) {
            buySellAnimal.GetComponent<BuySellAnimal>().pickUpItem(item);
            placedItems.RemoveIfExists(item.id);
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerInRange = false;
        }
    }
}
