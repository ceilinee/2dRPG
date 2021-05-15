using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mail {
    public string message;
    public string title;
    public int senderId;
    public int id;
    public Item item;
    public bool read;
    public Animal animal;
}

[CreateAssetMenu]
[System.Serializable]
public class Mailbox : ScriptableObject {
    public Mail[] mailbox;
    public int unread;

    public void Clear() {
        mailbox = new Mail[0];
        unread = 0;
    }
    public void addMessage(Mail mail) {
        List<Mail> temp = new List<Mail>(mailbox);
        temp.Insert(0, mail);
        mailbox = temp.ToArray();
        unread += 1;
    }
    public void deleteMessage(Mail mail) {
        List<Mail> temp = new List<Mail>(mailbox);
        temp.Remove(mail);
        mailbox = temp.ToArray();
    }
}
