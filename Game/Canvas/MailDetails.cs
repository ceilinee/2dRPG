using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MailDetails : MonoBehaviour
{
    // Start is called before the first frame update

    public Text charName;
    public Text title;
    public Image charImage;
    public Mail mail;
    public GameObject mailInformation;
    public void updateDetails(Mail newMail){
      mail = newMail;
      title.text = newMail.title;
      if(!mail.read){
        gameObject.GetComponent<Image>().color = new Color(255/255f, 189/255f,189/255f);
      }
    }
    public void selectMail(){
        mailInformation.GetComponent<MailInformation>().updateSelectedMail(mail);
    }
}
