using UnityEngine;

public class MeatCooking : MonoBehaviour
{
    [Header("Visual References")]
    public Renderer meatRenderer; 
    
    [Header("Cooking Colors")]
    public Color rawTint = Color.white;
    public Color cookedTint;
    public Color burntTint;

    [Header("Timers")]
    public float timeToCook = 60f;
    public float timeToBurn = 150f;

    private float cookingTimer = 0f;
    private bool isTouchingGrill = false;
    private Material instanceMaterial;

    void Awake()
    {
        if (meatRenderer != null)
        {
            instanceMaterial = meatRenderer.material;
        }
    }

    // --- TETİKLEYİCİ KONTROLLERİ (Is Trigger AÇIKSA) ---
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Grill")) isTouchingGrill = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grill")) isTouchingGrill = false;
    }

    // --- ÇARPIŞMA KONTROLLERİ (Is Trigger KAPALIYSA) ---
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grill")) isTouchingGrill = true;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grill")) isTouchingGrill = false;
    }

    void Update()
    {
        if (isTouchingGrill)
        {
            cookingTimer += Time.deltaTime;
            UpdateVisuals();
        }
        
        // Önemli: Her karede isTouchingGrill'i fizik fonksiyonları güncellemezse 
        // pişmenin durması için Stay metodlarını kullanıyoruz.
    }

    void UpdateVisuals()
    {
        if (instanceMaterial == null) return;

        if (cookingTimer <= timeToCook)
        {
            float lerpVal = cookingTimer / timeToCook;
            instanceMaterial.color = Color.Lerp(rawTint, cookedTint, lerpVal);
        }
        else if (cookingTimer <= timeToBurn)
        {
            float lerpVal = (cookingTimer - timeToCook) / (timeToBurn - timeToCook);
            instanceMaterial.color = Color.Lerp(cookedTint, burntTint, lerpVal);
        }
    }
}