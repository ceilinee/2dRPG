using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
[System.Serializable]
public class Forest : ScriptableObject {
    public int level = 1;
    public int width = 50;
    public int height = 50;
    public int starCount = 0;

    public Vector2 entrance;
    public Vector2 exit;
    public bool maze = true;
    public void LevelUp() {
        level++;
        starCount = (level + 1) * 2;
        width = 50 + System.Math.Min(200, level * 20);
        height = width;
        ForceSerialization();
        // maze = level % 5 == 0 ? false : true;
    }
    void ForceSerialization() {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
    public void Clear() {
        Debug.Log("Clear");
        level = 0;
        starCount = (level + 1) * 2;
        width = 50;
        height = 50;
        ForceSerialization();
    }
}
