using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SauceSqueezer : MonoBehaviour
{
    [Header("Sauce Settings")]
    public ParticleSystem sauceParticles;
    public float maxEmissionRate = 50f;

    private XRGrabInteractable _grab;

    void Awake()
    {
        _grab = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        if (_grab == null || sauceParticles == null) return;

        // Check if the bottle is currently being held by the player
        if (_grab.isSelected)
        {
            // Check for simulator mouse click
            bool isMousePressed = Mouse.current != null && Mouse.current.leftButton.isPressed;
            
            // Get the actual trigger pressure from the VR controller
            float force = GetForce(_grab.firstInteractorSelecting);

            // Simulator fallback: if held but no controller input, use mouse click
            if (isMousePressed && force == 0)
            {
                force = 1f;
            }
            
            UpdateSauceEmission(force);
        }
        else
        {
            // If the bottle is NOT held, force emission to stop immediately
            UpdateSauceEmission(0f);
        }
    }

    private void UpdateSauceEmission(float value)
    {
        var emission = sauceParticles.emission;
        emission.rateOverTime = value * maxEmissionRate;
    }

    private float GetForce(IXRSelectInteractor interactor)
    {
        if (interactor is XRBaseInputInteractor inputInteractor && inputInteractor.xrController != null)
        {
            return inputInteractor.xrController.activateInteractionState.value;
        }
        return 0f;
    }
}