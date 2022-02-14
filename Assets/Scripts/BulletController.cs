// Bullet controller script

using UnityEngine;

public class BulletController : ShootEffects
{
    private bool firstCollision = true;                 // Flag that signal about first arrow collision

    void OnCollisionEnter(Collision collision)
    {
        // Work just with first bullet collision
        if (firstCollision && collision.transform.GetComponent<Renderer>() != null)
        {
            // Get first collision
            firstCollision = false;

            // Get contact point info
            ContactPoint contactPoint = collision.contacts[0];

            // Get impact and hole effects
            GameObject [] effects = CreateImpactAndHole(collision);

            // Create an impact effect
            GameObject impact = Instantiate(effects[0], contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
            Destroy(impact, 1f);

            // Create bullet hole effect (rotate to player and move step from object)
            if (effects[1] != null)
            {
                Vector3 holePosition = contactPoint.point + contactPoint.normal * 0.0001f;
                GameObject bulletHole = Instantiate(effects[1], holePosition, Quaternion.LookRotation(-contactPoint.normal));
                bulletHole.transform.SetParent(collision.transform);
            }

            // Iron chain destroy on hit with no impact and bullet hole effects
            if (collision.transform.tag == "chain" && collision.transform.GetComponent<HingeJoint>() != null)
                Destroy(collision.transform.GetComponent<HingeJoint>());
        }
    }
}