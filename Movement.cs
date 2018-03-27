using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    [SerializeField] public float walkSpeed = 5.0f;
    [SerializeField] float jumpHeight;

    Rigidbody2D rigidbodyPlayer;
    bool inAir = false;
    bool isWalking = false;
    SpriteRenderer spriteRenderer;

    Animator animator;

	// Use this for initialization
	void Start ()
    {
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        OnPlayerInput();
        CheckAnimationStates();
    }

    private void CheckAnimationStates()
    {
        if (isWalking == false)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", true);
        }

        if (inAir == true)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", false);
            animator.SetBool("isJumping", true);
        }

        if (inAir == false)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isJumping", false);
        }

        if (isWalking == true)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isIdle", false);
        }
    }

    void OnPlayerInput ()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        ProcessWalking(horizontalInput);
        ProcessJumping();
    }

    private void ProcessJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && inAir == false) //FIXneed: if two surfaces are too close, greater jump is possible --> delay for jump
        {
            rigidbodyPlayer.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            inAir = true;
        }

        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            isWalking = false;
        }
    }

    private void ProcessWalking(float horizontalInput)
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector2.right * horizontalInput * walkSpeed * Time.deltaTime);
            isWalking = true;
            spriteRenderer.flipX = true;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector2.right * horizontalInput * walkSpeed * Time.deltaTime);
            isWalking = true;
            spriteRenderer.flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        CheckGroundCollision(other);
    }

    private void CheckGroundCollision(Collision2D other)
    {
        if (other.transform.tag == "Ground")
        {
            inAir = false;
            animator.SetBool("isJumping", false);
        }
    }
}
