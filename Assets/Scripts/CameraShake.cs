using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance; //puse un puglin DOTWEENS para el camara shake

    private void Awake() => Instance = this; // instanciamos

    private void Start()
    {
        DOTween.Init();
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(1250, 50);
    }

    public void OnShake(float duration, float strength) // se encarga de hacer el shake
    {
        transform.DOShakePosition(duration, strength);
        transform.DOShakeRotation(duration, strength);
    }

    public static void Shake(float duration, float strength) => Instance.OnShake(duration, strength);
}
