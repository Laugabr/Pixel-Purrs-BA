using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public bool isAlive;
    public Transform spikesCheck;
    public LayerMask spikesLayer;
    public float respawnTime = 5f;
    Transform _character;
    Vector2 startPos;
    public GameObject girl;
    public GameObject cat;
    public GirlMovement girlControler;
    public CatMovement catControler;
    public CharacterSwitch charSwitch;



    // Update is called once per frame
    private void Start()
    {
        isAlive = true;
        startPos = transform.position;
        _character = GetComponent<Transform>();
    }
    void Update()
    {
        if (isTouchingSpikes())
        {
            Death();
        }
    }

    private void Death()
    {
        isAlive = false;
        catControler.enabled = false;
        girlControler.enabled = false;
        
        StartCoroutine(RespawnTime(2f));
    }

    private IEnumerator RespawnTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        Respawn();
    }

    private void Respawn()
    {
        if (charSwitch.girlIsActive == true)
        {
            girlControler.enabled = true;
        }
        if (charSwitch.girlIsActive == false)
            if (charSwitch.girlIsActive == false)
            {
                catControler.enabled = true;
            }
        isAlive = true;
        transform.position = startPos;
    }


    private bool isTouchingSpikes()
    {
        return Physics2D.OverlapCapsule(spikesCheck.position, new Vector2(0.5f, 1f), CapsuleDirection2D.Vertical, 0, spikesLayer);
    }

    


}
