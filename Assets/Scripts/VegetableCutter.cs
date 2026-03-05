using UnityEngine;

public class VegetableCutter : MonoBehaviour
{
    public GameObject smokeEffectPrefab; 

    private void OnTriggerEnter(Collider other)
    {
        // 1. Verificamos si es algo cortable
        if (other.CompareTag("Cutable"))
        {
            // 2. Intentamos obtener sus datos de rodajas
            IngredientData data = other.GetComponent<IngredientData>();
            
            if (data != null && data.slicesPrefab != null)
            {
                EjecutarCorte(other.gameObject, data.slicesPrefab);
            }
        }
    }

    void EjecutarCorte(GameObject vegetalOriginal, GameObject prefabAUsar)
    {
        Vector3 pos = vegetalOriginal.transform.position;
        Quaternion rot = vegetalOriginal.transform.rotation;

        // Efecto de humo
        if (smokeEffectPrefab != null) Instantiate(smokeEffectPrefab, pos, rot);

        // Creamos las rodajas específicas (cebolla, lechuga, etc.)
        GameObject grupoRodajas = Instantiate(prefabAUsar, pos, rot);

        // Liberamos las rodajas hijas para que sean individuales como hicimos con el tomate
        Rigidbody[] hijos = grupoRodajas.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in hijos)
        {
            rb.transform.SetParent(null);
            rb.AddExplosionForce(100f, pos, 0.2f);
        }

        Destroy(grupoRodajas);
        Destroy(vegetalOriginal);
    }
}