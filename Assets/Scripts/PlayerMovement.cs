using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 10f;
    [SerializeField] bool isPlayerAlive = true;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform bow;
    bool isJumping = false;
   
    Animator animator;
    Rigidbody2D rigidBody;
    Vector2 moveInput;
    CapsuleCollider2D bodyCollider2D;
    BoxCollider2D feetCollider2D;
    float gravityScaleAtStart;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider2D = GetComponent<CapsuleCollider2D>();
        feetCollider2D = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = rigidBody.gravityScale;
        isPlayerAlive = true;
 
    }


    void Update()
    {
        if (!isPlayerAlive) { return; }
        FlipSprite();
        Run();
        isPlayerJumping();
        Die();
        ClimbLadder();

    }


    void OnMove(InputValue value)
    {
        if (!isPlayerAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isPlayerAlive) { return; }
        if (!feetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground","Climbable"))) { return; }

        if (value.isPressed)
        {
            isJumping = true;
            rigidBody.velocity += new Vector2(0f, jumpSpeed);
        }

    }

    void OnFire(InputValue value)
    {
        if(!isPlayerAlive) { return; }
        if(value.isPressed)
        {
            ShootArrow();
        }
    }

    void ShootArrow()
    {
        if(rigidBody.transform.localScale.x < Mathf.Epsilon)
        {
            Instantiate(arrow, bow.position, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
        } else if(rigidBody.transform.localScale.x > Mathf.Epsilon) 
        {
            Instantiate(arrow, bow.position, transform.rotation);
        }
    }

    void ClimbLadder()
    {

        bool playerHasVerticalSpeed = Mathf.Abs(rigidBody.velocity.y) > Mathf.Epsilon;

        if (!feetCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbable"))){ rigidBody.gravityScale = gravityScaleAtStart; animator.SetBool("isClimbing", false); return; }
        if (isJumping) { return;  }

        rigidBody.gravityScale = 0f; 
        Vector2 playerClimbVelocity = new Vector2(rigidBody.velocity.x, moveInput.y * climbSpeed);
        rigidBody.velocity = playerClimbVelocity;

        animator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    void Run()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > 0;

        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rigidBody.velocity.y);
        rigidBody.velocity = playerVelocity;

        animator.SetBool("isRunning", playerHasHorizontalSpeed);

    }
    private void FlipSprite()
    {

        if (rigidBody.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } else if (rigidBody.velocity.x > 0)
        {
            transform.localScale = new Vector3( 1f, 1f, 1f);
        }
    }

    void Die()
    {
        if (bodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            animator.SetTrigger("Dying");
            Debug.Log("YOU DIED");
            isPlayerAlive = false;

            Vector2 playerVelocity = new Vector2(rigidBody.velocity.x * UnityEngine.Random.Range(12f, 20f), rigidBody.velocity.y * UnityEngine.Random.Range(12f, 20f));
            rigidBody.velocity = playerVelocity;
        }
    }

    void isPlayerJumping()
    {
        if(rigidBody.velocity.y < 0)
        {
            isJumping = false;
        }
    }
}
