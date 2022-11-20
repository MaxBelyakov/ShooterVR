using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private AudioClip arrowDropAudio; // Sound of arrow drop

    private float _arrowSpeedStick = 8f;               // Arrow speed limit when start stick in objects or impact effect
    private float _arrowDropSpeedLimit = 3f;           // Arrow drop speed limit when can play drop sound

    private ImpactEffects effects;

    public bool FirstCollision { get; set; } = true;  // Flag that signal about first arrow collision

    private void Start()
    {
        effects = transform.GetComponent<ImpactEffects>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check for first collision, materail and arrow speed (if speed low - no imact and no stick)
        if (FirstCollision && collision.transform.GetComponent<Renderer>() != null && collision.relativeVelocity.magnitude > _arrowSpeedStick)
        {
            // Arrow get stuck in wood just at first collision
            FirstCollision = false;

            // Get contact point info
            ContactPoint contactPoint = collision.contacts[0];

            effects.CreateArrowImpact(collision.transform, contactPoint);

        } else if (!FirstCollision && collision.gameObject.layer != 6)
        {
            // Check collision speed if moving fast play sound and ignore player collision
            Vector3 collisionForce = collision.impulse / Time.fixedDeltaTime;

            if (collisionForce.magnitude > _arrowDropSpeedLimit)
                GetComponent<AudioSource>().PlayOneShot(arrowDropAudio);
        }
    }
}