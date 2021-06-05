using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SignalString : ScriptableObject {
    public List<SignalListenerString> listeners = new List<SignalListenerString>();
    public void Raise(string param) {
        for (int i = listeners.Count - 1; i >= 0; i--) {
            listeners[i].OnSignalRaised(param);
        }
    }
    public void RegisterListener(SignalListenerString listener) {
        listeners.Add(listener);
    }
    public void DeRegisterListener(SignalListenerString listener) {
        listeners.Remove(listener);
    }
}
