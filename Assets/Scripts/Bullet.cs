using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool firstCollision = true;                 // Flag that signal about first collision
    private ImpactEffects effects;

    private void Start()
    {
        effects = transform.GetComponent<ImpactEffects>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Work just with first bullet collision
        if (firstCollision && collision.transform.GetComponent<Renderer>() != null)
        {
            // Get first collision
            firstCollision = false;

            // Get contact point info
            ContactPoint contactPoint = collision.contacts[0];

            // Create impact and hole effects
            if (effects != null)
                effects.CreateImpactAndHole(collision.transform, contactPoint);
        }
    }
}