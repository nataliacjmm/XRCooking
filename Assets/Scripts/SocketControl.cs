using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketControl : MonoBehaviour
{
    public GameObject socketPile; // Arrastra aquí el objeto hijo 'SocketPile'
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    
        // Añadimos esta comprobación de seguridad
        if (socketPile != null)
        {
            socketPile.SetActive(false);
        }
        else 
        {
            Debug.LogError("¡Oye! Te olvidaste de arrastrar el SocketPile en el Inspector del Prefab: " + gameObject.name);
        }

        // Cuando soltamos el plato...
        grabInteractable.selectExited.AddListener(OnPlateDropped);
        // Cuando cogemos el plato...
        grabInteractable.selectEntered.AddListener(OnPlatePickedUp);
    }

    private void OnPlateDropped(SelectExitEventArgs args)
    {
        // Si el plato se ha soltado en un Socket (el de la mesa), encendemos el imán de comida
        if (args.isCanceled == false) 
        {
            socketPile.SetActive(true);
        }
    }

    private void OnPlatePickedUp(SelectEnterEventArgs args)
    {
        // Al cogerlo de la mesa, apagamos el imán para que no se peguen cosas por el camino
        socketPile.SetActive(false);
    }
}