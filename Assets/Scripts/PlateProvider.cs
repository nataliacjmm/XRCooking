using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlateProvider : MonoBehaviour
{
    public GameObject platePrefab;
    public Transform spawnPoint;
    private bool isQuitting = false; // Nueva variable para controlar el cierre

    private void Start()
    {
        SpawnNewPlate();
    }

    // Unity llama a esto automáticamente cuando pulsas Stop o cierras la app
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    public void SpawnNewPlate()
    {
        // Si el juego se está cerrando, salimos de la función sin crear nada
        if (isQuitting || platePrefab == null || spawnPoint == null) return;

        GameObject newPlate = Instantiate(platePrefab, spawnPoint.position, spawnPoint.rotation);
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabScript = newPlate.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (grabScript != null)
        {
            grabScript.selectExited.AddListener(OnPlateTaken);
        }
    }

    private void OnPlateTaken(SelectExitEventArgs args)
    {
        // Solo creamos un plato nuevo si NO nos estamos saliendo del juego
        if (!isQuitting)
        {
            SpawnNewPlate();
        }
    }
}