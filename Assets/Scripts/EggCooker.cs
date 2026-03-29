using UnityEngine;

public class EggCooker : MonoBehaviour
{
    [Header("Cooking Settings")]
    public float cookDelay = 1.5f;      
    public GameObject eggCooked;        
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

       
      
    }

   
}