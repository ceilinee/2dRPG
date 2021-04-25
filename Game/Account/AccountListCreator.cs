using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountListCreator : MonoBehaviour
{
    public Accounts accounts;
    public GameObject accountManager;
    public GameObject gameSaveManager;
    public GameObject birthModal;
    public GameObject selectBackground;
    public Text text0;
    public Text text1;
    public Text text2;
    public GameObject delete0;
    public GameObject delete1;
    public GameObject delete2;
    public GameObject male;
    public GameObject female;
    public Animals curAnimals;
    public Characters curCharacters;
    public Inventory curInventory;
    public FloatValue playerMoney;
    public VectorValue playerLocation;
    public CurTime curTime;
    public Player player;
    public bool confirmVar;
    // public GameObject nameGame;
    public GameObject confirmationModal;
    public GameObject confirmationBackground;

    // Start is called before the first frame update
    void Start()
    {
      female.GetComponent<Image>().color = new Color(0.6037736f, 0.6037736f, 0.6037736f, 1f);
      male.GetComponent<Image>().color = new Color(0.6037736f, 0.6037736f, 0.6037736f, 1f);
    }
    public void updateList(){
      if(accounts.accountDict.ContainsKey(0)){
        text0.text = "Load game: " + accounts.accountDict[0].name;
      }
      else{
        text0.text = "Start new game";
        delete0.SetActive(false);
      }
      if(accounts.accountDict.ContainsKey(1)){
        text1.text = "Load game: " + accounts.accountDict[1].name;
      }
      else{
        text1.text = "Start new game";
        delete1.SetActive(false);
      }
      if(accounts.accountDict.ContainsKey(2)){
        text2.text = "Load game: " + accounts.accountDict[2].name;
      }
      else{
        text2.text = "Start new game";
        delete2.SetActive(false);
      }
    }
    public void clearObjects(){
      curAnimals.Clear();
      curCharacters.Clear();
      curInventory.Clear();
      playerMoney.Clear();
      playerLocation.Clear();
      curTime.Clear();
    }
    public void selectGame0(){
      if(accounts.accountDict.ContainsKey(0)){
        accounts.selectedId = 0;
        gameSaveManager.GetComponent<GameSaveManager>().LoadScriptables();
        Loader.Load(Loader.Scene.MainScene);
      }
      else{
        clearObjects();
        accounts.addAccount("Account0", 0);
        GetPlayerName();
      }
    }
    void GetPlayerName(){
      StartCoroutine(waitConfirm());
    }
    void confirm(string name){
      player.playerName = name;
      confirmVar = true;
    }
    public void selectFemale(){
      player.female = true;
      female.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
      male.GetComponent<Image>().color = new Color(0.6037736f, 0.6037736f, 0.6037736f, 1f);
    }
    public void selectMale(){
      player.female = false;
      male.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
      female.GetComponent<Image>().color = new Color(0.6037736f, 0.6037736f, 0.6037736f, 1f);
    }
    IEnumerator waitConfirm(){
      birthModal.GetComponent<Alert>().initiateNameAlert("What should we call you?", ((string name) => confirm(name)));
      while(!confirmVar) yield return null;
      accountManager.GetComponent<AccountManager>().SaveAccounts();
      accountManager.GetComponent<AccountManager>().SaveScriptables();
      Loader.Load(Loader.Scene.MainScene);
      confirmVar = false;
    }
    public void selectGame1(){
      if(accounts.accountDict.ContainsKey(1)){
        accounts.selectedId = 1;
        gameSaveManager.GetComponent<GameSaveManager>().LoadScriptables();
        Loader.Load(Loader.Scene.MainScene);
      }
      else{
        clearObjects();
        accounts.addAccount("Account1", 1);
        GetPlayerName();
      }
    }
    public void selectGame2(){
      if(accounts.accountDict.ContainsKey(2)){
        clearObjects();
        accounts.selectedId = 2;
        gameSaveManager.GetComponent<GameSaveManager>().LoadScriptables();
        Loader.Load(Loader.Scene.MainScene);
      }
      else{
        clearObjects();
        accounts.addAccount("Account2", 2);
        GetPlayerName();
      }
    }

    private void renderDeleteConfirmationModal(int gameNum) {
      if (!accounts.accountDict.ContainsKey(gameNum)) {
        return;
      }

      selectBackground.SetActive(false);
      confirmationBackground.SetActive(true);

      confirmationModal.GetComponent<Confirmation>().initiateConfirmation(
        "Are you sure you want to delete this save file?",
        () => {
          clearObjects();
          accounts.removeExistingAccount(gameNum);
          accountManager.GetComponent<AccountManager>().DeleteScriptables(gameNum);
          accountManager.GetComponent<AccountManager>().SaveAccounts();
          updateList();
        },
        () => {},
        () => {
          selectBackground.SetActive(true);
          confirmationBackground.SetActive(false);
        }
      );
    }

    public void deleteGame0() {
      renderDeleteConfirmationModal(0);
    }

    public void deleteGame1(){
      renderDeleteConfirmationModal(1);
    }
    public void deleteGame2(){
      renderDeleteConfirmationModal(2);
    }
    // Update is called once per frame
    void Update()
    {
      if(Input.GetButtonDown("Cancel")){
        gameObject.SetActive(false);
      }
    }
}
