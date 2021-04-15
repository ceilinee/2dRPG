using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailInformation : MonoBehaviour
{
    public Mail selectedMail;
    public Mailbox mailbox;
    public Text title;
    public Text inbox;
    public Text message;
    public Text from;
    public Image portrait;
    public GameObject mailboxController;
    public GameObject mailView;
    public Characters charList;
    public GameObject aboutTab;
    public GameObject CanvasController;

    // Start is called before the first frame update
    void Start()
    {
      // if(mailbox.mailbox.Length > 0){
      //   updateSelectedMail(mailbox.mailbox[0]);
      // }
      // else{
      //   aboutTab.SetActive(false);
      // }
      aboutTab.SetActive(false);
      updateList();

    }
    public void deleteSelectedMail(){
      mailbox.deleteMessage(selectedMail);
      updateList();
      if(mailbox.mailbox.Length > 0){
        updateSelectedMail(mailbox.mailbox[0]);
      }
      else{
        aboutTab.SetActive(false);
      }
    }
    public void updateSelectedMail(Mail newMail){
      if(!aboutTab.activeInHierarchy){
        aboutTab.SetActive(true);
      }
      title.text = newMail.title;
      from.text = charList.characterDict[newMail.senderId].name;
      if(charList.characterDict[newMail.senderId].portrait.Length > 0){
        portrait.sprite = charList.characterDict[newMail.senderId].portrait[0];
      }
      message.text = newMail.message;
      for(int i = 0 ;i<mailbox.mailbox.Length; i++){
        if(mailbox.mailbox[i].id == newMail.id){
          if(!mailbox.mailbox[i].read){
            mailbox.unread -= 1;
            newMail.read = true;
            mailbox.mailbox[i].read = true;
            updateList();
            if(mailboxController){
              mailboxController.GetComponent<MailBoxController>().checkAlert();
            }
          }
          break;
        }
      }
      selectedMail = newMail;
      inbox.text = mailbox.unread == 0 ? "Inbox" : "Inbox (" + mailbox.unread + " new)";
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
    public void Clear(){
      mailView.GetComponent<MailList>().Clear();
    }
    public void updateList(){
      Clear();
      mailView.GetComponent<MailList>().mailInformation = gameObject;
      mailView.GetComponent<MailList>().PopulateList();
    }
}
