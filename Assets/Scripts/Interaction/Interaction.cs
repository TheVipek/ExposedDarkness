using UnityEngine;


public class Interaction : MonoBehaviour,IResponseInteraction
{
    public static Interaction Instance { get; private set; }
    [SerializeField] GameObject interactionUI;
    [SerializeField] Camera playerViewCamera;
    [SerializeField] float interactionDistance;
    bool interactionActivated = false;
    public GameObject lookingAt = null;
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
        interactionUI.SetActive(true);
        lookingAt = _lookingAt;
        interactionActivated = true;
    }
    public void OnDeselect()
    {
        interactionUI.SetActive(false);
        lookingAt = null;
        interactionActivated = false;
    }
}
