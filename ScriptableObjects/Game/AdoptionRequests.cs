using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AdoptionRequest {
    public int id;
    public int charId;
    public Type type;
    public Animal.StringAndAnimalColor coloring;
    public string breed;
    public Personality personality;
    public bool completed = false;
    public string message;
    public int price;
}
[CreateAssetMenu]
[System.Serializable]
public class AdoptionRequests : ScriptableObject {
    public AdoptionRequest[] requests;

    public void Clear() {
        requests = new AdoptionRequest[0];
    }
    public void addRequest(AdoptionRequest request) {
        List<AdoptionRequest> temp = new List<AdoptionRequest>(requests);
        temp.Insert(0, request);
        requests = temp.ToArray();
    }
    public void deleteRequest(AdoptionRequest request) {
        List<AdoptionRequest> temp = new List<AdoptionRequest>();
        for (int i = 0; i < requests.Length; i++) {
            if (requests[i].id != request.id) {
                temp.Add(requests[i]);
            }
        }
        requests = temp.ToArray();
    }
}
