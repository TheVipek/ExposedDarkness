using UnityEngine;


public class Interaction : MonoBehaviour,IResponseInteraction
{
    public static Interaction Instance { get; private set; }
    public GameObject interactionUI;
    [SerializeField] Camera playerViewCamera;
    [SerializeField] float interactionDistance;
    bool interactionActivated = false;
    internal GameObject lookingAt = null;
    Ray ray;
    RaycastHit hit;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        ray = playerViewCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        if (Physics.Raycast(ray, out hit, maxDistance: interactionDistance))
        {
            if (hit.transform.gameObject.CompareTag("interactionObject") && interactionActivated == false)
            {
                Debug.Log("You're looking at interactable object!");
                OnSelect(hit.transform.gameObject);
            }

        }
        else
        {
            if (interactionActivated == true)
            {
                OnDeselect();
            }
        }
    }

    public void OnSelect(GameObject _lookingAt)
    {
        lookingAt = _lookingAt;
        interactionActivated = true;
        interactionUI.SetActive(true);
    }
    public void OnDeselect()
    {
        lookingAt = null;
        interactionActivated = false;
        interactionUI.SetActive(false);
    }
}
