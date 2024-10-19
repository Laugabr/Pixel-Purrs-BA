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

    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;
    Vector2 vecGravity;

    bool isJumping;
    float jumpCounter;


    Rigidbody2D body;
    bool isTouchingWall;

    private void Start()
    {
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        IsFacingRight = true;
        body = GetComponent<Rigidbody2D>();
        HorizontalMovement();
    }
    public void HorizontalMovement() // Movimiento horizontal b�sico
    {
        float moveInput = Input.GetAxis("Horizontal");
        speed = Mathf.Lerp(body.velocity.y, lerpAmount, speed);
        body.velocity = new Vector2(moveInput * speed, body.velocity.y);
    }

    public virtual void Jump()
    {
        // Al saltar
        if (Input.GetKeyDown("space") && isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower); // Asigna directamente la velocidad para evitar acumulaci�n
            isJumping = true;
            jumpCounter = 0;

            if (body.velocity.y > 0 && isJumping)
            {
                jumpCounter += Time.deltaTime;
                if (jumpCounter > jumpTime)
                {
                    isJumping = false; // Detener el salto cuando se supera el tiempo m�ximo de salto
                }
            }

        }
        // Mientras el jugador est� subiendo y manteniendo el bot�n de salto
        if (body.velocity.y > 0 && isJumping)
        {
            jumpCounter += Time.deltaTime;
            if (jumpCounter > jumpTime)
            {
                isJumping = false; // Detener el salto cuando se supera el tiempo m�ximo de salto
            }
        }


        // Si suelta el bot�n de salto antes de llegar a la altura m�xima
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            if (body.velocity.y > 0)
            {
                // Aplica un multiplicador de ca�da r�pida si se suelta el bot�n de salto
                body.velocity += vecGravity * lowJumpMultiplier * Time.deltaTime;
            }
        }

        // Si el personaje est� cayendo
        if (body.velocity.y < 0)
        {
            // Aplica un multiplicador mayor al caer para hacerlo m�s r�pido
            body.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
        }
    }
    bool isGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1.8f, 0.3f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }
    public virtual void WallSliding()
    {
        // Detectar si est� tocando la pared usando BoxCast para mayor precisi�n
        bool isTouchingWall = Physics2D.BoxCast(transform.position, new Vector2(0.1f, 1f), 0f, Vector2.right * Mathf.Sign(moveInput), 0.1f, LayerMask.GetMask("Wall"));

        // Si est�s tocando una pared y hay movimiento hacia ella, reduce la velocidad de deslizamiento
        if (isTouchingWall && moveInput != 0)
        {
            // Limitar la velocidad de ca�da al deslizarse por la pared
            body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
    }

    bool wallTouching()
    {
        return Physics2D.OverlapCapsule(wallCheck.position, new Vector2(1.8f, 0.3f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    void Update()
    {
        Jump();
             HorizontalMovement();
        WallSliding();
    }
}
