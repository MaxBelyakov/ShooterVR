using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketInteractorEdited : XRSocketInteractor
{
    private string magazineTag = "pistol magazine ammo";

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.CompareTag(magazineTag);
    }
}