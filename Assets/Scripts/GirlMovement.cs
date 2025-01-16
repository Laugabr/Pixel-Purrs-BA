using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlMovement : CharacterMovement
{
    public Transform _girlTransform;
    public Animator _animator;
    public SpriteRenderer _spriteRenderer;
    
    public bool hitRightWall;
    public bool hitLefttWall;
    public bool canWallJump;
    public float wallgrabBufferTime = 0.1f;
    public float wallgrabBufferCounter;
    public CharacterHealth charHealth;

    [Header("Wall Sliding Settings")]
    [SerializeField] float _wallSlidingSpeed;
    [SerializeField] float speedY;

    protected override void Start()
    {
        base.Start();
        charHealth.isAlive = true;
        charHealth = gameObject.GetComponent<CharacterHealth>();
        _girlTransform.rotation = Quaternion.Euler(0f,0f,0f);
    }
    protected override void Update()
    {
        base.Update();
       
            HorizontalMovement();
            Jump();
            Dash();
        Animator();
        
        WallGrabing();
    }

    public bool IsTouchingWall()
    {
        return Physics2D.OverlapCapsule(wallCheck.position, new Vector2(0.7f, 0.4f), CapsuleDirection2D.Vertical, 0, wallLayer);
    }


    private void WallSlide()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if (IsTouchingWall() && !IsGrounded() && horizontal != 0f) //si toca la pared, no esta tocando el piso, y si esta recibiendo un Input horizontal (si sigue presionando contra la pared)
        {
            body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -_wallSlidingSpeed, float.MaxValue)); //limitamos la velocidad en y a ese valor

            if (Input.GetButtonDown("Jump"))
            {
               // WallJump();
            }
        }
    }

    public void Animator()
    {
        float speedX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;

        _animator.SetFloat("movement", speedX * speed);
        _animator.SetBool("startedDash", startDash);
        _animator.SetBool("grounded", IsGrounded());
        _animator.SetFloat("yVelocity", body.velocity.y);
        _animator.SetFloat("xVelocity", body.velocity.x);
        _animator.SetBool("isDashing", isDashing);
        if (isFacingRight)
        {
            _animator.SetBool("Facing Right", isFacingRight);
        }

        else if (isFacingRight == false)
        {
            _animator.SetBool("Facing Right", isFacingRight = false);
        }
        _animator.SetBool("isAlive", charHealth.isAlive);
        _animator.SetBool("isGrabingWall", IsTouchingWall());
        
    }
   
       private void WallGrabing()
    {
        if (IsTouchingWall() && Input.GetKey(KeyCode.Z) && IsGrounded() == false)
        { 
            body.velocity = new Vector2(0f, 0f);

            if (Input.GetButton("Dash"))
            {
                Dash();
            }
        }
        
    }

}
