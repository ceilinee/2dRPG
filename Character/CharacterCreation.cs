using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Assertions;

public class CharacterCreation : CustomMonoBehaviour {
    public GameObject input;
    private Action onSubmit;
    public BodyPartManager bodyPartManager;

    [Header("All the supported colors")]
    public AnimalColors colors;

    public Player player;

    private int currHairstyleIdx;
    // We show many colors at a time, so this is the idx of the leftmost shown color
    private int leftHairColorIdx;

    private int currHairColorIdx;

    [SerializeField]
    private ColorGroup hairColors;

    [Header("The name of the currently selected hairstyle")]
    public Text hairName;

    [Header("The currently displayed hair colors")]
    public List<Image> hairColorImages;

    private int currOutfitIdx;

    [Header("The name of the currently selected outfit")]
    public Text outfitName;
    private int currBottomIdx;

    [Header("The name of the currently selected bottom")]
    public Text bottomName;
    private int currEyesIdx;
    // We show many colors at a time, so this is the idx of the leftmost shown color
    private int leftEyesColorIdx;

    private int currEyesColorIdx;

    [SerializeField]
    private ColorGroup eyeColors;

    [Header("The name of the currently selected eyes")]
    public Text eyesName;

    [Header("The currently displayed eyes colors")]
    public List<Image> eyesColorImages;

    private int leftSkinColorIdx;
    private int currSkinColorIdx;

    [SerializeField]
    private ColorGroup skinColors;

    [Header("The currently displayed skin colors")]
    public List<Image> skinColorImages;

    public PlayerDesign playerDesign;
    private Notification notification;

    private CanvasController canvasController;
    void Awake() {
        Assert.IsTrue(bodyPartManager.hairStyles.Count > 0, "Must provide at least one hairstyle");
        Assert.IsTrue(bodyPartManager.outfits.Count > 0, "Must provide at least one outfit");
        Assert.IsTrue(bodyPartManager.bottoms.Count > 0, "Must provide at least one bottom");
        Assert.IsTrue(bodyPartManager.eyes.Count > 0, "Must provide at least one pair of eyes");
        Assert.IsTrue(colors.colorDictionary.Count >= 4, "Must support at least 4 colors");
    }

    private void UpdateIdxsFromPlayerSO() {
        var appearance = player.appearance;
        currHairstyleIdx = appearance.hairId;
        currHairColorIdx = appearance.hairColorId;
        currOutfitIdx = appearance.outfitId;
        currBottomIdx = appearance.bottomId;
        currEyesIdx = appearance.eyesId;
        currEyesColorIdx = appearance.eyeColorId;
        currSkinColorIdx = appearance.skinColorId;
    }

    private void OnEnable() {
        UpdateIdxsFromPlayerSO();
        input.GetComponent<InputField>().text = player.playerName;
        RenderHair();
        RenderOutfit();
        RenderBottom();
        RenderEyes();
        RenderSkin();
    }
    void Start() {
        notification = centralController.Get("Notification").GetComponent<Notification>();
        canvasController = centralController.Get("CanvasController").GetComponent<CanvasController>();
    }
    public void Open(Action onSubmit) {
        this.onSubmit = onSubmit;
        gameObject.SetActive(true);
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
        leftHairColorIdx += hairColorImages.Count;
        if (leftHairColorIdx >= hairColors.colorDictKeys.Count) {
            leftHairColorIdx %= hairColors.colorDictKeys.Count;
        }
        RenderHair();
    }

    public void HaircolorPrevious() {
        leftHairColorIdx -= hairColorImages.Count;
        if (leftHairColorIdx < 0) {
            leftHairColorIdx += hairColors.colorDictKeys.Count;
        }
        RenderHair();
    }

    public void HaircolorSelect(int hairColorIdx) {
        currHairColorIdx = (leftHairColorIdx + hairColorIdx) % hairColors.colorDictKeys.Count;
        RenderHair();
    }

    private void RenderHair() {
        BodyPart currHair = bodyPartManager.hairStyles[currHairstyleIdx];
        playerDesign.UpdateHair(currHair.bodyPartSprite, colors.GetColor(hairColors.colorDictKeys[currHairColorIdx]));
        hairName.text = currHair.name;
        for (int i = 0; i < hairColorImages.Count; ++i) {
            int colorIdx = (leftHairColorIdx + i) % hairColors.colorDictKeys.Count;
            hairColorImages[i].color = colors.GetColor(hairColors.colorDictKeys[colorIdx]);
        }
    }

    public void OutfitNext() {
        currOutfitIdx++;
        if (currOutfitIdx >= bodyPartManager.outfits.Count) {
            currOutfitIdx = 0;
        }
        RenderOutfit();
    }

    public void OutfitPrevious() {
        currOutfitIdx--;
        if (currOutfitIdx < 0) {
            currOutfitIdx = bodyPartManager.outfits.Count - 1;
        }
        RenderOutfit();
    }

    private void RenderOutfit() {
        BodyPart currOutfit = bodyPartManager.outfits[currOutfitIdx];
        playerDesign.UpdateOutfit(currOutfit.bodyPartSprite);
        outfitName.text = currOutfit.name;
    }

    public void BottomNext() {
        currBottomIdx++;
        if (currBottomIdx >= bodyPartManager.bottoms.Count) {
            currBottomIdx = 0;
        }
        RenderBottom();
    }

    public void BottomPrevious() {
        currBottomIdx--;
        if (currBottomIdx < 0) {
            currBottomIdx = bodyPartManager.bottoms.Count - 1;
        }
        RenderBottom();
    }

    private void RenderBottom() {
        BodyPart currBottom = bodyPartManager.bottoms[currBottomIdx];
        playerDesign.UpdateBottom(currBottom.bodyPartSprite);
        bottomName.text = currBottom.name;
    }

    public void EyesNext() {
        currEyesIdx++;
        if (currEyesIdx >= bodyPartManager.eyes.Count) {
            currEyesIdx = 0;
        }
        RenderEyes();
    }

    public void EyesPrevious() {
        currEyesIdx--;
        if (currEyesIdx < 0) {
            currEyesIdx = bodyPartManager.eyes.Count - 1;
        }
        RenderEyes();
    }

    public void EyeColorNext() {
        leftEyesColorIdx += eyesColorImages.Count;
        if (leftEyesColorIdx >= eyeColors.colorDictKeys.Count) {
            leftEyesColorIdx %= eyeColors.colorDictKeys.Count;
        }
        RenderEyes();
    }

    public void EyeColorPrevious() {
        leftEyesColorIdx -= eyesColorImages.Count;
        if (leftEyesColorIdx < 0) {
            leftEyesColorIdx += eyeColors.colorDictKeys.Count;
        }
        RenderEyes();
    }

    public void EyeColorSelect(int eyeColorIdx) {
        currEyesColorIdx = (leftEyesColorIdx + eyeColorIdx) % eyeColors.colorDictKeys.Count;
        RenderEyes();
    }

    private void RenderEyes() {
        BodyPart currEyes = bodyPartManager.eyes[currEyesIdx];
        playerDesign.UpdateEyes(currEyes.bodyPartSprite, colors.GetColor(eyeColors.colorDictKeys[currEyesColorIdx]));
        eyesName.text = currEyes.name;
        for (int i = 0; i < eyesColorImages.Count; ++i) {
            int colorIdx = (leftEyesColorIdx + i) % eyeColors.colorDictKeys.Count; ;
            eyesColorImages[i].color = colors.GetColor(eyeColors.colorDictKeys[colorIdx]);
        }
    }

    public void SkinColorNext() {
        leftSkinColorIdx += skinColorImages.Count;
        if (leftSkinColorIdx >= skinColors.colorDictKeys.Count) {
            leftSkinColorIdx %= skinColors.colorDictKeys.Count;
        }
        RenderSkin();
    }

    public void SkinColorPrevious() {
        leftSkinColorIdx -= skinColorImages.Count;
        if (leftSkinColorIdx < 0) {
            leftSkinColorIdx += skinColors.colorDictKeys.Count;
        }
        RenderSkin();
    }

    public void SkinColorSelect(int skinColorIdx) {
        currSkinColorIdx = (leftSkinColorIdx + skinColorIdx) % skinColors.colorDictKeys.Count;
        RenderSkin();
    }

    private void RenderSkin() {
        playerDesign.UpdateBody(colors.GetColor(skinColors.colorDictKeys[currSkinColorIdx]));
        for (int i = 0; i < skinColorImages.Count; ++i) {
            int colorIdx = (leftSkinColorIdx + i) % skinColors.colorDictKeys.Count;
            skinColorImages[i].color = colors.GetColor(skinColors.colorDictKeys[colorIdx]);
        }
    }

    public void Save() {
        string name = input.GetComponent<InputField>().text;
        if (name.Length == 0) {
            notification.initiateNotification("Your name has to be at least one character!", true);
        } else {
            input.GetComponent<InputField>().text = " ";
            player.playerName = name;
            player.setAppearance(
                bodyPartManager.hairStyles[currHairstyleIdx].id,
                hairColors.colorDictKeys[currHairColorIdx],
                bodyPartManager.outfits[currOutfitIdx].id,
                bodyPartManager.eyes[currEyesIdx].id,
                eyeColors.colorDictKeys[currEyesColorIdx],
                skinColors.colorDictKeys[currSkinColorIdx]
            );
            gameObject.SetActive(false);
            onSubmit();
        }
    }
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            Time.timeScale = 1;
            gameObject.SetActive(false);
            canvasController.GetComponent<CanvasController>().closeCanvas();
            Debug.Log("close");
        }
    }
}
