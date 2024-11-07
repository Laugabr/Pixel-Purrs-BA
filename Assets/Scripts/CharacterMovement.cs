using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterMovement : MonoBehaviour
{
    protected float speed = 5f;
    protected float wallSlideSpeed = 0.9f;  // Velocidad de deslizamiento en paredes
    protected float moveInput;
    [SerializeField] protected float lerpAmount = 3f;
    bool IsFacingRight;
    [SerializeField] protected int jumpMultiplier;
    [SerializeField] protected float jumpPower;
    [SerializeField] protected float jumpTime;
    [SerializeField] protected float fallMultiplier;
    [SerializeField] protected float lowJumpMultiplier;

    [SerializeField] protected float _jumpBufferTime = 0.2f;
    [SerializeField] protected float _jumpBufferCounter;
    [SerializeField] protected float hDrag;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    Vector2 vecGravity;
    bool canJump;
    
    protected bool isJumping;
    protected float jumpCounter;


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
            canJump = true;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        if (_jumpBufferCounter > 0f && isGrounded() && canJump == true)
        {
            canJump = false;
            body.velocity = new Vector2(body.velocity.x, 0);
            //body.velocity = new Vector2(body.velocity.x, jumpPower);

            body.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); // un lance para arriba 
            
            isJumping = true; // marcar que esta saltando
            jumpCounter = 0; // tiempo de salto igual a serop
            _jumpBufferCounter = 0f;           
        }
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

        if (body.velocity.y > 0)
        {
            jumpCounter += Time.deltaTime;
            body.velocity = new Vector2(hDrag * body.velocity.x, body.velocity.y);
            if (jumpCounter > jumpTime)
            {
                isJumping = false; // Detener el salto cuando se supera el tiempo máximo de salto

                body.velocity = new Vector2(body.velocity.x, 0);
                body.velocity -= vecGravity * fallMultiplier * Time.deltaTime;


            }
            if (Input.GetButtonUp("Jump") )
            {
              //  isJumping = false;
               // body.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
                //body.velocity = new Vector2(body.velocity.x, body.velocity.y * lowJumpMultiplier);
            }
        }
        
        
        if (isJumping == false)
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

