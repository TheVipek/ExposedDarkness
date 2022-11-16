using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class WeaponShootingTypeChanger : MonoBehaviour
{
    public static Action onChangeShootingType;
    [SerializeField] KeyCode typeChanger = KeyCode.B;
    void Update()
    {
        if(Input.GetKeyDown(typeChanger))
        {
            onChangeShootingType();
        }
    }
}
