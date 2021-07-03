using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSign : Sign {
    public GameSaveManager gameSaveManager;
    public VectorValue playerPosition;

    protected override void OnOpen() {
        playerPosition.updateInitialValue(transform.position - new Vector3(0, 2, 0));
        gameSaveManager.SaveScriptables();
    }

    protected override void OnClose() { }
}
