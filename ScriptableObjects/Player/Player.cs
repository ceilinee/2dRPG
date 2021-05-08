using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Player : ScriptableObject {
    public string playerName;
    public bool female;
    public bool married;
    public int marriedCharId;
    public List<int> datingCharId;
    public float reputation;
    public int dailyAdoption = 0;
    public int dailyTalk = 0;
    public int dailyWalk = 0;
    public int dailyGiftCharacter = 0;
    public int dailyGiftAnimal = 0;
    public float earnedMoney;
    public float earnedreputation;
    public List<int> exCharId;
    public List<int> dailyTalkedTo;
    public List<int> dailyGiftedTo;
    public List<int> dailyScenesVisited;
    public List<int> dailyCollected;
    public List<int> childrenCharId;

    public void divorce() {
        married = false;
        exCharId.Add(marriedCharId);
        breakUp(marriedCharId);
        marriedCharId = -1;
    }
    public void marry(int id) {
        married = true;
        marriedCharId = id;
    }
    public void breakUp(int id) {
        datingCharId.Remove(id);
        exCharId.Add(id);
    }
    public void date(int id) {
        datingCharId.Add(id);
    }
    public void clearDailies() {
        dailyAdoption = 0;
        dailyTalk = 0;
        dailyWalk = 0;
        dailyGiftCharacter = 0;
        dailyGiftAnimal = 0;
        dailyTalkedTo = new List<int>();
        dailyCollected = new List<int>();
        dailyGiftedTo = new List<int>();
        dailyScenesVisited = new List<int>();
    }
}
