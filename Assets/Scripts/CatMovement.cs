    using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CatMovement : CharacterMovement
{
    [Header("References")]

    public Animator _animator;
    public Transform catTransform;
    public CharacterHealth charHealth;    
    protected override void Start()
    {
        base.Start();
        charHealth.isAlive = true;
        charHealth = gameObject.GetComponent<CharacterHealth>();
        catTransform.rotation = Quaternion.Euler(0f, 0f, 0f);       
    }
    protected override void Update()
    {
        base.Update();
        Animator();
        if (coyoteTimeCounter > 0f) //si toca el piso puede hacer un doble salto de nuevo
        {
            doubleJump = true;
        }

    }


    public override void Jump()
    {
        base.Jump();
        
        if (IsGrounded() && !Input.GetButton("Jump")) //para que no haga el salto doble desde el suelo
        {
            doubleJump = false;
        }
    }




    public void Animator() //Funcion de animator distinta con la chica porque 
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

    }
    
    

}

