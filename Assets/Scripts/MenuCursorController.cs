using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCursorController : MonoBehaviour
{
    private void OnEnable() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    private void OnDisable() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
