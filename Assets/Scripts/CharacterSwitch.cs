using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitch : MonoBehaviour
{
    public GirlMovement girlControler;
    public CatMovement catControler;

    public GameObject girl;
    public GameObject cat;

    public bool girlIsActive = true;
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
            girlControler.enabled = false;
            catControler.enabled = true;
            girlIsActive = false;
            girl.SetActive(false);  
            cat.SetActive(true);
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
