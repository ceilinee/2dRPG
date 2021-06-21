using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Player : ScriptableObject {
    [System.Serializable]
    public class Appearance {
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
