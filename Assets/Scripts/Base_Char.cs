using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterMovement : MonoBehaviour
{
    protected float speed = 5f;
    protected float wallSlideSpeed = 0.9f;  // Velocidad de deslizamiento en paredes
    float moveInput = Input.GetAxis("Horizontal");
    [SerializeField] float lerpAmount = 3f;
    bool IsFacingRight;
    [SerializeField] int jumpMultiplier;
    [SerializeField] int jumpPower;
    [SerializeField] float jumpTime;
    [SerializeField] float fallMultiplier;
    [SerializeField] float lowJumpMultiplier;

    [SerializeField] private float _jumpBufferTime = 0.2f;
    [SerializeField] private float _jumpBufferCounter;

    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    Vector2 vecGravity;

    bool isJumping;
    float jumpCounter;


    Rigidbody2D body;
    bool isTouchingWall;

    private void Start()
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
            //body.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); // un lance para arriba 
            body.velocity = new Vector2(body.velocity.x, jumpPower);
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
        }
        if (Input.GetButtonUp("Jump") && isJumping == true && body.velocity.y > 0)
        {
            isJumping = false;
            body.velocity = new Vector2(body.velocity.x, body.velocity.y * lowJumpMultiplier);
        }
        
        if (body.velocity.y < 0 || isJumping == false)
        {
            body.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
        }
        
        
        

        // Si el personaje está cayendo

    } 
    // Al saltar
    /*   if (Input.GetKeyDown("space") && isGrounded())
       {
           body.velocity = new Vector2(body.velocity.x, jumpPower); // Asigna directamente la velocidad para evitar acumulación
           isJumping = true;
           jumpCounter = 0;

           if (body.velocity.y > 0 && isJumping)
           {
               jumpCounter += Time.deltaTime;
               if (jumpCounter > jumpTime)
               {
                   isJumping = false; // Detener el salto cuando se supera el tiempo máximo de salto
               }
           }

       }
       // Mientras el jugador está subiendo y manteniendo el botón de salto
       if (body.velocity.y > 0 && isJumping)
       {
           jumpCounter += Time.deltaTime;
           if (jumpCounter > jumpTime)
           {
               isJumping = false; // Detener el salto cuando se supera el tiempo máximo de salto
           }
       }


    // Si suelta el botón de salto antes de llegar a la altura máxima
    if (Input.GetButtonUp("Jump"))
    {
        isJumping = false;
        if (body.velocity.y > 0)
        {
            // Aplica un multiplicador de caída rápida si se suelta el botón de salto
            body.velocity += vecGravity * lowJumpMultiplier * Time.deltaTime;
        }
    }

    // Si el personaje está cayendo
    if (body.velocity.y < 0)
    {
        // Aplica un multiplicador mayor al caer para hacerlo más rápido
        body.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
    }
} */
    bool isGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1.8f, 0.3f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }
    public virtual void WallSliding()
    {
              

        // Si estás tocando una pared y hay movimiento hacia ella, reduce la velocidad de deslizamiento
        if (wallTouching() && moveInput != 0)
        {
            // Limitar la velocidad de caída al deslizarse por la pared
            body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
    }

    bool wallTouching()
    {
        return Physics2D.OverlapCapsule(wallCheck.position, new Vector2(1.8f, 0.3f), CapsuleDirection2D.Horizontal, 0, wallLayer);
    }

    void Update()
    {
        Jump();
             HorizontalMovement();
        WallSliding();
    }
}

