﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Type {
    PIG,
    LLAMA,
    FISH,
    SHEEP,
    GOOSE,
    RABBIT,
    NOTSELECTED
}
public class BreedScript : CustomMonoBehaviour {
    public Animal femaleAnimal;
    public GameObject animalModal;
    public Animal maleAnimal;
    public Animals curAnimals;
    public Animals shopAnimals;
    public Animals shopBreedAnimals;
    public Type[] types;
    public int[] multiplers;
    [System.Serializable] public class DictionaryOfTypeAndInt : SerializableDictionary<Type, int> { }
    [System.Serializable] public class DictionaryOfStringAndInt : SerializableDictionary<string, int> { }
    public DictionaryOfTypeAndInt typeDictionary = new DictionaryOfTypeAndInt();
    public Personalities personalityList;
    public AnimalBreed animalBreed;
    public AnimalColors animalColors;
    public GameObject animalList;
    public GameObject spawnAnimal;
    public GameObject modelAnimal;
    public GameObject confirmationModal;
    public GameObject timeController;
    public BuySellAnimal buySellAnimal;
    public CanvasController canvasController;
    public Scene scene;

    void Awake() {
        scene = SceneManager.GetActiveScene();
    }
    void Start() {
        for (int i = 0; i < types.Length; i++) {
            typeDictionary[types[i]] = multiplers[i];
        }
        buySellAnimal = centralController.Get("AnimalBuySell").GetComponent<BuySellAnimal>();
        canvasController = centralController.Get("CanvasController").GetComponent<CanvasController>();
        // fillShopBreedAnimal();
        // Only run following code when animalColors has been changed
        // animalColors.BreedMatrix = new AnimalColors.intArray[animalColors.LlamaArray.Length];
        // for (int i = 0; i < animalColors.LlamaArray.Length; i++) {
        //     Debug.Log(animalColors.BreedMatrix[i]);
        //     animalColors.BreedMatrix[i] = new AnimalColors.intArray();
        //     animalColors.BreedMatrix[i].array = new int[animalColors.LlamaArray.Length];
        // }
        // for (int i = 0; i < animalColors.LlamaArray.Length; i++) {
        //     animalColors.colorDictionary[animalColors.LlamaArray[i].id] = animalColors.LlamaArray[i];
        //     for (int j = 0; j < animalColors.LlamaArray.Length; j++) {
        //         Debug.Log(i);
        //         Debug.Log(j);
        //         Color mixture = new Color(
        //         (animalColors.LlamaArray[i].color.r + animalColors.LlamaArray[j].color.r) / 2f,
        //         (animalColors.LlamaArray[i].color.g + animalColors.LlamaArray[j].color.g) / 2f,
        //         (animalColors.LlamaArray[i].color.b + animalColors.LlamaArray[j].color.b) / 2f,
        //         (animalColors.LlamaArray[i].color.a + animalColors.LlamaArray[j].color.a) / 2f
        //         );
        //         if (animalColors.LlamaArray[i].color.a == 1 || animalColors.LlamaArray[j].color.a == 1) {

        //         }
        //         int colorId = findClosestColor(animalColors.LlamaArray, mixture);
        //         Debug.Log(colorId);
        //         animalColors.BreedMatrix[animalColors.LlamaArray[i].id].array[animalColors.LlamaArray[j].id] = colorId;
        //         animalColors.BreedMatrix[animalColors.LlamaArray[j].id].array[animalColors.LlamaArray[i].id] = colorId;
        //     }
        // }
        // List<int> list = new List<int>();
        // for (int i = 0; i < animalColors.LlamaArray.Length; i++) {
        //     for (int j = 0; j < animalColors.LlamaArray[i].probability; j++) {
        //         list.Add(animalColors.LlamaArray[i].id);
        //     }
        // }
        // animalColors.ShopArray = list.ToArray();
    }
    public void initDictionary() {
        for (int i = 0; i < types.Length; i++) {
            typeDictionary[types[i]] = multiplers[i];
        }
    }
    public int findClosestColor(AnimalColor[] array, Color mixture) {
        Vector4 mixtureVector4 = (Vector4) mixture;
        double minResult = 100000000000;
        int minId = 0;
        for (int i = 0; i < array.Length; i++) {
            if (System.Math.Abs(Vector4.SqrMagnitude((Vector4) array[i].color - mixtureVector4)) < minResult) {
                minResult = System.Math.Abs(Vector4.SqrMagnitude((Vector4) array[i].color - mixtureVector4));
                minId = i;
            }
        }
        // for(int i = 0; i<array.Length; i++){
        //   Debug.Log(ColorDifference(array[i].color, mixture));
        //   if(ColorDifference(array[i].color, mixture) < minResult){
        //     minResult = ColorCalculation.CalculateDeltaE(array[i].color, mixture);
        //     minId = i;
        //   }
        // }
        return minId;
    }
    public static double ColourDistance(Color e1, Color e2) {
        long rmean = ((long) e1.r + (long) e2.r) / 2;
        long r = (long) e1.r - (long) e2.r;
        long g = (long) e1.g - (long) e2.g;
        long b = (long) e1.b - (long) e2.b;
        return System.Math.Sqrt((((512 + rmean) * r * r) >> 8) + 4 * g * g + (((767 - rmean) * b * b) >> 8));
    }
    private double ColorDifference(Color color1, Color color2) {
        return System.Math.Abs((.11 * color1.b + .59 * color1.g + .30 * color1.r) - (.11 * color2.b + .59 * color2.g + .30 * color2.r));
    }
    public void BreedAnimals(Animal newAnimal1, Animal newAnimal2) {
        if (newAnimal1.gender == "Female") {
            femaleAnimal = newAnimal1;
            maleAnimal = newAnimal2;
        } else {
            maleAnimal = newAnimal1;
            femaleAnimal = newAnimal2;
        }
        GenerateColor();
        confirmationModal.GetComponent<Confirmation>().initiateConfirmation(
        "Are you sure you want to breed " + femaleAnimal.animalName + " and " + maleAnimal.animalName + "?",
        (() => confirm()),
        (() => cancel()),
        () => { },
        true
        );
    }

    public int GenerateAnimal() {
        Animal.StringAndAnimalColor coloring = GenerateColor();
        string gender = GenerateGender();
        int breedId = animalBreed.isBreed(coloring, femaleAnimal.type);
        int price = GetPrice(coloring, femaleAnimal.type);
        string breed = "Generic";
        if (breedId != -1) {
            breed = animalBreed.breedArray[breedId].breedName;
            price *= animalBreed.breedArray[breedId].multiplier;
        }
        int randomNumber = Random.Range(0, 1000000000);
        Personality personality = generatePersonality(false);
        Vector2 location = new Vector2(Random.Range(1.0F, 3.5F), Random.Range(-1.0F, -3.5F));
        curAnimals.addAnimal(GenerateName(), femaleAnimal.type, randomNumber, location, -1, "Excited", coloring, false, GenerateGender(), "Barn#0", price, false, 0, new int[] { }, 0.0f, breed, femaleAnimal.scene, false, 0, animalColors, personality, femaleAnimal.id, maleAnimal.id);
        return randomNumber;
    }

    public int GetPrice(Animal.StringAndAnimalColor coloring, Type type) {
        if (!typeDictionary.ContainsKey(type)) {
            initDictionary();
        }
        double price = 0;
        price += (animalColors.colorDictionary[coloring.body].cost * typeDictionary[type]);
        price += (animalColors.colorDictionary[coloring.ears].cost * 0.2);
        price += (animalColors.colorDictionary[coloring.eyes].cost * 0.2);
        price += (animalColors.colorDictionary[coloring.legs].cost * 0.8);
        price += (animalColors.colorDictionary[coloring.dots].cost * 0.6);
        price += (animalColors.colorDictionary[coloring.back].cost * 0.5);
        price += (animalColors.colorDictionary[coloring.star].cost * 0.8);
        price += (animalColors.colorDictionary[coloring.tail].cost * 0.2);
        return (int) System.Math.Floor(price);
    }
    public void confirm() {
        if (!buySellAnimal.CanBuyAnimal()) {
            canvasController.initiateNotification("Sorry! Looks like your barns have a capacity of " + buySellAnimal.totalAnimalCapacity + ", and you already have " + buySellAnimal.totalAnimals + " animals . Try upgrading your barns or purchasing more!", true);
            return;
        };
        int babyCount = (int) System.Math.Floor(Random.Range(1, Mathf.Min(buySellAnimal.totalAnimalCapacity - buySellAnimal.totalAnimals, 2 * ((femaleAnimal.love / 500) + (maleAnimal.love / 500) + 1))));
        int[] newBabyId = new int[babyCount];
        for (int i = 0; i < newBabyId.Length; i++) {
            int id = GenerateAnimal();
            newBabyId[i] = id;
        }
        curAnimals.animalDict[maleAnimal.id].babyId = newBabyId;
        curAnimals.animalDict[femaleAnimal.id].babyId = newBabyId;
        curAnimals.animalDict[femaleAnimal.id].pregnant = true;
        curAnimals.animalDict[femaleAnimal.id].deliveryDate = timeController.GetComponent<TimeController>().days + Random.Range(1, 2 * (babyCount + 1));
        // animalModal.GetComponent<AnimalInformation>().CloseIfPlayerMenuNotOpen();
        animalModal.GetComponent<AnimalInformation>().NotifyDeliveryDate(femaleAnimal);
        timeController.GetComponent<TimeController>().ResumeGame();
    }

    public void cancel() {
        // do nothing
    }

    public string GenerateGender() {
        var gender = new string[] { "Male", "Female" };
        return gender[Random.Range(0, 2)];
    }

    public int[] GenerateColorArray(string part) {
        var colorArray = new int[] {
       femaleAnimal.getPart(part),
       maleAnimal.getPart(part),
       animalColors.BreedMatrix[maleAnimal.getPart(part)].array[femaleAnimal.getPart(part)],
      };
        return colorArray;
    }

    public Animal.StringAndAnimalColor GenerateColor() {
        Animal.StringAndAnimalColor coloring = new Animal.StringAndAnimalColor();
        RectTransform trans = modelAnimal.GetComponent<RectTransform>();
        //Generate tail colouranimalColors.colorDictionary[]
        var tailColorArray = GenerateColorArray("tail");
        coloring.tail = femaleAnimal.type == Type.PIG ? 34 : tailColorArray[Random.Range(0, tailColorArray.Length)];
        //Generate body colour
        var bodyColorArray = GenerateColorArray("body");
        coloring.body = bodyColorArray[Random.Range(0, bodyColorArray.Length)];
        trans.Find("TailSocket").gameObject.GetComponent<Image>().color = animalColors.colorDictionary[coloring.tail].color;
        modelAnimal.GetComponent<Image>().color = animalColors.colorDictionary[coloring.body].color;
        //Generate eye colours
        var eyesColorArray = GenerateColorArray("eyes");
        coloring.eyes = eyesColorArray[Random.Range(0, eyesColorArray.Length)];
        trans.Find("EyesSocket").gameObject.GetComponent<Image>().color = animalColors.colorDictionary[coloring.eyes].color;
        //Generate star colour
        var starColorArray = GenerateColorArray("star");
        coloring.star = starColorArray[Random.Range(0, starColorArray.Length)];
        trans.Find("StarSocket").gameObject.GetComponent<Image>().color = animalColors.colorDictionary[coloring.star].color;
        //Generate star colour
        var faceColorArray = GenerateColorArray("face");
        coloring.face = faceColorArray[Random.Range(0, faceColorArray.Length)];
        trans.Find("FaceSocket").gameObject.GetComponent<Image>().color = animalColors.colorDictionary[coloring.face].color;
        //Generate star colour
        var dotsColorArray = GenerateColorArray("dots");
        coloring.dots = dotsColorArray[Random.Range(0, dotsColorArray.Length)];
        trans.Find("DotsSocket").gameObject.GetComponent<Image>().color = animalColors.colorDictionary[coloring.dots].color;

        var legsColorArray = GenerateColorArray("legs");
        coloring.legs = legsColorArray[Random.Range(0, legsColorArray.Length)];
        trans.Find("LegsSocket").gameObject.GetComponent<Image>().color = animalColors.colorDictionary[coloring.legs].color;

        var earsColorArray = GenerateColorArray("ears");
        coloring.ears = femaleAnimal.type == Type.FISH || femaleAnimal.type == Type.GOOSE ? 34 : earsColorArray[Random.Range(0, earsColorArray.Length)];
        trans.Find("EarsSocket").gameObject.GetComponent<Image>().color = animalColors.colorDictionary[coloring.ears].color;

        var backColorArray = GenerateColorArray("back");
        coloring.back = backColorArray[Random.Range(0, backColorArray.Length)];
        trans.Find("BackSocket").gameObject.GetComponent<Image>().color = animalColors.colorDictionary[coloring.back].color;
        animalBreed.isBreed(coloring, femaleAnimal.type);
        return coloring;
    }

    public string GenerateName() {
        var firstname = new string[] { "FurBall", "Pink", "Blue", "Night", "Cute", "Soft" };
        var secondname = new string[] { "e", "ee", "ie", "y" };
        return firstname[Random.Range(0, firstname.Length)] + secondname[Random.Range(0, secondname.Length)];
    }
    public Personality generatePersonality(bool shop) {
        if (shop) {
            return personalityList.getRandomPersonality();
        } else {
            if (Random.Range(0, 4) < 3 && maleAnimal.personality != null && femaleAnimal.personality != null) {
                Personality[] tempArray = new Personality[] { maleAnimal.personality, femaleAnimal.personality };
                return tempArray[Random.Range(0, tempArray.Length)];
            } else {
                return generatePersonality(true);
            }
        }
    }
    //Random Animal
    public int RandomShopAnimal(Animals paramAnimals, Type curType = Type.NOTSELECTED) {
        Type type = curType != Type.NOTSELECTED ? curType : getType();
        Animal.StringAndAnimalColor coloring = new Animal.StringAndAnimalColor();
        coloring.body = animalColors.ShopArray[Random.Range(0, animalColors.ShopArray.Length)];
        coloring.tail = type == Type.PIG ? 34 : coloring.body;
        coloring.face = animalColors.FaceArray[Random.Range(0, animalColors.FaceArray.Length)];
        coloring.star = 27;
        coloring.eyes = animalColors.EyesArray[Random.Range(0, animalColors.EyesArray.Length)];
        coloring.legs = 27;
        coloring.dots = 27;
        coloring.back = 27;
        coloring.ears = 27;
        if (Random.Range(0, 10) >= 9) {
            coloring.star = animalColors.ShopArray[Random.Range(0, animalColors.ShopArray.Length)];
        }
        if (Random.Range(0, 10) >= 9) {
            coloring.legs = animalColors.ShopArray[Random.Range(0, animalColors.ShopArray.Length)];
        }
        if (Random.Range(0, 10) >= 9) {
            coloring.dots = animalColors.ShopArray[Random.Range(0, animalColors.ShopArray.Length)];
        }
        if (Random.Range(0, 10) >= 9) {
            coloring.back = animalColors.ShopArray[Random.Range(0, animalColors.ShopArray.Length)];
        }
        if (Random.Range(0, 10) >= 9) {
            coloring.ears = animalColors.ShopArray[Random.Range(0, animalColors.ShopArray.Length)];
        }
        if (type == Type.FISH || type == Type.GOOSE) {
            coloring.ears = 34;
        }
        int breedId = animalBreed.isBreed(coloring, type);
        int price = GetPrice(coloring, type);
        string breed = "Generic";
        if (breedId != -1) {
            breed = animalBreed.breedArray[breedId].breedName;
            price *= animalBreed.breedArray[breedId].multiplier;
        }
        int randomNumber = Random.Range(0, 1000000000);
        Personality personality = generatePersonality(true);
        Vector2 location = new Vector2(Random.Range(1.0F, 3.5F), Random.Range(-1.0F, -3.5F));
        if (paramAnimals != null) {
            paramAnimals.addAnimal("Baby", type, randomNumber, location, -3, "Excited", coloring, false, GenerateGender(), "Barn#0", price, false, 0, new int[] { }, 0, breed, scene.name, false, 0, animalColors, personality, 0, 0);
        } else {
            shopAnimals.addAnimal("Baby", type, randomNumber, location, -3, "Excited", coloring, false, GenerateGender(), "Barn#0", price, false, 0, new int[] { }, 0, breed, scene.name, false, 0, animalColors, personality, 0, 0);
        }
        return randomNumber;
    }
    // public void fillShopBreedAnimal(int number){
    //   AnimalBreed.Breed curBreed = animalBreed.breedArray[Random.Range(0, animalBreed.breedArray.Length)];
    //   for(int i =0; i<number; i++){
    //     RandomBreedAnimal(curBreed);
    //   }
    // }
    public void clearSpecialShop() {
        shopBreedAnimals.animalDict = new Animals.DictionaryOfAnimals();
    }
    public Animal.StringAndAnimalColor adoptionGenericAnimal(Animal.StringAndAnimalColor modelColoring) {
        // TODO: Check if we need to make deep copy
        Animal.StringAndAnimalColor coloring = modelColoring.Clone();
        int[] curTypes = new int[] { coloring.body, coloring.tail, coloring.face, coloring.star, coloring.eyes, coloring.legs, coloring.dots, coloring.back, coloring.ears };
        for (int i = 0; i < curTypes.Length; i++) {
            if (curTypes[i] == 32) {
                if (i == 4) {
                    curTypes[i] = animalColors.EyesArray[Random.Range(0, animalColors.EyesArray.Length)];
                }
                if (i == 2) {
                    curTypes[i] = animalColors.FaceArray[Random.Range(0, animalColors.FaceArray.Length)];
                } else {
                    curTypes[i] = animalColors.ShopArray[Random.Range(0, animalColors.ShopArray.Length)];
                }
            }
        }
        coloring.body = curTypes[0];
        coloring.tail = curTypes[1];
        coloring.face = curTypes[2];
        coloring.star = curTypes[3];
        coloring.eyes = curTypes[4];
        coloring.legs = curTypes[5];
        coloring.dots = curTypes[6];
        coloring.back = curTypes[7];
        coloring.ears = curTypes[8];
        return coloring;
    }
    public Type getType(List<Type> notTypes = null) {
        List<Type> filtered = new List<Type>();
        Type[] types = (Type[]) System.Enum.GetValues(typeof(Type));
        for (int i = 0; i < types.Length; i++) {
            if (notTypes == null && types[i] != Type.NOTSELECTED) {
                filtered.Add(types[i]);
            } else if (notTypes != null && !notTypes.Contains(types[i]) && types[i] != Type.NOTSELECTED) {
                filtered.Add(types[i]);
            } else if (notTypes == null) {
                filtered.Add(types[i]);
            }
        }
        return filtered.ToArray()[Random.Range(0, filtered.ToArray().Length)];
    }
    public int RandomBreedAnimal(AnimalBreed.Breed curBreed, Animals paramAnimals, Type curType = Type.NOTSELECTED) {
        Type type = curType != Type.NOTSELECTED ? curType : getType(curBreed.notIncludeType);
        Animal.StringAndAnimalColor breedColoring = curBreed.coloring;
        Animal.StringAndAnimalColor coloring = new Animal.StringAndAnimalColor();
        int color1 = -1;
        int color2 = -1;
        int[] curTypes = new int[] { coloring.body, coloring.tail, coloring.face, coloring.star, coloring.eyes, coloring.legs, coloring.dots, coloring.back, coloring.ears };
        int[] breedTypes = new int[] { breedColoring.body, breedColoring.tail, breedColoring.face, breedColoring.star, breedColoring.eyes, breedColoring.legs, breedColoring.dots, breedColoring.back, breedColoring.ears };
        for (int i = 0; i < curTypes.Length; i++) {
            if (breedTypes[i] == 31) {
                if (i == 2) {
                    curTypes[i] = animalColors.FaceArray[Random.Range(0, animalColors.FaceArray.Length)];
                } else if (i == 0 || i == 1) {
                    curTypes[i] = animalColors.ShopArray[Random.Range(0, animalColors.ShopArray.Length)];
                } else if (i == 4) {
                    curTypes[i] = animalColors.EyesArray[Random.Range(0, animalColors.EyesArray.Length)];
                } else {
                    curTypes[i] = 27;
                }
            } else if (breedTypes[i] == 32 || breedTypes[i] == 33) {
                if (breedTypes[i] == 32) {
                    if (color1 == -1) {
                        color1 = animalColors.ShopArray[Random.Range(0, animalColors.ShopArray.Length)];
                    }
                    curTypes[i] = color1;
                } else {
                    if (color2 == -1) {
                        color2 = animalColors.ShopArray[Random.Range(0, animalColors.ShopArray.Length)];
                    }
                    curTypes[i] = color2;
                }
            } else {
                curTypes[i] = breedTypes[i];
            }
            // for geese and fish's ear trait && pig tail trait, set as 34 - ignore
            if (i == 8 && (type == Type.FISH || type == Type.GOOSE)) {
                curTypes[i] = 34;
            }
            if (i == 1 && (type == Type.PIG)) {
                curTypes[i] = 34;
            }
        }
        coloring.body = curTypes[0];
        coloring.tail = curTypes[1];
        coloring.face = curTypes[2];
        coloring.star = curTypes[3];
        coloring.eyes = curTypes[4];
        coloring.legs = curTypes[5];
        coloring.dots = curTypes[6];
        coloring.back = curTypes[7];
        coloring.ears = curTypes[8];
        int breedId = animalBreed.isBreed(coloring, type);
        int price = GetPrice(coloring, type);
        string breed = "Generic";
        if (breedId != -1) {
            breed = animalBreed.breedArray[breedId].breedName;
            price *= animalBreed.breedArray[breedId].multiplier;
        }
        int randomNumber = Random.Range(0, 1000000000);
        Personality personality = generatePersonality(true);
        Vector2 location = new Vector2(Random.Range(1.0F, 3.5F), Random.Range(-1.0F, -3.5F));
        if (paramAnimals != null) {
            paramAnimals.addAnimal("Baby", type, randomNumber, location, -3, "Excited", coloring, false, GenerateGender(), "Barn#0", price, false, 0, new int[] { }, 0, breed, scene.name, false, 0, animalColors, personality, 0, 0);
        } else {
            shopBreedAnimals.addAnimal("Baby", type, randomNumber, location, -3, "Excited", coloring, false, GenerateGender(), "Barn#0", price, false, 0, new int[] { }, 0, breed, scene.name, false, 0, animalColors, personality, 0, 0);
        }
        return randomNumber;
    }
}
