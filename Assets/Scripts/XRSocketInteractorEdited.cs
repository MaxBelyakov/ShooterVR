// Pistol socket interactor controller
// Need to reload pistol just by pistol ammo (check by tag)

using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketInteractorEdited : XRSocketInteractor
{
    private string magazineTag = "pistol magazine ammo";

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.CompareTag(magazineTag);
    }
}