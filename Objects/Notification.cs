using System.Collections;
  using System.Collections.Generic;
  using System;
  using UnityEngine;
  using UnityEngine.UI;


public class Notification : MonoBehaviour
{
      public Text question;
      public GameObject CanvasController;

      public void initiateNotification(string newQuestion){
        question.text = newQuestion;
        gameObject.SetActive(true);
      }
      public void submit()
      {
        CanvasController.GetComponent<CanvasController>().closeCanvas();
        gameObject.SetActive(false);
      }
  }
