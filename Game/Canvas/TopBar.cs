using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBar : CustomMonoBehaviour {
    public Text moneyText;

    [SerializeField]
    private FloatValue playerMoney;

    // Called by signal listener
    public void OnMoneyChangedSignalRaised() {
        moneyText.text = "$" + playerMoney.initialValue.ToString();
    }
}
