using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class TimeController : CustomMonoBehaviour {
    public CurTime currentTime;
    // Amount of time that has passed in seconds, in the current day
    private float time;
    [SerializeField] public float timeScale = 60f;
    const float secondsInDay = 86400f;
    public int days;
    public string dateString = "";
    public Animals curAnimals;
    public GameObject animalList;
    public GameObject animalSpawner;
    public GameObject birthModal;
    public GameObject notification;
    public GameObject dateController;
    public GameObject backgroundController;
    public GameObject EventController;
    public GameObject CanvasController;
    public GameObject ShopController;
    public GameObject BreedAnimal;
    public GameObject SpawnObject;
    public GameObject QuestController;
    public GameObject charAnimals;
    public GameObject AdoptionController;
    public Player player;
    public Animals runaways;
    public AnimalBreed animalBreeds;
    public GameObject lamps;
    private string curName;
    public GameObject DayUpdater;
    public Image NightBackground;
    public GameObject characterManager;
    [System.Serializable] public class DictionaryOfIntAndString : SerializableDictionary<int, string> { }
    public DictionaryOfIntAndString seasonDict = new DictionaryOfIntAndString();
    private bool confirmVar = false;
    [SerializeField] public Color nightLightColor;
    [SerializeField] AnimationCurve nightTimeCurve;
    [SerializeField] public Color dawnLightColor;
    [SerializeField] public Color dayLightColor = Color.white;
    [SerializeField] Light2D globalLight;

    [SerializeField] Text text;
    private string curTimeText;

    // This is a signal that is triggered every hour
    [SerializeField]
    private Signal timeChangeSignal;
    public Settings playerSettings;
    public SpawnWildAnimal spawnWildAnimal;
    public float Hours {
        get { return time / 3600f; }
    }

    public float Minutes {
        get { return (time % 3600f / 60f); }
    }
    // Update is called once per frame
    public void PauseGame() {
        Time.timeScale = 0;
    }
    public void ResumeGame() {
        Time.timeScale = 1;
    }
    void Update() {
        if (time == 0 && days == 0) {
            time = currentTime.time;
            days = currentTime.days;
        }
        time += Time.deltaTime * timeScale;
        int hh = (int) Hours;
        int mm = (int) Minutes;
        var newText = hh.ToString("00") + ":" + mm.ToString("00").Substring(0, 1) + "0";
        if (newText == "18:30") {
            OpenLamps();
        }
        if (newText == "22:00") {
            setAnimalsToSleep();
        }
        if (newText == "06:30") {
            CloseLamps();
        }
        if (newText != curTimeText) {
            UpdateTime(newText);
        }
    }
    void setAnimalsToSleep() {
        animalList.GetComponent<AnimalList>().setAnimalsToSleep();
    }
    void OpenLamps() {
        if (lamps) {
            foreach (Transform child in lamps.transform) {
                child.gameObject.GetComponent<Light2D>().intensity = 0.5f;
            }
        }
    }
    public void SetLightColors(Filter filter) {
        nightLightColor = filter.nightTimeColour;
        dawnLightColor = filter.dawnTimeColour;
        dayLightColor = filter.dayTimeColour;
        SetColour();
    }
    void Start() {
        if (playerSettings.HasFilterSet()) {
            SetLightColors(playerSettings.filter);
        }
        spawnWildAnimal = centralController.Get("WildAnimalSpawner").GetComponent<SpawnWildAnimal>();
    }
    void CloseLamps() {
        if (lamps) {
            foreach (Transform child in lamps.transform) {
                child.gameObject.GetComponent<Light2D>().intensity = 0f;
            }
        }
        animalList.GetComponent<AnimalList>().setAnimalsWake();
    }
    void UpdateTime(string newText) {
        if (dateString == "") {
            dateString = "Y" + currentTime.years.ToString() + ", " + currentTime.getSeasonInWords() + " " + currentTime.date.ToString() + ", ";
        }
        text.text = dateString + newText;
        curTimeText = newText;
        if ((curTimeText == "08:00" || curTimeText == "22:00" || curTimeText == "12:00" || curTimeText == "17:00") && SpawnObject) {
            SpawnObject.GetComponent<SpawnObject>().Spawn();
            if (Random.Range(0, 10) >= 8 && AdoptionController) {
                AdoptionController.GetComponent<AdoptionController>().startRequest();
            }
        }
        characterManager.GetComponent<CharacterManager>().checkCharacter(newText);
        if (backgroundController) {
            backgroundController.GetComponent<BackgroundController>().updateBackground(newText);
        }
        currentTime.UpdateTime(time);
        // If the time is an exact hour e.g.: 15:00
        if (newText.Substring(3) == "00") {
            // Broadcast time change to all listeners
            timeChangeSignal.Raise();
        }
        SetColour();
        if (time > secondsInDay) {
            NextDay();
        }
    }
    public void SetColour() {
        float v = nightTimeCurve.Evaluate(Hours);
        Color c = Color.Lerp(nightLightColor, dawnLightColor, v * 2);
        if (v < .5) {
            c = Color.Lerp(nightLightColor, dawnLightColor, v * 2);
        } else {
            c = Color.Lerp(dawnLightColor, dayLightColor, (v - 0.5f) * 2);
        }
        globalLight.color = c;
    }
    // ask if player wants kids or not

    void NextDay() {
        time = 0;
        days += 1;
        EventController.GetComponent<EventController>().updateDay(days);
        currentTime.UpdateDays(days);
        characterManager.GetComponent<CharacterManager>().updateCharAnimal();
        UpdateAnimals();
        ShopController.GetComponent<Shop>().updateShop();
        if (days % 5 == 0) {
            ShopController.GetComponent<Shop>().updateSpecialShop();
        }
        if (SpawnObject) {
            SpawnObject.GetComponent<SpawnObject>().Spawn();
        }
        dateString = "";
        spawnWildAnimal.Spawn();
        QuestController.GetComponent<QuestController>().increaseDate(days, 1);
        dateController.GetComponent<DateController>().UpdateDays();
        setAnimalsToSleep();
    }
    public void confirm(string name, int id) {
        curAnimals.animalDict[id].animalName = name;
        confirmVar = true;
    }
    public void fastForward() {
        PauseGame();
        //Get current color
        Color spriteColor = Color.white;
        NightBackground.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1f);
        NextDay();
        time = (secondsInDay / 24) * 8f;
        int hh = (int) Hours;
        int mm = (int) Minutes;
        var newText = hh.ToString("00") + ":" + mm.ToString("00").Substring(0, 1) + "0";
        UpdateTime(newText);
        animalList.GetComponent<AnimalList>().setAnimalsWake();
        ResumeGame();
        StartCoroutine(fadeOut(NightBackground, 2.0f));
    }
    // fade out the night background
    IEnumerator fadeOut(Image blackOut, float duration) {
        float counter = 0;
        //Get current color
        Color spriteColor = Color.white;
        blackOut.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1f);
        while (counter < duration) {
            counter += Time.deltaTime;
            //Fade from 1 to 0
            float alpha = Mathf.Lerp(1, 0, counter / duration);
            Debug.Log(alpha);
            blackOut.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            yield return null;
        }
        ResumeGame();
    }

    IEnumerator waitConfirm() {
        if (CanvasController.GetComponent<CanvasController>().openCanvas()) {
            NightBackground.gameObject.SetActive(true);
            DayUpdater.SetActive(true);
            DayUpdater.GetComponent<DayUpdater>().updateModal(player);
            while (DayUpdater.activeInHierarchy) yield return null;
        }
        player.clearDailies();
        curAnimals.dailyAnimalUpdate();
        curAnimals.ageAnimals(1);
        curAnimals.ClearDailies();
        List<int> remove = new List<int>();
        foreach (KeyValuePair<int, Animal> kvp in curAnimals.animalDict) {
            if (kvp.Value.health <= 5) {
                confirmVar = false;
                PauseGame();
                Time.timeScale = 0;
                CanvasController.GetComponent<CanvasController>().openCanvas();
                CanvasController.GetComponent<CanvasController>().notification.GetComponent<Notification>().initiateNotification("Last night, " + kvp.Value.animalName + " ran away due to lack of attention. If you talk to the Vet, he might be able to help you convince " + kvp.Value.animalName + " to come home.");
                while (CanvasController.GetComponent<CanvasController>().notification.activeInHierarchy) yield return null;
                CanvasController.GetComponent<CanvasController>().closeCanvas();
                runaways.addExistingAnimal(kvp.Value);
                remove.Add(kvp.Key);
            } else if (kvp.Value.pregnant == true && kvp.Value.deliveryDate <= days) {
                foreach (int id in kvp.Value.babyId) {
                    confirmVar = false;
                    curAnimals.animalDict[id].age = 0;
                    PauseGame();
                    Time.timeScale = 0;
                    curAnimals.animalDict[id].location = kvp.Value.location;
                    curAnimals.animalDict[id].scene = kvp.Value.scene;
                    CanvasController.GetComponent<CanvasController>().openCanvas();
                    birthModal.GetComponent<Alert>().initiateBirthAlert(kvp.Value.animalName + " gave birth! What should the baby be called?", ((string name, int newId) => confirm(name, id)), curAnimals.animalDict[id], id);
                    while (!confirmVar) yield return null;
                    if (animalBreeds.breedDictionary.ContainsKey(curAnimals.animalDict[id].breed) && !animalBreeds.breedDictionary[curAnimals.animalDict[id].breed].unlocked) {
                        animalBreeds.breedDictionary[curAnimals.animalDict[id].breed].unlocked = true;
                        animalBreeds.breedDictionary[curAnimals.animalDict[id].breed].exampleColoring = curAnimals.animalDict[id].coloring;
                        notification.GetComponent<Notification>().initiateNotification("Congratulations, you've discovered the " + curAnimals.animalDict[id].breed + " breed!");
                        while (notification.activeInHierarchy) yield return null;
                    }
                }
                CanvasController.GetComponent<CanvasController>().closeCanvas();
                kvp.Value.pregnant = false;
                kvp.Value.health -= 5;
                kvp.Value.deliveryDate = 0;
            } else if (kvp.Value.age <= -2) {
                confirmVar = false;
                kvp.Value.age = 0;
                PauseGame();
                Time.timeScale = 0;
                CanvasController.GetComponent<CanvasController>().openCanvas();
                birthModal.GetComponent<Alert>().initiateBirthAlert("Shopkeeper brought this baby over! What should he be called?", ((string name, int newId) => confirm(name, newId)), kvp.Value, kvp.Value.id);
                while (!confirmVar) yield return null;
                if (animalBreeds.breedDictionary.ContainsKey(kvp.Value.breed) && !animalBreeds.breedDictionary[kvp.Value.breed].unlocked) {
                    animalBreeds.breedDictionary[kvp.Value.breed].unlocked = true;
                    animalBreeds.breedDictionary[kvp.Value.breed].exampleColoring = kvp.Value.coloring;
                    notification.GetComponent<Notification>().initiateNotification("Congratulations, you've discovered the " + kvp.Value.breed + " breed!");
                    while (notification.activeInHierarchy) yield return null;
                }
                CanvasController.GetComponent<CanvasController>().closeCanvas();
            }
        }
        foreach (int key in remove) {
            if (curAnimals.animalDict.ContainsKey(key)) {
                curAnimals.animalDict.Remove(key);
            }
        }
        animalList.GetComponent<AnimalList>().deleteAnimals();
        animalSpawner.GetComponent<SpawnAnimal>().SpawnAll();
        ResumeGame();
    }
    void UpdateAnimals() {
        StartCoroutine(waitConfirm());
    }
}
