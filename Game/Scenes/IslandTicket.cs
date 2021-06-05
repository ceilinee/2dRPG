using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IslandTicket : SceneTransition {

    // Start is called before the first frame update
    private CanvasController canvasController;
    private GameObject shop;
    private DialogueManager dialogueManager;
    public Dialogue rangerDialogue;
    public GameObject ranger;
    public BuySellAnimal buySellAnimal;

    protected override void Start() {
        base.Start();
        canvasController = centralController.Get("CanvasController").GetComponent<CanvasController>();
        shop = centralController.Get("ItemShop");
        dialogueManager = centralController.Get("DialogueManager").GetComponent<DialogueManager>();
        buySellAnimal = centralController.Get("AnimalBuySell").GetComponent<BuySellAnimal>();
    }
    IEnumerator confirmIsland() {
        dialogueManager.startDialog(ranger, rangerDialogue);
        while (dialogueManager.gameObject.activeInHierarchy) {
            yield return null;
        }
        confirmation.initiateConfirmation(
                    "Want to head to the island for $200?",
                    () => confirmTravel(),
                    () => { },
                    () => { },
                    false
                );
    }
    public void confirmTravel() {
        if (buySellAnimal.payForService(200.0f)) {
            PlayerTransition();
        } else {
            canvasController.initiateNotification("Sorry, doesn't seem like you have enough there..");
        }
    }
    // Update is called once per frame
    protected override void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag(TagOfPlayer) && !other.isTrigger) {
            if (confirmation) {
                StartCoroutine(confirmIsland());
            } else {
                PlayerTransition();
            }
        } else if (other.CompareTag("pet") && !other.isTrigger) {
            curAnimals.animalDict[other.gameObject.GetComponent<GenericAnimal>().animalTrait.id].scene = sceneinfo.sceneName;
            curAnimals.animalDict[other.gameObject.GetComponent<GenericAnimal>().animalTrait.id].location = sceneinfo.entrance;
            animalList.GetComponent<AnimalList>().removeAnimal(other.gameObject.GetComponent<GenericAnimal>().animalTrait.id);
            other.gameObject.SetActive(false);
        }
    }
}
