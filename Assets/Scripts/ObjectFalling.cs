using UnityEngine;

public class ObjectFalling : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    void OnCollisionEnter(Collision collision)
    {
        // Check collision speed if fall fast play sound and ignore player collision
        if (collision.relativeVelocity.magnitude > 3f)
            GetComponent<AudioSource>().PlayOneShot(clip);
    }
}