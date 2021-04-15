using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MailDetails : MonoBehaviour
{
    // Start is called before the first frame update

    public Text title;
    public Image charImage;
    public Text charName;
    public Mail mail;
    public GameObject unread;
    public GameObject mailInformation;
    public void updateDetails(Mail newMail){
      mail = newMail;
      title.text = newMail.title;
      if(!mail.read){
        unread.SetActive(true);
      }
      else{
        unread.SetActive(false);
      }
    }
    public void selectMail(){
        mailInformation.GetComponent<MailInformation>().updateSelectedMail(mail);
    }
}
