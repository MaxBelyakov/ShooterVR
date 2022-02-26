using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class XRInventoryPistol : XRSocketInteractor
{
    public XRSimpleInteractable dragInteractable;                   // XRSimpleInteraction component of inventory (need to drag ammo)

    private string magazineTag = "pistol magazine ammo";            // Ammo tag (need to put in socket just it)

    private List<GameObject> inventory = new List<GameObject>();    // Inventory collection

    public TMP_Text ammoText;                                       // Text ammo status in inventory

    public AudioClip a_magazineTake;                                // Take magazine sound effect

    private MeshRenderer[] meshRenderers;                           // Need to show/hide for example pistol magazine and bullets inside

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

    // Can put in inventory socket only current ammo type and limit by max socket size
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.CompareTag(magazineTag) && inventory.Count < Pistol.s_ammoAll;
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
        ammoText.text = inventory.Count + " / " + Pistol.s_ammoAll;
    }

    // Hover socket just for selected weapon ammo and in case free place in inventory
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.transform.CompareTag(magazineTag) && inventory.Count < Pistol.s_ammoAll;
    }
}