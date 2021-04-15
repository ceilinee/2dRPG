using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MailBoxController : Interactable
{
    public Mailbox mailbox;
    public GameObject alert;
    public GameObject mailInformation;
    public GameObject CanvasController;
    // Start is called before the first frame update
    void Start()
    {
      if(mailbox.unread > 0){
        alert.SetActive(true);
      }
      else{
        alert.SetActive(false);
      }
    }
    public void checkAlert(){
      if(mailbox.unread > 0){
        alert.SetActive(true);
      }
      else{
        alert.SetActive(false);
      }
    }
    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.Space) && playerInRange){
        if(!CanvasController.activeInHierarchy){
          CanvasController.SetActive(true);
        }
        if(CanvasController.GetComponent<CanvasController>().openCanvas()){
          mailInformation.SetActive(true);
        }
      }
    }
}
