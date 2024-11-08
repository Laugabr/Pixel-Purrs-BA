using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    Rigidbody2D body;
    public Rigidbody2D character;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        body.transform.position = character.transform.position;
    }
}
