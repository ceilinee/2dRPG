using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Children : GenericCharacter {
    protected override void Update() {
        if (Input.GetKeyUp(KeyCode.Space) && playerInRange) {
            if (!conversation && !DialogueManager.activeInHierarchy) {
                if (playerInventory.currentItem != null) {
                    if (!CanvasController.activeInHierarchy) {
                        CanvasController.SetActive(true);
                    }
                    if (CanvasController.GetComponent<CanvasController>().openCanvas()) {
                        if (characterTrait.presentsDaily <= 2) {
                            if (characterTrait.presentsDaily == 0) {
                                spawnAnimal.GetComponent<SpawnAnimal>().playerGiftCharacter(characterTrait.id);
                            }
                            giveGift();
                        } else {
                            StartDialogue(characterTrait.characterEnoughGiftSpeech[characterTrait.friendshipScore / 100]);
                        }
                    }
                } else {
                    charDialogue();
                }
            }
        }
    }
}
