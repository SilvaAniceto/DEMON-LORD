using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abomination_Moviment : MonoBehaviour
{
    private float sightRange;
    private float backSightRange;

    [HideInInspector] public bool hasSight;

    [HideInInspector] public bool isFlip = false;

    public float moveSpeed;

    [SerializeField] private Transform sightLine;
    [SerializeField] private Transform backLine;

    [SerializeField] private Animator animator;

    [SerializeField] private Transform player;
   
    // Start is called before the first frame update
    void Start() {
        
    }
    // Update is called once per frame
    void FixedUpdate() {
        backSightRange = transform.position.x - backLine.position.x;
        RaycastHit2D backSight = Physics2D.Raycast(transform.position, Vector2.left,
            backSightRange, LayerMask.GetMask("Player"));
        if (backSight.collider != null) {
            Flip();
        }

        sightRange = sightLine.position.x - transform.position.x;
        RaycastHit2D sight = Physics2D.Raycast(transform.position, Vector2.right,
            sightRange, LayerMask.GetMask("Player"));
        if (sight.collider != null) {
            hasSight = true;
        }
        else {
            hasSight = false;
        }

        if (hasSight) {
            animator.SetBool("Move",true);
        }
        else if (!hasSight) {
            animator.SetBool("Move", false);            
        }        
    }
    public void Flip() {
        if (transform.position.x > player.position.x && isFlip) {
            transform.Rotate(0f, 180f, 0f);
            isFlip = false;
        }
        else if (transform.position.x < player.position.x && !isFlip) {
            transform.Rotate(0f, 180f, 0f);
            isFlip = true;
        }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, sightLine.position);
        Gizmos.DrawLine(transform.position, backLine.position);
    }
}
