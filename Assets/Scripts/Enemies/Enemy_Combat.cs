using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Combat : MonoBehaviour, IEnemyCombat, IDealDamage 
{
    public enum Combat { Shield, Spear, Slime};
    public Combat combatType;

    [SerializeField] private Animator animator;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;    
    [SerializeField] private float staminaDamage;
    private int attackDamage = 1;
    
    public bool attacking;
    public bool blocking { get; set; }

    [SerializeField] private Transform player;
    [HideInInspector] public float playerDistance;

    [SerializeField] private float attackTime;
    private float attackTimer;
    [SerializeField] private float blockTime;
    private float blockTimer;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }   
    void Start() {
        attackTimer = attackTime;
        blockTimer = blockTime;
    }   
    void FixedUpdate(){
        playerDistance = Mathf.Abs(transform.position.x - player.position.x);

        if (combatType == Combat.Shield) {
            ShieldCombat();
        }
        else if (combatType == Combat.Spear) {
            SpearCombat();
        }
        else if (combatType == Combat.Slime) {
            SlimeCombat();
        }
    }
    public void ShieldCombat() {
        if (playerDistance < 2 && GetComponent<Enemy_Moviment>().hasSight && !attacking) {
            blocking = true;
            animator.SetBool("Block", blocking);
        }
        else {
            blocking = false;            
            animator.SetBool("Block", blocking);
        }
        if (blocking) {
            attackTimer -= Time.fixedDeltaTime;
            if (attackTimer <= 0) {
                attacking = true;
                animator.SetTrigger("attack");
                attackTimer = attackTime;
            }
        }
        if (attacking) {
            blockTimer -= Time.fixedDeltaTime;
            if (blockTimer <= 0) {
                attacking = false;
                blockTimer = blockTime;
            }
        }
        
    }
    public void SpearCombat() {
        if (playerDistance < 3.5 && GetComponent<Enemy_Moviment>().hasSight == true && attacking == false) {
            attacking = true;
        }
        if (attacking) {
            animator.SetBool("Move", false);
            attackTimer -= Time.fixedDeltaTime;
            if (attackTimer < 0) {
                animator.SetTrigger("attack");
                attackTimer = attackTime;
                attacking = false;
            }
        }
    }
    public void SlimeCombat() {        
        if (playerDistance < 3 && GetComponent<Enemy_Moviment>().hasSight == true && attacking == false) {
            attackTimer -= Time.fixedDeltaTime;
            if (attackTimer < 0) {
                animator.SetTrigger("attack");
                attackTimer = attackTime;
                attacking = false;
            }
        }       
    }
    public void DealDamage() {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, LayerMask.GetMask("Player"));
        if (hit != null) {
            Character_Combat p = hit.GetComponent<Character_Combat>();
            p.DamageTaken(attackDamage, staminaDamage);
        }
    }       
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
