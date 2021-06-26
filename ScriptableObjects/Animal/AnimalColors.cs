using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Assertions;

[CreateAssetMenu]
[System.Serializable]
public class AnimalColors : ScriptableObject {
    public AnimalColor[] LlamaArray;
    public int[] FaceArray;
    public int[] EyesArray;
    public int[] ShopArray;
    [System.Serializable] public class DictionaryOfAnimalColor : SerializableDictionary<int, AnimalColor> { }
    public DictionaryOfAnimalColor colorDictionary = new DictionaryOfAnimalColor();
    [System.Serializable]
    public class intArray {
        public int[] array;
    }
    public intArray[] BreedMatrix;

    public int getRandomColor() {
        System.Random random = new System.Random();
        int index = random.Next(colorDictionary.Count);
        return colorDictionary.Values.ElementAt(index).id;
    }

    public Color GetColor(int colorDictKey) {
        Assert.IsTrue(colorDictionary.ContainsKey(colorDictKey));
        return colorDictionary[colorDictKey].color;
    }
}
