using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class GirlMovement : CharacterMovement
    {
    [SerializeField] private float wallRaycastDistance = 0.9f;
    
    private bool isGrabbingWall = false;
    public bool hitRightWall;
    public bool hitLefttWall;    
    public bool canWallJump;
    public float wallgrabBufferTime = 0.1f;
    public float wallgrabBufferCounter;
        protected override void Start()
        {
            base.Start();  // Llama a la lógica Start() de CharacterMovement
                           // Lógica adicional específica de GirlMovement, si es necesario
        }
    protected override void Update()
    {
        base.Update();
        WallGrab();
        OnDrawGizmos();
        
    }


    private void WallGrab()
    {
        hitRightWall = Physics2D.Raycast(transform.position, Vector2.right, wallRaycastDistance, groundLayer);
        hitLefttWall = Physics2D.Raycast(transform.position, Vector2.left, wallRaycastDistance, groundLayer);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            wallgrabBufferCounter = wallgrabBufferTime;
        }

        else
        {   
            wallgrabBufferCounter -= Time.deltaTime;
        }  

        if (hitLefttWall == true || hitRightWall == true && wallgrabBufferCounter > 0f)
        {
            isGrabbingWall = true;
            body.constraints = RigidbodyConstraints2D.FreezePosition;
            canWallJump = true;
            wallgrabBufferCounter = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                WallJump();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Z))
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
        isGrabbingWall = false;
        body.constraints = RigidbodyConstraints2D.None;
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (hitLefttWall == true)
        {
            body.AddForce(Vector2.right * jumpPower, ForceMode2D.Impulse);
        }
        if (hitRightWall == true)
        {
            body.AddForce(Vector2.left * jumpPower, ForceMode2D.Impulse);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * wallRaycastDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * wallRaycastDistance);
    }
}
