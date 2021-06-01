using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LlamaStatue : Interactable {
    private BuySellAnimal buySellAnimal;
    public Inventory inventory;
    public Forest forest;
    public SceneTransition sceneTransition;
    private Notification notification;
    // Start is called before the first frame update
    void Start() {
        notification = centralController.Get("Notification").GetComponent<Notification>();
        buySellAnimal = centralController.Get("AnimalBuySell").GetComponent<BuySellAnimal>();
    }
    public void CalculateStars() {
        // if stars
        int curcount = (int) inventory.GetItemCount(itemName: "Star");
        Item star = inventory.GetItem(itemName: "Star");
        if (curcount >= forest.starCount) {
            for (int i = 0; i < forest.starCount; i++) {
                buySellAnimal.sellItem(star);
            }
            notification.initiateNotification("The Llama Statue took "
+ forest.starCount + " Stars from you for $" + star.sellCost * forest.starCount + ", and am starting to open the next area..");
            StartCoroutine(NextLevel());
        } else {
            notification.initiateNotification("The Llama Statue says you don't have enough stars! It needs "
            + forest.starCount + " Stars to take you to the next area, you currently have " + curcount);
        }
    }
    IEnumerator NextLevel() {
        while (notification.gameObject.activeInHierarchy) {
            yield return null;
        }
        sceneTransition.PlayerTransition();
    }
    // Update is called once per frame
    void Update() {
        if (Input.GetKeyUp(KeyCode.Space) && playerInRange) {
            CalculateStars();
        }
    }
}
