using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;

[RequireComponent(typeof(AudioSource))]
public class Pistol : MonoBehaviour, IWeapon
{
    [SerializeField] private XRSocketInteractorPistol _magazineSocket;
    [SerializeField] private Transform _magazineLocation;
    [SerializeField] private Transform _barrelLocation;
    [SerializeField] private Transform _casingExitLocation;

    [SerializeField] private AudioClip _shotAudio;
    [SerializeField] private AudioClip _reloadAudio;
    [SerializeField] private AudioClip _noBulletsAudio;

    [SerializeField] private GameObject _casingPrefab;
    [SerializeField] private GameObject _muzzleFlashPrefab;
    [SerializeField] private GameObject _bulletPrefab;

    private ShootEffects shootEffects;

    private bool exitGame = false;                    // fix: debug error unparrenting pistol magazine when exit game

    private float _ejectPower = 50f;                 // Power of casing exit
    private float _flashDestroyTime = 2f;           // Shot flash destroy time
    private float _shotPower = 80f;                  // Shot power
    private float _bulletRange = 100f;               // Bullet working distance
    private string _gameTag = "pistol bullet";          // Tag for new bullet (using in dummy game)

    public AudioClip ShotAudio { get { return _shotAudio; } }
    public AudioClip ReloadAudio { get { return _reloadAudio; } }
    public AudioClip NoBulletsAudio { get { return _noBulletsAudio; } }
    public GameObject CasingPrefab { get { return _casingPrefab; } }
    public GameObject MuzzleFlashPrefab { get { return _muzzleFlashPrefab; } }
    public GameObject BulletPrefab { get { return _bulletPrefab; } }

    public int GetBullets()
    {
        if (_magazineSocket.Magazine != null)
            return _magazineSocket.Magazine.Bullets;
        else
            return 0;
    }

    public int GetMaxBullets()
    {
        if (_magazineSocket.Magazine != null)
            return _magazineSocket.Magazine.MaxBullets;
        else
            return 0;
    }

    public void PlayAudio(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }
    
    private void Start()
    {
        // Get Pistol grab and pistol magazine socket XR components
        XRGrabInteractable pistolGrab = transform.parent.GetComponent<XRGrabInteractable>();

        // Add listeners to Pistol grab
        pistolGrab.selectEntered.AddListener(GetPistol);
        pistolGrab.selectExited.AddListener(DropPistol);
        pistolGrab.activated.AddListener(StartShooting);

        // Add listeners to Pistol magazine socket
        _magazineSocket.selectEntered.AddListener(AddMagazine);
        _magazineSocket.selectExited.AddListener(RemoveMagazine);

        shootEffects = new ShootEffects(this);
    }

    // Listener. Shooting
    private void StartShooting(ActivateEventArgs interactor)
    {
        if (GetBullets() != 0)
            //Calls animation on the gun that has the relevant animation events that will fire
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

        // Minus bullet from counter
        _magazineSocket.Magazine.RemoveBullet();
    }

    // Calls from Animator
    private void NoBullets()
    {
        shootEffects.NoBulletsEffects();
    }

    // Listener. Drop pistol
    private void DropPistol(SelectExitEventArgs interactor)
    {
        Player.Instance.Weapon = null;
    }

    // Listener. Get pistol
    private void GetPistol(SelectEnterEventArgs interactor)
    {
        Player.Instance.Weapon = this;
    }

    // Listener. Add magazine
    private void AddMagazine(SelectEnterEventArgs interactor)
    {
        // fix: Ignore grabbing weapon magazine when pistol not in the hands (more info in MagazineGrabInteractable)
        interactor.interactableObject.transform.gameObject.layer = 11;

        // Put the magazine to pistol magazine location
        interactor.interactableObject.transform.parent = _magazineLocation;

        // Reload audio effect
        shootEffects.ReloadEffects();
    }

    // Listener. Remove magazine
    private void RemoveMagazine(SelectExitEventArgs interactor)
    {
        // fix: Ignore grabbing weapon magazine when pistol not in the hands (more info in MagazineGrabInteractable)
        interactor.interactableObject.transform.gameObject.layer = 0;

        if (!exitGame) // fix: debug error unparrenting pistol magazine when exit game
            interactor.interactableObject.transform.parent = null;

        // Reload audio effect
        shootEffects.ReloadEffects();
    }

    // fix: debug error unparrenting pistol magazine when exit game
    void OnApplicationQuit()
    {
        exitGame = true;
    }
}