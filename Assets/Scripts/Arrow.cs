using UnityEngine;

public class Arrow : MonoBehaviour
{
    public bool firstCollision = true;                 // Flag that signal about first arrow collision

    public AudioClip hitWoodAudio;                      // Sound of hit arrow in wood
    public AudioClip impactAudioStandart;               // Sound of standart arrow impact
    public AudioClip impactAudioMetal;                  // Sound of arrow impact in metal
    public AudioClip dropAudio;                         // Sound of arrow drop

    void OnCollisionEnter(Collision collision)
    {
        // Check for first collision, materail and arrow speed (if speed low - no imact and no stick)
        if (firstCollision && collision.transform.GetComponent<Renderer>() != null && collision.relativeVelocity.magnitude > Bow.arrowSpeedStick)
        {
            // Arrow get stuck in wood just at first collision
            firstCollision = false;

            // Arrow behavior when hit in wood
            /*if (MaterialCheck(collision.transform.GetComponent<Renderer>().material.name) == "wood")
            {
                // Stop the arrow
                transform.GetComponent<Rigidbody>().isKinematic = true;

                // Mark box collider as trigger
                GetComponent<BoxCollider>().isTrigger = true;
                
                // Move arrow inside wood
                transform.Translate(Bow.depth * Vector3.forward);

                // Stop target velocity and get simple impulse
                if (collision.transform.GetComponent<Rigidbody>() != null)
                {
                    collision.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    collision.transform.GetComponent<Rigidbody>().AddForce(-Vector3.forward, ForceMode.Impulse);
                }
                
                // Fix: arrow become flat when hit the floor. It is because of difference with the floor scale
                if (collision.transform.GetComponent<Renderer>().material.name == "Grass (Instance)")
                    this.transform.parent = collision.transform.parent;
                else
                    this.transform.parent = collision.transform;

                // Arrow hit sound
                transform.GetComponent<AudioSource>().PlayOneShot(hitWoodAudio);

            } else {

                // Check the object for stone and metal to choose effect style
                AudioClip impactAudio = impactAudioStandart;

                GameObject [] effects = CreateImpactAndHole(collision);

                if (MaterialCheck(collision.transform.GetComponent<Renderer>().material.name) == "metal")
                    impactAudio = impactAudioMetal;

                // Impact sound
                this.transform.GetComponent<AudioSource>().PlayOneShot(impactAudio);

                // Create an impact effect
                ContactPoint contact = collision.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;
                GameObject impact = Instantiate(effects[0], pos, rot);
                Destroy(impact, 1f);
            }*/
        } else if (!firstCollision && collision.gameObject.layer != 6)
        {
            // Check collision speed if moving fast play sound and ignore player collision
            Vector3 collisionForce = collision.impulse / Time.fixedDeltaTime;

            if (collisionForce.magnitude > Bow.arrowDropSpeedLimit)
                this.GetComponent<AudioSource>().PlayOneShot(dropAudio);
        }
    }
}