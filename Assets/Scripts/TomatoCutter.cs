using UnityEngine;


public class TomatoCutter : MonoBehaviour
{
    [Header("Cutting Settings")]
    public Transform bladeEdge; // The Empty object at the bottom of the blade
    public float rayDistance = 0.15f;
    public GameObject tomatoSlicesPrefab; // The sliced tomato model

    private bool isCutting = false;
    private GameObject targetTomato;
    private Vector3 initialCutPosition;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    void Update()
    {
        if (!isCutting)
        {
            DetectTomato();
        }
        else
        {
            HandleCuttingMovement();
        }
    }

    void DetectTomato()
    {
        RaycastHit hit;
        // Shoot ray downwards from the blade edge
        if (Physics.Raycast(bladeEdge.position, Vector3.down, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("Cutable"))
            {
                StartCutting(hit.collider.gameObject);
            }
        }
    }

    void StartCutting(GameObject tomato)
    {
        isCutting = true;
        targetTomato = tomato;
        initialCutPosition = transform.position;
        
        Debug.Log("Tomato detected. Locking X/Z axis for vertical cut.");
    }

    void HandleCuttingMovement()
    {
        // Lock X and Z so the knife only moves up/down (Y axis)
        float currentY = transform.position.y;
        transform.position = new Vector3(initialCutPosition.x, currentY, initialCutPosition.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If we are in cutting mode and hit the board, finish the cut
        if (isCutting && collision.gameObject.CompareTag("CuttingBoard"))
        {
            FinishCut();
        }
    }

    void FinishCut()
    {
        // Swap models
        Instantiate(tomatoSlicesPrefab, targetTomato.transform.position, targetTomato.transform.rotation);
        Destroy(targetTomato);
        
        isCutting = false;
        Debug.Log("Cut complete!");
    }
}