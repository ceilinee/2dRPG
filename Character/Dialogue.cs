using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class Dialogue {
    public string[] sentence;
    public int[] portrait;
    public bool displayed = false;
    public bool oneTime = false;
    public List<string> itemId;
    public string GetItemId() {
        string[] itemArray = itemId.ToArray();
        return itemArray[Random.Range(0, itemArray.Length)];
    }
}
[System.Serializable]
public class ChoiceDialogue : Dialogue {
    public bool selectable = false;
    public string[] choices = null;
    public Dialogue[] choiceResponse = null;
    public int[] choicesConsequence = null;
}