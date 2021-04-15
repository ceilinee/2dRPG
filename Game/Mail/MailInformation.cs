using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailInformation : MonoBehaviour
{
    public Mail selectedMail;
    public Mailbox mailbox;
    public Text title;
    public Text message;
    public Text from;
    public GameObject mailView;
    public Characters charList;
    public GameObject CanvasController;

    // Start is called before the first frame update
    void Start()
    {
      updateList();

    }
    public void updateSelectedMail(Mail newMail){
      title.text = newMail.title;
      from.text = charList.characterDict[newMail.senderId].name;
      message.text = newMail.message;
      for(int i = 0 ;i<mailbox.mailbox.Length; i++){
        if(mailbox.mailbox[i].id == newMail.id){
          mailbox.mailbox[i].read = true;
          newMail.read = true;
        }
      }
    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetButtonDown("Cancel")){
        gameObject.SetActive(false);
        CanvasController.GetComponent<CanvasController>().closeCanvas();
      }
      if(gameObject.activeInHierarchy && Time.timeScale != 0){
        Time.timeScale = 0;
      }
    }

    public void updateList(){
      mailView.GetComponent<MailList>().mailInformation = gameObject;
      mailView.GetComponent<MailList>().PopulateList();
    }
}
