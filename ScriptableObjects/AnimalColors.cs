using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
[System.Serializable]
public class AnimalColors : ScriptableObject
{
  public AnimalColor[] LlamaArray;
  public int[] FaceArray;
  public int[] EyesArray;
  public int[] ShopArray;
  [System.Serializable] public class DictionaryOfAnimalColor : SerializableDictionary<int, AnimalColor> {}
  public DictionaryOfAnimalColor colorDictionary = new DictionaryOfAnimalColor();
  [System.Serializable]
  public class intArray
  {
      public int[] array;
  }
  public intArray[] BreedMatrix;
}
