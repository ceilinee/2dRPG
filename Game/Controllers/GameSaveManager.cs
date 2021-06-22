
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameSaveManager : MonoBehaviour {
    public Accounts account;
    public Vector2 forceLocation;
    void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("save");

        if (objs.Length > 1) {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    public GameObject characterManager;
    public GameObject animalList;
    public GameObject[] characters;
    public SpawnAnimal spawnAnimal;
    public List<ScriptableObject> objects = new List<ScriptableObject>();

    public void ResetScriptables(int accountId) {
        for (int i = 0; i < objects.Count; i++) {
            if (File.Exists(Application.persistentDataPath +
                string.Format("/" + accountId.ToString() + "{0}.dat", i))) {
                File.Delete(Application.persistentDataPath +
                    string.Format("/" + accountId.ToString() + "{0}.dat", i));
            }
        }
    }
    public void updateAnimalAndCharacter() {
        characterManager = GameObject.FindGameObjectsWithTag("characterManager").Length > 0 ? GameObject.FindGameObjectsWithTag("characterManager")[0] : null;
        animalList = GameObject.FindGameObjectsWithTag("animalList").Length > 0 ? GameObject.FindGameObjectsWithTag("animalList")[0] : null;
        if (characterManager) {
            characterManager.GetComponent<CharacterManager>().updateCurCharacter();
        }
        if (animalList) {
            animalList.GetComponent<AnimalList>().updateList();
        }
    }

    public void OnEnable() {
        LoadScriptables();
    }

    public void SaveScriptables() {
        updateAnimalAndCharacter();
        for (int i = 0; i < objects.Count; i++) {
            // Debug.Log("save: "+ Path.Combine(Application.persistentDataPath, string.Format(account.selectedId.ToString() + "{0}.dat", i)));
            FileStream file = File.Create(Path.Combine(Application.persistentDataPath, string.Format(account.selectedId.ToString() + "{0}.dat", i)));
            BinaryFormatter binary = new BinaryFormatter();
            var json = JsonUtility.ToJson(objects[i]);
            // Debug.Log(json);

            binary.Serialize(file, json);
            file.Close();
        }
    }

    public void LoadScriptables() {
        // If we are in MainMenu, no need to load any SO
        if (account.selectedId == -1) {
            return;
        }
        for (int i = 0; i < objects.Count; i++) {
            if (File.Exists(Path.Combine(Application.persistentDataPath, string.Format(account.selectedId.ToString() + "{0}.dat", i)))) {
                FileStream file = File.Open(Path.Combine(Application.persistentDataPath, string.Format(account.selectedId.ToString() + "{0}.dat", i)), FileMode.Open);
                BinaryFormatter binary = new BinaryFormatter();
                JsonUtility.FromJsonOverwrite((string) binary.Deserialize(file),
                    objects[i]);
                file.Close();
            }
        }
    }

    public void ClearSOState() {
        foreach (ScriptableObject so in objects) {
            (so as Clearable)?.Clear();
        }
    }
}
