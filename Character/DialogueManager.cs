using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    public GameObject dialogueBox;
    public Text dialogueText;
    public Text dialogueName;
    public Image portraitImage;
    public GameObject CanvasController;
    public bool progress;
    public Dialogue curDialogue;
    public GameObject character;
    public bool goose;
    public Sprite gooseMock;
    public Dialogue gooseDialogue;

    void Update() {
        if (Input.GetKeyUp(KeyCode.Space) && !progress) {
            progress = true;
        }
    }
    public void startDialog(GameObject newCharacter, Dialogue newDialogue) {
        gameObject.SetActive(true);
        dialogueBox.SetActive(true);
        PauseGame();
        goose = false;
        curDialogue = newDialogue;
        character = newCharacter;
        StartCoroutine(waitRead());
    }
    public void startGooseDialog() {
        gameObject.SetActive(true);
        dialogueBox.SetActive(true);
        PauseGame();
        curDialogue = gooseDialogue;
        goose = true;
        portraitImage.sprite = gooseMock;
        StartCoroutine(waitRead());
    }
    public void PauseGame() {
        Time.timeScale = 0;
    }
    public void ResumeGame() {
        Time.timeScale = 1;
    }
    IEnumerator waitRead() {
        dialogueName.text = goose ? "A Goose" : character.GetComponent<GenericCharacter>().characterTrait.name;
        for (int i = 0; i < curDialogue.sentence.Length; i++) {
            progress = false;
            updateDialogueBox(curDialogue.sentence[i]);
            if (i < curDialogue.portrait.Length) {
                portraitImage.sprite = goose ? gooseMock : character.GetComponent<GenericCharacter>().characterTrait.portrait[curDialogue.portrait[i]];
            }
            while (!progress) yield return null;
        }
        if (!goose) {
            character.GetComponent<GenericCharacter>().conversation = false;
        }
        dialogueBox.SetActive(false);
        progress = false;
        gameObject.SetActive(false);
        CanvasController.GetComponent<CanvasController>().closeCanvas();
    }
    public void updateDialogueBox(string words) {
        dialogueText.text = words;
    }
}
