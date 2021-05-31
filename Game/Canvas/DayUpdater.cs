using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayUpdater : MonoBehaviour {
    public Text peopleTalkedAnswer;
    public Text peopleTalkedRep;
    public Text peopleGiftAnswer;
    public Text peopleGiftRep;
    public Text animalWalkAnswer;
    public Text animalWalkRep;
    public Text animalGiftAnswer;
    public Text animalGiftRep;
    public Text adoptionAnswer;
    public Text adoptionRep;
    public Text questAnswer;
    public Text questRep;
    public Text moneyAnswer;
    public Text totalRep;
    public Player player;
    public GameObject CanvasController;
    // Start is called before the first frame update
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            closeModal();
        }
        if (gameObject.activeInHierarchy && Time.timeScale != 0) {
            Time.timeScale = 0;
        }
    }
    public void closeModal() {
        gameObject.SetActive(false);
        CanvasController.GetComponent<CanvasController>().closeCanvas();
    }
    public void updateModal(Player player) {
        int rep = 0;
        peopleTalkedAnswer.text = player.dailyTalk.ToString();
        peopleTalkedRep.text = "+" + player.dailyTalk.ToString() + " rep";
        rep += player.dailyTalk;
        peopleGiftAnswer.text = player.dailyGiftCharacter.ToString();
        peopleGiftRep.text = "+" + player.dailyGiftCharacter.ToString() + " rep";
        rep += player.dailyGiftCharacter;
        animalWalkAnswer.text = player.dailyWalk.ToString();
        animalWalkRep.text = "+" + player.dailyWalk.ToString() + " rep";
        rep += player.dailyWalk;
        animalGiftAnswer.text = player.dailyGiftAnimal.ToString();
        animalGiftRep.text = "+" + player.dailyGiftAnimal.ToString() + " rep";
        rep += player.dailyGiftAnimal;
        adoptionAnswer.text = player.dailyAdoption.ToString();
        adoptionRep.text = "+" + (player.dailyAdoption * 2).ToString() + " rep";
        rep += (player.dailyAdoption * 2);
        questAnswer.text = player.dailyQuest.ToString();
        questRep.text = "+" + (player.dailyQuest * 2).ToString() + " rep";
        rep += (player.dailyQuest * 2);
        player.reputation += rep;
        moneyAnswer.text = player.earnedMoney.ToString();
        totalRep.text = rep.ToString() + " rep earned!";
    }
}
