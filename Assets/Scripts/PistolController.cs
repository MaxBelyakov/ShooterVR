using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PistolController : MonoBehaviour
{
    [SerializeField] private GameObject magazineLocationSocket;        // Socket to put/get pistol magazine
    [SerializeField] private Transform magazineLocation;               // Magazine location
    [SerializeField] private Transform pistolModel;                    // Pistol model transform location
    [SerializeField] private GameObject baseMagazine;                  // Magazine inside the pistol on game start

    [SerializeField] private AudioClip reloadAudio;                    // Reload sound

    private bool exitGame = false;                                     // fix: debug error unparrenting pistol magazine when exit game

    void Start()
    {
        // Get Pistol grab and pistol magazine socket XR components
        XRGrabInteractable pistolGrab = gameObject.GetComponent<XRGrabInteractable>();
        XRSocketInteractor pistolSocket = magazineLocationSocket.GetComponent<XRSocketInteractor>();

        // Add listeners to Pistol grab
        pistolGrab.selectEntered.AddListener(GetPistol);
        pistolGrab.selectExited.AddListener(DropPistol);
        pistolGrab.activated.AddListener(StartShooting);

        // Add listeners to Pistol magazine socket
        pistolSocket.selectEntered.AddListener(AddMagazine);
        pistolSocket.selectExited.AddListener(RemoveMagazine);

        // Put base magazine inside pistol magazine location (it fix the XR grab relocate object)
        baseMagazine.transform.parent = magazineLocation;
    }

    void Update()
    {
        // Update current amount bullets inside the pistol
        if (magazineLocation.childCount != 0)
            Pistol.s_bulletsCurrent = magazineLocation.GetChild(0).transform.GetComponent<PistolMagazineController>().bulletsCurrent;
        else
            Pistol.s_bulletsCurrent = 0;
    }

    // Listener. Shooting
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

    // Listener. Drop pistol
    private void DropPistol(SelectExitEventArgs interactor)
    {
        WeaponController.s_weapon = "noWeapon";
    }

    // Listener. Get pistol
    private void GetPistol(SelectEnterEventArgs interactor)
    {
        WeaponController.s_weapon = "Pistol";
    }

    // Listener. Add magazine
    private void AddMagazine(SelectEnterEventArgs interactor)
    {
        // FIX ME
        interactor.interactableObject.transform.gameObject.layer = 11;

        // Put the magazine to pistol magazine location
        interactor.interactableObject.transform.parent = magazineLocation;

        // Reload audio effect
        GetComponent<AudioSource>().PlayOneShot(reloadAudio);
    }

    // Listener. Remove magazine
    private void RemoveMagazine(SelectExitEventArgs interactor)
    {
        // FIX ME
        interactor.interactableObject.transform.gameObject.layer = 0;

        if (!exitGame) // fix: debug error unparrenting pistol magazine when exit game
            interactor.interactableObject.transform.parent = null;

        // Reload audio effect
        GetComponent<AudioSource>().PlayOneShot(reloadAudio);
    }

    // fix: debug error unparrenting pistol magazine when exit game
    void OnApplicationQuit()
    {
        exitGame = true;
    }
}