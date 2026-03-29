using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DynamicStacking : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket;
    public Transform attachPoint; 
    public float extraSpacing = 0.01f; // Un pequeño margen para que no se traspasen

    void Awake() => socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
    void OnEnable() => socket.selectEntered.AddListener(OnIngredientAdded);
    void OnDisable() => socket.selectEntered.RemoveListener(OnIngredientAdded);

    private void OnIngredientAdded(SelectEnterEventArgs args)
    {
        GameObject ingredient = args.interactableObject.transform.gameObject;

        // 1. Congelamos las físicas para que la torre sea estable
        if (ingredient.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }

        // 2. Calculamos la altura real del ingrediente
        float height = 0.02f; // Altura por defecto mínima
        if (ingredient.TryGetComponent<Collider>(out Collider col))
        {
            height = col.bounds.size.y; // Medimos el tamaño real del objeto
        }

        // 3. Subimos el punto de pegado exactamente esa altura
        if (attachPoint != null)
        {
            attachPoint.localPosition += new Vector3(0, height + extraSpacing, 0);
        }
    }
}