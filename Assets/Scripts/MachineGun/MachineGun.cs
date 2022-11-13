using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class MachineGun : MonoBehaviour, IWeapon
{
    [SerializeField] private XRSocketInteractorMachineGun _magazineSocket;
    [SerializeField] private Transform _magazineLocation;
    [SerializeField] private Transform _barrelLocation;
    [SerializeField] private Transform _casingExitLocation;

    [SerializeField] private AudioClip _shotAudio;
    [SerializeField] private AudioClip _reloadAudio;
    [SerializeField] private AudioClip _noBulletsAudio;

    [SerializeField] private GameObject _casingPrefab;
    [SerializeField] private GameObject _muzzleFlashPrefab;
    [SerializeField] private GameObject _bulletPrefab;

    private bool exitGame = false;                  // fix: debug error unparrenting weapon magazine when exit game
    private bool autoShooting = false;              // Flag start/stop auto shooting

    private ShootEffects shootEffects;

    private float _ejectPower = 50f;                // Power of casing exit
    private float _flashDestroyTime = 2f;           // Shot flash destroy time
    private float _shotPower = 100f;                // Shot power
    private float _bulletRange = 100f;              // Bullet working distance
    private string _gameTag = "machine gun bullet";          // Tag for new bullet (using in dummy game)

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
        // Get weapon grab and weapon magazine socket XR components
        XRGrabInteractable weaponGrab = transform.parent.GetComponent<XRGrabInteractable>();

        // Add listeners to weapon grab
        weaponGrab.selectEntered.AddListener(GetWeapon);
        weaponGrab.selectExited.AddListener(DropWeapon);
        weaponGrab.activated.AddListener(StartShooting);
        weaponGrab.deactivated.AddListener(StopShooting);

        // Add listeners to weapon magazine socket
        _magazineSocket.selectEntered.AddListener(AddMagazine);
        _magazineSocket.selectExited.AddListener(RemoveMagazine);

        shootEffects = new ShootEffects(this);
    }

    // Listener. Shooting
    private void StartShooting(ActivateEventArgs interactor)
    {
        // Activate autoshooting by default
        autoShooting = true;
        StartCoroutine(AutoShooting());
    }

    // Shooting repeater while autoshooting flag is true
    private IEnumerator AutoShooting()
    {
        if (autoShooting)
        {
            if (GetBullets() > 0)
                // Calls animation on the gun that has the relevant animation events that will fire
                GetComponent<Animator>().SetTrigger("Shoot");
            else
                // No bullets animation
                GetComponent<Animator>().SetTrigger("NoBullets");

            yield return new WaitForSeconds(0.05f);

            // Repeat shooting
            StartCoroutine(AutoShooting());
        }
    }

    // Listener. Stop shooting
    private void StopShooting(DeactivateEventArgs interactor)
    {
        // Deactivate autoshooting
        autoShooting = false;
        StopCoroutine(AutoShooting());
    }

    // Calls from animator
    private void Shoot()
    {
        shootEffects.ShowShootingEffects(_barrelLocation, _flashDestroyTime, _bulletRange, _shotPower, _gameTag);
    }

    // Calls from animator
    private void CasingRelease()
    {
        shootEffects.ShowCasingEffects(_casingExitLocation, _ejectPower);

        // Minus bullet from counter
        _magazineSocket.Magazine.RemoveBullet();
    }

    // Calls from animator
    private void NoBullets()
    {
        shootEffects.NoBulletsEffects();
    }

    // Listener. Drop weapon
    private void DropWeapon(SelectExitEventArgs interactor)
    {
        Player.Instance.Weapon = null;

        // Deactivate autoshooting
        autoShooting = false;
        StopCoroutine(AutoShooting());
    }

    // Listener. Get weapon
    private void GetWeapon(SelectEnterEventArgs interactor)
    {
        Player.Instance.Weapon = this;
    }

    // Listener. Add magazine
    private void AddMagazine(SelectEnterEventArgs interactor)
    {
        // fix: Ignore grabbing weapon magazine when weapon not in the hands (more info in MagazineGrabInteractable)
        interactor.interactableObject.transform.gameObject.layer = 11;

        // Put the magazine to weapon magazine location
        interactor.interactableObject.transform.parent = _magazineLocation;

        // Reload audio effect
        shootEffects.ReloadEffects();
    }

    // Listener. Remove magazine
    private void RemoveMagazine(SelectExitEventArgs interactor)
    {
        // fix: Ignore grabbing weapon magazine when weapon not in the hands (more info in MagazineGrabInteractable)
        interactor.interactableObject.transform.gameObject.layer = 0;

        if (!exitGame) // fix: debug error unparrenting weapon magazine when exit game
            interactor.interactableObject.transform.parent = null;

        // Reload audio effect
        shootEffects.ReloadEffects();
    }

    // fix: debug error unparrenting weapon magazine when exit game
    void OnApplicationQuit()
    {
        exitGame = true;
    }
}