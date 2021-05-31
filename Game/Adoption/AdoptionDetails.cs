using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AdoptionDetails : MonoBehaviour {
    // Start is called before the first frame update

    public Image charImage;
    public Text message;
    public Text charName;
    public AdoptionRequest adoptionRequest;
    public GameObject unread;
    public GameObject adoptionInformation;
    public void updateDetails(AdoptionRequest request) {
        adoptionRequest = request;
        message.text = request.message;
        if (!adoptionRequest.completed) {
            unread.SetActive(true);
        } else {
            unread.SetActive(false);
        }
    }
    public void selectMail() {
        adoptionInformation.GetComponent<AdoptionInformation>().updateSelectedRequest(adoptionRequest);
    }
}
