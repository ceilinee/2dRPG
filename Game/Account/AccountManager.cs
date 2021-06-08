using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class AccountManager : MonoBehaviour {
    public Accounts accounts;

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
                accounts);
            file.Close();
        }
        // When the game starts at the menu scene, make sure we start off at a clean
        // slate; all scriptable objects have default values
        accounts.selectedId = -1;
        accountListCreator.clearObjects();
    }
    public void SaveAccounts() {
        FileStream file = File.Create(Path.Combine(Application.persistentDataPath, string.Format("accounts.dat")));
        BinaryFormatter binary = new BinaryFormatter();
        var json = JsonUtility.ToJson(accounts);

        binary.Serialize(file, json);
        file.Close();
    }
}
