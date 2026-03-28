using UnityEngine;

public class EggCooker : MonoBehaviour
{
    [Header("Cooking Settings")]
    public float cookDelay = 1.5f;      
    public GameObject eggCooked;        

    [Header("Burn Settings")]
    public float burnDelay = 15f;       

    [Header("Spawn Settings")]
    public Transform eggSpawnPoint;     

    private bool isCooking = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("pan") && !isCooking)
        {
            StartCoroutine(CookEgg());
        }
    }

    System.Collections.IEnumerator CookEgg()
    {
        isCooking = true;

        Debug.Log("Egg started cooking!");

        yield return new WaitForSeconds(cookDelay);

        if (eggSpawnPoint == null)
        {
            Debug.LogWarning("EggSpawnPoint not assigned! Using fallback position.");
        }

        Vector3 spawnPos = eggSpawnPoint != null ? eggSpawnPoint.position : transform.position;
        Quaternion spawnRot = eggSpawnPoint != null ? eggSpawnPoint.rotation : transform.rotation;

         GameObject cookedEggInstance = Instantiate(
            eggCooked,
            spawnPos,
            spawnRot,
            eggSpawnPoint != null ? eggSpawnPoint : null
        );

       
        Destroy(gameObject);

       
        StartCoroutine(BurnEgg(cookedEggInstance));
    }

    System.Collections.IEnumerator BurnEgg(GameObject cookedEgg)
    {
        yield return new WaitForSeconds(burnDelay);

        
        if (cookedEgg == null) yield break;

        Debug.Log("Egg is burned!");

        Renderer[] renderers = cookedEgg.GetComponentsInChildren<Renderer>();

        foreach (Renderer r in renderers)
        {
            if (r.material.HasProperty("_BaseColor"))
            {
                r.material.SetColor("_BaseColor", Color.black);
            }
            else
            {
                r.material.color = Color.black;
            }
        }
    }
}