using UnityEngine;
using System.Collections;

public class Singleton<Instance> : MonoBehaviour where Instance : Singleton<Instance> {
    public Instance instance;

    // Use this for initialization
    public virtual void Awake() {
        if (instance == null) {
            instance = this as Instance;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    // public virtual void Awake() {
    //     if(isPersistant) {
    //         if(!instance) {
    //             instance = this as Instance;
    //         }
    //         else {
    //             Destroy(gameObject);
    //         }
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else {
    //         instance = this as Instance;
    //     }
    // }
}
