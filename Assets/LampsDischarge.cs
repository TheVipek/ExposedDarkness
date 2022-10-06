using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampsDischarge : MonoBehaviour
{
    [SerializeField] List<GameObject> lights;
    bool deactivated = false;
    void Start()
    {
        
    }

    void lampDischarge()
    {
        for (int i = 0; i < lights.Count; i++)
        {
            lights[i].GetComponent<BlinkingLamp>().enabled = false;
            lights[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other) {
         if(deactivated == false)
        {
            if(other.gameObject.tag == "Player")
            {
                lampDischarge();
                deactivated = true;
            }
        }
    }
}
