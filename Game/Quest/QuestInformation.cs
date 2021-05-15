using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestInformation : MonoBehaviour {
    public GameObject CanvasController;
    public GameObject questView;
    public GameObject questDisplay;
    public Quest quest;
    public Quests availableQuests;

    // Start is called before the first frame update
    // void Start()
    // {
    //   updateList();
    // }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            gameObject.SetActive(false);
            CanvasController.GetComponent<CanvasController>().closeCanvas();
        }
        if (gameObject.activeInHierarchy && Time.timeScale != 0) {
            Time.timeScale = 0;
        }
    }
    public void refresh() {
        updateList();
        displayQuest(quest);
    }
    public void refreshDelete() {
        updateList();
    }
    public void displayQuest(Quest curQuest) {
        quest = curQuest;
        questDisplay.SetActive(true);
        questDisplay.GetComponent<QuestDisplay>().displayQuest(curQuest);
    }
    public void Clear() {
        questView.GetComponent<QuestList>().Clear();
        questDisplay.SetActive(false);
    }
    public void Open() {
        gameObject.SetActive(true);
        updateList();
    }
    public void updateList() {
        Clear();
        questView.GetComponent<QuestList>().questInformation = gameObject;
        questView.GetComponent<QuestList>().PopulateList();
    }
}
