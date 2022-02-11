using UnityEngine;

public class ObjectFalling : MonoBehaviour
{
    public AudioClip clip;

    void OnCollisionEnter(Collision collision)
    {
        // Check collision speed if fall fast play sound and ignore player collision
        if (collision.relativeVelocity.magnitude > 3f && collision.transform.name != "Player")
            this.GetComponent<AudioSource>().PlayOneShot(clip);
    }
}