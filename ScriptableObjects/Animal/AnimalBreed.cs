using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu]
[System.Serializable]
public class AnimalBreed : ScriptableObject
{
  [System.Serializable]
  public class Breed
  {
      public bool unlocked;
      public Animal.StringAndAnimalColor exampleColoring = new Animal.StringAndAnimalColor();
      public int multiplier;
      public string breedName;
      public Animal.StringAndAnimalColor coloring = new Animal.StringAndAnimalColor();
  }
  public Breed[] breedArray;
  [System.Serializable] public class DictionaryOfBreed : SerializableDictionary<string, Breed> {}
  public DictionaryOfBreed breedDictionary = new DictionaryOfBreed();

  public Breed getRandomBreed(){
    System.Random random = new System.Random();
    int index = random.Next(breedDictionary.Count);
    return breedDictionary.Values.ElementAt(index);
  }

  public void updateBreedDictionary(){
    breedDictionary = new DictionaryOfBreed();
    for(int i = 0;i<breedArray.Length; i++){
      breedDictionary[breedArray[i].breedName] = breedArray[i];
    }
  }

  public int isBreed(Animal.StringAndAnimalColor coloring){
    int selected = -1;
    for(int j = 0; j<breedArray.Length; j++){
      //go through breed array
                Animal.StringAndAnimalColor breedColor = breedArray[j].coloring;
                int[] partArray = new int[]{breedColor.body, breedColor.eyes, breedColor.back, breedColor.dots, breedColor.star, breedColor.ears, breedColor.legs, breedColor.face, breedColor.tail};
                int[] coloringArray = new int[]{coloring.body, coloring.eyes, coloring.back, coloring.dots, coloring.star, coloring.ears, coloring.legs, coloring.face, coloring.tail};
                int all2 = -1;
                int all3 = -1;
                for(int i = 0; i<partArray.Length; i++){
                  if(partArray[i] == 32 || partArray[i] == 33){
                    if(coloringArray[i] == 27){
                      break;
                    }
                    if(partArray[i] == 32){
                      if(all2 == -1){
                        all2 = coloringArray[i];
                      }
                      else if(all2 != coloringArray[i]){
                        break;
                      }
                    }
                    if(partArray[i] == 33){
                      if(all3 == -1){
                        all3 = coloringArray[i];
                      }
                      else if(all3 != coloringArray[i]){
                        break;
                      }
                    }
                  }
                  else if(partArray[i] != 31){
                    if(partArray[i] != coloringArray[i]){
                      break;
                    }
                  }
                  if(i == partArray.Length-1){
                    if(selected == -1){
                      selected = j;
                    }
                    else if(breedArray[selected].multiplier < breedArray[j].multiplier){
                      selected = j;
                    }
                  }
                }
    }
    return selected;
  }
  
  public bool matchRequest(Animal.StringAndAnimalColor coloring, Animal.StringAndAnimalColor request){
      bool selected = false;
      Animal.StringAndAnimalColor breedColor = request;
      int[] partArray = new int[]{breedColor.body, breedColor.eyes, breedColor.back, breedColor.dots, breedColor.star, breedColor.ears, breedColor.legs, breedColor.face, breedColor.tail};
      int[] coloringArray = new int[]{coloring.body, coloring.eyes, coloring.back, coloring.dots, coloring.star, coloring.ears, coloring.legs, coloring.face, coloring.tail};
      int all2 = -1;
      int all3 = -1;
      for(int i = 0; i<partArray.Length; i++){
        if(partArray[i] == 32 || partArray[i] == 33){
          if(coloringArray[i] == 27){
            break;
          }
          if(partArray[i] == 32){
            if(all2 == -1){
              all2 = coloringArray[i];
            }
            else if(all2 != coloringArray[i]){
              break;
            }
          }
          if(partArray[i] == 33){
            if(all3 == -1){
              all3 = coloringArray[i];
            }
            else if(all3 != coloringArray[i]){
              break;
            }
          }
        }
        else if(partArray[i] != 31){
          if(partArray[i] != coloringArray[i]){
            break;
          }
        }
        if(i == partArray.Length-1){
          return true;
        }
      }
      return selected;
  }

}
