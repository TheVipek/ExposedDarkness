using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] GameObject weaponPrefab;
    public string WeaponName{get{return weaponPrefab.name;}}
    [SerializeField] WeaponRole weaponRole;
    [SerializeField] GameObject weaponToReplace;
    public void GetWeapon()
    {
//        Debug.Log(WeaponSwitcher.Instance.AllWeapons.Count);
        foreach (Weapon item in WeaponsManager.Instance.AllWeapons)
        {
           // Debug.Log(item.name);
            if(item.WeaponRole == weaponRole && item != null)
            {
//                Debug.Log(item.WeaponRole);
                //Debug.Log("Replace Item");
                ReplaceWeapon(item.gameObject);


                // So other weapons won't be checked
                return;
            }
        }
        //Debug.Log("Set weapon");
        SetWeapon(WeaponsManager.Instance.weaponSwitcher.CurrentWeapon.transform);
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
     //   Debug.Log($"weaponParent:{weaponPrefab.transform.parent}");
      //  Debug.Log($"posToSet parent : {posToSet.transform.parent}");
        weaponPrefab.transform.SetParent(posToSet.transform.parent);
      //  Debug.Log($"weaponParent:{weaponPrefab.transform.parent}");
        weaponPrefab.transform.localPosition = weaponPrefabPosition;
  //      Debug.Log(weaponPrefab.transform.localPosition);
        weaponPrefab.transform.localRotation = Quaternion.Euler(0,weaponPrefab.transform.localRotation.y,0); 
        foreach(Behaviour comp in weaponPrefab.GetComponents<Behaviour>())
        {
            comp.enabled = true;
        }
        if(idxToSet == -1)
        {
            idxToSet = (int)weaponRole;
            // 0 for primary , 1 for secondary , 2 for teritiary
            weaponPrefab.transform.SetSiblingIndex(idxToSet);

        }else
        {
           // Debug.Log(idxToSet);
            weaponPrefab.transform.SetSiblingIndex(idxToSet);
            PutOnGround();

        }
        

        WeaponsManager.Instance.onWeaponPickup();
        WeaponsManager.Instance.weaponSwitcher.weaponChange(idxToSet);
    }
       
    public void PutOnGround()
    {
        foreach(Behaviour comp in weaponToReplace.GetComponents<Behaviour>())
        {
        //    Debug.Log(comp);
            comp.enabled = false;
        }
        if(!weaponToReplace.activeSelf) weaponToReplace.SetActive(true);
        weaponToReplace.transform.SetParent(gameObject.transform);
        weaponToReplace.transform.localPosition = new Vector3(0,0,0);
        weaponToReplace.transform.rotation = Quaternion.Euler(0,0,0);
        weaponPrefab = weaponToReplace;

    }
}
