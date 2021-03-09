using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour
{
    Rigidbody2D rgbd2;
    float destroyTime = 3.0f;


    void Awake() {
        rgbd2 = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate() {
        destroyTime -= Time.fixedDeltaTime;
        if (destroyTime <= 0f)
            Destroy(gameObject);
    }

    public void ChargeShoot(Vector2 dir) {
        rgbd2.AddForce(dir * 100.0f);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            Character_Combat p = other.GetComponent<Character_Combat>();
            p.DamageTaken(1, 1);
        }
    }
}
