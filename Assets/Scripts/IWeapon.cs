using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public interface IWeapon
{
    public AudioClip ShotAudio { get; }
    public AudioClip ReloadAudio { get; }
    public AudioClip NoBulletsAudio { get; }
    public GameObject CasingPrefab { get; }
    public GameObject MuzzleFlashPrefab { get; }
    public GameObject BulletPrefab { get; }

    public int GetBullets();
    public int GetMaxBullets();
    public void PlayAudio(AudioClip clip);
}