using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdoptionInformation : MonoBehaviour
{
    public AdoptionRequest selectedAdoption;
    public AdoptionRequests adoptionRequests;
    public Text inbox;
    public Text message;
    public Text from;
    public Text dots;
    public Text back;
    public Text legs;
    public Text tail;
    public Text star;
    public Text body;
    public Text breed;
    public Text eyes;
    public Text ears;
    public Text personality;
    public Image portrait;
    public AnimalColors animalColors;
    public GameObject adoptionController;
    public GameObject adoptionView;
    public Characters charList;
    public GameObject aboutTab;
    public GameObject CanvasController;

    // Start is called before the first frame update
    void Start()
    {
      aboutTab.SetActive(false);
      updateList();
    }
    // public void deleteSelectedMail(){
    //   mailbox.deleteMessage(selectedMail);
    //   updateList();
    //   if(mailbox.mailbox.Length > 0){
    //     updateSelectedMail(mailbox.mailbox[0]);
    //   }
    //   else{
    //     aboutTab.SetActive(false);
    //   }
    // }
    public void updateSelectedRequest(AdoptionRequest request){
      aboutTab.SetActive(true);
      dots.text = animalColors.colorDictionary[request.coloring.dots].ColorName;
      back.text = animalColors.colorDictionary[request.coloring.back].ColorName;
      legs.text = animalColors.colorDictionary[request.coloring.legs].ColorName;
      tail.text = animalColors.colorDictionary[request.coloring.tail].ColorName;
      star.text = animalColors.colorDictionary[request.coloring.star].ColorName;
      body.text = animalColors.colorDictionary[request.coloring.body].ColorName;
      breed.text = request.breed;
      eyes.text = animalColors.colorDictionary[request.coloring.eyes].ColorName;
      ears.text = animalColors.colorDictionary[request.coloring.ears].ColorName;
      personality.text = request.personality.personality;
    }
    // public void updateSelectedMail(Mail newMail){
    //   if(!aboutTab.activeInHierarchy){
    //     aboutTab.SetActive(true);
    //   }
    //   title.text = newMail.title;
    //   from.text = charList.characterDict[newMail.senderId].name;
    //   if(charList.characterDict[newMail.senderId].portrait.Length > 0){
    //     portrait.sprite = charList.characterDict[newMail.senderId].portrait[0];
    //   }
    //   message.text = newMail.message;
    //   for(int i = 0 ;i<mailbox.mailbox.Length; i++){
    //     if(mailbox.mailbox[i].id == newMail.id){
    //       if(!mailbox.mailbox[i].read){
    //         mailbox.unread -= 1;
    //         newMail.read = true;
    //         mailbox.mailbox[i].read = true;
    //         updateList();
    //         if(mailboxController){
    //           mailboxController.GetComponent<MailBoxController>().checkAlert();
    //         }
    //       }
    //       break;
    //     }
    //   }
    //   selectedMail = newMail;
    //   inbox.text = mailbox.unread == 0 ? "Inbox" : "Inbox (" + mailbox.unread + " new)";
    // }

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
      adoptionView.GetComponent<AdoptionList>().Clear();
    }
    public void updateList(){
      Clear();
      adoptionView.GetComponent<AdoptionList>().adoptionInformation = gameObject;
      adoptionView.GetComponent<AdoptionList>().PopulateList();
    }
}
