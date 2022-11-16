using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    [Range(-1,1)]
    [SerializeField] float showWeaponOffset = -0.5f;
    WeaponZoom weaponZoom;
    Vector3 desiredPosition;
    float timeToAccomplish = 0.1f;  
    Animator animator;
    void Awake()
    {
        weaponZoom = GetComponent<WeaponZoom>();
        desiredPosition = weaponZoom.defaultWeaponPosition;
        animator = GetComponent<Animator>();
    }
    private void OnEnable() {
        Debug.Log(animator);
        animator.enabled = false;
        Debug.Log(animator.isActiveAndEnabled);
        StartCoroutine(showWeapon(desiredPosition));
        transform.localRotation = Quaternion.Euler(0,0,0);

        //Debug.Log("Enabling");
    }

    // Update is called once per frame
    void Update()
    {

        if(PlayerMovement.instance.Moving == true)
        {
            animator.SetBool("moving" , true);
        }else
        {
            animator.SetBool("moving" , false);
        }
        if(PlayerMovement.instance.Sprinting == true)
        {
            animator.SetBool("sprinting" , true);
        }else
        {
            if(animator.GetBool("sprinting") == true)
            {
                animator.SetBool("sprinting" , false);
            }
        }
    }
    
    public IEnumerator showWeapon(Vector3 desiredVal)
    {
        float timeToElapse = 0f;
        Vector3 desiredValue = desiredVal;
        transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y + showWeaponOffset,transform.localPosition.z);
        while(timeToElapse < timeToAccomplish)
        {
            transform.localPosition = new Vector3 (transform.localPosition.x,Mathf.Lerp(transform.localPosition.y,desiredValue.y,timeToElapse/timeToAccomplish),transform.localPosition.z);
            timeToElapse+=Time.deltaTime;
            yield return null;
        }
        transform.localPosition = new Vector3(transform.localPosition.x,desiredValue.y,transform.localPosition.z);
        animator.enabled = true;
    }

}
