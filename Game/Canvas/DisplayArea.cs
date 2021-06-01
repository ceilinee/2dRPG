using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayArea : CustomMonoBehaviour {
    // public Text areaName;
    public void startAlert(string _areaName) {
        gameObject.GetComponent<Text>().text = _areaName;
        gameObject.SetActive(true);
        StartCoroutine(displayName());
    }
    IEnumerator displayName() {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
