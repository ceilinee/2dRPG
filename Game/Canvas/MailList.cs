using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MailList : MonoBehaviour
{
        // [SerializeField]
        public Transform SpawnPoint = null;
        //
        // [SerializeField]
        public GameObject item = null;
        public RectTransform content = null;
        private int numberOfItems = 3;
        public Mailbox mailbox;
        public GameObject mailInformation;
        public Characters charList;

        void Start () {

        }
        public void Clear(){
          foreach (Transform child in SpawnPoint.transform) {
              // GameObject.Destroy(child.gameObject);
              child.gameObject.SetActive(false);
          }
        }
        public void PopulateList(){
          //setContent Holder Height;
          int numberOfItemsInRow = 1;
          numberOfItems = mailbox.mailbox.Length;
          content.sizeDelta = new Vector2(0, System.Math.Max(numberOfItems/numberOfItemsInRow, 3) * 45);
          for (int i = 0; i < numberOfItems; i++)
          {
            float spawnY = (int)System.Math.Floor((double)i/numberOfItemsInRow) * 45;
            //newSpawn Position
            Vector3 pos = new Vector3(35.49f, 15+ -spawnY, 0);
            //instantiate item
            GameObject SpawnedItem = Instantiate(item, pos, SpawnPoint.rotation);
            //setParent
            SpawnedItem.transform.SetParent(SpawnPoint, false);
            Debug.Log(mailbox.mailbox[i]);
            MailDetails mailDetails = SpawnedItem.GetComponent<MailDetails>();
            mailDetails.updateDetails(mailbox.mailbox[i]);
            mailDetails.mailInformation = mailInformation;
            if(charList.characterDict[mailbox.mailbox[i].senderId].image){
              mailDetails.charImage.sprite = charList.characterDict[mailbox.mailbox[i].senderId].image;
            }
          }
        }
  }
