// Shotgun socket interactor controller
// Need to reload weapon just by shotgun ammo (check by tag)
// Ignore interacting if current amount of bullets more or equal to maximum amount of bullets inside weapon

using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketInteractorShotgun : XRSocketInteractor
{
    private string magazineTag = "shotgun ammo";

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.CompareTag(magazineTag) && Shotgun.s_bulletsCurrent < Shotgun.s_bulletsAll;
    }
}