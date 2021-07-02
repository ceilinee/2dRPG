using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInformation : CustomMonoBehaviour {
    private bool child = false;
    private Character character;
    public Text charName;
    public Text occupation;
    public Text friendship;
    public Text age;
    public Text mood;
    public Text friends;
    public GameObject follow;
    public Text followText;
    public CanvasController canvasController;
    public Characters curCharacters;
    public GameObject characterGO;
    // public GameObject playerDesign;
    public Image characterImage;
    // Start is called before the first frame update
    void Start() {
        FindDependencies();
    }
    public void FindDependencies() {
        canvasController = centralController.Get("CanvasController").GetComponent<CanvasController>();
    }
    public void TriggerFollow() {
        character.follow = character.follow ? false : true;
        followText.text = character.follow ? "Ask to Stop Following" : "Ask to Follow";
        characterGO.GetComponent<Children>().SetFollow();
        Close();
    }

    public void SetUp(GameObject GO) {
        if (!canvasController) {
            FindDependencies();
        }
        canvasController.openCanvas();
        gameObject.SetActive(true);
        this.characterGO = GO;
        this.character = characterGO.GetComponent<GenericCharacter>().characterTrait;
        child = character.child;
        charName.text = character.name;
        occupation.text = character.occupation;
        friendship.text = character.determineFriendship();
        friendship.color = character.determineFriendshipColor();
        characterImage.sprite = character.image;
        // playerDesign.SetActive(false);
        if (child) {
            SetChildItemsActive(true);
            age.text = character.age.ToString();
            mood.text = character.GetMood();
            friends.text = System.String.Join(", ", character.GetFriendNames(curCharacters).ToArray());
            followText.text = character.follow ? "Ask to Stop Following" : "Ask to Follow";
        } else {
            SetChildItemsActive(false);
        }
    }
    private void SetChildItemsActive(bool active = true) {
        age.gameObject.SetActive(active);
        mood.gameObject.SetActive(active);
        friends.gameObject.SetActive(active);
        follow.SetActive(active);
    }
    public void Close() {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        canvasController.closeCanvas();
        characterGO.GetComponent<GenericCharacter>().conversation = false;
    }
    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            Close();
        }
        if (gameObject.activeInHierarchy && Time.timeScale != 0) {
            Time.timeScale = 0;
        }
    }
}
