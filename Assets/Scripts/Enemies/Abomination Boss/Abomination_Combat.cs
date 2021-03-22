using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abomination_Combat : MonoBehaviour,IEnemyCombat, IDealDamage
{
    [SerializeField] private Transform attackPoint;
    public float attackRange;
    private int attackDamage = 1;
    [SerializeField] private float staminaDamage;
    
    [SerializeField] Transform target;
    [HideInInspector] public float playerDistance;

    private int slimeCount;
    public GameObject slime;
    public GameObject charge;

    public Transform spawnPosition;
    public bool blocking { get; set; }
    void Awake() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update(){
        slimeCount = GameObject.FindGameObjectsWithTag("Slime").Length;
    }    
    public void DealDamage() {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, LayerMask.GetMask("Player"));
        if (hit != null) {
            Character_Combat p = hit.GetComponent<Character_Combat>();
            p.DamageTaken(attackDamage, staminaDamage);
        }
        if (GetComponent<Abomination_Health>().spawnSlime) {
            if (slimeCount <= 3) {
                GameObject slimePrefab = Instantiate(slime, spawnPosition.position, spawnPosition.rotation);
            }
        }
    }
    public void ChargeAttack() {
        Vector2 dir = target.position - transform.position;
        GameObject chargePrefab = Instantiate(charge, attackPoint.position, Quaternion.identity);
        chargePrefab.GetComponent<Charge>().ChargeShoot(dir);       
    }    
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    public IEnumerator Attack() {
        throw new System.NotImplementedException();
    }
}
