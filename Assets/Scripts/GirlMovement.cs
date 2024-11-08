using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlMovement : CharacterMovement
{
    [SerializeField] private float wallRaycastDistance = 0.9f;
    [SerializeField] private Transform _girlTransform;

    private bool isGrabbingWall;
    public bool hitRightWall;
    public bool hitLefttWall;
    public bool canWallJump;
    public float wallgrabBufferTime = 0.1f;
    public float wallgrabBufferCounter;


    protected override void Start()
    {
        base.Start();
        isGrabbingWall = false;
    }
    protected override void Update()
    {
        base.Update();
        WallGrab();
        
        
    }


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
            body.constraints = RigidbodyConstraints2D.FreezePosition;
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
