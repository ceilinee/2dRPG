using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    public GameObject dialogueBox;
    public Text dialogueText;
    public Image portraitImage;
    public GameObject CanvasController;
    public bool progress;
    public Dialogue curDialogue;
    public GameObject character;

    void Update() {
        if (Input.GetKeyUp(KeyCode.Space) && !progress) {
            progress = true;
        }
    }
    public void startDialog(GameObject newCharacter, Dialogue newDialogue) {
        dialogueBox.SetActive(true);
        PauseGame();
        curDialogue = newDialogue;
        character = newCharacter;
        StartCoroutine(waitRead());
    }
    public void PauseGame() {
        Time.timeScale = 0;
    }
    public void ResumeGame() {
        Time.timeScale = 1;
    }
    IEnumerator waitRead() {
        for (int i = 0; i < curDialogue.sentence.Length; i++) {
            progress = false;
            updateDialogueBox(curDialogue.sentence[i]);
            if (i < curDialogue.portrait.Length) {
                portraitImage.sprite = character.GetComponent<GenericCharacter>().characterTrait.portrait[curDialogue.portrait[i]];
            }
            while (!progress) yield return null;
        }
        character.GetComponent<GenericCharacter>().conversation = false;
        dialogueBox.SetActive(false);
        progress = false;
        gameObject.SetActive(false);
        CanvasController.GetComponent<CanvasController>().closeCanvas();
    }
    public void updateDialogueBox(string words) {
        dialogueText.text = words;
    }
}
