using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterSwitch : MonoBehaviour
{
    public GirlMovement girlControler;
    public CatMovement catControler;
    public CharacterMovement dashingTime;
    public GameObject girl;
    public GameObject cat;
    public bool girlIsActive = true;
    public CharacterMovement finishedDashing;
    public CharacterMovement startedDash;
    public Transform girlTransform;
    public Transform catTransform;
    public CharacterHealth charHealth;
    // Update is called once per frame
    private void Start() //empezamos como la chica
    {
        girlControler.enabled = true;
        catControler.enabled = false;
        girlIsActive = true;
        girl.SetActive(true);
        cat.SetActive(false);
        charHealth = gameObject.GetComponent<CharacterHealth>();
    }
    private void Update()
    {
        if (startedDash)
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
            ResetRotation();
            catControler.doubleJump = true; //tras convertirse en gato, puede hacer un salto
            charHealth.isAlive = true;
        }
        else
        {
            girlControler.enabled = true;
            catControler.enabled = false;
            girlIsActive = true;
            girl.SetActive(true);
            cat.SetActive(false);
            ResetRotation();
            charHealth.isAlive = true;

        }

    }

    private void ResetRotation() //para que no choque con la rotacion de la animacion de transformacion (Dash) y quede el personaje rotado
    {
        girlTransform.rotation = Quaternion.identity; // Resetea la rotación a (0,0,0)
        catTransform.rotation = Quaternion.identity; // Resetea la rotación a (0,0,0)

    }
}
