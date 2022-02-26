// Machine Gun socket interactor controller
// Need to reload weapon just by machine gun ammo (check by tag)

using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketInteractorMachineGun : XRSocketInteractor
{
    private string magazineTag = "machine gun ammo";

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.CompareTag(magazineTag);
    }

    // Hover socket just for selected weapon ammo
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.transform.CompareTag(magazineTag);
    }
}