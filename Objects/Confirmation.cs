using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Confirmation : MonoBehaviour
{
    public Text question;
    public GameObject CanvasController;
    public Action confirmFunction;
    public Action cancelFunction;

    public void initiateConfirmation(string newQuestion, Action newConfirmFunction, Action newCancelFunction){
      question.text = newQuestion;
      confirmFunction = newConfirmFunction;
      cancelFunction = newCancelFunction;
      gameObject.SetActive(true);
    }
    void Update()
    {
        if(gameObject.activeInHierarchy && Time.timeScale != 0){
          Time.timeScale = 0;
        }
    }
    public void confirm(){
      confirmFunction();
      CanvasController.GetComponent<CanvasController>().closeCanvas();
      gameObject.SetActive(false);
    }
    public void cancel(){
      cancelFunction();
      gameObject.SetActive(false);
      CanvasController.GetComponent<CanvasController>().closeCanvasIfAllElseClosed();
    }
}
