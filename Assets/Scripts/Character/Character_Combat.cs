using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Combat : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private bool comboEnabled = false;

    private int attackDamage = 1;

    [HideInInspector] public bool shieldIsUp;

    [HideInInspector] public bool attacking;

    [SerializeField] private Transform attackPoint;

    private float swordRange = 0.8f;

    Vector2 push;

    [SerializeField] private GameObject blockFlash;
   
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void FixedUpdate(){
        push = gameObject.GetComponent<Character_Controller>().faceDirection;
        if (Input.GetButtonDown("Attack"))
        {
            animator.SetTrigger("Attack");
            StartCoroutine("Attacking");
        }

        if (comboEnabled)
        {
            animator.SetBool("Combo", comboEnabled);
        }

        if (Input.GetButton("Block"))
        {
            shieldIsUp = true;
            animator.SetBool("ShieldUp", shieldIsUp);
        }
        else
        {
            shieldIsUp = false;
            animator.SetBool("ShieldUp", shieldIsUp);
        }        
    }   

    IEnumerator Attacking(){
        attacking = true;

        yield return new WaitForSeconds(0.6f);

        attacking = false;

    }
    void InflictDamage(){        
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, swordRange, LayerMask.GetMask("Enemies"));
        if (hit != null ){
            IEnemyCombat enemy = hit.GetComponent<IEnemyCombat>();
            if(attacking && !enemy.blocking) { 
                ITakeDamage damaging = hit.GetComponent<ITakeDamage>(); 
                damaging.TakeDamage(attackDamage, gameObject.GetComponent<Character_Controller>().flipSide, push);
            }
            else if (attacking && enemy.blocking) {
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 5.0f * push, ForceMode2D.Impulse);
                Instantiate(blockFlash, attackPoint.position, Quaternion.identity);
            }
        }
    }

    void OnDrawGizmos(){
        Gizmos.DrawWireSphere(attackPoint.position, swordRange);        
    }
}
