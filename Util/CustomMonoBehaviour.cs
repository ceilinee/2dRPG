using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;
using UnityEngine.SceneManagement;

public class CustomMonoBehaviour : MonoBehaviour {
    public const string TagOfControllers = "manager";
    public const string TagOfPlayer = "Player";
    public const string CentralControllerPath = "Game/Controllers/CentralGameObjectController";

    // Do not use centralController in Awake as it may not have been initialized
    protected static CentralController centralController;

    // Called in Awake of CentralController.cs
    protected static void SetCentralController(CentralController centralController) {
        CustomMonoBehaviour.centralController = centralController;
    }

    protected static Scene ActiveScene() {
        return SceneManager.GetActiveScene();
    }

    protected static Loader.Scene ActiveSceneType() {
        return (Loader.Scene) Enum.Parse(typeof(Loader.Scene), ActiveScene().name);
    }
}
