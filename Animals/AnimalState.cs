using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimalStates {
    idle,
    walk,
    rest,
    eat
}

public class AnimalState : MonoBehaviour {
    public AnimalStates currentState;
    public FloatValue maxHealth;
    public float health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;
    // public string animalName;
    // public string home;
    // public string type;
    // public int id;
    // public int cost;
    // public Vector2 location;
    // public int age;
    // public string mood;
    // public string gender;

    private void Awake() {
        health = maxHealth.initialValue;
    }
    private void TakeDamage(float damage) {
        // health -= damage;
        // if(health <= 0){
        //   this.gameObject.SetActive(false);
        // }
    }
    public void Knock(Rigidbody2D myRigidbody, float knockTime, float damage) {
        StartCoroutine(KnockCo(myRigidbody, knockTime));
        TakeDamage(damage);
    }
    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime) {
        if (myRigidbody != null) {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = AnimalStates.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }
}
