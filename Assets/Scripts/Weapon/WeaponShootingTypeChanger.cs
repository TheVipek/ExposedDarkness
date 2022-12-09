using UnityEngine;
using System;
using UnityEngine.InputSystem;
public class WeaponShootingTypeChanger : MonoBehaviour
{
    public static Action onChangeShootingType;
    public InputActionReference shootingTypeAction;

    void Update()
    {
        if(shootingTypeAction.action.WasPerformedThisFrame())
        {
            onChangeShootingType();
        }
    }
}
