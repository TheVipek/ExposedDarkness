using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShootingTypeChanger : MonoBehaviour
{
    public delegate void OnChangeShootingType();
    public static event OnChangeShootingType onChangeShootingType;
    [SerializeField] KeyCode typeChanger = KeyCode.B;
    [Header("Audio")]
    [SerializeField] string bulletTypeChange; 
    public string BulletTypeChange {get{return bulletTypeChange;}}
    static WeaponShootingTypeChanger instance;
    public static WeaponShootingTypeChanger Instance{get{return instance;}} 
    private void Awake() {
        if(instance!=null && instance !=this)
        {
            Destroy(this.gameObject);
        }else
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(typeChanger))
        {
            onChangeShootingType();
        }
    }
}
