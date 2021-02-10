using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Combat : MonoBehaviour, IDealDamage
{
    [SerializeField] private Animator animator;
    [HideInInspector] public Rigidbody2D body;

    [SerializeField] private bool comboEnabled = false;

    private int attackDamage = 1;

    [HideInInspector] public bool shieldIsUp;

    [HideInInspector] public bool attacking;

    [SerializeField] private Transform attackPoint;

    private float swordRange = 0.8f;

    [HideInInspector] public Vector2 push;

    [SerializeField] private GameObject blockFlash;

    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    [SerializeField] private float maxStamina;
    [SerializeField] private float currentStamina;

    [SerializeField] private float time;
    [SerializeField] private float timer;


    // Start is called before the first frame update
    void Start(){
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate(){
        Debug.Log(timer);
        push = gameObject.GetComponent<Character_Controller>().faceDirection;
        if (Input.GetButtonDown("Attack"))
        {
            if (currentStamina >= 1.0f) {
                currentStamina -= 2.0f;
                timer = time;
                animator.SetTrigger("Attack");
                StartCoroutine("Attacking");
            }
        }
        if (currentStamina < maxStamina) {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0) {
                currentStamina += Time.fixedDeltaTime;                
            }
        }
        HuD_Script.instance.StaminaValue(currentStamina / maxStamina);

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
    public void DealDamage(){        
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, swordRange, LayerMask.GetMask("Enemies"));
        if (hit != null ){
            IEnemyCombat enemy = hit.GetComponent<IEnemyCombat>();
            if(attacking && !enemy.blocking) { 
                ITakeDamage damaging = hit.GetComponent<ITakeDamage>(); 
                damaging.TakeDamage(attackDamage, gameObject.GetComponent<Character_Controller>().flipSide, push);
            }
            else if (attacking && enemy.blocking) {
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 2.5f * push, ForceMode2D.Impulse);
                Instantiate(blockFlash, attackPoint.position, Quaternion.identity);
            }
        }
    }

    public void DamageTaken(int damage) {
        if (shieldIsUp) {
            animator.SetTrigger("Block");
            currentStamina -= 1.0f;
            timer = time;
        }
        else {
            animator.SetTrigger("Hit");
            currentHealth -= damage;
            if (currentHealth <= 0) {
                animator.SetTrigger("Die");
                gameObject.GetComponent<Character_Controller>().enabled = false;
                gameObject.GetComponent<CircleCollider2D>().enabled = false;
                this.enabled = false;
            }
        }
        body.AddForce(Vector2.left * 2.5f * push, ForceMode2D.Impulse);
        HuD_Script.instance.HealthValue(currentHealth / maxHealth);
    }

    

    void OnDrawGizmos(){
        Gizmos.DrawWireSphere(attackPoint.position, swordRange);        
    }
}
