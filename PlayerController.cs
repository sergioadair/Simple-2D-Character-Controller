using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed;
    public float jumpForce;
    public float jumpTime;
    public float horizontalDrag;

    private float hMoveInput;
    private bool jumpInputDown;
    private bool jumpInputHold;
    private bool facingRight = true;

    private bool isGrounded;
    public Transform feetTransform;
    public LayerMask groundLayer;
    private float feetRadius = 0.3f;

    private float jumpTimeCounter;
    private bool isJumping;
    private int extraJumpsValue = 1;
    private int extraJumps;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumps = extraJumpsValue;
    }

    // Update is called once per frame
    void Update()
    {
        hMoveInput = Input.GetAxisRaw("Horizontal");
        jumpInputDown = Input.GetButtonDown("Jump");
        jumpInputHold = Input.GetButton("Jump");

        // Fliping the character
        if((!facingRight && hMoveInput > 0) || (facingRight && hMoveInput < 0)) FlipCharacter();

        // Is the player on the ground?
        isGrounded = Physics2D.OverlapCircle(feetTransform.position, feetRadius, groundLayer);

        if(isGrounded){
            extraJumps = extraJumpsValue;
        }

        Jump();
    }
    
    void FixedUpdate()
    {
        // Horizontal movement
        rb.AddForce(new Vector2(hMoveInput * speed, 0f), ForceMode2D.Impulse);

        //Drag
        if(Mathf.Abs(rb.velocity.x) > 0.0001f)
            rb.velocity = new Vector2(rb.velocity.x * (1-Mathf.Clamp(horizontalDrag, 0, 1)), rb.velocity.y);
        else
            rb.velocity = new Vector2(0f, rb.velocity.y);
    }

    void FlipCharacter()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void Jump()
    {
        // Hold to jump higher
        if(jumpInputHold){
            if(isGrounded){
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isJumping = true;
                jumpTimeCounter = jumpTime;
            }

            if(isJumping){
                if(jumpTimeCounter > 0){
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    jumpTimeCounter -= Time.deltaTime;
                }else{
                    isJumping = false;
                }
            }
        }else{
            isJumping = false;
        }

        // Extra jumps
        if(jumpInputDown){
            if(extraJumps > 0 && !isGrounded){
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isJumping = true;
                jumpTimeCounter = jumpTime;
                --extraJumps;
            }
        }
    }
    
}
