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
    
    [HideInInspector] public bool attacking;
    public bool blocking { get; set; }

    [SerializeField] private Transform player;
    [HideInInspector] public float playerDistance;

    [SerializeField] private float attackTime;
    private float attackTimer;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }   
    void Start() {
        attackTimer = attackTime;
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
            if (!attacking) {
                attackTimer -= Time.fixedDeltaTime;
                if (attackTimer <= 0) {
                    blocking = false;
                    StartCoroutine("Attack");
                }
            }
        }
    }
    public void SpearCombat() {
        if (playerDistance < 2 && GetComponent<Enemy_Moviment>().hasSight == true && attacking == false) {
            attacking = true;
        }
        if (attacking) {
            animator.SetBool("Move", false);
            attackTimer -= Time.fixedDeltaTime;
            if (attackTimer < 0) {
                StartCoroutine("Attack");
            }
        }
    }
    public void SlimeCombat() {
        if (playerDistance < 3 && GetComponent<Enemy_Moviment>().hasSight == true && attacking == false) {
            attacking = true;
            StartCoroutine("Attack");
        }
    }
    public void DealDamage() {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, LayerMask.GetMask("Player"));
        if (hit != null) {
            Character_Combat p = hit.GetComponent<Character_Combat>();
            p.DamageTaken(attackDamage, staminaDamage);
        }
    }
    public IEnumerator Attack() {
        attacking = true;
        animator.SetBool("Attack", attacking);
        attackTimer = attackTime;

        yield return new WaitForSeconds(attackTime);

        attacking = false;
        animator.SetBool("Attack", attacking);
    }    
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
