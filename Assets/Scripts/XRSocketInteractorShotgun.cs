// Shotgun socket interactor controller
// Need to reload weapon just by shotgun ammo (check by tag)
// Ignore interacting if current amount of bullets more or equal to maximum amount of bullets inside weapon
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketInteractorShotgun : XRSocketInteractor
{
    private string magazineTag = "shotgun ammo";

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.CompareTag(magazineTag);
    }

    // Hover socket just for selected weapon ammo
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.transform.CompareTag(magazineTag);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        args.interactableObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        args.interactableObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        base.OnSelectExited(args);
    }
}