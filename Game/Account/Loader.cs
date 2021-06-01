
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader {
    private class loadingMonoBehaviour : MonoBehaviour { }
    public enum Scene {
        MainScene,
        Load,
        Island,
        Barn1,
        MainMenu,

        Forest
    }
    private static Action onLoaderCallback;
    private static AsyncOperation loadingAsyncOperation;

    public static void Load(Scene scene = Scene.MainScene, string sceneName = "") {
        if (sceneName != "") {
            if (System.Enum.TryParse<Scene>(sceneName, out Scene _scene)) {
                scene = _scene;
            }
        }
        onLoaderCallback = () => {
            GameObject loadingGameObject = new GameObject("Loading");
            loadingGameObject.AddComponent<loadingMonoBehaviour>().StartCoroutine(loadSceneAsync(scene));
        };
        SceneManager.LoadScene(Scene.Load.ToString());
    }
    public static IEnumerator loadSceneAsync(Scene scene) {
        yield return null;
        loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        while (!loadingAsyncOperation.isDone) {
            yield return null;
        }
    }
    public static float GetLoadingProgress() {
        if (loadingAsyncOperation != null) {
            return loadingAsyncOperation.progress;
        } else {
            return 0f;
        }
    }
    public static void LoaderCallback() {
        if (onLoaderCallback != null) {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
