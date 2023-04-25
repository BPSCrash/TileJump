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
        Run();
        FlipSprite();
        ClimbLadder();
        Die();

    }


    void OnMove(InputValue value)
    {
        if (!isPlayerAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isPlayerAlive) { return; }
        if (!feetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (value.isPressed)
        {
            rigidBody.velocity += new Vector2(0f, jumpSpeed);
        }

    }

    void OnFire(InputValue value)
    {
        if(!isPlayerAlive) { return; }
        if(value.isPressed)
        {
            Instantiate(arrow, bow.position, transform.rotation);
        }
    }

    void ClimbLadder()
    {

        bool playerHasVerticalSpeed = Mathf.Abs(rigidBody.velocity.y) > Mathf.Epsilon;

        if (!feetCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbable"))) { rigidBody.gravityScale = gravityScaleAtStart; animator.SetBool("isClimbing", false); return; }

        rigidBody.gravityScale = 0f; 
        Vector2 playerClimbVelocity = new Vector2(rigidBody.velocity.x, moveInput.y * climbSpeed);
        rigidBody.velocity = playerClimbVelocity;

        animator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    void Run()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;

        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rigidBody.velocity.y);
        rigidBody.velocity = playerVelocity;

        animator.SetBool("isRunning", playerHasHorizontalSpeed);

    }
    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(MathF.Sign(rigidBody.velocity.x), 1f);

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
}
