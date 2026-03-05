using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public partial class PlateProvider : MonoBehaviour
{
    public GameObject platePrefab; 
    public Transform spawnPoint;   // El objeto vacío que marca dónde aparece el plato

    private void Start()
    {
        SpawnNewPlate(); // Creamos el primer plato al empezar
    }

    public void SpawnNewPlate()
    {
        if (platePrefab != null && spawnPoint != null)
        {
            // Creamos el plato en el soporte
            GameObject newPlate = Instantiate(platePrefab, spawnPoint.position, spawnPoint.rotation);
            
            // Obtenemos su componente de agarre
            UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabScript = newPlate.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

            if (grabScript != null)
            {
                // Cuando el jugador suelte el plato (ya se lo llevó), creamos otro
                grabScript.selectExited.AddListener(OnPlateTaken);
            }
        }
    }

    private void OnPlateTaken(SelectExitEventArgs args)
    {
        // El plato ya no está en el soporte, llamamos al creador
        SpawnNewPlate();
    }
}