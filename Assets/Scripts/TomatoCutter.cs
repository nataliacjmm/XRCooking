using UnityEngine;
using System.Collections.Generic; // Para que funcionen las listas
using System.Linq;                // Para que funcione el .Cast y .ToList

public class TomatoCutter : MonoBehaviour
{
    [Header("Configuración de Efectos")]
    public GameObject tomatoSlicesPrefab; 
    public GameObject smokeEffectPrefab;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cutable"))
        {
            EjecutarCorteInmediato(other.gameObject);
        }
    }

    void EjecutarCorteInmediato(GameObject tomateOriginal)
    {
        Vector3 pos = tomateOriginal.transform.position;
        Quaternion rot = tomateOriginal.transform.rotation;

        if (smokeEffectPrefab != null) 
            Instantiate(smokeEffectPrefab, pos, rot);

        if (tomatoSlicesPrefab != null)
        {
            GameObject grupoRodajas = Instantiate(tomatoSlicesPrefab, pos, rot);

            // Opción más segura y simple que no da errores:
            // Buscamos todos los Rigidbody en los hijos y los "liberamos"
            Rigidbody[] hijos = grupoRodajas.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rb in hijos)
            {
                rb.transform.SetParent(null); // Ahora cada rodaja es independiente
                
                // Les damos un pequeño empujón para que no se atraviesen con el cuchillo
                rb.AddExplosionForce(100f, pos, 0.2f);
            }

            // Destruimos el contenedor vacío que quedó
            Destroy(grupoRodajas);
        }

        Destroy(tomateOriginal);
    }
}