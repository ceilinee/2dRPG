using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mail {
  public string message;
  public string senderId;
  public Item item;
  public bool read;
  public Animal animal;
}

[CreateAssetMenu]
[System.Serializable]
public class Mailbox : ScriptableObject
{
  public Mail[] mailbox;
}
