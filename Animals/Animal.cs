using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Personality {
    public string personality;
    public int multiplier;
    public float probability;
    public int id;
}

[System.Serializable]
public class Animal {
    public string animalName;
    public string type;
    public int momId;
    public int dadId;
    public string breed;
    public int health = 100;
    public bool sold;
    public int id;
    public bool adoption;
    public Vector2 location;
    public Vector2 target = new Vector2(0, 0);
    public int age;
    public Personality personality = null;
    public string scene;
    public string mood;
    public string moodId = "1";
    public bool follow;
    public bool characterOwned;
    public bool wild;
    public int charId;
    public string gender;

    // Right now, home has the form Barn1#<id of the placed building>
    public string home;
    public int cost;
    public int shopCost;
    public int presentsDaily;
    public bool pregnant;
    public int deliveryDate;
    public int[] babyId;
    public float love;
    public GameObject spawnAnimal;
    public AnimalColors animalColors;
    public bool walked = false;
    [System.Serializable]
    public class StringAndAnimalColor {
        public int body;
        public int tail;
        public int eyes;
        public int dots;
        public int face;
        public int legs;
        public int ears;
        public int back;
        public int star;
        public StringAndAnimalColor Clone() => new StringAndAnimalColor {
            body = this.body,
            tail = this.tail,
            eyes = this.eyes,
            dots = this.dots,
            face = this.face,
            legs = this.legs,
            ears = this.ears,
            back = this.back,
            star = this.star,
        };
    }
    public StringAndAnimalColor coloring = new StringAndAnimalColor();
    // [System.Serializable] public class DictionaryOfStringAndAnimalColor : SerializableDictionary<string, AnimalColor> {}
    // public DictionaryOfStringAndAnimalColor coloring = new DictionaryOfStringAndAnimalColor();
    // [System.Serializable] public class DictionaryOfStringAndColor : SerializableDictionary<string, Color> {}
    // public DictionaryOfStringAndColor coloring = new DictionaryOfStringAndColor();
    public int getPart(string part) {
        if (part == "eyes") {
            return coloring.eyes;
        }
        if (part == "back") {
            return coloring.back;
        }
        if (part == "tail") {
            return coloring.tail;
        }
        if (part == "dots") {
            return coloring.dots;
        }
        if (part == "face") {
            return coloring.face;
        }
        if (part == "legs") {
            return coloring.legs;
        }
        if (part == "ears") {
            return coloring.ears;
        }
        if (part == "star") {
            return coloring.star;
        }
        return coloring.body;
    }

    public void createAnimal(string newAnimalName,
    string newType, int newId, Vector2 newLocation, int newAge, string newMood,
    StringAndAnimalColor newColoring, bool newFollow, string newGender,
    string newHome, int newCost, bool newPregnant, int newDeliveryDate, int[] newBabyId, float newLove, string newBreed, string newScene, bool newCharOwned, int newCharId, AnimalColors _animalColor, Personality _personality, int _momId, int _dadId) {
        animalName = newAnimalName;
        type = newType;
        id = newId;
        location = newLocation;
        age = newAge;
        mood = newMood;
        coloring = newColoring;
        follow = newFollow;
        gender = newGender;
        home = newHome;
        cost = newCost;
        shopCost = 2 * newCost;
        breed = newBreed;
        personality = _personality;
        pregnant = newPregnant;
        deliveryDate = newDeliveryDate;
        babyId = newBabyId;
        love = newLove;
        scene = newScene;
        characterOwned = newCharOwned;
        charId = newCharId;
        momId = _momId;
        dadId = _dadId;
        animalColors = _animalColor;
    }
    // public void setAnimalImage(GameObject animalImage, Animal animalTrait){
    //     spawnAnimal.GetComponent<SpawnAnimal>().setAnimalImage(animalImage, animalTrait);
    // }
    public void colorAnimal(GameObject instance) {
        // setAnimalImage(instance, this);
        Transform trans = instance.transform;
        if (!animalColors) {
            return;
        }
        instance.GetComponent<Image>().color = animalColors.colorDictionary[coloring.body].color;

        Transform childTrans = trans.Find("TailSocket");
        childTrans.gameObject.GetComponent<Image>().color = animalColors.colorDictionary[coloring.tail].color;

        childTrans = trans.Find("StarSocket");
        childTrans.gameObject.GetComponent<Image>().color = animalColors.colorDictionary[coloring.star].color;


        childTrans = trans.Find("DotsSocket");
        childTrans.gameObject.GetComponent<Image>().color = animalColors.colorDictionary[coloring.dots].color;


        childTrans = trans.Find("EyesSocket");
        childTrans.gameObject.GetComponent<Image>().color = animalColors.colorDictionary[coloring.eyes].color;

        childTrans = trans.Find("FaceSocket");
        childTrans.gameObject.GetComponent<Image>().color = animalColors.colorDictionary[coloring.face].color;


        childTrans = trans.Find("BackSocket");
        childTrans.gameObject.GetComponent<Image>().color = animalColors.colorDictionary[coloring.back].color;


        childTrans = trans.Find("EarsSocket");
        childTrans.gameObject.GetComponent<Image>().color = animalColors.colorDictionary[coloring.ears].color;


        childTrans = trans.Find("LegsSocket");
        childTrans.gameObject.GetComponent<Image>().color = animalColors.colorDictionary[coloring.legs].color;
    }
}
