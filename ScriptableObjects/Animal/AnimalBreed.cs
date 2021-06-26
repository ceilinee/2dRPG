using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu]
[System.Serializable]
public class AnimalBreed : ScriptableObject {
    [System.Serializable]
    public class Breed {
        public bool unlocked;
        public Animal.StringAndAnimalColor exampleColoring = new Animal.StringAndAnimalColor();
        public int multiplier;
        public List<Type> notIncludeType = null;
        public string breedName;
        public string breedDescription;
        public bool userCreated;
        public Animal.StringAndAnimalColor coloring = new Animal.StringAndAnimalColor();
    }
    public Breed[] breedArray;
    [System.Serializable] public class DictionaryOfBreed : SerializableDictionary<string, Breed> { }
    public DictionaryOfBreed breedDictionary = new DictionaryOfBreed();
    public Breed getRandomBreed(string notInclude = "n/a", Type type = Type.NOTSELECTED) {
        System.Random random = new System.Random();
        List<Breed> breed = new List<Breed>();
        for (int i = 0; i < breedDictionary.Count; i++) {
            Breed curBreed = breedDictionary.Values.ElementAt(i);
            if (curBreed.breedName != notInclude && !curBreed.notIncludeType.Contains(type)) {
                breed.Add(breedDictionary.Values.ElementAt(i));
            }
        }
        return breed.ToArray()[Random.Range(0, breed.Count)];
    }
    public Breed getRandomBreed(int rarity, string notInclude = "n/a", Type type = Type.LLAMA) {
        System.Random random = new System.Random();
        List<Breed> breed = new List<Breed>();
        for (int i = 0; i < breedDictionary.Count; i++) {
            Breed curBreed = breedDictionary.Values.ElementAt(i);
            if (curBreed.breedName != notInclude && !curBreed.notIncludeType.Contains(type) && curBreed.multiplier <= rarity) {
                breed.Add(breedDictionary.Values.ElementAt(i));
            }
        }
        Debug.Log("breeds in list: " + breed.Count + " rarity: " + rarity);
        return breed.ToArray()[Random.Range(0, breed.Count)];
    }
    public void Add(Breed breed) {
        List<Breed> breedList = breedArray.ToList();
        breed.userCreated = true;
        breed.unlocked = true;
        breed.exampleColoring = breed.coloring;
        breedList.Add(breed);
        breedArray = breedList.ToArray();
        breedDictionary[breed.breedName] = breed;
    }

    public void updateBreedDictionary() {
        breedDictionary = new DictionaryOfBreed();
        for (int i = 0; i < breedArray.Length; i++) {
            breedDictionary[breedArray[i].breedName] = breedArray[i];
        }
    }
    public void updateAllBreeds(AnimalBreed fullList) {
        List<Breed> breeds = new List<Breed>();
        DictionaryOfBreed breedDict = new DictionaryOfBreed();
        for (int i = 0; i < fullList.breedArray.Length; i++) {
            Breed curBreed = fullList.breedArray[i];
            if (breedDictionary.ContainsKey(curBreed.breedName)) {
                curBreed.unlocked = breedDictionary[curBreed.breedName].unlocked;
            }
            breedDict[curBreed.breedName] = curBreed;
            breeds.Add(curBreed);
        }
        breedDictionary = breedDict;
        breedArray = breeds.ToArray();
    }
    public void Clear() {
        List<Breed> newList = new List<Breed>();
        for (int j = 0; j < breedArray.Length; j++) {
            breedArray[j].unlocked = false;
            if (!breedArray[j].userCreated) {
                newList.Add(breedArray[j]);
            }
        }
        breedArray = newList.ToArray();
    }
    public int isBreed(Animal.StringAndAnimalColor coloring, Type type = Type.LLAMA) {
        int selected = -1;
        for (int j = 0; j < breedArray.Length; j++) {
            //go through breed array
            Animal.StringAndAnimalColor breedColor = breedArray[j].coloring;
            int[] partArray = new int[] { breedColor.body, breedColor.eyes, breedColor.back, breedColor.dots, breedColor.star, breedColor.ears, breedColor.legs, breedColor.tail, breedColor.face };
            int[] coloringArray = new int[] { coloring.body, coloring.eyes, coloring.back, coloring.dots, coloring.star, coloring.ears, coloring.legs, coloring.tail, coloring.face };
            int all2 = -1;
            int all3 = -1;
            for (int i = 0; i < partArray.Length; i++) {
                // 34 means ignore the part (ears for fish for example)
                if (coloringArray[i] == 34) {
                    continue;
                }
                if (partArray[i] == 32 || partArray[i] == 33) {
                    if (coloringArray[i] == 27) {
                        break;
                    }
                    if (partArray[i] == 32) {
                        if (all2 == -1) {
                            all2 = coloringArray[i];
                        } else if (all2 != coloringArray[i]) {
                            break;
                        }
                    }
                    if (partArray[i] == 33) {
                        if (all3 == -1) {
                            all3 = coloringArray[i];
                        } else if (all3 != coloringArray[i]) {
                            break;
                        }
                    }
                } else if (partArray[i] != 31) {
                    if (partArray[i] != coloringArray[i]) {
                        break;
                    }
                }
                if (i == partArray.Length - 1) {
                    if (selected == -1) {
                        selected = j;
                    } else if (breedArray[selected].multiplier < breedArray[j].multiplier) {
                        selected = j;
                    }
                }
            }
        }
        return selected;
    }

    public bool matchRequest(Animal.StringAndAnimalColor coloring, Animal.StringAndAnimalColor request) {
        bool selected = false;
        Animal.StringAndAnimalColor breedColor = request;
        int[] partArray = new int[] { breedColor.body, breedColor.eyes, breedColor.back, breedColor.dots, breedColor.star, breedColor.ears, breedColor.legs, breedColor.face, breedColor.tail };
        int[] coloringArray = new int[] { coloring.body, coloring.eyes, coloring.back, coloring.dots, coloring.star, coloring.ears, coloring.legs, coloring.face, coloring.tail };
        int all2 = -1;
        int all3 = -1;
        for (int i = 0; i < partArray.Length; i++) {
            // 34 means ignore the part (ears for fish for example)
            if (partArray[i] == 34) {
                continue;
            }
            if (partArray[i] == 32 || partArray[i] == 33) {
                if (coloringArray[i] == 27) {
                    break;
                }
                if (partArray[i] == 32) {
                    if (all2 == -1) {
                        all2 = coloringArray[i];
                    } else if (all2 != coloringArray[i]) {
                        break;
                    }
                }
                if (partArray[i] == 33) {
                    if (all3 == -1) {
                        all3 = coloringArray[i];
                    } else if (all3 != coloringArray[i]) {
                        break;
                    }
                }
            } else if (partArray[i] != 31) {
                if (partArray[i] != coloringArray[i]) {
                    break;
                }
            }
            if (i == partArray.Length - 1) {
                return true;
            }
        }
        return selected;
    }

}
