using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour, ITakeDamage
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    private Rigidbody2D body;
    [SerializeField] private Animator animator;    
    void Start(){
        body = gameObject.GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, bool side, Vector2 push) {
        animator.SetTrigger("Hit");
        currentHealth -= damage;
        if (currentHealth <= 0) {
            animator.SetTrigger("Die");
            gameObject.GetComponent<Enemy_Moviment>().enabled = false;
            Destroy(gameObject,3.0f);
        }
        else { 
            body.AddForce(Vector2.right * 2.5f * push, ForceMode2D.Impulse);
        }
    }
}
