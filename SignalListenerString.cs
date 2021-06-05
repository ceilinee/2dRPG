using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityEventString : UnityEvent<string> { }

public class SignalListenerString : CustomMonoBehaviour {
    public SignalString signal;
    public UnityEventString signalEvent;

    public void OnSignalRaised(string param) {
        signalEvent.Invoke(param);
    }

    private void OnEnable() {
        signal.RegisterListener(this);
    }
    private void OnDisable() {
        signal.DeRegisterListener(this);
    }
}
