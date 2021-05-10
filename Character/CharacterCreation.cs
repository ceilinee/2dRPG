using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Assertions;

public class CharacterCreation : MonoBehaviour {
    public GameObject input;
    private Action<string> nameFunction;
    public BodyPartManager bodyPartManager;

    [Header("All the supported colors")]
    public AnimalColors colors;

    private int currHairstyleIdx;
    // We show many colors at a time, so this is the idx of the leftmost shown color
    private int leftHairColorIdx;

    private int currHairColorIdx;

    [Header("The currently selected hairstyle")]
    public Image hair;

    [Header("The name of the currently selected hairstyle")]
    public Text hairName;

    [Header("The currently displayed hair colors")]
    public List<Image> hairColors;

    public Player player;

    void Awake() {
        Assert.IsTrue(bodyPartManager.hairStyles.Count > 0, "Must provide at least one hairstyle");
        Assert.IsTrue(colors.colorDictionary.Count >= 4, "Must support at least 4 colors");
    }

    void Start() {
        RenderHair();
    }

    public void initiateNameAlert(Action<string> newNameFunction) {
        nameFunction = newNameFunction;
        gameObject.SetActive(true);
    }

    public void submitName() {
        string name = input.GetComponent<InputField>().text;
        input.GetComponent<InputField>().text = " ";
        nameFunction(name);
        gameObject.SetActive(false);
    }

    public void HairstyleNext() {
        currHairstyleIdx++;
        if (currHairstyleIdx >= bodyPartManager.hairStyles.Count) {
            currHairstyleIdx = 0;
        }
        RenderHair();
    }

    public void HairstylePrevious() {
        currHairstyleIdx--;
        if (currHairstyleIdx < 0) {
            currHairstyleIdx = bodyPartManager.hairStyles.Count - 1;
        }
        RenderHair();
    }

    public void HaircolorNext() {
        leftHairColorIdx += hairColors.Count;
        if (leftHairColorIdx >= colors.colorDictionary.Count) {
            leftHairColorIdx %= colors.colorDictionary.Count;
        }
        RenderHair();
    }

    public void HaircolorPrevious() {
        leftHairColorIdx -= hairColors.Count;
        if (leftHairColorIdx < 0) {
            leftHairColorIdx += colors.colorDictionary.Count;
        }
        RenderHair();
    }

    public void HaircolorSelect(int hairColorIdx) {
        currHairColorIdx = (leftHairColorIdx + hairColorIdx) % colors.colorDictionary.Count;
        RenderHair();
    }

    private void RenderHair() {
        BodyPart currHair = bodyPartManager.hairStyles[currHairstyleIdx];
        hair.sprite = currHair.bodyPartSprite;
        hair.color = colors.colorDictionary[currHairColorIdx].color;
        hairName.text = currHair.name;
        for (int i = 0; i < hairColors.Count; ++i) {
            int colorIdx = (leftHairColorIdx + i) % colors.colorDictionary.Count;
            hairColors[i].color = colors.colorDictionary[colorIdx].color;
        }
    }

    public void Save() {
        player.setAppearance(
            bodyPartManager.hairStyles[currHairstyleIdx].id,
            currHairColorIdx
        );
        submitName();
    }
}
