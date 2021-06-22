using UnityEngine;


[System.Serializable]
public class CustomScriptableObject : ScriptableObject {
    [TextArea]
    [Tooltip("Just comments shown in the inspector.")]
    public string notes = "N/A";

    public void ForceSerialization() {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
