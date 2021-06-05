using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CustomScriptableObject : ScriptableObject {
    public void ForceSerialization() {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
