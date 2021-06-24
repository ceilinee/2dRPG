using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GoalItem : MonoBehaviour {
    // Start is called before the first frame update

    public Text message;
    public Text goal;
    public Quest quest;
    public GameObject complete;
    public GameObject settingsMenu;
    public void updateDetails(Quest newQuest) {
        quest = newQuest;
        message.text = newQuest.message;
        goal.text = newQuest.currentProgress;
        complete.SetActive(false);
        if (newQuest.completed) {
            complete.SetActive(true);
        }
    }
    public void selectQuest() {
        settingsMenu.GetComponent<SettingsMenu>().displayGoal(quest);
    }
}
