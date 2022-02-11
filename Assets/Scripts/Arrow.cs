using UnityEngine;

public class Arrow : ShootEffects
{
    private float depth = 0.3f;                         // Depth that arrow move in target

    private bool firstCollision = true;                 // Flag that signal about first arrow collision

    public AudioClip hitWoodAudio;                      // Sound of hit arrow in wood
    public AudioClip impactAudioStandart;               // Sound of standart arrow impact
    public AudioClip impactAudioMetal;                  // Sound of arrow impact in metal
    public AudioClip dropAudio;                         // Sound of arrow drop

    void OnCollisionEnter(Collision collision)
    {
        if (firstCollision && collision.transform.GetComponent<Renderer>() != null)
        {
            // Arrow get stuck in wood just at first collision
            firstCollision = false;

            // Set default imapct and hole effects
            impactEffect = impactStandartEffect;
            bulletHoleEffect = null;

            // Arrow behavior when hit in wood
            if (MaterialCheck(collision.transform.GetComponent<Renderer>().material.name) == "wood")
            {
                // Stop the arrow and remove physic body
                //this.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                if (collision.transform.GetComponent<Rigidbody>() != null)
                    collision.transform.GetComponent<Rigidbody>().AddForce(-Vector3.forward, ForceMode.Impulse);

                this.transform.GetComponent<Rigidbody>().isKinematic = true;
                Destroy(this.GetComponent<Rigidbody>());

                // Remove box collider to ignore collision with player, but save triggered collider to take arrow in inventory
                foreach (BoxCollider col in this.GetComponents<BoxCollider>())
                    if (col.isTrigger == false)
                        Destroy(col);
                
                // Move arrow inside wood
                this.transform.Translate(depth * Vector2.up);

                // Stop target velocity
                if (collision.transform.GetComponent<Rigidbody>() != null)
                    collision.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                
                // Fix: arrow become flat when hit the floor. It is because of difference with the floor scale
                if (collision.transform.GetComponent<Renderer>().material.name == "Grass (Instance)")
                    this.transform.parent = collision.transform.parent;
                else
                    this.transform.parent = collision.transform;

                // Arrow hit sound
                this.transform.GetComponent<AudioSource>().PlayOneShot(hitWoodAudio);

            } else {

                // Check the object for stone and metal to choose effect style
                AudioClip impactAudio = impactAudioStandart;
                if (MaterialCheck(collision.transform.GetComponent<Renderer>().material.name) == "stone")
                    impactEffect = impactStoneEffect;

                if (MaterialCheck(collision.transform.GetComponent<Renderer>().material.name) == "metal")
                {
                    impactEffect = impactMetalEffect;
                    impactAudio = impactAudioMetal;
                }

                // Impact sound
                this.transform.GetComponent<AudioSource>().PlayOneShot(impactAudio);

                // Create an impact effect
                ContactPoint contact = collision.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;
                GameObject impact = Instantiate(impactEffect, pos, rot);
                Destroy(impact, 1f);
            }

            // Hit dummy target, check for "dummy" tag also parent object because bullet can hit the bullet hole
            if ((collision.transform.tag == "dummy" || collision.transform.parent?.tag == "dummy") 
                && DummyGenerator.s_dummyWeapon == WeaponController.s_weapon)
            {
                // Dummy weapon compares with current player weapon and start drop the dummy
                collision.transform.gameObject.AddComponent<Rigidbody>();
                collision.transform.gameObject.transform.GetComponent<Rigidbody>().mass = DummyGenerator.s_dummyMass;
                collision.transform.tag = "Untagged";
                DummyGenerator.s_dummy = false;
            }

        } else if (!firstCollision && collision.relativeVelocity.magnitude > 3f && collision.transform.name != "Player")
            // Check collision speed if fall fast play sound and ignore player collision
            this.GetComponent<AudioSource>().PlayOneShot(dropAudio);
    }
}