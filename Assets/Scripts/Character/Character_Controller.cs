﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour
{
    [HideInInspector] public float moveInput;
    [SerializeField] private float speed = 4.5f;
    [SerializeField] private float jumpForce;
    [SerializeField] private float wallJumpForce;
    public bool grounded = true;

    [HideInInspector] public Vector2 faceDirection = new Vector2(1,0);
    [SerializeField] private bool wallSliding = false;
    [HideInInspector] public bool flipSide = false;

    [SerializeField] private Transform rollPoint;
    [SerializeField] private Animator animator;
    private Rigidbody2D body;    
    void Start(){
        body = GetComponent<Rigidbody2D>();        
    }
    void Update() {
        if (Input.GetButtonDown("Jump") && grounded) {
            body.AddForce((Vector2.up * jumpForce), ForceMode2D.Impulse);
        }
        
        if (Input.GetButtonDown("Jump") && wallSliding && !grounded && moveInput == 0
            && body.velocity.y < -0.2f) {
            body.AddForce((Vector2.up * jumpForce * 1.2f), ForceMode2D.Impulse);
            if (flipSide) {
                body.AddForce((Vector2.right * wallJumpForce), ForceMode2D.Impulse);
                Flip();
            }
            else {
                body.AddForce((Vector2.left * wallJumpForce), ForceMode2D.Impulse);
                Flip();
            }
        }

        if (!wallSliding) {
            if (body.velocity.y < -0.02 || body.velocity.y > 0.02) {
                grounded = false;
            }
            else {
                grounded = true;
            }       
        }
        animator.SetFloat("Jump", body.velocity.y);
        animator.SetBool("Grounded", grounded);        
    }
    void FixedUpdate(){        
        moveInput = Input.GetAxisRaw("Move");
        if (moveInput != 0 && !gameObject.GetComponent<Character_Combat>().attacking && !wallSliding){
            Moviment(moveInput);
            animator.SetBool("Move", true);
        }
        else{
            animator.SetBool("Move", false);
        }           
    }
    void Moviment(float input){
        if (!gameObject.GetComponent<Character_Combat>().shieldIsUp){
            transform.Translate(Mathf.Abs(input) * speed * Time.fixedDeltaTime, 0f, 0f);
        }
        if (input > 0 && flipSide){
            Flip();                  
        }
        if (input < 0 && !flipSide){
            Flip();
        }
    }
    void Flip(){
        flipSide = !flipSide;

        transform.Rotate(0f, 180f, 0f);
        faceDirection *= -1;
    }
    void OnCollisionStay2D(Collision2D other){
        if (other.gameObject.CompareTag("Walls") && !grounded){
            wallSliding = true;
            if (body.velocity.y < 0){
                body.velocity = new Vector2(body.velocity.x, -1.0f);
                animator.SetBool("WallSlide", wallSliding);
            }
        }
        else{
            wallSliding = false;
            animator.SetBool("WallSlide", wallSliding);            
        }
        
    }
    void OnCollisionExit2D(Collision2D other){
        if (other.gameObject.CompareTag("Walls")){
            wallSliding = false;
            animator.SetBool("WallSlide", wallSliding);
        }
    }   
}
