using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CatMovement : CharacterMovement
{

        public Animator _animator;
        public Transform catTransform;
        [SerializeField] bool canDJump = true;

        [SerializeField] private float wallRaycastDistance = 0.9f;
        [SerializeField] private Transform _catTransform;
        public bool hitFloor;

   
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
    public void Animator()
    {
        float speedX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;

        _animator.SetFloat("movement", speedX * speed);

        if (isFacingRight)
        {
            _animator.SetBool("Facing Right", isFacingRight);
            
        }
        else if (isFacingRight == false)
        {
            _animator.SetBool("Facing Right", isFacingRight = false);
            
        }
        _animator.SetBool("grounded", isGrounded());
        _animator.SetBool("grounded", isGrounded());
        _animator.SetFloat("yVelocity", body.velocity.y);
        _animator.SetFloat("xVelocity", body.velocity.x);
    }
    protected override void Update()
    {
        base.Update();
        Jump();
        Animator();
    }




}

