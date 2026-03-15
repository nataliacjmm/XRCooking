using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ResetIngredientPhysics : MonoBehaviour
{
    private Rigidbody rb;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    void OnEnable() => grab.selectEntered.AddListener(OnGrab);
    void OnDisable() => grab.selectEntered.RemoveListener(OnGrab);

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (rb != null)
        {
            // En cuanto lo agarras, "descongelamos" el objeto
            rb.isKinematic = false; 
            rb.useGravity = true; 
        }
    }
}