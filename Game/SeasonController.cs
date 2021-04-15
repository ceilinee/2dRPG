using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonController : MonoBehaviour
{
    public CurTime curtime;
    public List<GameObject> spring;
    public List<GameObject> summer;
    public List<GameObject> fall;
    public List<GameObject> winter;

    // Start is called before the first frame update
    void Start()
    {
      UpdateSeason();
    }

    // Update is called once per frame
    public void UpdateSeason()
    {
      if(curtime.season == 0){
        foreach(GameObject ob in spring){
          ob.SetActive(true);
        }
        foreach(GameObject ob in summer){
          ob.SetActive(false);
        }
        foreach(GameObject ob in fall){
          ob.SetActive(false);
        }
        foreach(GameObject ob in winter){
          ob.SetActive(false);
        }
      }
      else if(curtime.season == 1){
        foreach(GameObject ob in spring){
          ob.SetActive(false);
        }
        foreach(GameObject ob in summer){
          ob.SetActive(true);
        }
        foreach(GameObject ob in fall){
          ob.SetActive(false);
        }
        foreach(GameObject ob in winter){
          ob.SetActive(false);
        }
      }
      else if(curtime.season == 2){
        foreach(GameObject ob in spring){
          ob.SetActive(false);
        }
        foreach(GameObject ob in summer){
          ob.SetActive(false);
        }
        foreach(GameObject ob in fall){
          ob.SetActive(true);
        }
        foreach(GameObject ob in winter){
          ob.SetActive(false);
        }
      }
      else{
        foreach(GameObject ob in spring){
          ob.SetActive(false);
        }
        foreach(GameObject ob in summer){
          ob.SetActive(false);
        }
        foreach(GameObject ob in fall){
          ob.SetActive(false);
        }
        foreach(GameObject ob in winter){
          ob.SetActive(true);
        }
      }
    }
}
