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
    [SerializeField] private bool rolling = true;
    [HideInInspector] public bool attacking;

    [SerializeField] private Transform attackPoint;

    private float swordRange = 0.8f;

    [HideInInspector] public Vector2 push;

    [SerializeField] private GameObject blockFlash;

    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    [SerializeField] private float maxStamina;
    [SerializeField] private float currentStamina;
    [SerializeField] private float staminaCost;

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
        push = gameObject.GetComponent<Character_Controller>().faceDirection;
        if (Input.GetButtonDown("Attack") && GetComponent<Character_Controller>().grounded == true)
        {
            if (currentStamina >= 1.0f) {
                currentStamina -= staminaCost;
                timer = time;
                animator.SetTrigger("Attack");
                StartCoroutine("Attacking");
            }
        }

        if (Input.GetButtonDown("Roll") && rolling && GetComponent<Character_Controller>().grounded) {
            if (currentStamina >= 0.5f) {
                currentStamina -= 0.75f;
                timer = time;
                rolling = false;
                StartCoroutine("Roll");
            }
            
        }

        if (currentStamina < maxStamina) {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0) {
                currentStamina += Time.fixedDeltaTime * 2;                
            }
        }
        HuD_Script.instance.StaminaValue(currentStamina / maxStamina);

        if (comboEnabled)
        {
            animator.SetBool("Combo", comboEnabled);
            staminaCost = 1.0f;
        }
        else {
            staminaCost = 2.0f;
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
    IEnumerator Roll() {
        body.AddForce(new Vector2(GetComponent<Character_Controller>().moveInput * 8.0f, 0f), ForceMode2D.Impulse);
        animator.SetBool("Roll", true);
        GetComponent<CircleCollider2D>().enabled = false;

        yield return new WaitForSeconds(0.9f);

        animator.SetBool("Roll", false);
        GetComponent<CircleCollider2D>().enabled = true;
        rolling = true;
    }
    IEnumerator Attacking(){
        attacking = true;

        yield return new WaitForSeconds(0.6f);

        attacking = false;

    }
    public void DealDamage(){        
        
        Collider2D[] hit = Physics2D.OverlapCircleAll(attackPoint.position, swordRange, LayerMask.GetMask("Enemies"));
        foreach (Collider2D e in hit) {
            if (hit != null) {             
                IEnemyCombat enemy = e.GetComponent<IEnemyCombat>();
                if (attacking && !enemy.blocking) {
                    ITakeDamage damaging = e.GetComponent<ITakeDamage>();
                    damaging.TakeDamage(attackDamage, gameObject.GetComponent<Character_Controller>().flipSide, push);
                }
                else if (attacking && enemy.blocking) {
                    gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 2.5f * push, ForceMode2D.Impulse);
                    Instantiate(blockFlash, attackPoint.position, Quaternion.identity);
                }
            }
        }
    }

    public void DamageTaken(int damage, float stamina) {
        if (shieldIsUp) {
            animator.SetTrigger("Block");
            currentStamina -= stamina;
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
