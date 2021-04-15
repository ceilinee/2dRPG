using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalList : MonoBehaviour
{
  public List<GameObject> list = new List<GameObject>();
  public Transform target;
  public Animals curAnimals;
  public Animals charAnimals;
  public bool optimize;
  public int distance = 10;
  public Vector3 targetPastPosition;
  public void addExistingAnimal(GameObject newAnimal){
    list.Add(newAnimal);

  }
  // public static AnimalList instance = null;
  //
  // void Start(){
  //   DontDestroyOnLoad(gameObject);
  // }
  void Update(){
    if(target.position != targetPastPosition && optimize){
      disableAnimalsInRange();
      enableAnimalsInRange();
      targetPastPosition = target.position;
    }
  }
  public void updateList(){
    foreach (GameObject curObject in list) {
      Animal animalTrait = curObject.GetComponent<GenericAnimal>().animalTrait;
      animalTrait.location = curObject.transform.position;
      if(curAnimals.animalDict.ContainsKey(animalTrait.id)){
        curAnimals.animalDict[animalTrait.id] = animalTrait;
      }
    }
  }
  public void clearList(){
    list.Clear();
  }
  public void OnDisable (){
    list.Clear();
  }
  public void removeAnimal(Animal animal){
    List<GameObject> tempList = new List<GameObject>();

    foreach (GameObject curObject in list) {
        if(curObject.GetComponent<GenericAnimal>().animalTrait.id == animal.id){
          tempList.Remove(curObject);
          // curObject.SetActive(false);
          // curObject.GetComponent<GenericAnimal>().animalTrait.sold = true;
        }
        else{
          tempList.Add(curObject);
        }
    }
    list = tempList;
  }
  public Transform findAnimal(int id){
    foreach (GameObject child in list) {
      if(child.GetComponent<GenericAnimal>().animalTrait.id == id){
        return child.transform;
      }
    }
    return null;
  }
  public void disableAnimals(){
    foreach (GameObject child in list) {
      if(child.activeInHierarchy){
        child.SetActive(false);
      }
    }
  }
  public void enableAnimals(){
    foreach (GameObject child in list) {
      if(!child.activeInHierarchy){
        child.SetActive(true);
      }
    }
  }
  public void disableAnimalsInRange(){
    foreach (GameObject child in list) {
      if(child.activeInHierarchy){
        if(Vector3.Distance(target.position, child.transform.position) > distance && !child.GetComponent<GenericAnimal>().animalTrait.characterOwned){
          child.SetActive(false);
        }
      }
    }
  }
  public void enableAnimalsInRange(){
    foreach (GameObject child in list) {
        if(!child.activeInHierarchy){
          if(Vector3.Distance(target.position, child.transform.position) <= distance){
            child.SetActive(true);
          }
        }
    }
  }
  public void setAnimalsToSleep(){
    foreach (GameObject child in list) {
      child.GetComponent<GenericAnimal>().setRest();
    }
  }
  public void setAnimalsWake(){
    foreach (GameObject child in list) {
      child.GetComponent<GenericAnimal>().unsetRest();
    }
  }
  // void Awake ()
  // {
  //   if(instance != this && instance != null)
  //   {
  //       Destroy(gameObject);
  //   }
  //   instance = this;
  // }
  public void deleteAnimals(){
    List<GameObject> tempList = new List<GameObject>();
    foreach (GameObject child in list) {
        int id = child.GetComponent<GenericAnimal>().animalTrait.id;
        if (curAnimals.animalDict.ContainsKey(id)){
            curAnimals.animalDict[id].location = child.transform.position;
            GameObject.Destroy(child);
        }
        else{
          tempList.Add(child);
        }
    }
    list = tempList;
  }
  public void deleteCharAnimals(){
    List<GameObject> tempList = new List<GameObject>();
    foreach (GameObject child in list) {
        int id = child.GetComponent<GenericAnimal>().animalTrait.id;
        if (charAnimals.animalDict.ContainsKey(id)){
            GameObject.Destroy(child);
            removeAnimal(child.GetComponent<GenericAnimal>().animalTrait);
        }
        else{
          tempList.Add(child);
        }
    }
    list = tempList;
  }
}
