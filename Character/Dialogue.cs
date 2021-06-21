using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {
    public string[] sentence;
    public int[] portrait;
}
[System.Serializable]
public class ChoiceDialogue : Dialogue {
    public bool selectable = false;
    public string[] choices = null;
    public Dialogue[] choiceResponse = null;
    public int[] choicesConsequence = null;
}