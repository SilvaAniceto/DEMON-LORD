using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy_Combat : MonoBehaviour , IEnemyCombat, IDealDamage
{
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject blockFlash;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    private int attackDamage = 1;
    public bool blocking { get; set; }

    [HideInInspector] public bool attacking;
    [SerializeField] private float attackTime;
    private float attackTimer;

    [SerializeField] private Transform player;
    [HideInInspector] public float playerDistance;
    // Start is called before the first frame update
    void Start(){
        attackTimer = attackTime;        
    }

    // Update is called once per frame
    void FixedUpdate(){
        playerDistance = Mathf.Abs(transform.position.x - player.position.x);
        if (playerDistance <= 2.0f && GetComponent<ShieldEnemy_Moviment>().hasSight && !attacking){
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

    public void DealDamage() {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, LayerMask.GetMask("Player"));
        if (hit != null) {
            Character_Combat p = hit.GetComponent<Character_Combat>();            
            p.DamageTaken(attackDamage);
            
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
