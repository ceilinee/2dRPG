using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Alert : MonoBehaviour {
    public Text question;
    private int id;
    public Action<string, int> confirmFunction;
    public Action<string> nameFunction;
    public GameObject input;
    public GameObject picture;
    public GameObject spawnAnimal;
    public AnimalColors animalColors;
    public GameObject confirmationBackground;
    private bool useSubmitName;

    void Start() {
        input.GetComponent<InputField>().onEndEdit.AddListener(displayText);
    }
    private void displayText(string textInField) {
        print(textInField);
    }
    public void initiateBirthAlert(string newQuestion, Action<string, int> newConfirmFunction, Animal instance, int newId) {
        question.text = newQuestion;
        id = newId;
        confirmFunction = newConfirmFunction;
        instance.animalColors = animalColors;
        spawnAnimal.GetComponent<SpawnAnimal>().setAnimalImage(picture, instance);
        instance.colorAnimal(picture);
        gameObject.SetActive(true);
        if (confirmationBackground) {
            confirmationBackground.SetActive(true);
        }
    }
    public void initiateNameAlert(string newQuestion, Action<string> newNameFunction, bool _submitName = false) {
        useSubmitName = _submitName;
        question.text = newQuestion;
        nameFunction = newNameFunction;
        gameObject.SetActive(true);
        if (confirmationBackground) {
            confirmationBackground.SetActive(true);
        }
    }
    public void submit() {
        if (useSubmitName) {
            submitName();
        } else {
            string name = input.GetComponent<InputField>().text;
            input.GetComponent<InputField>().text = " ";
            confirmFunction(name, id);
            if (confirmationBackground) {
                confirmationBackground.SetActive(false);
            }
            gameObject.SetActive(false);
        }

    }
    public void submitName() {
        string name = input.GetComponent<InputField>().text;
        input.GetComponent<InputField>().text = " ";
        nameFunction(name);
        gameObject.SetActive(false);
        if (confirmationBackground) {
            confirmationBackground.SetActive(false);
        }
    }
}
