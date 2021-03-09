using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk_State : StateMachineBehaviour
{
    Transform player;
    Rigidbody2D rb;
    float speed;
    Abomination_Moviment boss;    

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Abomination_Moviment>();
        speed = boss.moveSpeed;       
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.Flip();

        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos =  Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);

        rb.MovePosition(newPos);
        if (Vector2.Distance(player.position, rb.position ) <= 3.0f) {
            animator.SetTrigger("Attack");
        }
        if (Vector2.Distance(player.position, rb.position) >= 6.0f) {
            animator.SetTrigger("PowerAttack");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("PowerAttack");
    }    
}
