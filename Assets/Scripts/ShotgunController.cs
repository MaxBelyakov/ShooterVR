using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShotgunController : MonoBehaviour
{
    [SerializeField] private GameObject magazineLocationSocket;        // Socket to put/get weapon magazine
    [SerializeField] private Transform weaponModel;                    // Weapon model transform location
    [SerializeField] private GameObject shotgunAmmoPrefab;             // Shotgun ammo prefab

    [SerializeField] private AudioClip reloadAudio;                    // Reload sound

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
    }

    // Listener. Shooting
    public void StartShooting(ActivateEventArgs interactor)
    {
        if (!WeaponController.s_reloading && !WeaponController.s_shooting)
        {
            WeaponController.s_shooting = true;
            if (Shotgun.s_bulletsCurrent > 0)
                // Calls animation on the gun that has the relevant animation events that will fire
                weaponModel.GetComponent<Animator>().SetTrigger("Fire");
            else
                // No bullets animation
                weaponModel.GetComponent<Animator>().SetTrigger("noBullets");
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
        WeaponController.s_weapon = "Shotgun";
    }

    // Listener. Add magazine
    private void AddMagazine(SelectEnterEventArgs interactor)
    {
        Shotgun.s_bulletsCurrent++;

        // Destroy ammo body
        Destroy(interactor.interactableObject.transform.gameObject);

        // Reload audio effect
        GetComponent<AudioSource>().PlayOneShot(reloadAudio);
    }

    // Listener. Remove magazine
    private void RemoveMagazine(SelectExitEventArgs interactor)
    {
        // Reload audio effect
        //GetComponent<AudioSource>().PlayOneShot(reloadAudio);
    }
}