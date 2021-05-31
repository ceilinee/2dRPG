using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Accounts : ScriptableObject {
    [System.Serializable]
    public class Account {
        public int id;
        public string name;
    }
    [System.Serializable] public class DictionaryOfAccounts : SerializableDictionary<int, Account> { }
    public DictionaryOfAccounts accountDict = new DictionaryOfAccounts();
    public int selectedId;

    public void addAccount(string name, int id) {
        Account newAccount = new Account();
        newAccount.name = name;
        newAccount.id = id;
        selectedId = id;
        accountDict[newAccount.id] = newAccount;
    }
    public void addExistingAccount(Account newAccount) {
        accountDict[newAccount.id] = newAccount;
    }
    public void removeExistingAccount(int accountId) {
        accountDict.Remove(accountId);
        if (selectedId == accountId) {
            selectedId = -1;
        }
    }
}
