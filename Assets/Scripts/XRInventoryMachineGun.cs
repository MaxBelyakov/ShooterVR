using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using TMPro;

public class XRInventoryMachineGun : XRSocketInteractor
{
    public XRSimpleInteractable dragInteractable;           // XRSimpleInteraction component of inventory (need to drag ammo)

    public GameObject ammoPrefab;                           // Ammo prefab
    private string magazineTag = "machine gun ammo";        // Ammo tag (need to put in socket just it)

    private int ammoAll = 3;                                // Max value of ammo in inventory
    private int ammoCurrent = 0;                            // Current value of ammo in inventory
    public TMP_Text ammoText;                               // Text ammo status in inventory

    public AudioClip a_magazineTake;                        // Take magazine sound effect

    private MeshRenderer[] meshRenderers;                   // Need to show/hide for example pistol magazine and bullets inside

    protected override void Start()
    {
        // Add listener to drag ammo from belt trigger
        dragInteractable.selectEntered.AddListener(DragAmmo);

        // Collect all mesh render components in inventory socket
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    private void DragAmmo(SelectEnterEventArgs args)
    {
        // Drag only if ammo exists in inventory
        if (ammoCurrent > 0)
        {
            // Cancel interaction between hand (interactor) and inventory socket
            interactionManager.SelectExit(args.interactorObject, args.interactableObject);

            // Create new ammo
            GameObject ammo = Instantiate(ammoPrefab, args.interactableObject.transform.position, args.interactableObject.transform.rotation);
            
            // Put new ammo in hand (interactor)
            interactionManager.SelectEnter(args.interactorObject, ammo.GetComponent<IXRSelectInteractable>());

            // Decrease ammo value
            ammoCurrent--;

            // Play sound effect
            GetComponent<AudioSource>().PlayOneShot(a_magazineTake);
        }
    }

    // Can put in inventory socket only current ammo type and limit by max socket size
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.CompareTag(magazineTag) && ammoCurrent < ammoAll;
    }

    // Put ammo inside inventory socket
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Cancel interaction between hand (interactor) and ammo
        interactionManager.SelectExit(args.interactorObject, args.interactableObject);

        // Destroy ammo
        Destroy(args.interactableObject.transform.gameObject);

        // Increase ammo value
        ammoCurrent++;

        // Play sound effect
        GetComponent<AudioSource>().PlayOneShot(a_magazineTake);

        base.OnSelectEntered(args);
    }

    void Update()
    {
        // Show/hide ammo on belt
        if (ammoCurrent > 0)
            foreach (MeshRenderer component in meshRenderers)
                component.enabled = true;           
        else
            foreach (MeshRenderer component in meshRenderers)
                component.enabled = false;

        // Update ammo status text
        ammoText.text = ammoCurrent + " / " + ammoAll;
    }
}