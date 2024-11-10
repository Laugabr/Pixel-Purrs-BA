using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CatMovement : CharacterMovement
{

        public Animator animator;
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
    private void Animator()
    {
        float speedX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        animator.SetFloat("movement", speedX * speed);
        /* if (speedX < 0)
         {
             transform.localScale = new Vector3(-1,1,1);
         }
         if (speedX > 0)
         {
             transform.localScale = new Vector3(1,1,1);
         }
         */
        if (isFacingRight)
        {
            animator.SetBool("Facing Right", isFacingRight);
        }
        if (isFacingRight)
        {
            animator.SetBool("Facing Right", isFacingRight = false);
        }
    }
    protected override void Update()
    {
        base.Update();
        Jump();
        Animator();
    }




}

