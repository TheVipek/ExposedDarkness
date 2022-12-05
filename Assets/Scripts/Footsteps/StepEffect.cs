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
    [SerializeField] float distanceBetweenStep = 1f;
    Vector3 lastStep;
    private PlayerMovement playerMovement;
    private void Awake()
    {
        terrainGroundDetector = new TerrainGroundDetector();
        lastStep = new Vector3(transform.position.x,transform.position.y,transform.position.z);
    }
    private void Start() {
        playerMovement = PlayerMovement.Instance;
        
    }
    private void Update() {

        if(Vector3.Distance(lastStep,transform.position) > distanceBetweenStep)
        {
            //Debug.Log(Vector3.Distance(lastStep,transform.position) + ", " + distanceBetweenStep);
            //Debug.Log(PlayerMovement.instance.IsGrounded);
            if(playerMovement.IsGrounded == true)
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
//                Debug.Log(terrainTextureIndex);
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

            if(hit.transform.TryGetComponent(out groundType _groundType) != false)
            {
                return _groundType.footstepsCollection.getRandomClip();
            }
            
        }
        return null;
    }
    

}
