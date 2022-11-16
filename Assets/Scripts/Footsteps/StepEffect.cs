using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepEffect : MonoBehaviour
{
    [SerializeField] List<AudioClip> groundSounds;
    [SerializeField] List<AudioClip> rockSounds;
    [SerializeField] AudioSource audioSource;
    private TerrainGroundDetector terrainGroundDetector;
    [SerializeField] float distanceBetweenStep = 1f;
    Vector3 lastStep;
    void Awake()
    {
        terrainGroundDetector = new TerrainGroundDetector();
        lastStep = new Vector3(transform.position.x,transform.position.y,transform.position.z);
    }
    private void Update() {

        if(Vector3.Distance(lastStep,transform.position) > distanceBetweenStep)
        {
            //Debug.Log(Vector3.Distance(lastStep,transform.position) + ", " + distanceBetweenStep);
            //Debug.Log(PlayerMovement.instance.IsGrounded);
            if(PlayerMovement.instance.IsGrounded == true)
            {
                StepSound();
                lastStep = transform.position;
            }
        }
        
    }
    private void StepSound()
    {
        //Debug.Log("playing step sound!");
        AudioClip clipToPlay = getClip();
        audioSource.PlayOneShot(clipToPlay,0.2f);
    }
    private AudioClip getClip()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position,Vector3.down,out hit,2))
        {

            if(hit.transform.GetComponent<Terrain>() != null)
            {
                int terrainTextureIndex = terrainGroundDetector.GetTerrainAtPosition(transform.position);
                switch (terrainTextureIndex)
                    {
                        case <=3:
                        default:
                            return groundSounds[Random.Range(0,groundSounds.Count)];
                        
                    }
            }

            if(hit.transform.TryGetComponent(out groundType _groundType) != false)
            {
                return _groundType.footstepsCollection.getRandomClip();
            }
            
        }
        return null;
    }
    

}
