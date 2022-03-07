using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

public class ShotgunController : MonoBehaviour
{
    [SerializeField] private Transform weaponModel;                    // Weapon model transform location
    [SerializeField] private GameObject shotgunAmmoPrefab;             // Shotgun ammo prefab

    [SerializeField] private AudioClip reloadAudio;                    // Reload sound

    [SerializeField] private List<XRSocketInteractor> weaponSockets = new List<XRSocketInteractor>();

    void Start()
    {
        // Get weapon grab and weapon magazine socket XR components
        XRGrabInteractable weaponGrab = gameObject.GetComponent<XRGrabInteractable>();

        // Add listeners to weapon grab
        weaponGrab.selectEntered.AddListener(GetWeapon);
        weaponGrab.selectExited.AddListener(DropWeapon);
        weaponGrab.activated.AddListener(StartShooting);

        // Add listeners to weapon magazine socket
        foreach (var socket in weaponSockets)
        {
            socket.selectEntered.AddListener(AddMagazine);
            socket.selectExited.AddListener(RemoveMagazine);
        }
    }

    // Listener. Shooting
    public void StartShooting(ActivateEventArgs interactor)
    {
        if (Shotgun.s_bulletsCurrent > 0)
        {
            // Destroy call remove magazine, minus bullet
            foreach (var socket in weaponSockets)
            {
                if (socket.hasSelection)
                {
                    Destroy(socket.interactablesSelected[0].transform.gameObject);
                    break;
                }
            }
            // Calls animation on the gun that has the relevant animation events that will fire
            weaponModel.GetComponent<Animator>().SetTrigger("Fire");
        }
        else
            // No bullets animation
            weaponModel.GetComponent<Animator>().SetTrigger("noBullets");
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

        // Reload audio effect
        GetComponent<AudioSource>().PlayOneShot(reloadAudio);
    }

    // Listener. Remove magazine
    private void RemoveMagazine(SelectExitEventArgs interactor)
    {
        Shotgun.s_bulletsCurrent--;

        // Reload audio effect
        GetComponent<AudioSource>().PlayOneShot(reloadAudio);
    }
}