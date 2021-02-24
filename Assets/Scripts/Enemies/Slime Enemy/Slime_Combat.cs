using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Combat : MonoBehaviour, IEnemyCombat, IDealDamage
{
    [SerializeField] private Animator animator;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    private int attackDamage = 2;
    public bool blocking { get; set; }
    public bool attacking = false;

    [SerializeField] private Transform player;
    [HideInInspector] public float playerDistance;

    [SerializeField] private float attackTime;
    private float attackTimer;

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = attackTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerDistance = Mathf.Abs(transform.position.x - player.position.x);
        if (playerDistance < 2 && attacking == false) {
            attacking = true;
        }

        if (attacking) {
            attackTimer -= Time.fixedDeltaTime;
            if (attackTimer < 0) {
                StartCoroutine("Attack");
            }
        }
    }
    public IEnumerator Attack() {
        animator.SetTrigger("Attack");
        attacking = false;
        attackTimer = attackTime;
        yield return null;
    }

    public void DealDamage() {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, LayerMask.GetMask("Player"));
        if (hit != null) {
            Character_Combat p = hit.GetComponent<Character_Combat>();
            p.DamageTaken(attackDamage, 0.5f);

        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
