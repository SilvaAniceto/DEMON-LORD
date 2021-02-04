using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy_Moviment : MonoBehaviour
{
    private float sightRange;
    private float backSightRange;
    [HideInInspector] public bool hasSight;
    

    [SerializeField] private float moveSpeed;

    [SerializeField] private Transform sightLine;
    [SerializeField] private Transform backLine;
    

    [SerializeField] private Animator animator;    

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void FixedUpdate(){
        backSightRange = transform.position.x - backLine.position.x;
        RaycastHit2D backSight = Physics2D.Raycast(transform.position, Vector2.left, 
            backSightRange, LayerMask.GetMask("Player"));
        if (backSight.collider != null ){
            transform.Rotate(0f, 180f, 0f);
        }
        
        sightRange = sightLine.position.x - transform.position.x;
        RaycastHit2D sight = Physics2D.Raycast(transform.position, Vector2.right,
            sightRange, LayerMask.GetMask("Player"));
        if (sight.collider != null){
            hasSight = true;
        }
        else{
            hasSight = false;
        }

        
        
        if (hasSight && !GetComponent<ShieldEnemy_Combat>().blocking &&
            !GetComponent<ShieldEnemy_Combat>().attacking){
            Move();
        }            
        else{
            animator.SetBool("Move", false);
        }
        
    }

    void Move(){
        transform.Translate(moveSpeed * Time.fixedDeltaTime, 0f, 0f);
        animator.SetBool("Move", true);
    }

    private void OnDrawGizmos(){
        Gizmos.DrawLine(transform.position, backLine.position);

        Gizmos.DrawLine(transform.position, sightLine.position);
    }
}
