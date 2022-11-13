using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class XRInventoryShotgun : XRSocketInteractor
{
    public XRSimpleInteractable dragInteractable;                   // XRSimpleInteraction component of inventory (need to drag ammo)

    public GameObject ammoPrefab;                                   // Ammo prefab
    private string magazineTag = "shotgun ammo";                    // Ammo tag (need to put in socket just it)
    private string boxTag = "shotgun ammo box";                     // Ammo box tag (box with bullets)

    private List<GameObject> inventory = new List<GameObject>();    // Inventory collection

    public TMP_Text ammoText;                                       // Text ammo status in inventory

    public AudioClip a_magazineTake;                                // Take magazine sound effect

    private MeshRenderer[] meshRenderers;                           // Need to show/hide for example pistol magazine and bullets inside

    public static int s_ammoAll = 30;                       // Max ammo inventory size
    public static int s_ammoBox = 10;                       // Amount of bullets in box ammo

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
        return base.CanSelect(interactable) && inventory.Count < s_ammoAll 
        && (interactable.transform.CompareTag(magazineTag) || interactable.transform.CompareTag(boxTag));
    }

    // Put ammo inside inventory socket
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Cancel interaction between hand (interactor) and ammo
        interactionManager.SelectExit(args.interactorObject, args.interactableObject);

        // Get ammo gameobject
        GameObject ammo = args.interactableObject.transform.gameObject;

        // Ammo box or single ammo
        if (ammo.tag == boxTag)
        {
            // Destroy box
            Destroy(ammo);
            
            // Ammo box contain a collection of single bullets
            for (int i = 1; i <= s_ammoBox; i++)
            {
                ammo = Instantiate(ammoPrefab, args.interactableObject.transform.position, args.interactableObject.transform.rotation);
                inventory.Add(ammo);
                ammo.SetActive(false);
                
                // Not more than weapon inventory max size
                if (inventory.Count == s_ammoAll)
                    break;
            }
        } else
        {
            // Put single ammo in inventory
            inventory.Add(ammo);
            ammo.SetActive(false);
        }

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
        ammoText.text = inventory.Count + " / " + s_ammoAll;
    }

    // Hover socket just for selected weapon ammo and in case free place in inventory
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && inventory.Count < s_ammoAll 
        && (interactable.transform.CompareTag(magazineTag) || interactable.transform.CompareTag(boxTag));
    }
}