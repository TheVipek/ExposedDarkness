using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    [Range(-1,1)]
    [SerializeField] float showWeaponOffset = -0.5f;
    WeaponZoom weaponZoom;
    Vector3 desiredPosition;

    [Range(-1,1)]
    [SerializeField] float movementXRange,movementYRange,movementZRange;
    [Range(-15f,15f)]
    [SerializeField] float rotationXRange;
    float timeToAccomplish = 0.075f;
    [SerializeField] float sprintWeaponMoving;
    [SerializeField] float normalWeaponMoving;
    Vector3 velocity;
    Animator animator;
    bool isAnimating = false;
    void Awake()
    {
        weaponZoom = GetComponent<WeaponZoom>();
        desiredPosition = weaponZoom.weaponZoomOutPosition;
        animator = GetComponent<Animator>();
    }
    private void OnEnable() {
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
                // transform.localRotation = Quaternion.Euler(0,0,0);
               // transform.localPosition = desiredPosition;


            }
        }

        
       /* if(PlayerMovement.instance.Moving == true && isAnimating == false && weaponZoom.isZoomed == false )
        {
            movementYRange = -movementYRange;
            movementXRange = -movementXRange;
            movementZRange = -movementZRange;
            if(PlayerMovement.instance.Sprinting == true)
            {
                timeToAccomplish = sprintWeaponMoving;
            }else
            {
                timeToAccomplish = normalWeaponMoving;
            }
            StartCoroutine(moveWeapon(movementXRange,movementYRange,movementZRange));
        }
        */
    }
    
    public IEnumerator showWeapon(Vector3 desiredVal)
    {
        isAnimating = true;
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
        isAnimating = false;
    }
    public IEnumerator moveWeapon(float XMovement,float YMovement,float ZMovement)
    {
        isAnimating = true;
        float timeToElapse = 0f;
        Vector3 desiredWeaponPosition = new Vector3(transform.localPosition.x + XMovement,transform.localPosition.y + YMovement,transform.localPosition.z +ZMovement);
        while(timeToElapse < timeToAccomplish && (PlayerMovement.instance.Moving == true && weaponZoom.isZoomed == false))
        {
            float t = timeToElapse/timeToAccomplish;
            t = t*t;
            transform.localPosition = Vector3.Lerp(transform.localPosition,desiredWeaponPosition,t);
            timeToElapse+=Time.deltaTime;
            yield return null;
        }
        isAnimating = false;
        transform.localPosition = desiredWeaponPosition;
    }

}
