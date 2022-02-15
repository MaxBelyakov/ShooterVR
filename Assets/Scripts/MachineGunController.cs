using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MachineGunController : MonoBehaviour
{
    [SerializeField] private GameObject magazineLocationSocket;        // Socket to put/get weapon magazine
    [SerializeField] private Transform magazineLocation;               // Magazine location
    [SerializeField] private Transform weaponModel;                    // Weapon model transform location
    [SerializeField] private GameObject baseMagazine;                  // Magazine inside the weapon on game start

    [SerializeField] private AudioClip reloadAudio;                    // Reload sound

    private bool exitGame = false;                                     // fix: debug error unparrenting weapon magazine when exit game

    void Start()
    {
        // Get weapon grab and weapon magazine socket XR components
        XRGrabInteractable weaponGrab = gameObject.GetComponent<XRGrabInteractable>();
        XRSocketInteractor weaponSocket = magazineLocationSocket.GetComponent<XRSocketInteractor>();

        // Add listeners to weapon grab
        weaponGrab.selectEntered.AddListener(GetWeapon);
        weaponGrab.selectExited.AddListener(DropWeapon);
        weaponGrab.activated.AddListener(StartShooting);

        // Add listeners to weapon magazine socket
        weaponSocket.selectEntered.AddListener(AddMagazine);
        weaponSocket.selectExited.AddListener(RemoveMagazine);

        // Put base magazine inside weapon magazine location (it fix the XR grab relocate object)
        baseMagazine.transform.parent = magazineLocation;
    }

    void Update()
    {   
        // Update current amount bullets inside the weapon
        if (magazineLocation.childCount != 0)
            MachineGun.s_bulletsCurrent = magazineLocation.GetChild(0).transform.GetComponent<MagazineController>().bulletsCurrent;
        else
            MachineGun.s_bulletsCurrent = 0;
    }

    // Listener. Shooting
    public void StartShooting(ActivateEventArgs interactor)
    {
        if (!WeaponController.s_reloading && !WeaponController.s_shooting)
        {
            WeaponController.s_shooting = true;
            if (MachineGun.s_bulletsCurrent > 0)
                // Calls animation on the gun that has the relevant animation events that will fire
                weaponModel.GetComponent<Animator>().SetTrigger("Shoot");
            else
                // No bullets animation
                weaponModel.GetComponent<Animator>().SetTrigger("NoBullets");
        }
    }

    // Listener. Drop weapon
    private void DropWeapon(SelectExitEventArgs interactor)
    {
        WeaponController.s_weapon = "noWeapon";
    }

    // Listener. Get weapon
    private void GetWeapon(SelectEnterEventArgs interactor)
    {
        WeaponController.s_weapon = "Machine Gun";
    }

    // Listener. Add magazine
    private void AddMagazine(SelectEnterEventArgs interactor)
    {
        // FIX ME
        interactor.interactableObject.transform.gameObject.layer = 11;

        // Put the magazine to weapon magazine location
        interactor.interactableObject.transform.parent = magazineLocation;

        // Reload audio effect
        GetComponent<AudioSource>().PlayOneShot(reloadAudio);
    }

    // Listener. Remove magazine
    private void RemoveMagazine(SelectExitEventArgs interactor)
    {
        // FIX ME
        interactor.interactableObject.transform.gameObject.layer = 0;

        if (!exitGame) // fix: debug error unparrenting weapon magazine when exit game
            interactor.interactableObject.transform.parent = null;

        // Reload audio effect
        GetComponent<AudioSource>().PlayOneShot(reloadAudio);
    }

    // fix: debug error unparrenting weapon magazine when exit game
    void OnApplicationQuit()
    {
        exitGame = true;
    }

}