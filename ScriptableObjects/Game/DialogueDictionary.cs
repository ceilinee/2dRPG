using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class DialogueDictionary : ScriptableObject {
    // contains 
    public DictionaryOfIntAndDialogueArrayArray personalityDialogueArrayArrayDict = new DictionaryOfIntAndDialogueArrayArray();
    public DictionaryOfIntAndDialogueArray personalityDialogueArrayDict = new DictionaryOfIntAndDialogueArray();
    public DictionaryOfIntAndDialogueArray personalityDialogueDict = new DictionaryOfIntAndDialogueArray();

}
[System.Serializable]
public class DictionaryOfIntAndDialogueArrayArray : SerializableDictionary<int, dialogueArrayArray> { }

[System.Serializable]
public class DictionaryOfIntAndDialogueArray : SerializableDictionary<int, dialogueArray> { }

[System.Serializable]
public class DictionaryOfIntAndDialogue : SerializableDictionary<int, Dialogue> { }

[System.Serializable]
public class dialogueArrayArray {
    public dialogueArray[] dialogue;
}
