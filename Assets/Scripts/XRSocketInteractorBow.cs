// Bow socket interactor controller
// Need to reload weapon just by arrows (check by tag)
using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketInteractorBow : XRSocketInteractor
{
    private string arrowTag = "arrow";
    
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        // s_shooting flag disable socket for arrow release
        return base.CanSelect(interactable) && !WeaponController.s_shooting && interactable.transform.CompareTag(arrowTag);
    }
}