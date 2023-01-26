using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
        
        private Rigidbody2D rb;
        private BoxCollider2D collision;
        private SpriteRenderer sprite;
        private Animator anim;
        
        [SerializeField] private LayerMask jumpableGround;

        private float directionX = 0f;
        // [SerializeField] private = it shows in unity editor
        // but if used "public float moveSpeed = 7f;" it still shows, but
        // then this variable is accessible by every script
        [SerializeField] private float moveSpeed = 7f;
        [SerializeField] private float jumpForce = 14f;

        private enum MovementState { idle, running, jumping, falling }

        [SerializeField] private AudioSource jumpSoundEffect;

        // console log = Debug.Log("Hello world");

        // some stuff to learn later

        // int wholeNumber = 16;
        // float decimalNumber = 4.56f;
        // string text = "text example";
        // bool boolean = true;
    
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collision = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        //GetAxisRaw = will remove sliding aniamtion after player stops moving left or right
        directionX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(directionX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
//shorter version of code

        // if (directionX != 0)
        // {
        //     anim.SetBool("Running", true);
        // }

        MovementState state;

        if (directionX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (directionX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else 
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f) 
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded() 
    {
       return Physics2D.BoxCast(collision.bounds.center, collision.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
