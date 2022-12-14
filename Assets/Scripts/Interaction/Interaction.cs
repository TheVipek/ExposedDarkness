using UnityEngine;


public class Interaction : MonoBehaviour,IResponseInteraction
{
    [SerializeField] GameObject interactionUI;
    [SerializeField] interactionListener listener;
    [SerializeField] Camera playerViewCamera;
    [SerializeField] float interactionDistance;
    private bool interactionActivated = false; 
  //  [HideInInspector] public GameObject lookingAt = null;
  //  [HideInInspector] public GameObject lastInteracted = null;
    Ray ray;
    RaycastHit hit;



    void Update()
    {
        ray = playerViewCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        if (Physics.Raycast(ray, out hit, maxDistance: interactionDistance))
        {
            // Debug.DrawLine(ray.origin,hit.point,Color.red);
            if (hit.transform.gameObject.CompareTag("interactionObject"))
            {
                if(interactionActivated == false)
                {
                    OnSelect(hit.transform.gameObject);
                }
            }
            else
            {
                if(interactionActivated == true) OnDeselect();
            }

        }
        else
        {
            if(interactionActivated == true) OnDeselect();
        }
    }

    public void OnSelect(GameObject _lookingAt)
    {
        //lookingAt = _lookingAt;
        listener.SetLookingAt(_lookingAt);
        interactionActivated = true;
        interactionUI.SetActive(true);
    }
    public void OnDeselect()
    {
        listener.SetLookingAt(null);
        interactionActivated = false;
        interactionUI.SetActive(false);
    }
    // public void setLastInteracted()
    // {
    //     lastInteracted = lookingAt;
    // }
}
