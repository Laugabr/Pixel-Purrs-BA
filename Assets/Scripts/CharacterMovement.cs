using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterMovement : MonoBehaviour
{
    [Header("Horizontal Movement Settings")]

    public float speed = 5f;

    protected float moveInput;

    protected bool isFacingRight;

    [SerializeField] protected float lerpAmount = 3f;


    [Header("Jump Settings")]

    [SerializeField] protected int jumpMultiplier;

    [SerializeField] protected float jumpPower;

    [SerializeField] protected float jumpTime;

    [SerializeField] protected float fallMultiplier;

    [SerializeField] protected float lowJumpMultiplier;
    public bool doubleJump;

    public bool grounded;

    [SerializeField] protected float _jumpBufferTime = 0.2f;

    [SerializeField] protected float _jumpBufferCounter;

    protected bool isJumping;

    protected float jumpCounter;


    [Header("References")]

    public Transform groundCheck;

    public Transform wallCheck;

    [SerializeField] GirlMovement _girlMovement;

    public LayerMask groundLayer;

    public LayerMask wallLayer;

    Vector2 vecGravity;

    public CharacterSwitch switchChar;

    protected Rigidbody2D body;

    public GameObject girl;

    public CharacterHealth characterHealth;


    [Header("Dash Settings")]
    public bool startDash;

    bool canDash;

    protected bool isDashing;

    [SerializeField] protected float dashingVelocity = 20f;

    [SerializeField]  protected float dashingTime = .1f;

    private Vector2 _dashingDir;

    protected bool finishedDashing;

    private float _originalGravity;

    private float maxSpeed;

    private float maxYSpeed;

    protected virtual void Start()
    {
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        float moveInput = Input.GetAxis("Horizontal");
        body = GetComponent<Rigidbody2D>();
        characterHealth = GetComponent<CharacterHealth>();
        HorizontalMovement();
        isFacingRight = true;
        canDash = true; //empieza pudiendo dashear
        _girlMovement = gameObject.GetComponent<GirlMovement>(); //tomamos el componente del movimiento de la chica
        maxSpeed = 23f; //seteamos las velocidades maximas que puede tomar el personaje, para evitar que haya excesos con las fisicas
        maxYSpeed = 15;
    }
    protected virtual void Update()
    {
        HorizontalMovement();
        FacingDirections();
        Jump();
        Dash();
        MaxVelocity();
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
        }
        else if (body.velocity.x > 0 && isFacingRight == false)
        {
            isFacingRight = true;
        }
    }

    public virtual void Jump // salto
()
    {
        if (Input.GetButtonDown("Jump")) //si presionamos el salto se crea una cuenta atras, dentro de ese tiempo, cuando toques el piso, saltas
        {
            _jumpBufferCounter = _jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        if (_jumpBufferCounter > 0f && (IsGrounded() || doubleJump == true)) 
        {
            body.velocity = new Vector2(body.velocity.x, 0);
            body.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); // un lance para arriba 
            isJumping = true; // marcar que esta saltando
            jumpCounter = 0; // tiempo de salto igual a cero
            _jumpBufferCounter = 0f; //se el buffer
            if (doubleJump) //si puede hacer double jump, puede hacer otro salto
            {
                doubleJump = false;
            }
            if (jumpCounter > jumpTime) 
            {
                isJumping = false; // Detener el salto cuando se supera el tiempo máximo de salto
            }
        }

        if (Input.GetButtonUp("Jump")) // si dejas de tocar el salto antes 
        {
            isJumping = false;
        }

        if (body.velocity.y > 0) //mientras subis
        {
            jumpCounter += Time.deltaTime;
            body.velocity = new Vector2(body.velocity.x, body.velocity.y);
            if (jumpCounter > jumpTime)
            {
                isJumping = false; // Detener el salto cuando se supera el tiempo máximo de salto
                body.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
            }
        }
        
        if (body.velocity.y < 0f)
        {
            body.velocity -= vecGravity * fallMultiplier * Time.deltaTime;

        }

    }
    
    public bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.2f, 0.4f), CapsuleDirection2D.Vertical, 0, groundLayer);
    }

    

    public virtual void Dash()
    {
        var dashInput = Input.GetButtonDown("Dash");
        finishedDashing = false;
        if (dashInput && canDash && characterHealth.isAlive) //si tocas el boton de dash ("x")
        {
            isDashing = true;   
            canDash = false;
            startDash = true;
            _dashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical") ); //te toma la direccion del dash, hay 9 en total

            if (_dashingDir == new Vector2(0,0)) //si lo tocas mientras no insertas ningun input, la direccion depende de que a que direccion estas viendo
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

            _originalGravity = body.gravityScale;
            StartCoroutine(StopDashing()); //tras empezar el dash, se llama a la corutina para esperar el tiempo que dura

        }

        if (isDashing) //movimiento del dash
        {
            body.velocity = _dashingDir.normalized * dashingVelocity;
            startDash = false;
           
            body.gravityScale = 0f; // le sacamos la gravedad para mejorar el dash
            return;
        }
        if (IsGrounded() ||(switchChar.girlIsActive == true && _girlMovement.IsTouchingWall() == true)) //cada vez que toca el piso se resetea el dash, o si sos la chica al tocar una pared
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
        body.gravityScale = _originalGravity; // le devolvemos la gravedad
    }

 

    private void MaxVelocity()
    {
        Vector2 velocity = body.velocity;

        // Limitar la velocidad en el eje X
        if (Mathf.Abs(velocity.x) > maxSpeed)
        {
            velocity.x = Mathf.Sign(velocity.x) * maxSpeed;
        }

        // Limitar la velocidad en el eje Y
        if (Mathf.Abs(velocity.y) > maxYSpeed)
        {
            velocity.y = Mathf.Sign(velocity.y) * maxYSpeed;
        }

        body.velocity = velocity;
    }


}
    
    




    

    


