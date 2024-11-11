using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterSwitch : MonoBehaviour
{
    public GirlMovement girlControler;
    public CatMovement catControler;

    public GameObject girl;
    public GameObject cat;
    Rigidbody2D body;
    public bool girlIsActive = true;
    CharacterMovement dashFinished;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SwitchCharacter();
        }
    }

    public void SwitchCharacter()
    {
        if (girlIsActive == true)
        {
            
            
            girlIsActive = false;
            girl.SetActive(false);  
            cat.SetActive(true);
            girlControler.enabled = false;
            catControler.enabled = true;
        }
        else
        {
            girlControler.enabled = true;
            catControler.enabled = false;
            girlIsActive = true;
            girl.SetActive(true);
            cat.SetActive(false);
        }
    }
}
