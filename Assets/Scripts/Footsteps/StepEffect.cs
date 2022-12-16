using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepEffect : MonoBehaviour
{
    [SerializeField] List<AudioClip> groundSounds;
    [SerializeField] List<AudioClip> rockSounds;
    [SerializeField] List<AudioClip> gravelSounds;
    [SerializeField] List<AudioClip> grassSounds;
    [SerializeField] AudioSource audioSource;
    private TerrainGroundDetector terrainGroundDetector;
    [SerializeField] float distanceBetweenStep;
    private float distanceTraveledSinceStep;
    Vector3 previousPosition;
    Vector3 lastStep;
    [SerializeField] PlayerMoveSettings playerSettings;
    [SerializeField] LayerMask layerToIgnore;
    private void Awake()
    {
        previousPosition = transform.position;
        terrainGroundDetector = new TerrainGroundDetector();
        lastStep = new Vector3(transform.position.x,transform.position.y,transform.position.z);
    }
    private void Update() 
    {
        if(previousPosition != transform.position)
        {
            distanceTraveledSinceStep += Vector3.Distance(previousPosition,transform.position);
            previousPosition = transform.position;
            if(distanceTraveledSinceStep >= distanceBetweenStep)
            {
                if(playerSettings.MovementActions != MovementActions.JUMPING)
                {
                    StepSound();
                    distanceTraveledSinceStep = 0;
                }
            }
        }
        // if(Vector3.Distance(lastStep,transform.position) > distanceBetweenStep)
        // {
        //     if(playerSettings.MovementActions != MovementActions.JUMPING)
        //     {
        //         StepSound();
        //         lastStep = transform.position;
        //     }
        // }
        
    }
    private void StepSound()
    {
        //Debug.Log("playing step sound!");
        AudioClip clipToPlay = getClip();
        audioSource.PlayOneShot(clipToPlay,0.2f);
    }
    private AudioClip getClip()
    {
     //   Debug.Log($"Trying to get random clip");
        RaycastHit hit;
        Debug.DrawRay(transform.position,Vector3.down*.5f,Color.red,15f);
        if(Physics.Raycast(transform.position,Vector3.down,out hit,.5f,~layerToIgnore))
        {
       //     Debug.Log($"Ray touched something {hit.collider.gameObject.name}");
            if(hit.transform.GetComponent<Terrain>() != null)
            {
                int terrainTextureIndex = terrainGroundDetector.GetTerrainAtPosition(transform.position);
//                Debug.Log($"Terrain: {terrainTextureIndex}");

                switch (terrainTextureIndex)
                    {
                        case 0:
                            return groundSounds[Random.Range(0,groundSounds.Count)];
                        case 1:
                            return rockSounds[Random.Range(0,rockSounds.Count)];
                        case 2:
                            return gravelSounds[Random.Range(0,gravelSounds.Count)];
                        case 3:
                            return grassSounds[Random.Range(0,grassSounds.Count)];
                        default:
                            return groundSounds[Random.Range(0,groundSounds.Count)];
                        
                    }
            }
            else if(hit.transform.TryGetComponent(out groundType _groundType) != false)
            {
        //        Debug.Log($"Not terrain: {_groundType}");
                return _groundType.footstepsCollection.getRandomClip();
            }
            
        }
        return null;
    }
    

}
