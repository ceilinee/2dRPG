using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public Sprite[] sprites;
    public string[] times;
    public CurTime curtime;
    [System.Serializable] public class DictionaryOfStringAndInt : SerializableDictionary<string, int> {}
    public DictionaryOfStringAndInt timesSpot = new DictionaryOfStringAndInt();
    // Start is called before the first frame update
    void Start(){
      Sprite background = checkBackground();
      updateBackgroundAtStart(background);
      for (int i = 0; i < times.Length; i++)
      {
          timesSpot[times[i]] = i;
      }
    }
    public Sprite checkBackground(){
      for(int i = 0; i< times.Length; i++){
        //looking for time minimally smaller than curtime
        if(curtime.isCurrentTimeBigger(times[i])){
        }
        else{
          if(i == 0){
            return sprites[sprites.Length-1];
          }
          else{
            return sprites[i-1];
          }
        }
      }
      return sprites[0];
    }
    public void updateBackgroundAtStart(Sprite background){
      foreach (Transform child in transform) {
          // GameObject.Destroy(child.gameObject);
          child.gameObject.GetComponent<SpriteRenderer>().sprite = background;
      }
    }
    public void updateBackground(string time){
      if(timesSpot.ContainsKey(time)){
        updateBackgroundAtStart(sprites[timesSpot[time]]);
      }
    }
}
