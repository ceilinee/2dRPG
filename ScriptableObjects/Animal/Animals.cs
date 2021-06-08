using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Animals : ScriptableObject {
    [System.Serializable] public class DictionaryOfAnimals : SerializableDictionary<int, Animal> { }
    public DictionaryOfAnimals animalDict = new DictionaryOfAnimals();
    public string breedName;
    public void addAnimal(string newAnimalName,
    string newType, int newId, Vector2 newLocation, int newAge, string newMood,
    Animal.StringAndAnimalColor newColoring, bool newFollow, string newGender,
    string newHome, int newCost, bool newPregnant, int newDeliveryDate, int[] newBabyId, float newLove, string newBreed, string scene, bool charOwned, int charId, AnimalColors animalColors, Personality personality, int _momId, int _dadId) {
        Animal newAnimal = new Animal();
        newAnimal.createAnimal(newAnimalName,
        newType, newId, newLocation, newAge, newMood,
        newColoring, newFollow, newGender, newHome, newCost, newPregnant, newDeliveryDate, newBabyId, newLove, newBreed, scene, charOwned, charId, animalColors, personality, _momId, _dadId);
        animalDict[newId] = newAnimal;
    }
    public List<Animal> GetNewBreedAnimals() {
        List<List<Animal>> breeds = new List<List<Animal>>();
        // List<string> generalizableBreeds = new List<string> { "Generic", "Winter", "Spotted", "Starburst", "Dutch", "Starburst", "Sunbeam", "Star", "Sunshine", "Boots", "Spotted", "Masked", "Back Patch", "Moonlight", "Tuxedo", "Uniform", "Semiformal" };
        foreach (KeyValuePair<int, Animal> kvp in animalDict) {
            // if (generalizableBreeds.Contains(kvp.Value.breed)) {
            //check if current animal fits in any of the categories
            bool fit = false;
            foreach (List<Animal> list in breeds) {
                if (isSame(list[0].coloring, kvp.Value.coloring)) {
                    list.Add(kvp.Value);
                    fit = true;
                }
            }
            if (!fit) {
                List<Animal> newList = new List<Animal>();
                newList.Add(kvp.Value);
                breeds.Add(newList);
            }
            // }
        }
        List<Animal> result = new List<Animal>();
        foreach (List<Animal> list in breeds) {
            if (list.Count >= 2) {
                result.Add(list[0]);
            }
        }
        return result;
    }
    public bool isSame(Animal.StringAndAnimalColor coloring1, Animal.StringAndAnimalColor coloring2) {
        AnimalBreed animalBreed = new AnimalBreed();
        return animalBreed.matchRequest(coloring1, coloring2);
    }
    public float GetAverageHealth() {
        float sum = 0;
        float count = 0;
        foreach (KeyValuePair<int, Animal> kvp in animalDict) {
            sum += kvp.Value.health;
            count++;
        }
        return sum / count;
    }
    public int TotalAnimals(string barnName = "", bool includeUnbornAnimals = false) {
        int count = 0;
        foreach (KeyValuePair<int, Animal> kvp in animalDict) {
            if (barnName == "" || barnName == kvp.Value.home) {
                if (includeUnbornAnimals && kvp.Value.age <= -3) {
                    continue;
                }
                count++;
            }
        }
        return count;
    }
    public float GetSickAnimals() {
        float sum = 0;
        foreach (KeyValuePair<int, Animal> kvp in animalDict) {
            if (kvp.Value.health <= 30) {
                sum++;
            }
        }
        return sum;
    }
    public float GetTypes() {
        List<string> types = new List<string>();
        foreach (KeyValuePair<int, Animal> kvp in animalDict) {
            if (!types.Contains(kvp.Value.type)) {
                types.Add(kvp.Value.type);
            }
        }
        return types.Count;
    }
    public float GetBreeds() {
        List<string> breeds = new List<string>();
        foreach (KeyValuePair<int, Animal> kvp in animalDict) {
            if (!breeds.Contains(kvp.Value.breed)) {
                breeds.Add(kvp.Value.breed);
            }
        }
        return breeds.Count;
    }
    public float GetAverageFriendship() {
        float sum = 0;
        float count = 0;
        foreach (KeyValuePair<int, Animal> kvp in animalDict) {
            sum += kvp.Value.love;
            count++;
        }
        return sum / count;
    }
    public string GetEvaluation() {
        float health = GetAverageHealth(); //0-100
        float sick = GetSickAnimals(); //0-?
        float types = GetTypes(); // 0-6
        float breeds = GetBreeds(); // 0 - 40 
        float friendship = GetAverageFriendship(); // 0-500+
        float score = (health * 2) - (sick * 5) + (types * 2) + (breeds * 5) + (friendship / 2);
        string message = "";
        //construct initial message
        if (score < 100) {
            message += "F: Oh my, I think you have lots of work to do! Your score is " + score.ToString() + ". You should definitely try: ";
        } else if (score < 200) {
            message += "D: You've still got a bit to do! Your score is " + score.ToString() + ". You should really try: ";
        } else if (score < 300) {
            message += "C: You've definitely made solid progress since the beginning! Your score is " + score.ToString() + ". To improve even further you should try: ";
        } else if (score < 400) {
            message += "B: Your farm is really coming along! Your score is " + score.ToString() + ". To improve more you should try: ";
        } else if (score < 550) {
            message += "A: WOW, you're doing great! Your score is " + score.ToString() + ". To improve you should try: ";
        } else if (score >= 600 && score < 650) {
            message += "A+: These are some of the the best cared for animals I've seen! Your score is " + score.ToString() + ". Keep doing what you're doing! I just might have a surprise for you if you keep improving your score.";
            return message;
        } else if (score >= 650) {
            message += "A++: I submitted your name to the PAWS Best Farm of the Year Award, keep an eye out for the announcement! You definitely deserve it. Your score is " + score.ToString() + ". Keep doing what you're doing!";
            return message;
        }
        if (health < 100) {
            message += "ensuring all your animals are as healthy as can be";
        }
        if (sick > 1) {
            if (message.Substring(message.Length - 2) != ": ") {
                message += ", ";
            }
            message += "ensuring no animals are sick";
        }
        if (types < 6) {
            if (message.Substring(message.Length - 2) != ": ") {
                message += ", ";
            }
            message += "raising more types of animals to increase biodiversity";
        }
        if (breeds < 40) {
            if (message.Substring(message.Length - 2) != ": ") {
                message += ", ";
            }
            message += "finding and owning more breeds to increase genetic diversity";
        }
        if (friendship < 500) {
            if (message.Substring(message.Length - 2) != ": ") {
                message += ", ";
            }
            message += "increasing your friendship with your animals";
        }
        return message;
    }
    public bool ContainsAnimal(int id) {
        return animalDict.ContainsKey(id);
    }

    public void addExistingAnimal(Animal newAnimal) {
        animalDict[newAnimal.id] = newAnimal;
    }
    public void ClearDailies() {
        foreach (KeyValuePair<int, Animal> kvp in animalDict) {
            kvp.Value.walked = false;
            kvp.Value.presentsDaily = 0;
        }
    }
    public void Clear() {
        animalDict = new DictionaryOfAnimals();
        breedName = null;
    }
    public void removeExistingAnimal(int id) {
        animalDict.Remove(id);
    }
    public void ageAnimals(int time) {
        foreach (KeyValuePair<int, Animal> kvp in animalDict) {
            if (kvp.Value.age != -1) {
                kvp.Value.age += time;
            }
        }
    }
    public void dailyAnimalUpdate() {
        foreach (KeyValuePair<int, Animal> kvp in animalDict) {
            if (kvp.Value.presentsDaily == 0 && !kvp.Value.walked) {
                kvp.Value.mood = "Feeling lonely, would like to go on a walk";
                kvp.Value.moodId = "sad";
                kvp.Value.health -= 2;
            }
            if (kvp.Value.presentsDaily >= 1 || kvp.Value.walked || kvp.Value.health >= 90) {
                kvp.Value.mood = "Feeling content";
                kvp.Value.moodId = "2";
                kvp.Value.health += 1;
            } else if (kvp.Value.presentsDaily >= 1 && kvp.Value.walked) {
                kvp.Value.mood = "Feeling quite pleased!";
                kvp.Value.moodId = "3";
                kvp.Value.health += 2;
            } else if (kvp.Value.presentsDaily == 3 && kvp.Value.walked) {
                kvp.Value.mood = "Feeling ecstatic and lucky!";
                kvp.Value.moodId = "4";
                kvp.Value.health += 3;
            }
            if (kvp.Value.health <= 30) {
                kvp.Value.mood = "Feeling under the weather";
                kvp.Value.moodId = "";
            }
            if (kvp.Value.health <= 20) {
                kvp.Value.mood = "Feeling sick";
                kvp.Value.moodId = "sick";
            }
            if (kvp.Value.health <= 10) {
                kvp.Value.mood = "Needs to go see the vet";
                kvp.Value.moodId = "sick";
            }
        }
    }
    public void updateAnimal(Animal animal) {
        if (animalDict.ContainsKey(animal.id)) {
            animalDict[animal.id] = animal;
        }
    }
}
