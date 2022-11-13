using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

public class Shotgun : MonoBehaviour, IWeapon
{
    [SerializeField] private List<XRSocketInteractor> _magazineSockets = new List<XRSocketInteractor>();
    [SerializeField] private Transform _barrelLocation;
    [SerializeField] private Transform _casingExitLocation;

    [SerializeField] private AudioClip _shotAudio;
    [SerializeField] private AudioClip _reloadAudio;
    [SerializeField] private AudioClip _noBulletsAudio;

    [SerializeField] private GameObject _casingPrefab;
    [SerializeField] private GameObject _muzzleFlashPrefab;
    [SerializeField] private GameObject _bulletPrefab;

    private ShootEffects shootEffects;

    private int _maxBullets = 2;                      // Max bullets in weapon
    private float _ejectPower = 250f;                 // Power of casing exit
    private float _flashDestroyTime = 2f;             // Shot flash destroy time
    private float _shotPower = 130f;                  // Shot power
    private float _bulletRange = 100f;                // Bullet working distance
    private static int _buckshotBullets = 6;          // Buckshot bullets amount
    private string _gameTag = "shotgun bullet";       // Tag for new bullet (using in dummy game)

    public AudioClip ShotAudio { get { return _shotAudio; } }
    public AudioClip ReloadAudio { get { return _reloadAudio; } }
    public AudioClip NoBulletsAudio { get { return _noBulletsAudio; } }
    public GameObject CasingPrefab { get { return _casingPrefab; } }
    public GameObject MuzzleFlashPrefab { get { return _muzzleFlashPrefab; } }
    public GameObject BulletPrefab { get { return _bulletPrefab; } }
    public static int BuckshotBullets { get { return _buckshotBullets; } }

    public int GetBullets()
    {
        int i = 0;

        foreach (var socket in _magazineSockets)
        {
            if (socket.hasSelection)
                i++;
        }

        return i;
    }

    public int GetMaxBullets()
    {
        return _maxBullets;
    }

    public void PlayAudio(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }

    private void Start()
    {
        // Get weapon grab and weapon magazine socket XR components
        XRGrabInteractable shotgunGrab = transform.parent.GetComponent<XRGrabInteractable>();

        // Add listeners to weapon grab
        shotgunGrab.selectEntered.AddListener(GetWeapon);
        shotgunGrab.selectExited.AddListener(DropWeapon);
        shotgunGrab.activated.AddListener(StartShooting);

        // Add listeners to weapon magazine socket
        foreach (var socket in _magazineSockets)
        {
            socket.selectEntered.AddListener(AddMagazine);
            socket.selectExited.AddListener(RemoveMagazine);
        }

        shootEffects = new ShootEffects(this);
    }

    // Listener. Shooting
    private void StartShooting(ActivateEventArgs interactor)
    {
        if (GetBullets() > 0)
            // Calls animation on the gun that has the relevant animation events that will fire
            GetComponent<Animator>().SetTrigger("Fire");
        else
            // No bullets animation
            GetComponent<Animator>().SetTrigger("noBullets");
    }

    // Calls from Animator
    private void Shoot()
    {
        shootEffects.ShowShootingEffects(_barrelLocation, _flashDestroyTime, _bulletRange, _shotPower, _gameTag);
    }

    // Calls from Animator
    private void CasingRelease()
    {
        shootEffects.ShowCasingEffects(_casingExitLocation, _ejectPower);

        // Destroy bullet
        foreach (var socket in _magazineSockets)
        {
            if (socket.hasSelection)
            {
                Destroy(socket.interactablesSelected[0].transform.gameObject);
                break;
            }
        }
    }

    // Calls from Animator
    private void NoBullets()
    {
        shootEffects.NoBulletsEffects();
    }

    // Listener. Drop weapon
    private void DropWeapon(SelectExitEventArgs interactor)
    {
        Player.Instance.Weapon = null;
    }

    // Listener. Get weapon
    private void GetWeapon(SelectEnterEventArgs interactor)
    {
        Player.Instance.Weapon = this;
    }

    // Listener. Add magazine
    private void AddMagazine(SelectEnterEventArgs interactor)
    {
        // Reload audio effect
        shootEffects.ReloadEffects();
    }

    // Listener. Remove magazine
    private void RemoveMagazine(SelectExitEventArgs interactor)
    {
        // Reload audio effect
        shootEffects.ReloadEffects();
    }
}