using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearEnemy_Moviment : MonoBehaviour
{
    private float sightRange;
    private float backSightRange;
    [HideInInspector] public bool hasSight;

    [SerializeField] private float moveSpeed;

    [SerializeField] private Transform sightLine;
    [SerializeField] private Transform backLine;

    [SerializeField] private Animator animator;
    
    [SerializeField] private float changeTime = 3.0f;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = changeTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        backSightRange = transform.position.x - backLine.position.x;
        RaycastHit2D backSight = Physics2D.Raycast(transform.position, Vector2.left,
            backSightRange, LayerMask.GetMask("Player"));
        if (backSight.collider != null) {
            transform.Rotate(0f, 180f, 0f);
        }

        sightRange = sightLine.position.x - transform.position.x;
        RaycastHit2D sight = Physics2D.Raycast(transform.position, Vector2.right,
            sightRange, LayerMask.GetMask("Player"));
        if (sight.collider != null && !GetComponent<SpearEnemy_Combat>().attacking) {
            Follow();
            hasSight = true;
        }
        else if(sight.collider == null && !GetComponent<SpearEnemy_Combat>().attacking) {
            Move();
            hasSight = false;
        }
        else {
            GetComponent<SpearEnemy_Combat>().attacking = false;
        }
    }

    public void Move() {
        animator.SetBool("Move", true);
        transform.Translate(moveSpeed * Time.fixedDeltaTime, 0f, 0f);
        timer -= Time.fixedDeltaTime;
        if (timer <= 0) {
            transform.Rotate(0f, 180f, 0f);
            timer = changeTime;
        }
    }

    public void Follow() {
        animator.SetBool("Move", true);
        transform.Translate(moveSpeed * Time.fixedDeltaTime, 0f, 0f);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, backLine.position);

        Gizmos.DrawLine(transform.position, sightLine.position);
    }
}
