using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PistolController : MonoBehaviour
{
    [SerializeField] private GameObject magazineLocationSocket;
    [SerializeField] private Transform magazineLocation;
    [SerializeField] private Transform pistolModel;
    [SerializeField] private GameObject baseMagazine;

    private XRSocketInteractor pistolSocket;

    [SerializeField] private AudioClip reloadAudio;                    // Reload sound

    void Start()
    {
        XRGrabInteractable pistolGrab = gameObject.GetComponent<XRGrabInteractable>();
        pistolSocket = magazineLocationSocket.GetComponent<XRSocketInteractor>();

        pistolGrab.selectEntered.AddListener(GetPistol);
        pistolGrab.selectExited.AddListener(DropPistol);
        pistolGrab.activated.AddListener(StartShooting);

        pistolSocket.selectEntered.AddListener(AddMagazine);
        pistolSocket.selectExited.AddListener(RemoveMagazine);

        baseMagazine.transform.parent = magazineLocation;
    }

    void Update()
    {
        if (magazineLocation.childCount != 0)
            Pistol.s_bulletsCurrent = magazineLocation.GetChild(0).transform.GetComponent<PistolMagazineController>().bulletsCurrent;
        else
            Pistol.s_bulletsCurrent = 0;
    }

    public void StartShooting(ActivateEventArgs interactor)
    {
        if (!WeaponController.s_reloading && !WeaponController.s_shooting)
        {
            WeaponController.s_shooting = true;
            if (Pistol.s_bulletsCurrent != 0)
            {
                //Calls animation on the gun that has the relevant animation events that will fire
                pistolModel.GetComponent<Animator>().SetTrigger("Fire");
            } else {
                // No bullets animation
                pistolModel.GetComponent<Animator>().SetTrigger("noBullets");
            }
        }
    }

    private void DropPistol(SelectExitEventArgs interactor)
    {
        WeaponController.s_weapon = "noWeapon";
    }

    private void GetPistol(SelectEnterEventArgs interactor)
    {
        WeaponController.s_weapon = "Pistol";
    }

    private void AddMagazine(SelectEnterEventArgs interactor)
    {
        interactor.interactableObject.transform.gameObject.layer = 11;
        interactor.interactableObject.transform.parent = magazineLocation;

        // Reload audio effect
        GetComponent<AudioSource>().PlayOneShot(reloadAudio);
    }

    private void RemoveMagazine(SelectExitEventArgs interactor)
    {
        interactor.interactableObject.transform.gameObject.layer = 0;
        magazineLocation.DetachChildren();

        // Reload audio effect
        GetComponent<AudioSource>().PlayOneShot(reloadAudio);
    }
}