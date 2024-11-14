using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CatMovement : CharacterMovement
{
    private bool isAlivv;

    public Animator _animator;
    [SerializeField] Transform _catTransform;
    [SerializeField] bool canDJump = true;
    [SerializeField] private float wallRaycastDistance = 0.9f;
    public bool hitFloor;
    public CharacterHealth charHealth;
    private bool _isMovementEnabled;
    protected override void Start()
    {
        base.Start();
        _isMovementEnabled = true;
        charHealth.isAlive = true;
    }


    /*  private void DoubleJump()
      {
          hitFloor = Physics2D.Raycast(_catTransform.position, Vector2.down, wallRaycastDistance, groundLayer);

          if (hitFloor == true)
          {
              canDJump = true;
              return;
          }

          if (hitFloor == false && canDJump == true && Input.GetButtonDown("jump"))
          {
              body.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); // un lance para arriba 

              isJumping = true; // marcar que esta saltando
              jumpCounter = 0; // tiempo de salto igual a serop
              canDJump = false;
          }

      }
    */

    public override void Jump()
    {
        base.Jump();
        if (isGrounded())
        {
            doubleJump = true;
        }
        if (isGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }
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
    protected override void Update()
    {
        base.Update();
        if (_isMovementEnabled)
        {
            HorizontalMovement();
            Jump();
            Dash();
        }
        Animator();
        isAlivv = charHealth.isAlive;

    }



}

