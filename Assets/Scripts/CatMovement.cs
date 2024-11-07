using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMovement : CharacterMovement
{

    [SerializeField] bool canDJump;
   
    
    public override void Jump()
    {
        base.Jump();
        DoubleJump();
    }

    private void DoubleJump()
    {
        if (isGrounded() == false && canDJump == true)
        {
            
            body.velocity = new Vector2(body.velocity.x, 0);
            //body.velocity = new Vector2(body.velocity.x, jumpPower);

            body.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); // un lance para arriba 

            isJumping = true; // marcar que esta saltando
            jumpCounter = 0; // tiempo de salto igual a serop
            _jumpBufferCounter = 0f;
            canDJump = false;
        }
        if (isGrounded())
        {
            canDJump = true;
        }

    }
}

/*bool inAir()
{
    return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.6f, 0.01f), CapsuleDirection2D.Vertical, 0, groundLayer);
}*/