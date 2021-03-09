using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abomination_Health : MonoBehaviour, ITakeDamage
{
    public int maxHealth;
    public int currentHealth;
    public bool spawnSlime = false;

    private Rigidbody2D body;
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start() {
        body = gameObject.GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, bool side, Vector2 push) {
        animator.SetTrigger("Hit");
        currentHealth -= damage;
        if (currentHealth <= (maxHealth /2)) {
            spawnSlime = true; 
        }
        if (currentHealth <= 0) {
            this.enabled = false;
            animator.SetTrigger("Die");
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            gameObject.GetComponent<Abomination_Moviment>().enabled = false;
            gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
        }
        else {
            body.AddForce(Vector2.right * 2.5f * push, ForceMode2D.Impulse);
        }
    }
}
