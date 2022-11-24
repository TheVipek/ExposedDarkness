using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class Pickup : MonoBehaviour
{   
    public TMP_Text pickupDescription;
    [SerializeField] Animator pickupAnimator;

    public void disableObject()
    {
        gameObject.SetActive(false);
    }
}
