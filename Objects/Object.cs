using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : CustomMonoBehaviour {
    public Item item;
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
        if (Input.GetKeyUp(KeyCode.Space) && playerInRange) {
            buySellAnimal.GetComponent<BuySellAnimal>().pickUpItem(item);
            placedItems.RemoveIfExists(ActiveScene().name, item.id);
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
