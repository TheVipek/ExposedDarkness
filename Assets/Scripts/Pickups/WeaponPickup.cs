using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] GameObject weaponPrefab;
    [SerializeField] WeaponRole weaponRole;
    [SerializeField] GameObject weaponToReplace;
    public void GetWeapon()
    {
//        Debug.Log(WeaponSwitcher.Instance.AllWeapons.Count);
        foreach (Weapon item in WeaponSwitcher.Instance.AllWeapons)
        {
            Debug.Log(item.name);
            if(item.WeaponRole == weaponRole)
            {
                Debug.Log(item.WeaponRole);
                // Replace Item;
                ReplaceWeapon(item.gameObject);


                // So other weapons won't be checked
                return;
            }
        }
        SetWeapon(WeaponSwitcher.Instance.CurrentWeapon.transform);
    }
    public void ReplaceWeapon(GameObject _weaponToReplace)
    {
        //Weapon that player is using now
        weaponToReplace = _weaponToReplace;
        Weapon weapon = weaponToReplace.GetComponent<Weapon>();
        int idxToSet = (int)weapon.WeaponRole;

        SetWeapon(_weaponToReplace.transform,idxToSet);
        
    }
    public void SetWeapon(Transform posToSet,int idxToSet = -1)
    {
        Vector3 weaponPrefabPosition = weaponPrefab.GetComponent<WeaponAnimation>().defaultWeaponPosition; 

       // Debug.Log(weaponPrefabPosition);
        weaponPrefab.transform.parent = posToSet.transform.parent;
        weaponPrefab.transform.localPosition = weaponPrefabPosition;
      //  Debug.Log(weaponPrefab.transform.localPosition);
        weaponPrefab.transform.localRotation = Quaternion.Euler(0,weaponPrefab.transform.localRotation.y,0); 
        foreach(Behaviour comp in weaponPrefab.GetComponents<Behaviour>())
        {
            comp.enabled = true;
        }
        if(idxToSet == -1)
        {
            // 0 for primary , 1 for secondary , 2 for teritiary
            weaponPrefab.transform.SetSiblingIndex((int)weaponRole);
        }else
        {
           // Debug.Log(idxToSet);
            weaponPrefab.transform.SetSiblingIndex(idxToSet);
            PutOnGround();

        }
        WeaponSwitcher.Instance.getCurrentWeapon();
    }
       
    public void PutOnGround()
    {

        foreach(Behaviour comp in weaponToReplace.GetComponents<Behaviour>())
        {
            comp.enabled = false;
        }
        weaponToReplace.transform.SetParent(gameObject.transform);
        weaponToReplace.transform.localPosition = new Vector3(0,0,0);
        weaponToReplace.transform.rotation = Quaternion.Euler(0,0,0);
        weaponPrefab = weaponToReplace;

    }
}
