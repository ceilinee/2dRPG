using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SettingsMenu : CustomMonoBehaviour {
    public Dropdown speedDropdown;
    public Dropdown filterDropdown;
    public Dropdown geeseDropdown;
    public Filters filters;
    public Settings playerSettings;
    public TimeController TimeController;
    private Speed[] speedEnum;
    private CanvasController CanvasController;
    public Quests goals;
    public GoalList incomplete;
    public GoalList completed;
    public Player player;

    // Start is called before the first frame update
    void Start() {
        CanvasController = centralController.Get("CanvasController").GetComponent<CanvasController>();
        TimeController = centralController.Get("TimeController").GetComponent<TimeController>();
    }
    public void Update() {
        if (Input.GetButtonDown("Cancel")) {
            CanvasController.closeAllCanvas();
        }
    }
    public void Open() {
        SetUpDropdowns();
        SetUpGoals();
        gameObject.SetActive(true);
    }
    public void displayGoal(Quest quest) {

    }
    public void SetUpGoals() {
        foreach (Quest quest in goals.curQuests) {
            quest.currentProgress = CalculateGoal(quest, true);
        }
        goals.SortQuestsByCompleted();
        incomplete.Clear();
        completed.Clear();
        incomplete.quests = goals.GetIncompletedQuests();
        completed.quests = goals.GetCompletedQuests();
        incomplete.PopulateList();
        completed.PopulateList();
    }
    public void setQuestCompletedFunction(Quest quest) {
        goals.SetQuestCompleted(quest);
    }
    public void setQuestIncompletedFunction(Quest quest) {
        goals.SetQuestIncompleted(quest);
    }

    public string CalculateGoal(Quest newQuest, bool total = false) {
        string result = "";
        switch (newQuest.type) {
            case QuestType.adoption:
                int adoption = total ? player.totalAdoption : player.dailyAdoption;
                result = adoption.ToString() + " /" + newQuest.adoptionCount;
                float percentage = adoption / newQuest.adoptionCount;
                if (percentage >= 1.0 && !newQuest.completed) {
                    setQuestCompletedFunction(newQuest);
                } else if (percentage < 1.0) {
                    setQuestIncompletedFunction(newQuest);
                }
                break;
            case QuestType.talk:
                int talk = total ? player.totalTalk : player.dailyTalk;
                result = talk.ToString() + " /" + newQuest.talk;
                percentage = (float) talk / newQuest.talk;
                Debug.Log(percentage);
                if (percentage >= 1.0 && !newQuest.completed) {
                    setQuestCompletedFunction(newQuest);
                } else if (percentage < 1.0) {
                    setQuestIncompletedFunction(newQuest);
                }
                break;
            case QuestType.walk:
                int walk = total ? player.totalWalk : player.dailyWalk;
                result = walk.ToString() + " /" + newQuest.walk;
                percentage = (float) walk / newQuest.walk;
                if (percentage >= 1.0 && !newQuest.completed) {
                    setQuestCompletedFunction(newQuest);
                } else if (percentage < 1.0) {
                    setQuestIncompletedFunction(newQuest);
                }
                break;
            case QuestType.talkCharacter:
                List<int> talkedTo = total ? player.totalTalkedTo : player.dailyTalkedTo;
                result = talkedTo.Count.ToString() + " /" + newQuest.talkQuestCharId.Length;
                percentage = (float) talkedTo.Count / newQuest.talkQuestCharId.Length;
                if (percentage >= 1.0 && !newQuest.completed) {
                    setQuestCompletedFunction(newQuest);
                } else if (percentage < 1.0) {
                    setQuestIncompletedFunction(newQuest);
                }
                break;
            case QuestType.giftCharacter:
                List<int> gifted = total ? player.totalGiftedTo : player.dailyGiftedTo;
                result = gifted.Count.ToString() + " /" + newQuest.talkQuestCharId.Length;
                percentage = (float) gifted.Count / newQuest.talkQuestCharId.Length;
                if (percentage >= 1.0 && !newQuest.completed) {
                    setQuestCompletedFunction(newQuest);
                } else if (percentage < 1.0) {
                    setQuestIncompletedFunction(newQuest);
                }
                break;
            case QuestType.giftAnimal:
                int giftedAnimal = total ? player.totalGiftAnimal : player.dailyGiftAnimal;
                result = giftedAnimal.ToString() + " /" + newQuest.talkQuestCharId.Length;
                percentage = (float) giftedAnimal / newQuest.talkQuestCharId.Length;
                if (percentage >= 1.0 && !newQuest.completed) {
                    setQuestCompletedFunction(newQuest);
                } else if (percentage < 1.0) {
                    setQuestIncompletedFunction(newQuest);
                }
                break;
            default:
                List<int> scenesVisited = total ? player.totalScenesVisited : player.dailyScenesVisited;
                if (!newQuest.completed && !scenesVisited.Contains(newQuest.sceneId)) {
                    result = "0 /1";
                    if (newQuest.completed) {
                        setQuestIncompletedFunction(newQuest);
                    }
                } else {
                    if (!newQuest.completed) {
                        setQuestCompletedFunction(newQuest);
                    }
                    result = "1 /1";
                }
                break;
        }
        return result;
    }
    public void SetUpDropdowns() {
        SetupSpeedDropdown();
        speedDropdown.onValueChanged.AddListener(delegate {
            UpdateSpeed(speedDropdown);
        });
        SetupFilterDropdown();
        filterDropdown.onValueChanged.AddListener(delegate {
            UpdateFilter(filterDropdown);
        });
        SetUpGeeseDropdown();
        geeseDropdown.onValueChanged.AddListener(delegate {
            UpdateGeese(geeseDropdown);
        });
    }
    public void RemoveListeners() {
        if (geeseDropdown) {
            geeseDropdown.onValueChanged.RemoveListener(delegate {
                UpdateGeese(geeseDropdown);
            });
        };
        filterDropdown.onValueChanged.RemoveListener(delegate {
            UpdateFilter(filterDropdown);
        });
        speedDropdown.onValueChanged.RemoveListener(delegate {
            UpdateSpeed(speedDropdown);
        });
    }
    public void UpdateSpeed(Dropdown target) {
        playerSettings.speed = speedEnum[target.value];
        TimeController.timeScale = (150) + (50) * (target.value);
    }
    public void UpdateFilter(Dropdown target) {
        CanvasController.ChangeCanvasColor(filters.filters[target.value].canvasColour);
        playerSettings.filter = filters.filters[target.value];
        TimeController.SetLightColors(playerSettings.filter);
    }
    public void UpdateGeese(Dropdown target) {
        playerSettings.speed = speedEnum[target.value];
    }
    public void SetupSpeedDropdown() {
        speedEnum = (Speed[]) System.Enum.GetValues(typeof(Speed));
        int position = 0;
        List<string> dropdownOptions = new List<string>();
        for (int i = 0; i < speedEnum.Length; ++i) {
            dropdownOptions.Add(speedEnum[i].ToString());
            if (speedEnum[i] == playerSettings.speed) {
                position = i;
            }
        }
        speedDropdown.ClearOptions();
        speedDropdown.AddOptions(dropdownOptions);
        speedDropdown.value = position;
    }
    public void SetupFilterDropdown() {
        int position = 0;
        List<string> dropdownOptions = new List<string>();
        for (int i = 0; i < filters.filters.Count; ++i) {
            dropdownOptions.Add(filters.filters[i].name.ToString().ToUpper());
            if (filters.filters[i].name == playerSettings.filter.name) {
                Debug.Log("equal filter");
                position = i;
            }
        }
        filterDropdown.ClearOptions();
        filterDropdown.AddOptions(dropdownOptions);
        filterDropdown.value = position;
    }
    public void SetUpGeeseDropdown() {
        speedEnum = (Speed[]) System.Enum.GetValues(typeof(Speed));
        int position = 0;
        List<string> dropdownOptions = new List<string>();
        for (int i = 0; i < speedEnum.Length; ++i) {
            dropdownOptions.Add(speedEnum[i].ToString());
            if (speedEnum[i] == playerSettings.geese) {
                position = i;
            }
        }
        geeseDropdown.ClearOptions();
        geeseDropdown.AddOptions(dropdownOptions);
        geeseDropdown.value = position;
    }
}