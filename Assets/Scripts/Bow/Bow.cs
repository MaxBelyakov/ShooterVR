using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private AudioClip _shotAudio;

    private float _shootSpeed = 350f;                  // Speed of string unstretch
    private float _inertia = 1000f;                    // String inertia divider
    private float _forceReducing = 70f;                // Arrow move force reducing value

    public bool IsCharged { get; set; } = false;
    public float ShootSpeed { get { return _shootSpeed; } }
    public float Inertia { get { return _inertia; } }
    public float ForceReducing { get { return _forceReducing; } }
    public AudioClip ShotAudio { get { return _shotAudio; } }
    public AudioClip ReloadAudio { get { return null; } }
    public AudioClip NoBulletsAudio { get { return null; } }
    public GameObject CasingPrefab { get { return null; } }
    public GameObject MuzzleFlashPrefab { get { return null; } }
    public GameObject BulletPrefab { get { return null; } }

    public int GetBullets()
    {
        if (IsCharged)
            return 1;
        else
            return 0;
    }

    public int GetMaxBullets()
    {
        return 1;
    }
    public void PlayAudio(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
