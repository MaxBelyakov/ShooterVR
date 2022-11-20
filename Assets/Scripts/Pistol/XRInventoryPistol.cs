// Use List<GameObject> to save current bullets inside ammo that was put in inventory. When you grab, you get the same magazine that you put

using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class XRInventoryPistol : XRSocketInteractor
{
    [SerializeField] private TMP_Text ammoText;                     // Text ammo status in inventory

    [SerializeField] private AudioClip a_magazineTake;              // Take magazine sound effect

    private MeshRenderer[] meshRenderers;                           // Need to show/hide for example pistol magazine and bullets inside

    private List<GameObject> inventory = new List<GameObject>();    // Inventory collection

    private int _maxAmmo = 3;                                       // Max ammo inventory size

    protected override void Start()
    {
        // Add listener to drag ammo from belt trigger
        GetComponent<XRSimpleInteractable>().selectEntered.AddListener(DragAmmo);

        // Collect bullets mesh render components in pistol magazine
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    private void DragAmmo(SelectEnterEventArgs args)
    {
        // Drag only if ammo exists in inventory
        if (inventory.Count > 0)
        {
            // Cancel interaction between hand (interactor) and inventory socket
            interactionManager.SelectExit(args.interactorObject, args.interactableObject);

            // Get ammo from inventory
            GameObject ammo = inventory[inventory.Count - 1];
            ammo.SetActive(true);
            inventory.Remove(ammo);

            // Put ammo in hand (interactor)
            interactionManager.SelectEnter(args.interactorObject, ammo.GetComponent<IXRSelectInteractable>());

            // Play sound effect
            GetComponent<AudioSource>().PlayOneShot(a_magazineTake);
        }
    }

    // Put ammo inside inventory socket
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Cancel interaction between hand (interactor) and ammo
        interactionManager.SelectExit(args.interactorObject, args.interactableObject);

        // Put ammo in inventory
        GameObject ammo = args.interactableObject.transform.gameObject;
        inventory.Add(ammo);
        ammo.SetActive(false);

        // Play sound effect
        GetComponent<AudioSource>().PlayOneShot(a_magazineTake);

        base.OnSelectEntered(args);
    }

    void Update()
    {
        // Show/hide ammo on belt
        if (inventory.Count > 0)
            foreach (MeshRenderer component in meshRenderers)
                component.enabled = true;           
        else
            foreach (MeshRenderer component in meshRenderers)
                component.enabled = false;

        // Update ammo status text
        ammoText.text = inventory.Count + " / " + _maxAmmo;
    }

    // Hover socket just for selected weapon ammo and in case free place in inventory
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && (interactable.transform.GetComponent<PistolMagazine>() != null) && inventory.Count < _maxAmmo;
    }

    // Can put in inventory socket only current ammo type and limit by max socket size
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && (interactable.transform.GetComponent<PistolMagazine>() != null) && inventory.Count < _maxAmmo;
    }
}