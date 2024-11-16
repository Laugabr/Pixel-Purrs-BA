using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static CharacterHealth;

public class elTexto : MonoBehaviour
{
    public TextMeshProUGUI muertes;
    void Update()
    {
        int score = SceneData.muertes;
        muertes.SetText("MUERTES: " + score);
    }
}
