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
    public bool girlIsActive;
    CharacterMovement charMovement;
    private float dashDuration;
    public CharacterMovement finishedDashing;
    public CharacterMovement startedDash;
    public Transform girlTransform;
    public Transform catTransform;
    // Update is called once per frame
    private void Start()
    {
        girlControler.enabled = true;
        catControler.enabled = false;
        girlIsActive = true;
        girl.SetActive(true);
        cat.SetActive(false);
    }

   /* private IEnumerator ChangeCharacter()
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("corutina");
        SwitchCharacter();
    }
   */
    private void Update()
    {
        if (startedDash)
        {
            //tartCoroutine(ChangeCharacter());
            SwitchCharacter();  
            Debug.Log("se llama a la corutina");
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
        }
        else
        {
            girlControler.enabled = true;
            catControler.enabled = false;
            girlIsActive = true;
            girl.SetActive(true);
            cat.SetActive(false);
            ResetRotation();
        }
 
    }


    private void ResetRotation()
    {
        girlTransform.rotation = Quaternion.identity; // Resetea la rotación a (0,0,0)
        catTransform.rotation = Quaternion.identity; // Resetea la rotación a (0,0,0)

    }
}
