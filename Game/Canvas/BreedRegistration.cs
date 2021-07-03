using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreedRegistration : CustomMonoBehaviour {
    // Start is called before the first frame update
    public InputField breedName;
    public Text body;
    public Text spots;
    public Text back;
    public Text legs;
    public Text tail;
    public Text star;
    public Text face;
    public Text eyes;
    public Text ears;
    public Player player;
    public AnimalBreed animalBreed;
    private CanvasController canvasController;

    public Animal.StringAndAnimalColor coloring;
    public AnimalColors animalColors;
    public GameObject background;
    void Start() {
        breedName.onEndEdit.AddListener(displayText);
        canvasController = centralController.centralDictionary["CanvasController"].GetComponent<CanvasController>();
    }
    private void displayText(string textInField) {
        print(textInField);
    }
    public void StartRegistration(Animal.StringAndAnimalColor _coloring) {
        coloring = _coloring;
        body.text = animalColors.colorDictionary[coloring.body].ColorName;
        eyes.text = animalColors.colorDictionary[coloring.eyes].ColorName;
        face.text = animalColors.colorDictionary[coloring.face].ColorName;
        ears.text = animalColors.colorDictionary[coloring.ears].ColorName;
        if (animalColors.colorDictionary[coloring.ears].ColorName == "Invisible") {
            ears.text = "N/A";
        }
        spots.text = animalColors.colorDictionary[coloring.dots].ColorName;
        if (animalColors.colorDictionary[coloring.dots].ColorName == "Invisible") {
            spots.text = "N/A";
        }
        back.text = animalColors.colorDictionary[coloring.back].ColorName;
        if (animalColors.colorDictionary[coloring.back].ColorName == "Invisible") {
            back.text = "N/A";
        }
        legs.text = animalColors.colorDictionary[coloring.legs].ColorName;
        if (animalColors.colorDictionary[coloring.legs].ColorName == "Invisible") {
            legs.text = "N/A";
        }
        tail.text = animalColors.colorDictionary[coloring.tail].ColorName;
        if (animalColors.colorDictionary[coloring.tail].ColorName == "Invisible") {
            tail.text = "N/A";
        }
        star.text = animalColors.colorDictionary[coloring.star].ColorName;
        if (animalColors.colorDictionary[coloring.star].ColorName == "Invisible") {
            star.text = "N/A";
        }
        gameObject.SetActive(true);
        if (background) {
            background.SetActive(true);
        }
    }
    public void submit() {
        string name = breedName.text;
        breedName.text = " ";
        AnimalBreed.Breed breed = new AnimalBreed.Breed();
        breed.breedName = name;
        breed.coloring = coloring;
        breed.breedDescription = "This breed was created by "
         + player.playerName +
         ", a farmer in Llama Town. This breed is characterized by: "
          + (body.text + " coloured body, ")
            + (eyes.text + " coloured eyes, ")
              + (face.text + " coloured face, ")
                + (ears.text + " coloured ears, ")
                  + (spots.text == "N/A" ? "" : spots.text + " coloured spots, ")
                    + (back.text == "N/A" ? "" : back.text + " coloured back, ")
                      + (legs.text == "N/A" ? "" : legs.text + " coloured legs, ")
                        + (tail.text == "N/A" ? "" : tail.text + " coloured tail")
                          + (star.text == "N/A" ? "." : ", " + star.text + " coloured star.");

        breed.multiplier = System.Math.Min(5, 1 + (int) System.Math.Floor(player.reputation / 300));
        animalBreed.Add(breed);
        gameObject.SetActive(false);
        if (background) {
            background.SetActive(false);
        }
        canvasController.initiateNotification("Your new breed " + breed.breedName + " has been registered! Its multiplier is " + breed.multiplier, true);
    }
    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            gameObject.SetActive(false);
            if (background) {
                background.SetActive(false);
            }
        }
    }
}
