using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : CustomMonoBehaviour {
    public GameObject dialogueBox;
    public Text dialogueText;
    public Text dialogueName;
    public Image portraitImage;
    public CanvasController canvasController;
    public bool progress;
    public Dialogue curDialogue;
    public GameObject character;
    public bool goose;
    public Sprite gooseMock;
    public Dialogue gooseDialogue;
    private bool hasSelectedChoice;
    private int selectedChoice;
    public GameObject selection;
    private Button choice0;
    private Button choice1;
    private Button choice2;
    void Update() {
        if (Input.GetKeyUp(KeyCode.Space) && !progress) {
            progress = true;
        }
    }
    void Start() {
        fetchDependencies();
    }
    private void fetchDependencies() {
        selection = centralController.Get("Selection");
        canvasController = centralController.Get("CanvasController").GetComponent<CanvasController>();

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
    public void select0() {
        closeSelection();
        selectedChoice = 0;
        hasSelectedChoice = true;
    }
    public void select1() {
        closeSelection();
        selectedChoice = 1;
        hasSelectedChoice = true;
    }
    public void select2() {
        closeSelection();
        selectedChoice = 2;
        hasSelectedChoice = true;
    }
    //Set up all the buttons
    public void subscribe(string[] choices) {
        choice0 = selection.transform.Find("Shop").gameObject.GetComponent<Button>();
        choice0.onClick.AddListener(select0);
        selection.transform.Find("Shop").Find("ConfirmText").gameObject.GetComponent<Text>().text = choices[0];
        if (choices.Length == 2) {
            choice1 = selection.transform.Find("Talk").gameObject.GetComponent<Button>();
            selection.transform.Find("Talk").Find("ConfirmText").gameObject.GetComponent<Text>().text = choices[1];
            choice1.onClick.AddListener(select1);
        }
        if (choices.Length == 3) {
            choice2 = selection.transform.Find("Give").gameObject.GetComponent<Button>();
            selection.transform.Find("Give").Find("ConfirmText").gameObject.GetComponent<Text>().text = choices[2];
            choice2.onClick.AddListener(select2);
            choice2.gameObject.SetActive(true);
        }
    }
    public void closeSelection() {
        canvasController.closeAllCanvas();
        choice0.onClick.RemoveListener(select0);
        choice1.onClick.RemoveListener(select1);
        if (choice2) {
            choice2.onClick.RemoveListener(select2);
            choice2.gameObject.SetActive(false);
        }
    }
    //display potential choices
    public void displayChoices(string[] choices) {
        canvasController.openCanvasAgain();
        selection.SetActive(true);
        subscribe(choices);
    }
    IEnumerator waitReadChoiceDialogue(ChoiceDialogue choiceDialogue) {
        //read initial things
        for (int i = 0; i < choiceDialogue.sentence.Length; i++) {
            Debug.Log(choiceDialogue.sentence.Length);
            progress = false;
            updateDialogueBox(choiceDialogue.sentence[i]);
            if (i < choiceDialogue.portrait.Length) {
                portraitImage.sprite = goose ? gooseMock : character.GetComponent<GenericCharacter>().characterTrait.portrait[choiceDialogue.portrait[i]];
            }
            while (!progress) yield return null;
        }
        hasSelectedChoice = false;
        //display choices
        displayChoices(choiceDialogue.choices);
        while (!hasSelectedChoice) yield return null;
        //show choice dialogue
        canvasController.openCanvas(false);
        dialogueBox.SetActive(true);
        Dialogue choiceResponse = choiceDialogue.choiceResponse[selectedChoice];
        for (int i = 0; i < choiceResponse.sentence.Length; i++) {
            Debug.Log(hasSelectedChoice);
            progress = false;
            updateDialogueBox(choiceResponse.sentence[i]);
            if (i < choiceResponse.portrait.Length) {
                portraitImage.sprite = goose ? gooseMock :
                character.GetComponent<GenericCharacter>().characterTrait.portrait[choiceResponse.portrait[i]];
            }
            while (!progress) yield return null;
        }
        //consequences
        if (!goose && selectedChoice < choiceDialogue.choicesConsequence.Length) {
            character.GetComponent<GenericCharacter>().increaseFriendship(choiceDialogue.choicesConsequence[selectedChoice]);
        }
    }
    IEnumerator waitRead(Dialogue dialogue = null) {
        if (!canvasController || !selection) {
            fetchDependencies();
        }
        if (dialogue == null) {
            dialogue = curDialogue;
        }
        Debug.Log(dialogue is ChoiceDialogue);
        dialogueName.text = goose ? "A Goose" : character.GetComponent<GenericCharacter>().characterTrait.name;
        portraitImage.sprite = goose ? gooseMock : character.GetComponent<GenericCharacter>().characterTrait.portrait[0];
        // if dialogue is selectable dialogue
        if (dialogue is ChoiceDialogue) {
            yield return waitReadChoiceDialogue((ChoiceDialogue) dialogue);
        } else {
            for (int i = 0; i < dialogue.sentence.Length; i++) {
                progress = false;
                updateDialogueBox(dialogue.sentence[i]);
                if (i < dialogue.portrait.Length) {
                    portraitImage.sprite = goose ? gooseMock : character.GetComponent<GenericCharacter>().characterTrait.portrait[curDialogue.portrait[i]];
                }
                while (!progress) yield return null;
            }
        }
        if (!goose) {
            character.GetComponent<GenericCharacter>().conversation = false;
        }
        dialogueBox.SetActive(false);
        progress = false;
        gameObject.SetActive(false);
        canvasController.closeCanvas();
    }
    public void updateDialogueBox(string words) {
        dialogueText.text = words;
    }
}
