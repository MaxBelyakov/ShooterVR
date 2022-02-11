using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PistolController : MonoBehaviour
{
    [SerializeField] private GameObject magazineLocationSocket;
    [SerializeField] private GameObject magazineLocation;

    private XRSocketInteractor pistolSocket;

    void Start()
    {
        XRGrabInteractable pistolGrab = gameObject.GetComponent<XRGrabInteractable>();
        pistolSocket = magazineLocationSocket.GetComponent<XRSocketInteractor>();

        pistolGrab.selectEntered.AddListener(GetPistol);
        pistolGrab.selectExited.AddListener(DropPistol);

        pistolSocket.selectEntered.AddListener(AddMagazine);
        pistolSocket.selectExited.AddListener(RemoveMagazine);
    }

    private void DropPistol(SelectExitEventArgs interactor)
    {
        WeaponController.s_weapon = "NoWeapon";
    }

    private void GetPistol(SelectEnterEventArgs interactor)
    {
        WeaponController.s_weapon = "Pistol";      
    }

    private void AddMagazine(SelectEnterEventArgs interactor)
    {
        interactor.interactableObject.transform.gameObject.layer = 11;
        interactor.interactableObject.transform.parent = magazineLocation.transform;
    }

    private void RemoveMagazine(SelectExitEventArgs interactor)
    {
        interactor.interactableObject.transform.gameObject.layer = 0;
        interactor.interactableObject.transform.parent = null;
    }
}