using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public Characters curCharacters;
    public Player player;
    public AdoptionRequests adoption;
    public Animals curAnimals;
    public Quests availableQuests;

    // Start is called before the first frame update
    void Start()
    {
      generateQuest();
    }

    public void generateQuest(){
      QuestType questType = (QuestType)Random.Range(0, 8);
      Debug.Log(questType);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
