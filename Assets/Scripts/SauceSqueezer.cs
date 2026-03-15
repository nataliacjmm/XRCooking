using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SauceSqueezer : MonoBehaviour
{
    [Header("Configuration de la Sauce")]
    [SerializeField] private ParticleSystem sauceParticles;
    [SerializeField] private float maxEmissionRate = 50f;

    private XRGrabInteractable _grabInteractable;

    void Awake()
    {
        // On récupère automatiquement le composant Grab sur la bouteille
        _grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        // On vérifie si la bouteille est actuellement tenue par le joueur
        if (_grabInteractable.isSelected)
        {
            // On récupère le premier interrupteur (la main) qui tient la bouteille
            var interactor = _grabInteractable.firstInteractorSelecting;

            // On récupère la valeur de pression de la gâchette (Trigger)
            float squeezeValue = GetSqueezeForce(interactor);

            // On met à jour le débit de particules
            UpdateSauceEmission(squeezeValue);
        }
        else
        {
            // Si on lâche la bouteille, on arrête la sauce immédiatement
            UpdateSauceEmission(0f);
        }
    }

    private void UpdateSauceEmission(float value)
    {
        if (sauceParticles == null) return;

        var emission = sauceParticles.emission;
        // On multiplie la pression (0 à 1) par le débit max souhaité
        emission.rateOverTime = value * maxEmissionRate;
    }

    private float GetSqueezeForce(IXRSelectInteractor interactor)
    {
        // Dans Unity 6 / XRI 3.0, on accède à l'état de l'action "Activate" (la gâchette)
        if (interactor is XRBaseInputInteractor inputInteractor)
        {
            return inputInteractor.xrController.activateInteractionState.value;
        }
        return 0f;
    }
}