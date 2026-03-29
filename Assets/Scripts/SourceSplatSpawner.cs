using System.Collections.Generic;
using UnityEngine;

public class SauceSplatSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject splatPrefab; // Drag your Quad prefab here
    public float splatScale = 0.05f; // Size of the ketchup drop
    public float offsetFromSurface = 0.001f; // Avoids flickering (Z-fighting)

    private ParticleSystem _ps;
    private List<ParticleCollisionEvent> _collisionEvents = new List<ParticleCollisionEvent>();

    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = _ps.GetCollisionEvents(other, _collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            SpawnSplat(_collisionEvents[i], other.transform);
        }
    }

    void SpawnSplat(ParticleCollisionEvent ev, Transform hitTransform)
    {
        // Calculate position slightly above the surface to avoid flickering
        Vector3 spawnPos = ev.intersection + (ev.normal * offsetFromSurface);
        
        // Create the splat and align it with the surface normal
        GameObject splat = Instantiate(splatPrefab, spawnPos, Quaternion.LookRotation(ev.normal));
        
        // Randomize size and rotation for variety
        float randomSize = Random.Range(splatScale * 0.8f, splatScale * 1.2f);
        splat.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
        splat.transform.Rotate(Vector3.forward, Random.Range(0, 360));

        // Parent it to the object hit so it moves with the hot-dog/plate
        splat.transform.SetParent(hitTransform);

        // Optional: Destroy after 20 seconds to keep the game fast
        Destroy(splat, 20f);
    }
}