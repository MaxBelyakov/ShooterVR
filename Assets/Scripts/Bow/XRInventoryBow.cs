using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using TMPro;

public class XRInventoryBow : XRSocketInteractor
{
    [SerializeField] private GameObject ammoPrefab;     // Ammo prefab
    [SerializeField] private TMP_Text ammoText;         // Text ammo status in inventory

    [SerializeField] private AudioClip a_magazineTake;  // Take magazine sound effect

    private MeshRenderer[] meshRenderers;               // Need to show/hide for example pistol magazine and bullets inside

    private string _magazineTag = "arrow";               // Ammo tag (need to put in socket just it)
    private string _boxTag = "bow ammo box";             // Ammo box tag (box with bullets)

    private int _maxAmmo = 30;                          // Max arrows inventory size
    private int _ammoBox = 10;                          // Amount of arrows in quiver
    private int _currentAmmo = 0;

    protected override void Start()
    {
        // Add listener to drag ammo from belt trigger
        GetComponent<XRSimpleInteractable>().selectEntered.AddListener(DragAmmo);

        // Collect all mesh render components in inventory socket
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    private void DragAmmo(SelectEnterEventArgs args)
    {
        // Drag only if ammo exists in inventory
        if (_currentAmmo > 0)
        {
            // Cancel interaction between hand (interactor) and inventory socket
            interactionManager.SelectExit(args.interactorObject, args.interactableObject);

            // Get ammo from inventory
            _currentAmmo--;

            // Put ammo in hand (interactor)
            GameObject ammo = Instantiate(ammoPrefab);
            interactionManager.SelectEnter(args.interactorObject, ammo.GetComponent<IXRSelectInteractable>());

            // Play sound effect
            GetComponent<AudioSource>().PlayOneShot(a_magazineTake);
        }
    }

    // Put ammo inside inventory socket
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        GameObject ammo = args.interactableObject.transform.gameObject;

        // Cancel interaction between hand (interactor) and ammo
        interactionManager.SelectExit(args.interactorObject, args.interactableObject);

        // Ammo box or single ammo
        if (ammo.tag == _boxTag)
            _currentAmmo = _currentAmmo + _ammoBox;
        else
            _currentAmmo++;

        // Limit by max ammo size
        if (_currentAmmo > _maxAmmo)
            _currentAmmo = _maxAmmo;

        // Destroy ammo prefab
        Destroy(ammo);

        // Play sound effect
        GetComponent<AudioSource>().PlayOneShot(a_magazineTake);

        base.OnSelectEntered(args);
    }

    void Update()
    {
        // Show/hide ammo on belt
        if (_currentAmmo > 0)
            foreach (MeshRenderer component in meshRenderers)
                component.enabled = true;           
        else
            foreach (MeshRenderer component in meshRenderers)
                component.enabled = false;

        // Update ammo status text
        ammoText.text = _currentAmmo + " / " + _maxAmmo;
    }

    // Hover socket just for selected weapon ammo and in case free place in inventory
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && _currentAmmo < _maxAmmo
        && (interactable.transform.CompareTag(_magazineTag) || interactable.transform.CompareTag(_boxTag));
    }

    // Can put in inventory socket only current ammo type and limit by max socket size
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && _currentAmmo < _maxAmmo
        && (interactable.transform.CompareTag(_magazineTag) || interactable.transform.CompareTag(_boxTag));
    }
}
