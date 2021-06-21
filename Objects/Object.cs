using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : CustomMonoBehaviour {
    public Item item;

    // An instance of the PlacedItem SO this item corresponds to
    // For an item that is spawned randomly, this field is null
    [Header(Annotation.HeaderMsgDoNotSetInInspector)]
    public PlacedItem placedItem;

    public bool playerInRange;

    [Header(Annotation.HeaderMsgDoNotSetInInspector)]
    public GameObject buySellAnimal;

    [SerializeField]
    private PlacedItems placedItems;

    // Start is called before the first frame update
    void Start() {
        buySellAnimal = centralController.Get("AnimalBuySell");
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("f") && playerInRange) {
            buySellAnimal.GetComponent<BuySellAnimal>().pickUpItem(item);
            placedItems.RemoveIfExists(ActiveScene().name, placedItem);
            Destroy(gameObject);
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
