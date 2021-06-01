using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class AccountManager : MonoBehaviour {
    public Accounts account;
    public List<ScriptableObject> objects = new List<ScriptableObject>();

    [SerializeField]
    private AccountListCreator accountListCreator;

    // Start is called before the first frame update
    public void OnEnable() {
        LoadAccounts();
    }
    public void LoadAccounts() {
        if (File.Exists(Path.Combine(Application.persistentDataPath, string.Format("accounts.dat")))) {
            FileStream file = File.Open(Path.Combine(Application.persistentDataPath, string.Format("accounts.dat")), FileMode.Open);
            BinaryFormatter binary = new BinaryFormatter();
            JsonUtility.FromJsonOverwrite((string) binary.Deserialize(file),
                account);
            file.Close();
        }
        // When the game starts at the menu scene, make sure we start off at a clean
        // slate; all scriptable objects have default values
        account.selectedId = -1;
        accountListCreator.clearObjects();
    }
    public void SaveScriptables() {
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
    public void DeleteScriptables(int id) {
        for (int i = 0; i < objects.Count; i++) {
            if (File.Exists(Application.persistentDataPath +
                string.Format("/" + id.ToString() + "{0}.dat", i))) {
                File.Delete(Application.persistentDataPath +
                    string.Format("/" + id.ToString() + "{0}.dat", i));
            }
        }
    }
    public void SaveAccounts() {
        FileStream file = File.Create(Path.Combine(Application.persistentDataPath, string.Format("accounts.dat")));
        BinaryFormatter binary = new BinaryFormatter();
        var json = JsonUtility.ToJson(account);

        binary.Serialize(file, json);
        file.Close();
    }
    public void selectAccount() {

    }
    // Update is called once per frame
    void Update() {

    }
}
