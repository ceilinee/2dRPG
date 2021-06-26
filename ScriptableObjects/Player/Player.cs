using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Player : ScriptableObject {
    [System.Serializable]
    public class Appearance {
        // Note that colorIds refer to the keys in SO AnimalColors.colorDictionary
        public int hairId;
        public int hairColorId;
        public int outfitId;
        public int eyesId;
        public int eyeColorId;
        public int skinColorId;
        public int bottomId;
    }
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
    public int dailyQuest = 0;
    public float earnedMoney;
    public List<int> exCharId;
    public List<int> dailyTalkedTo;
    public List<int> dailyGiftedTo;
    public List<int> dailyScenesVisited;
    public List<string> dailyCollected;
    public List<int> childrenCharId;
    public int totalAdoption = 0;
    public int totalTalk = 0;
    public int totalWalk = 0;
    public int totalGiftCharacter = 0;
    public int totalGiftAnimal = 0;
    public int totalQuest = 0;
    public float totalEarnedMoney;
    public List<int> totalExCharId;
    public List<int> totalTalkedTo;
    public List<int> totalGiftedTo;
    public List<int> totalScenesVisited;
    public List<string> totalCollected;
    public Appearance appearance;

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
    public void Clear() {
        married = false;
        marriedCharId = -1;
        datingCharId = new List<int>();
        exCharId = new List<int>();
        reputation = 0;
        clearDailies();
        playerName = "";
        appearance = new Appearance();
    }
    public void AddTalkedTo(int id) {
        dailyTalkedTo.Add(id);
        if (!totalTalkedTo.Contains(id)) {
            totalTalkedTo.Add(id);
        }
    }
    public void AddCollected(string id) {
        dailyCollected.Add(id);
        if (!totalCollected.Contains(id)) {
            totalCollected.Add(id);
        }
    }
    public void AddGiftedTo(int id) {
        dailyGiftedTo.Add(id);
        if (!totalGiftedTo.Contains(id)) {
            totalGiftedTo.Add(id);
        }
    }
    public void AddScenesVisited(int id) {
        dailyScenesVisited.Add(id);
        if (!totalScenesVisited.Contains(id)) {
            totalScenesVisited.Add(id);
        }
    }
    public void AddAdoption(int add = 1) {
        dailyAdoption += add;
        totalAdoption += add;
    }
    public void AddTalk(int add = 1) {
        dailyTalk += add;
        totalTalk += add;
    }
    public void AddWalk(int add = 1) {
        dailyWalk += add;
        totalWalk += add;
    }
    public void AddGiftCharacter(int add = 1) {
        dailyGiftCharacter += add;
        totalGiftCharacter += add;
    }
    public void AddGiftAnimal(int add = 1) {
        dailyGiftAnimal += add;
        totalGiftAnimal += add;
    }
    public void AddQuest(int add = 1) {
        dailyQuest += add;
        totalQuest += add;
    }
    public void AddEarnedMoney(float add = 1) {
        earnedMoney += add;
        totalEarnedMoney += add;
    }
    public void clearDailies() {
        dailyAdoption = 0;
        dailyTalk = 0;
        dailyWalk = 0;
        dailyGiftCharacter = 0;
        dailyGiftAnimal = 0;
        dailyQuest = 0;
        earnedMoney = 0;
        dailyTalkedTo = new List<int>();
        dailyCollected = new List<string>();
        dailyGiftedTo = new List<int>();
        dailyScenesVisited = new List<int>();
    }

    public void setAppearance(
        int hairId,
        int hairColorId,
        int outfitId,
        int eyesId,
        int eyeColorId,
        int skinColorId) {
        appearance = new Appearance();
        appearance.hairId = hairId;
        appearance.hairColorId = hairColorId;
        appearance.outfitId = outfitId;
        appearance.eyesId = eyesId;
        appearance.eyeColorId = eyeColorId;
        appearance.skinColorId = skinColorId;
    }
}
