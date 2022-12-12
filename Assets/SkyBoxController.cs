using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxController : MonoBehaviour
{
    Material skyboxMaterial;
    private float startRotation;
    private int rotationProp;
    [SerializeField] float rotateSpeed;
    private void Start() {
        rotationProp = Shader.PropertyToID("_Rotation");
        skyboxMaterial = RenderSettings.skybox;
        startRotation = skyboxMaterial.GetFloat(rotationProp);
    }
    void Update()
    {
        skyboxMaterial.SetFloat(rotationProp,Time.time * rotateSpeed);
    }
}
