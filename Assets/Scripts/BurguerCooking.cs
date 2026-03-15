using UnityEngine;
using System.Collections;

public class BurgerCooking : MonoBehaviour
{
    [Header("Configuración de Tiempos")]
    public float timeToCook = 10f;
    public float timeToBurn = 10f;

    [Header("Estados de la Hamburguesa")]
    public GameObject cookedPrefab;
    public GameObject burntPrefab;

    private bool isOnPan = false;
    private float timer = 0f;
    private bool isCooked = false;
    private bool isBurnt = false;

    void Update()
    {
        if (isOnPan && !isBurnt)
        {
            timer += Time.deltaTime;

            // Fase 1: De cruda a cocinada
            if (!isCooked && timer >= timeToCook)
            {
                ChangeState(cookedPrefab);
                isCooked = true;
                timer = 0; // Reiniciamos el timer para la fase de quemado
                Debug.Log("¡Hamburguesa cocinada!");
            }
            // Fase 2: De cocinada a quemada
            else if (isCooked && timer >= timeToBurn)
            {
                ChangeState(burntPrefab);
                isBurnt = true;
                Debug.Log("¡Se ha quemado!");
            }
        }
    }

    private void ChangeState(GameObject nextStatePrefab)
    {
        if (nextStatePrefab == null) return;

        // Instanciamos el nuevo estado en la misma posición y rotación
        GameObject nextStage = Instantiate(nextStatePrefab, transform.position, transform.rotation);
        
        // Si la hamburguesa está en un plato (socket), esto ayuda a que el nuevo objeto se mantenga
        nextStage.transform.parent = transform.parent;

        // Destruimos la versión actual
        Destroy(gameObject);
    }

    // Detectar la sartén
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sarten")) // Asegúrate de que tu sartén tenga el Tag "Sarten"
        {
            isOnPan = true;
            Debug.Log("Empezando a cocinar...");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Sarten"))
        {
            isOnPan = false;
            Debug.Log("Cocinado pausado (fuera de la sartén)");
        }
    }
}