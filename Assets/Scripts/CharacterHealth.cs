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
    Vector2 checkPoint  ;
    Vector2 deathPosition;
    public GameObject girl;
    public GameObject cat;
    public GirlMovement girlControler;
    public CatMovement catControler;
    public CharacterSwitch charSwitch;
    Rigidbody2D body;
   

    // Update is called once per frame
    private void Start()
    {

        isAlive = true;
        checkPoint = transform.position;
        _character = GetComponent<Transform>();
        body = GetComponent<Rigidbody2D>();
    }
    

    private void Death()
    {
        body.velocity = new Vector2(0f, 0f);
        _character.transform.position = deathPosition;
        StartCoroutine(RespawnT());
    }

    private IEnumerator RespawnT()
    {
        yield return new WaitForSeconds(respawnTime);   
        Respawn();
    }
    public virtual void Respawn()
    {
        isAlive = true;
        transform.position = checkPoint;
    }

    public void UpdateCheckPoint(Vector2 pos)
    {
        checkPoint = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spikes")) 
        {
            SceneData.muertes++;
            deathPosition = _character.transform.position;
            isAlive = false;
        }
    }

    private void Update()
    {
        if (isAlive == false)
        {
            Death();
        }
    }

    public static class SceneData
    {
        public static int muertes;
    }


}
