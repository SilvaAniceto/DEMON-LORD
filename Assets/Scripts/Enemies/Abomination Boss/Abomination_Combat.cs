using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abomination_Combat : MonoBehaviour, IEnemyCombat, IDealDamage
{
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

    private int mH;
    private int cH;
    
    [SerializeField] private GameObject slime;
    // Start is called before the first frame update
    void Start(){
        attackTimer = attackTime;
        mH = GetComponent<Abomination_Health>().maxHealth;
        cH = mH;
    }

    // Update is called once per frame
    void Update(){
        playerDistance = Mathf.Abs(transform.position.x - player.position.x);

        if (attacking ) {            
            animator.SetBool("Attack", true);
            attackTimer -= Time.fixedDeltaTime;                
            if (attackTimer <= 0) {
                attackTimer = attackTime;
                animator.SetBool("Attack", false);
                attacking = false;
            }          
        }               
    }
    
    public void SpawnMinion() {
        if (cH < 3) {
            Instantiate(slime, new Vector3(player.position.x - 2, -0.35f, 0f), player.rotation);
        }
        else {
            return;
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

    public IEnumerator Attack() {
        throw new System.NotImplementedException();
    }
}
