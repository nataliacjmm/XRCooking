using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
// Usamos el namespace de interactables para acortar el código
using UnityEngine.XR.Interaction.Toolkit.Interactables; 

public class PlateProvider : MonoBehaviour
{
    public GameObject platePrefab;
    public Transform spawnPoint;
    private bool isQuitting = false;

    private void Start()
    {
        SpawnNewPlate();
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    public void SpawnNewPlate()
    {
        if (isQuitting || platePrefab == null || spawnPoint == null) return;

        GameObject newPlate = Instantiate(platePrefab, spawnPoint.position, spawnPoint.rotation);
        XRGrabInteractable grabScript = newPlate.GetComponent<XRGrabInteractable>();

        if (grabScript != null)
        {
            // CAMBIO: Usamos selectEntered (cuando lo COGEN) en lugar de selectExited
            grabScript.selectEntered.AddListener(OnPlateTaken);
        }
    }

    private void OnPlateTaken(SelectEnterEventArgs args)
    {
        if (!isQuitting)
        {
            // IMPORTANTE: Quitamos el listener para que este plato ya "usado" 
            // no vuelva a llamar a esta función si el jugador lo suelta y lo vuelve a coger.
            XRGrabInteractable grab = args.interactableObject.transform.gameObject.GetComponent<XRGrabInteractable>();
            grab.selectEntered.RemoveListener(OnPlateTaken);

            SpawnNewPlate();
        }
    }
}