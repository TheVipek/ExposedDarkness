using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShootingTypeChanger : MonoBehaviour
{
    public delegate void OnChangeShootingType();
    public static event OnChangeShootingType onChangeShootingType;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("changing shooting type");
            onChangeShootingType();
        }
    }
}
