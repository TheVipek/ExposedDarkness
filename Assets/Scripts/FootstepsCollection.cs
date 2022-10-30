using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "FootstepsCollection", menuName = "ZombieFPS/FootstepsCollection", order = 0)]
public class FootstepsCollection : ScriptableObject {
    public List<AudioClip> footstepsList;
    
    public AudioClip getRandomClip()
    {
        return footstepsList[Random.Range(0,footstepsList.Count)];
    }
}

