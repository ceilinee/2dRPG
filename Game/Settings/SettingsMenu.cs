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
        gameObject.SetActive(true);
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
        CanvasController.ChangeCanvasColor(filters.filters[target.value].canvasColour, playerSettings.filter.canvasColour);
        playerSettings.filter = filters.filters[target.value];
        TimeController.nightLightColor = playerSettings.filter.nightTimeColour;
        TimeController.dawnLightColor = playerSettings.filter.dawnTimeColour;
        TimeController.dayLightColor = playerSettings.filter.dayTimeColour;
        TimeController.SetColour();
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
            if (filters.filters[i] == playerSettings.filter) {
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