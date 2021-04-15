using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : Interactable
{
    public GameObject dialogBox;
    public Text dialogText;
    public Item curItem;
    public Inventory playerInventory;
    public GameSaveManager gameSaveManager;
    public string dialog;
    public VectorValue playerPosition;
    public GameObject breedAnimal;
    public GameObject characterManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.Space) && playerInRange){
         if(dialogBox.activeInHierarchy){
           dialogBox.SetActive(false);
         }
         else{
           // Debug.Log("True" + playerInRange);
           dialogBox.SetActive(true);
           dialogText.text = dialog;
           // playerInventory.Additem(curItem);
           characterManager.GetComponent<CharacterManager>().updateCurCharacter();
           // playerInventory.currentItem = curItem;
           playerPosition.updateInitialValue(transform.position - new Vector3 (0,2, 0));
           // breedAnimal.GetComponent<BreedScript>().RandomAnimal();
           gameSaveManager.SaveScriptables();
         }
      }
    }
}
