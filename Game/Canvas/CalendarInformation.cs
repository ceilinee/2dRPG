using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarInformation : MonoBehaviour
{
    public Calendar calendar;
    public Events events;
    public CurTime curTime;
    public Text season;
    public GameObject dates;
    public Sprite birthdayIcon;
    public GameObject CanvasController;

    // Start is called before the first frame update
    void Start()
    {

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
}
