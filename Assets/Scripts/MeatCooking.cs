using UnityEngine;

public class MeatCooking : MonoBehaviour
{
    [Header("Visual References")]
    public Renderer meatRenderer; // The Renderer component of the meat
    
    [Header("Cooking Colors (Pick them in Inspector!)")]
    public Color rawTint = Color.white;    // Initial color tint (White shows the original texture)
    public Color cookedTint;  // Target color when perfectly cooked
    public Color burntTint;  // Target color when overcooked/burnt"

    [Header("Timers")]
    public float timeToCook = 10f;
    public float timeToBurn = 20f;

    private float cookingTimer = 0f;
    private bool isTouchingGrill = false;
    private Material instanceMaterial; // Unique material instance for this specific object

    void Awake()
    {
        // a copy of the material so each patty cooks independently
        instanceMaterial = meatRenderer.material;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grill")) isTouchingGrill = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grill")) isTouchingGrill = false;
    }

    void Update()
    {
        if (isTouchingGrill)
        {
            cookingTimer += Time.deltaTime;
            UpdateVisuals();
        }
    }

    void UpdateVisuals()
    {
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