using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlMovement : CharacterMovement
{
    public Transform _girlTransform;
    public Animator _animator;
    public SpriteRenderer _spriteRenderer;
    private bool isGrabbingWall;
    public bool hitRightWall;
    public bool hitLefttWall;
    public bool canWallJump;
    public float wallgrabBufferTime = 0.1f;
    public float wallgrabBufferCounter;
    public CharacterHealth charHealth;
    private bool isAlivv;
    bool _isMovementEnabled;
    protected override void Start()
    {
        base.Start();
        _isMovementEnabled = true;
        charHealth.isAlive = true;
        charHealth = gameObject.GetComponent<CharacterHealth>();

        isGrabbingWall = false;      
        _girlTransform.rotation = Quaternion.Euler(0f,0f,0f);
    }
    protected override void Update()
    {
        base.Update();
       if (_isMovementEnabled)
        {
            HorizontalMovement();
            Jump();
            Dash();
        } 
        WallGrab();
        Animator();
        isAlivv = charHealth.isAlive;
    }
    
    public void Animator()
    {
        float speedX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;

        _animator.SetFloat("movement", speedX * speed);
        _animator.SetBool("startedDash", startDash);
        _animator.SetBool("grounded", isGrounded());
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
        
    }
    public void DisableMovement()
    {
        _isMovementEnabled = false;
        body.velocity = Vector2.zero; // Para asegurarse de que esté quieto
    }

    public void EnableMovement()
    {
        _isMovementEnabled = true;
    }

    private void WallGrab()
    {
        if (!isGrabbingWall)
        {
            return;
        }



    }



    /*
    private void WallGrab()
    {
        hitRightWall = Physics2D.Raycast(_girlTransform.position, Vector2.right, wallRaycastDistance, groundLayer);
        hitLefttWall = Physics2D.Raycast(_girlTransform.position, Vector2.left, wallRaycastDistance, groundLayer);

        if (Input.GetKey(KeyCode.Z))
        {
            wallgrabBufferCounter = wallgrabBufferTime;
        }
        else
        {   
            wallgrabBufferCounter -= Time.deltaTime;
        }  

        if ((hitLefttWall == true || hitRightWall == true) && wallgrabBufferCounter > 0f)
        {
            isGrabbingWall = true;
            body.velocity = new Vector2(body.velocity.x, body.velocity.y * Input.GetAxis("Vertical"));
            canWallJump = true;
            wallgrabBufferCounter = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                isGrabbingWall = false;
                body.constraints = RigidbodyConstraints2D.None;
                body.constraints = RigidbodyConstraints2D.FreezeRotation;
                WallJump();
            }
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            isGrabbingWall = false;
            body.gravityScale = 2;
            body.constraints = RigidbodyConstraints2D.None;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            canWallJump = false;
        }
        // Si hay una pared a la izquierda o derecha y el jugador presiona "Z", se agarra

    }
    */
    private void WallJump()
    {
        if (isGrabbingWall == true)
        
        if (hitLefttWall == true)
        {
            body.AddForce(Vector2.right * jumpPower, ForceMode2D.Impulse);
        }
        if (hitRightWall == true)
        {
            body.AddForce(Vector2.left * jumpPower, ForceMode2D.Impulse);
        }
    }
   
}
