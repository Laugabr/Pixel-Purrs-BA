using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterMovement : MonoBehaviour
{
    protected float speed = 5f;
    protected float wallSlideSpeed = 0.9f;  // Velocidad de deslizamiento en paredes
    protected float moveInput = Input.GetAxis("Horizontal");
    [SerializeField] protected float lerpAmount = 3f;
    bool IsFacingRight;
    [SerializeField] protected int jumpMultiplier;
    [SerializeField] protected float jumpPower;
    [SerializeField] protected float jumpTime;
    [SerializeField] protected float fallMultiplier;
    [SerializeField] protected float lowJumpMultiplier;

    [SerializeField] protected float _jumpBufferTime = 0.2f;
    [SerializeField] protected float _jumpBufferCounter;

    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    Vector2 vecGravity;

    bool isJumping;
    float jumpCounter;
     
    
    protected Rigidbody2D body;
    bool isTouchingWall;

    protected virtual void Start()
    {
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        IsFacingRight = true;
        float moveInput = Input.GetAxis("Horizontal");
        body = GetComponent<Rigidbody2D>();
        HorizontalMovement();
    }
    public void HorizontalMovement() // Movimiento horizontal básico
    {
        float moveInput = Input.GetAxis("Horizontal");
        speed = Mathf.Lerp(body.velocity.y, lerpAmount, speed);
        body.velocity = new Vector2(moveInput * speed, body.velocity.y);

           }

    

    public virtual void Jump()
    {
      
        if (Input.GetButtonDown("Jump"))
        {
            _jumpBufferCounter = _jumpBufferTime;
        }

        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        if (_jumpBufferCounter > 0f && isGrounded())
        {
            body.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); // un lance para arriba 
            
            isJumping = true; // marcar que esta saltando
            jumpCounter = 0; // tiempo de salto igual a serop
            _jumpBufferCounter = 0f;           
        }

        if (body.velocity.y > 0 && isJumping)
        {
            jumpCounter += Time.deltaTime;
            if (jumpCounter > jumpTime)
            {
                isJumping = false; // Detener el salto cuando se supera el tiempo máximo de salto
                if (isJumping == false)
                {
                    body.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
                }

            }
            if (Input.GetButtonUp("Jump"))
            {
                isJumping = false;
                body.velocity = new Vector2(body.velocity.x, body.velocity.y * lowJumpMultiplier);
            }
        }
        
        
        if (body.velocity.y < 0 || isJumping == false)
        {
            body.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
        }
        
        
    } 
    public bool isGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.6f, 0.01f), CapsuleDirection2D.Vertical, 0, groundLayer);
    }
   

   

    protected virtual void Update()
    {
        Jump();
             HorizontalMovement();
        
    }
}

