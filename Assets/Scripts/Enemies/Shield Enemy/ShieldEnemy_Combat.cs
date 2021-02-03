using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy_Combat : MonoBehaviour , IEnemyCombat
{
    [SerializeField] private Animator animator;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;

    public bool blocking { get; set; }

    [HideInInspector] public bool attacking;
    [SerializeField] private float attackTime;
    private float attackTimer;
    // Start is called before the first frame update
    void Start(){
        attackTimer = attackTime;        
    }

    // Update is called once per frame
    void FixedUpdate(){  
        if (GetComponent<ShieldEnemy_Moviment>().playerDistance <= 2.0f && GetComponent<ShieldEnemy_Moviment>().hasSight && !attacking){
            blocking = true;
            animator.SetBool("Block", blocking);           
        }
        else{
            blocking = false;
            animator.SetBool("Block", blocking);
        }

        if (blocking){
            if (!attacking){
                attackTimer -= Time.fixedDeltaTime;
                if (attackTimer <= 0){
                    blocking = false;
                    StartCoroutine("Attack");
                }
            }
        }
    }
    
    public IEnumerator Attack(){
        attacking = true;
        animator.SetBool("Attack", attacking);
        attackTimer = attackTime;

        yield return new WaitForSeconds(1.2f);

        attacking = false;        
        animator.SetBool("Attack", attacking);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
