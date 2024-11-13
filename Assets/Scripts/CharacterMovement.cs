using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterMovement : MonoBehaviour
{
    protected float speed = 5f;
    protected float wallSlideSpeed = 0.9f;  // Velocidad de deslizamiento en paredes
    protected float moveInput;
    protected bool isFacingRight;
    
    [SerializeField] protected float lerpAmount = 3f;

    [SerializeField] protected int jumpMultiplier;
    [SerializeField] protected float jumpPower;
    [SerializeField] protected float jumpTime;
    [SerializeField] protected float fallMultiplier;
    [SerializeField] protected float lowJumpMultiplier;
    public bool doubleJump;
    [SerializeField] bool grounded;
    [SerializeField] protected float _jumpBufferTime = 0.2f;
    [SerializeField] protected float _jumpBufferCounter;
    [SerializeField] protected float hDrag;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    Vector2 vecGravity;
    bool canJump;

    protected bool startDash;

    [SerializeField] public bool canDash;
    [SerializeField] public  bool isDashing;
    protected float dashingVelocity = 20f;
    public float dashingTime = .1f;
    private Vector2 _dashingDir;

    public CharacterSwitch switchChar;

    // public TrailRenderer trailRenderer;

    public bool finishedDashing;

    public CharacterSwitch SwitchCharacter;

    protected bool isJumping;
    protected float jumpCounter;
    float originalGravity;

    protected Transform transfo;
    protected Rigidbody2D body;
    bool isTouchingWall;
    protected virtual void Start()
    {
        vecGravity = new Vector2(0, -Physics2D.gravity.y);

        float moveInput = Input.GetAxis("Horizontal");
        body = GetComponent<Rigidbody2D>();
        HorizontalMovement();
        isFacingRight = true;
        canDash = true;
        SwitchCharacter = new CharacterSwitch();
    }
    public void HorizontalMovement() // Movimiento horizontal básico
    {
        float moveInput = Input.GetAxis("Horizontal");
        speed = Mathf.Lerp(body.velocity.y, lerpAmount, speed);
        body.velocity = new Vector2(moveInput * speed, body.velocity.y);
    }

    public virtual void FacingDirections()
    {

        // Cambia la dirección del personaje según el input
        if (body.velocity.x < 0 && isFacingRight)
        {
            isFacingRight = false;
            Debug.Log("Cambia de direccion a 3");
        }
        else if (body.velocity.x > 0 && isFacingRight == false)
        {
            isFacingRight = true;
            Debug.Log("Cambia de direccion a 4");
        }
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

        if (_jumpBufferCounter > 0f && (isGrounded() || doubleJump == true))
        {
            canJump = false;
            body.velocity = new Vector2(body.velocity.x, 0);
            //body.velocity = new Vector2(body.velocity.x, jumpPower);
            body.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); // un lance para arriba 
            isJumping = true; // marcar que esta saltando
            jumpCounter = 0; // tiempo de salto igual a serop
            _jumpBufferCounter = 0f;
            if (doubleJump)
            {
                doubleJump = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

        if (body.velocity.y > 0)
        {
            jumpCounter += Time.deltaTime;
            body.velocity = new Vector2(body.velocity.x, body.velocity.y);
            if (jumpCounter > jumpTime)
            {
                isJumping = false; // Detener el salto cuando se supera el tiempo máximo de salto

                body.velocity = new Vector2(body.velocity.x, 0);
                body.velocity -= vecGravity * fallMultiplier * Time.deltaTime;


            }
            if (Input.GetButtonUp("Jump"))
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
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.4f, 0.6f), CapsuleDirection2D.Vertical, 0, groundLayer);
    }

    protected virtual void Update()
    {

        Jump();
        HorizontalMovement();
        if (isGrounded() == true)
        {
            grounded = true;
        }
        Dash();
                
        FacingDirections();
        
    }

    public void Dash()
    {
        var dashInput = Input.GetButtonDown("Dash");
        finishedDashing = false;
        if (dashInput && canDash)
        {
            isDashing = true;   
            canDash = false;
            startDash = true;
          //  trailRenderer.emitting = true;
            _dashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical") );

            if (_dashingDir == new Vector2(0,0))
            {
                if (isFacingRight == true)
                {
                    _dashingDir = new Vector2(1, 0);
                }
                if (isFacingRight == false)
                {
                    _dashingDir = new Vector2(-1, 0);
                }
                }

            StartCoroutine(StopDashing());
        }

        if (isDashing)
        {
            body.velocity = _dashingDir.normalized * dashingVelocity;
            startDash = false;
            return;
        }
        if (isGrounded())
        {
            canDash = true;
        }   
    }
    private IEnumerator StopDashing()
    {

        yield return new WaitForSeconds(0.2f); //esperamos a que pase la duracion del dash
        isDashing = false; //que pasa despues de ese tiempo
        finishedDashing = true;
        GetComponent<CharacterSwitch>().SwitchCharacter(); 
        //trailRenderer.emitting = false;
       ;
    }



}
    
    




    

    


