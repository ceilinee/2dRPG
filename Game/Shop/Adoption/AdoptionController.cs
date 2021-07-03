using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdoptionController : MonoBehaviour {
    public AdoptionRequests adoptionRequests;
    public Characters charList;
    public AnimalColors animalColors;
    public GameObject BreedScript;
    public AnimalBreed animalBreeds;
    public AnimalBreed finalAnimalBreeds;
    public Personalities personalities;
    public AnimalBreed genericAnimals;
    public GameObject mailController;
    public Player player;
    // Start is called before the first frame update
    void Start() {
        animalBreeds.updateBreedDictionary();
        animalBreeds.updateAllBreeds(finalAnimalBreeds);
        genericAnimals.updateBreedDictionary();
    }
    public void startRequest() {
        if (player.reputation <= 1000) {
            float prob = Random.Range(0f, 1f);
            if (prob < 0.3) {
                generateRequest(true, false, false);
            } else if (prob < 0.6) {
                generateRequest(false, true, false);
            } else if (prob < 0.9) {
                generateRequest(true, true, false);
            } else {
                generateRequest(false, false, true);
            }
        } else if (player.reputation <= 2000) {
            float prob = Random.Range(0f, 1f);
            if (prob < 0.1) {
                generateRequest(true, false, false);
            } else if (prob < 0.2) {
                generateRequest(false, true, false);
            } else if (prob < 0.6) {
                generateRequest(true, true, false);
            } else if (prob < 0.9) {
                generateRequest(false, false, true);
            } else {
                generateRequest(true, false, true);
            }
        } else {
            float prob = Random.Range(0f, 1f);
            if (prob < 0.1) {
                generateRequest(true, false, false);
            } else if (prob < 0.2) {
                generateRequest(false, true, false);
            } else if (prob < 0.5) {
                generateRequest(true, true, false);
            } else if (prob < 0.8) {
                generateRequest(false, false, true);
            } else {
                generateRequest(true, false, true);
            }
        }
    }
    public void generateRequest(bool personality, bool generic, bool breed) {
        AdoptionRequest request = new AdoptionRequest();
        Character selectedChar = charList.getRandomCharacter();
        int id = Random.Range(0, 100000000);
        request.charId = selectedChar.id;
        request.id = id;
        if (personality) {
            Personality selectedPersonality = personalities.getRandomPersonality();
            AnimalBreed.Breed selectedBreed = genericAnimals.breedDictionary["All"];
            request.coloring = selectedBreed.coloring;
            request.personality = selectedPersonality;
        }
        if (generic) {
            if (selectedChar.preferredAnimals.Length >= 1) {
                request.type = selectedChar.preferredAnimals[Random.Range(0, selectedChar.preferredAnimals.Length)];
            } else {
                request.type = BreedScript.GetComponent<BreedScript>().getType();
            }
            AnimalBreed.Breed selectedBreed = genericAnimals.getRandomBreed(type: request.type);
            request.coloring = BreedScript.GetComponent<BreedScript>().adoptionGenericAnimal(selectedBreed.coloring);
            request.breed = selectedBreed.breedName;
            request.price = (int) System.Math.Floor(BreedScript.GetComponent<BreedScript>().GetPrice(request.coloring, request.type) * selectedBreed.multiplier * selectedChar.multiplier);
        }
        if (breed) {
            if (selectedChar.preferredAnimals.Length >= 1) {
                request.type = selectedChar.preferredAnimals[Random.Range(0, selectedChar.preferredAnimals.Length)];
            } else {
                request.type = BreedScript.GetComponent<BreedScript>().getType();
            }
            AnimalBreed.Breed selectedBreed = animalBreeds.getRandomBreed(type: request.type);
            Animal.StringAndAnimalColor coloring = selectedBreed.coloring;
            request.coloring = coloring;
            request.breed = selectedBreed.breedName;
            request.price = (int) System.Math.Floor(BreedScript.GetComponent<BreedScript>().GetPrice(coloring, request.type) * selectedBreed.multiplier * selectedChar.multiplier);
        }
        request.message = "I want this llamamaaa";
        addToAdoptionRequest(request);
    }
    public void addToAdoptionRequest(AdoptionRequest request) {
        adoptionRequests.addRequest(request);
    }
    // Update is called once per frame
    void Update() {
        // startRequest();
    }
}
