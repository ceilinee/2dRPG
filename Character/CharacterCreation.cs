using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CharacterCreation : MonoBehaviour {
    public Action<string> nameFunction;
    public GameObject input;

    public void initiateNameAlert(Action<string> newNameFunction) {
        nameFunction = newNameFunction;
        gameObject.SetActive(true);
    }

    public void submitName() {
        string name = input.GetComponent<InputField>().text;
        input.GetComponent<InputField>().text = " ";
        nameFunction(name);
        gameObject.SetActive(false);
    }
}
